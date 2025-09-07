using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoptisFormulaAnalyzer.Application.Services;

public class FileWatcherService : BackgroundService
{
    private readonly ILogger<FileWatcherService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _watchFolder;
    private FileSystemWatcher? _watcher;
    private readonly SemaphoreSlim _processingLock = new(1, 1);

    public FileWatcherService(ILogger<FileWatcherService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _watchFolder = GetConfiguredImportPath(configuration);
        
        _logger.LogInformation("FileWatcherService initialized with folder: {WatchFolder}", _watchFolder);
        
        // Create the import folder if it doesn't exist
        if (!Directory.Exists(_watchFolder))
        {
            Directory.CreateDirectory(_watchFolder);
            _logger.LogInformation("Created import folder: {WatchFolder}", _watchFolder);
        }
    }

    private static string GetConfiguredImportPath(IConfiguration configuration)
    {
        // Try to get from appsettings.json first
        var configuredPath = configuration["ImportFolder"];
        
        if (!string.IsNullOrEmpty(configuredPath))
        {
            // Handle special folder placeholders
            if (configuredPath.Contains("{ProgramData}"))
            {
                return configuredPath.Replace("{ProgramData}", 
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            }
            
            if (configuredPath.Contains("{CommonDocuments}"))
            {
                return configuredPath.Replace("{CommonDocuments}", 
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
            }
            
            if (configuredPath.Contains("{MyDocuments}"))
            {
                return configuredPath.Replace("{MyDocuments}", 
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            }
            
            // If it's an absolute path, use it as-is
            if (Path.IsPathRooted(configuredPath))
            {
                return configuredPath;
            }
            
            // If relative path, combine with application base directory
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuredPath);
        }
        
        // Default fallback to ProgramData
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "CoptisFormulas");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Creating FileSystemWatcher for path: {Path}", _watchFolder);
            
            _watcher = new FileSystemWatcher(_watchFolder, "*.json")
            {
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName,
                EnableRaisingEvents = true,
                IncludeSubdirectories = false
            };

            _watcher.Created += OnFileCreated;
            _watcher.Changed += OnFileChanged;
            _watcher.Error += OnWatcherError;

            _logger.LogInformation("File watcher started for folder: {WatchFolder}", _watchFolder);
            _logger.LogInformation("FileSystemWatcher - EnableRaisingEvents: {Enabled}, Filter: {Filter}, NotifyFilter: {NotifyFilter}", 
                _watcher.EnableRaisingEvents, _watcher.Filter, _watcher.NotifyFilter);
            
            // Start a background task to periodically check for files
            Task.Run(async () => await PeriodicFileCheck(stoppingToken), stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize FileSystemWatcher for path: {Path}", _watchFolder);
        }

        return Task.CompletedTask;
    }

    private void OnWatcherError(object sender, ErrorEventArgs e)
    {
        _logger.LogError(e.GetException(), "FileSystemWatcher error occurred");
    }

    private async Task PeriodicFileCheck(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(5000, cancellationToken); // Check every 5 seconds
                
                if (Directory.Exists(_watchFolder))
                {
                    var jsonFiles = Directory.GetFiles(_watchFolder, "*.json");
                    if (jsonFiles.Length > 0)
                    {
                        _logger.LogInformation("Periodic check found {Count} JSON files in {Folder}", jsonFiles.Length, _watchFolder);
                        
                        foreach (var file in jsonFiles)
                        {
                            _logger.LogInformation("Processing file from periodic check: {File}", file);
                            await ProcessFile(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during periodic file check");
            }
        }
    }

    private async void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("FileSystemWatcher - File created event: {FilePath}", e.FullPath);
        await ProcessFile(e.FullPath);
    }

    private async void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation("FileSystemWatcher - File changed event: {FilePath}", e.FullPath);
        await ProcessFile(e.FullPath);
    }

    private async Task ProcessFile(string filePath)
    {
        // Use semaphore to prevent concurrent file processing
        await _processingLock.WaitAsync();
        try
        {
            _logger.LogInformation("Processing file: {FilePath}", filePath);
            
            // Wait a bit to ensure file is completely written
            await Task.Delay(1000);

            if (File.Exists(filePath))
            {
                _logger.LogInformation("File exists, reading content: {FilePath}", filePath);
                var jsonContent = await File.ReadAllTextAsync(filePath);
                
                // Use a scope to get the scoped services
                using var scope = _serviceProvider.CreateScope();
                var fileImportService = scope.ServiceProvider.GetRequiredService<FileImportService>();
                
                _logger.LogInformation("Attempting to import formula from file: {FilePath}", filePath);
                var success = await fileImportService.ImportFromJsonAsync(jsonContent);

                if (success)
                {
                    _logger.LogInformation("Successfully imported formula from file: {FilePath}", filePath);
                    // Move file to processed folder
                    var processedFolder = Path.Combine(_watchFolder, "Processed");
                    if (!Directory.Exists(processedFolder))
                        Directory.CreateDirectory(processedFolder);
                    
                    var processedFilePath = Path.Combine(processedFolder, Path.GetFileName(filePath));
                    if (File.Exists(processedFilePath))
                        File.Delete(processedFilePath);
                    File.Move(filePath, processedFilePath);
                    _logger.LogInformation("File moved to processed folder: {ProcessedFilePath}", processedFilePath);
                }
                else
                {
                    _logger.LogError("Failed to import formula from file: {FilePath}", filePath);
                    // Move file to error folder
                    var errorFolder = Path.Combine(_watchFolder, "Errors");
                    if (!Directory.Exists(errorFolder))
                        Directory.CreateDirectory(errorFolder);
                    
                    var errorFilePath = Path.Combine(errorFolder, Path.GetFileName(filePath));
                    if (File.Exists(errorFilePath))
                        File.Delete(errorFilePath);
                    File.Move(filePath, errorFilePath);
                    _logger.LogInformation("File moved to error folder: {ErrorFilePath}", errorFilePath);
                }
            }
            else
            {
                _logger.LogWarning("File no longer exists: {FilePath}", filePath);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogDebug("File processing was cancelled for: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing file: {FilePath}", filePath);
        }
        finally
        {
            _processingLock.Release();
        }
    }

    public override void Dispose()
    {
        try
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Created -= OnFileCreated;
                _watcher.Changed -= OnFileChanged;
                _watcher.Error -= OnWatcherError;
                _watcher.Dispose();
                _watcher = null;
                _logger.LogDebug("FileSystemWatcher disposed successfully");
            }
            
            _processingLock?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error disposing FileWatcherService");
        }
        finally
        {
            base.Dispose();
        }
    }
}
