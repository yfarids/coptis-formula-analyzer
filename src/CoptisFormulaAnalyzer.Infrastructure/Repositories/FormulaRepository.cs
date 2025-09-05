using CoptisFormulaAnalyzer.Core.Entities;
using CoptisFormulaAnalyzer.Core.Interfaces;
using CoptisFormulaAnalyzer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CoptisFormulaAnalyzer.Infrastructure.Repositories;

public class FormulaRepository : IFormulaRepository
{
    private readonly FormulaAnalyzerContext _context;

    public FormulaRepository(FormulaAnalyzerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Formula>> GetAllAsync()
    {
        return await _context.Formulas
            .Include(f => f.Components)
            .ThenInclude(c => c.RawMaterial)
            .ToListAsync();
    }

    public async Task<Formula?> GetByIdAsync(int id)
    {
        return await _context.Formulas
            .Include(f => f.Components)
            .ThenInclude(c => c.RawMaterial)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<Formula> AddAsync(Formula formula)
    {
        _context.Formulas.Add(formula);
        await _context.SaveChangesAsync();
        return formula;
    }

    public async Task UpdateAsync(Formula formula)
    {
        _context.Formulas.Update(formula);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var formula = await _context.Formulas.FindAsync(id);
        if (formula != null)
        {
            _context.Formulas.Remove(formula);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(string name)
    {
        return await _context.Formulas.AnyAsync(f => f.Name == name);
    }
}
