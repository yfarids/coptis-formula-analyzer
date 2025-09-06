namespace CoptisFormulaAnalyzer.Core.Interfaces;

/// <summary>
/// Interface for sending real-time notifications to clients
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Event triggered when data changes occur
    /// </summary>
    event Action? OnDataChanged;

    /// <summary>
    /// Notify clients of a new formula import
    /// </summary>
    Task NotifyFormulaImportedAsync(string formulaName);

    /// <summary>
    /// Notify clients of a formula deletion
    /// </summary>
    Task NotifyFormulaDeletedAsync(string formulaName);

    /// <summary>
    /// Notify clients of a raw material price update
    /// </summary>
    Task NotifyPriceUpdatedAsync(string rawMaterialName, decimal newPrice);
}
