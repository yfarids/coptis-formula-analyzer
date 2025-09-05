using CoptisFormulaAnalyzer.Core.DTOs;
using CoptisFormulaAnalyzer.Core.Entities;
using CoptisFormulaAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoptisFormulaAnalyzer.Application.Services;

public class FormulaService : IFormulaService
{
    private readonly IFormulaRepository _formulaRepository;
    private readonly IRawMaterialRepository _rawMaterialRepository;
    private readonly ILogger<FormulaService> _logger;

    public FormulaService(
        IFormulaRepository formulaRepository,
        IRawMaterialRepository rawMaterialRepository,
        ILogger<FormulaService> logger)
    {
        _formulaRepository = formulaRepository;
        _rawMaterialRepository = rawMaterialRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<FormulaDto>> GetAllFormulasAsync()
    {
        var formulas = await _formulaRepository.GetAllAsync();
        return formulas.Select(f => new FormulaDto
        {
            Name = f.Name,
            Weight = f.TotalWeight,
            WeightUnit = "g",
            RawMaterials = f.Components.Select(c => new RawMaterialDto
            {
                Name = c.RawMaterial.Name,
                Price = new RawMaterialPriceDto
                {
                    Amount = Math.Round(c.RawMaterial.PricePerKg, 2),
                    Currency = "EUR",
                    ReferenceUnit = "kg"
                },
                Substances = new List<SubstanceDto> { new SubstanceDto { Name = c.RawMaterial.Name } },
                SubstancePercentages = new List<decimal> { 100 }
            }).ToList(),
            RawMaterialPercentages = f.Components.Select(c => Math.Round((c.WeightInGrams / f.TotalWeight) * 100, 2)).ToList()
        });
    }

    public async Task<FormulaDto?> GetFormulaByIdAsync(int id)
    {
        var formula = await _formulaRepository.GetByIdAsync(id);
        if (formula == null) return null;

        return new FormulaDto
        {
            Name = formula.Name,
            Weight = formula.TotalWeight,
            WeightUnit = "g",
            RawMaterials = formula.Components.Select(c => new RawMaterialDto
            {
                Name = c.RawMaterial.Name,
                Price = new RawMaterialPriceDto
                {
                    Amount = Math.Round(c.RawMaterial.PricePerKg, 2),
                    Currency = "EUR",
                    ReferenceUnit = "kg"
                },
                Substances = new List<SubstanceDto> { new SubstanceDto { Name = c.RawMaterial.Name } },
                SubstancePercentages = new List<decimal> { 100 }
            }).ToList(),
            RawMaterialPercentages = formula.Components.Select(c => Math.Round((c.WeightInGrams / formula.TotalWeight) * 100, 2)).ToList()
        };
    }

    public async Task<bool> ImportFormulaAsync(FormulaDto formulaDto)
    {
        try
        {
            // Check if formula already exists
            if (await _formulaRepository.ExistsAsync(formulaDto.Name))
            {
                _logger.LogWarning("Formula {FormulaName} already exists", formulaDto.Name);
                return false;
            }

            var formula = new Formula
            {
                Name = formulaDto.Name,
                CreatedDate = DateTime.UtcNow
            };

            decimal totalWeight = 0;
            decimal totalCost = 0;

            for (int i = 0; i < formulaDto.RawMaterials.Count; i++)
            {
                var rawMaterialDto = formulaDto.RawMaterials[i];
                var percentage = i < formulaDto.RawMaterialPercentages.Count ? formulaDto.RawMaterialPercentages[i] : 0;
                
                // Calculate actual weight from percentage
                var actualWeight = (formulaDto.Weight * percentage) / 100;

                // Get or create raw material
                var rawMaterial = await _rawMaterialRepository.GetByNameAsync(rawMaterialDto.Name);
                if (rawMaterial == null)
                {
                    rawMaterial = new RawMaterial
                    {
                        Name = rawMaterialDto.Name,
                        PricePerKg = rawMaterialDto.Price.Amount,
                        CreatedDate = DateTime.UtcNow
                    };
                    rawMaterial = await _rawMaterialRepository.AddAsync(rawMaterial);
                }

                var component = new FormulaComponent
                {
                    RawMaterialId = rawMaterial.Id,
                    WeightInGrams = actualWeight,
                    Cost = Math.Round((actualWeight / 1000) * rawMaterial.PricePerKg, 2)
                };

                formula.Components.Add(component);
                totalWeight += actualWeight;
                totalCost += component.Cost;
            }

            formula.TotalWeight = Math.Round(totalWeight, 2);
            formula.TotalCost = Math.Round(totalCost, 2);

            await _formulaRepository.AddAsync(formula);
            _logger.LogInformation("Formula {FormulaName} imported successfully", formulaDto.Name);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing formula {FormulaName}", formulaDto.Name);
            return false;
        }
    }

    public async Task<bool> DeleteFormulaAsync(int id)
    {
        try
        {
            await _formulaRepository.DeleteAsync(id);
            _logger.LogInformation("Formula with ID {FormulaId} deleted successfully", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting formula with ID {FormulaId}", id);
            return false;
        }
    }

    public async Task<bool> DeleteFormulaByNameAsync(string name)
    {
        try
        {
            var formulas = await _formulaRepository.GetAllAsync();
            var formula = formulas.FirstOrDefault(f => f.Name == name);
            
            if (formula == null)
            {
                _logger.LogWarning("Formula {FormulaName} not found for deletion", name);
                return false;
            }

            await _formulaRepository.DeleteAsync(formula.Id);
            _logger.LogInformation("Formula {FormulaName} deleted successfully", name);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting formula {FormulaName}", name);
            return false;
        }
    }

    public async Task RecalculateFormulaCostsAsync()
    {
        var formulas = await _formulaRepository.GetAllAsync();
        
        foreach (var formula in formulas)
        {
            decimal totalCost = 0;
            bool priceUpdated = false;

            foreach (var component in formula.Components)
            {
                var newCost = Math.Round((component.WeightInGrams / 1000) * component.RawMaterial.PricePerKg, 2);
                if (newCost != component.Cost)
                {
                    component.Cost = newCost;
                    priceUpdated = true;
                }
                totalCost += component.Cost;
            }

            if (priceUpdated)
            {
                formula.TotalCost = Math.Round(totalCost, 2);
                formula.IsPriceUpdated = true;
                formula.LastModifiedDate = DateTime.UtcNow;
                await _formulaRepository.UpdateAsync(formula);
            }
        }
    }

    public async Task<IEnumerable<SubstanceAnalysisDto>> GetSubstanceAnalysisByWeightAsync()
    {
        var formulas = await _formulaRepository.GetAllAsync();
        
        var substances = formulas
            .SelectMany(f => f.Components)
            .GroupBy(c => c.RawMaterial.Name)
            .Select(g => new SubstanceAnalysisDto
            {
                Name = g.Key,
                TotalWeight = Math.Round(g.Sum(c => c.WeightInGrams), 2),
                NumberOfFormulas = g.Select(c => c.FormulaId).Distinct().Count()
            })
            .OrderByDescending(s => s.TotalWeight)
            .ToList();

        return substances;
    }

    public async Task<IEnumerable<SubstanceAnalysisDto>> GetSubstanceAnalysisByUsageAsync()
    {
        var formulas = await _formulaRepository.GetAllAsync();
        
        var substances = formulas
            .SelectMany(f => f.Components)
            .GroupBy(c => c.RawMaterial.Name)
            .Select(g => new SubstanceAnalysisDto
            {
                Name = g.Key,
                TotalWeight = Math.Round(g.Sum(c => c.WeightInGrams), 2),
                NumberOfFormulas = g.Select(c => c.FormulaId).Distinct().Count()
            })
            .OrderByDescending(s => s.NumberOfFormulas)
            .ThenByDescending(s => s.TotalWeight)
            .ToList();

        return substances;
    }
}
