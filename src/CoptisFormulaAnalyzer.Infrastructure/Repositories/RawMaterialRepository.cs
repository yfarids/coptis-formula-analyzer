using CoptisFormulaAnalyzer.Core.Entities;
using CoptisFormulaAnalyzer.Core.Interfaces;
using CoptisFormulaAnalyzer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoptisFormulaAnalyzer.Infrastructure.Repositories;

public class RawMaterialRepository : IRawMaterialRepository
{
    private readonly FormulaAnalyzerContext _context;

    public RawMaterialRepository(FormulaAnalyzerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RawMaterial>> GetAllAsync()
    {
        return await _context.RawMaterials.ToListAsync();
    }

    public async Task<RawMaterial?> GetByIdAsync(int id)
    {
        return await _context.RawMaterials.FindAsync(id);
    }

    public async Task<RawMaterial?> GetByNameAsync(string name)
    {
        return await _context.RawMaterials.FirstOrDefaultAsync(rm => rm.Name == name);
    }

    public async Task<RawMaterial> AddAsync(RawMaterial rawMaterial)
    {
        _context.RawMaterials.Add(rawMaterial);
        await _context.SaveChangesAsync();
        return rawMaterial;
    }

    public async Task UpdateAsync(RawMaterial rawMaterial)
    {
        _context.RawMaterials.Update(rawMaterial);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var rawMaterial = await _context.RawMaterials.FindAsync(id);
        if (rawMaterial != null)
        {
            _context.RawMaterials.Remove(rawMaterial);
            await _context.SaveChangesAsync();
        }
    }
}
