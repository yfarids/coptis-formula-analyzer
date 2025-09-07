using CoptisFormulaAnalyzer.Core.DTOs;

namespace CoptisFormulaAnalyzer.Core.Interfaces;

public interface IFormulaService
{
    Task<IEnumerable<FormulaDto>> GetAllFormulasAsync();
    Task<FormulaDto?> GetFormulaByIdAsync(int id);
    Task<bool> ImportFormulaAsync(FormulaDto formulaDto);
    Task<bool> ImportFormulaAsync(FormulaDto formulaDto, bool sendNotification);
    Task<bool> DeleteFormulaAsync(int id);
    Task<bool> DeleteFormulaByNameAsync(string name);
    Task RecalculateFormulaCostsAsync();
    Task<IEnumerable<SubstanceAnalysisDto>> GetSubstanceAnalysisByWeightAsync();
    Task<IEnumerable<SubstanceAnalysisDto>> GetSubstanceAnalysisByUsageAsync();
}

public interface IRawMaterialService
{
    Task<IEnumerable<RawMaterialDisplayDto>> GetAllRawMaterialsAsync();
    Task<bool> UpdateRawMaterialPriceAsync(int id, decimal newPrice);
}

public class RawMaterialDisplayDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PricePerKg { get; set; }
}
