using CoptisFormulaAnalyzer.Core.Entities;

namespace CoptisFormulaAnalyzer.Core.Interfaces;

public interface IRawMaterialRepository
{
    Task<IEnumerable<RawMaterial>> GetAllAsync();
    Task<RawMaterial?> GetByIdAsync(int id);
    Task<RawMaterial?> GetByNameAsync(string name);
    Task<RawMaterial> AddAsync(RawMaterial rawMaterial);
    Task UpdateAsync(RawMaterial rawMaterial);
    Task DeleteAsync(int id);
}
