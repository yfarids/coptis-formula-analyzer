namespace CoptisFormulaAnalyzer.Application.Services;

/// <summary>
/// Coordinates formula imports to prevent concurrent database operations
/// </summary>
public class ImportCoordinatorService
{
    private readonly ILogger<ImportCoordinatorService> _logger;
    private readonly SemaphoreSlim _importLock = new(1, 1);

    public ImportCoordinatorService(ILogger<ImportCoordinatorService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes an import operation with concurrency protection
    /// </summary>
    public async Task<bool> ExecuteImportAsync(Func<Task<bool>> importOperation, string operationName = "Import")
    {
        await _importLock.WaitAsync();
        try
        {
            _logger.LogInformation("Starting {OperationName} operation", operationName);
            var result = await importOperation();
            _logger.LogInformation("Completed {OperationName} operation with result: {Result}", operationName, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during {OperationName} operation", operationName);
            return false;
        }
        finally
        {
            _importLock.Release();
        }
    }

    public void Dispose()
    {
        _importLock?.Dispose();
    }
}
