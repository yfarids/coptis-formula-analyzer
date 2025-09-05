using CoptisFormulaAnalyzer.Core.Entities;

namespace CoptisFormulaAnalyzer.Core.Interfaces;

public interface IFormulaRepository
{
    Task<IEnumerable<Formula>> GetAllAsync();
    Task<Formula?> GetByIdAsync(int id);
    Task<Formula> AddAsync(Formula formula);
    Task UpdateAsync(Formula formula);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(string name);
}
