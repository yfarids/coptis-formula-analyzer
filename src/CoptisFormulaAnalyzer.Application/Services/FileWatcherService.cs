using CoptisFormulaAnalyzer.Core.Interfaces;
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

    public FileWatcherService(ILogger<FileWatcherService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _watchFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportFolder");
        
        // Create the import folder if it doesn't exist
        if (!Directory.Exists(_watchFolder))
        {
            Directory.CreateDirectory(_watchFolder);
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _watcher = new FileSystemWatcher(_watchFolder, "*.json")
        {
            NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        _watcher.Created += OnFileCreated;
        _watcher.Changed += OnFileChanged;

        _logger.LogInformation("File watcher started for folder: {WatchFolder}", _watchFolder);

        return Task.CompletedTask;
    }

    private async void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        await ProcessFile(e.FullPath);
    }

    private async void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        await ProcessFile(e.FullPath);
    }

    private async Task ProcessFile(string filePath)
    {
        try
        {
            // Wait a bit to ensure file is completely written
            await Task.Delay(1000);

            if (File.Exists(filePath))
            {
                var jsonContent = await File.ReadAllTextAsync(filePath);
                
                // Use a scope to get the scoped services
                using var scope = _serviceProvider.CreateScope();
                var fileImportService = scope.ServiceProvider.GetRequiredService<FileImportService>();
                
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
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing file: {FilePath}", filePath);
        }
    }

    public override void Dispose()
    {
        _watcher?.Dispose();
        base.Dispose();
    }
}
