namespace CoptisFormulaAnalyzer.Application.Services
{
    public class NotificationService : INotificationService
    {
        public event Action? OnDataChanged;

        public Task NotifyFormulaImportedAsync(string formulaName)
        {
            OnDataChanged?.Invoke();
            return Task.CompletedTask;
        }

        public Task NotifyFormulaDeletedAsync(string formulaName)
        {
            OnDataChanged?.Invoke();
            return Task.CompletedTask;
        }

        public Task NotifyPriceUpdatedAsync(string rawMaterialName, decimal newPrice)
        {
            OnDataChanged?.Invoke();
            return Task.CompletedTask;
        }
    }
}
