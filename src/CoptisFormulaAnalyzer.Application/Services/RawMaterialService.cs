using CoptisFormulaAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoptisFormulaAnalyzer.Application.Services;

public class RawMaterialService : IRawMaterialService
{
    private readonly IRawMaterialRepository _rawMaterialRepository;
    private readonly ILogger<RawMaterialService> _logger;

    public RawMaterialService(
        IRawMaterialRepository rawMaterialRepository,
        ILogger<RawMaterialService> logger)
    {
        _rawMaterialRepository = rawMaterialRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<RawMaterialDisplayDto>> GetAllRawMaterialsAsync()
    {
        var rawMaterials = await _rawMaterialRepository.GetAllAsync();
        return rawMaterials.Select(rm => new RawMaterialDisplayDto
        {
            Id = rm.Id,
            Name = rm.Name,
            PricePerKg = Math.Round(rm.PricePerKg, 2)
        });
    }

    public async Task<bool> UpdateRawMaterialPriceAsync(int id, decimal newPrice)
    {
        try
        {
            var rawMaterial = await _rawMaterialRepository.GetByIdAsync(id);
            if (rawMaterial == null)
            {
                _logger.LogWarning("Raw material with ID {RawMaterialId} not found", id);
                return false;
            }

            rawMaterial.PricePerKg = Math.Round(newPrice, 2);
            rawMaterial.LastModifiedDate = DateTime.UtcNow;
            
            await _rawMaterialRepository.UpdateAsync(rawMaterial);
            
            _logger.LogInformation("Raw material {RawMaterialName} price updated to {NewPrice}", 
                rawMaterial.Name, newPrice);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating raw material price for ID {RawMaterialId}", id);
            return false;
        }
    }
}
