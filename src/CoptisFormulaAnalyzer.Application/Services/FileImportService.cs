using CoptisFormulaAnalyzer.Core.DTOs;
using CoptisFormulaAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CoptisFormulaAnalyzer.Application.Services;

public class FileImportService
{
    private readonly IFormulaService _formulaService;
    private readonly ILogger<FileImportService> _logger;
    private readonly string _watchFolder;

    public FileImportService(IFormulaService formulaService, ILogger<FileImportService> logger, IConfiguration configuration)
    {
        _formulaService = formulaService;
        _logger = logger;
        _watchFolder = GetConfiguredImportPath(configuration);
        
        // Create the import folder if it doesn't exist
        if (!Directory.Exists(_watchFolder))
        {
            Directory.CreateDirectory(_watchFolder);
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

    public async Task<bool> ImportFromJsonAsync(string jsonContent)
    {
        return await ImportFromJsonAsync(jsonContent, sendNotification: true);
    }

    public async Task<bool> ImportFromJsonAsync(string jsonContent, bool sendNotification)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var formulaDto = JsonSerializer.Deserialize<FormulaDto>(jsonContent, options);
            if (formulaDto == null)
            {
                _logger.LogError("Failed to deserialize JSON content");
                return false;
            }

            return await _formulaService.ImportFormulaAsync(formulaDto, sendNotification);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Invalid JSON format");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing formula from JSON");
            return false;
        }
    }

    public void StartFileWatcher()
    {
        var watcher = new FileSystemWatcher(_watchFolder, "*.json")
        {
            NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite,
            EnableRaisingEvents = true
        };

        watcher.Created += OnFileCreated;
        watcher.Changed += OnFileChanged;

        _logger.LogInformation("File watcher started for folder: {WatchFolder}", _watchFolder);
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
                var success = await ImportFromJsonAsync(jsonContent);

                if (success)
                {
                    _logger.LogInformation("Successfully imported formula from file: {FilePath}", filePath);
                    // Optionally move file to processed folder
                    var processedFolder = Path.Combine(_watchFolder, "Processed");
                    if (!Directory.Exists(processedFolder))
                        Directory.CreateDirectory(processedFolder);
                    
                    var processedFilePath = Path.Combine(processedFolder, Path.GetFileName(filePath));
                    File.Move(filePath, processedFilePath);
                }
                else
                {
                    _logger.LogError("Failed to import formula from file: {FilePath}", filePath);
                    // Optionally move file to error folder
                    var errorFolder = Path.Combine(_watchFolder, "Errors");
                    if (!Directory.Exists(errorFolder))
                        Directory.CreateDirectory(errorFolder);
                    
                    var errorFilePath = Path.Combine(errorFolder, Path.GetFileName(filePath));
                    File.Move(filePath, errorFilePath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing file: {FilePath}", filePath);
        }
    }

    public string GetWatchFolderPath() => _watchFolder;
}
