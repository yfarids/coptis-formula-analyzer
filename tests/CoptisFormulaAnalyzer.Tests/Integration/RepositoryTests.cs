using CoptisFormulaAnalyzer.Core.Entities;
using CoptisFormulaAnalyzer.Infrastructure.Data;
using CoptisFormulaAnalyzer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoptisFormulaAnalyzer.Tests.Repositories;

public class FormulaRepositoryTests : IDisposable
{
    private readonly FormulaAnalyzerContext _context;
    private readonly FormulaRepository _repository;

    public FormulaRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FormulaAnalyzerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FormulaAnalyzerContext(options);
        _repository = new FormulaRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddFormula_WhenFormulaIsValid()
    {
        // Arrange
        var formula = new Formula
        {
            Name = "Test Formula",
            TotalWeight = 100.0m,
            TotalCost = 50.0m,
            CreatedDate = DateTime.UtcNow
        };

        // Act
        var result = await _repository.AddAsync(formula);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Test Formula", result.Name);

        var savedFormula = await _context.Formulas.FindAsync(result.Id);
        Assert.NotNull(savedFormula);
        Assert.Equal("Test Formula", savedFormula.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllFormulas_WithComponents()
    {
        // Arrange
        var rawMaterial = new RawMaterial
        {
            Name = "Water",
            PricePerKg = 1.0m,
            CreatedDate = DateTime.UtcNow
        };
        await _context.RawMaterials.AddAsync(rawMaterial);

        var formula = new Formula
        {
            Name = "Test Formula",
            TotalWeight = 100.0m,
            TotalCost = 50.0m,
            CreatedDate = DateTime.UtcNow
        };
        await _context.Formulas.AddAsync(formula);
        await _context.SaveChangesAsync();

        var component = new FormulaComponent
        {
            FormulaId = formula.Id,
            RawMaterialId = rawMaterial.Id,
            WeightInGrams = 100.0m,
            Cost = 50.0m
        };
        await _context.FormulaComponents.AddAsync(component);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Single(result);
        var retrievedFormula = result.First();
        Assert.Equal("Test Formula", retrievedFormula.Name);
        Assert.Single(retrievedFormula.Components);
        Assert.Equal("Water", retrievedFormula.Components.First().RawMaterial.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFormula_WhenExists()
    {
        // Arrange
        var formula = new Formula
        {
            Name = "Test Formula",
            TotalWeight = 100.0m,
            TotalCost = 50.0m,
            CreatedDate = DateTime.UtcNow
        };
        await _context.Formulas.AddAsync(formula);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(formula.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Formula", result.Name);
        Assert.Equal(formula.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenFormulaExists()
    {
        // Arrange
        var formula = new Formula
        {
            Name = "Existing Formula",
            TotalWeight = 100.0m,
            TotalCost = 50.0m,
            CreatedDate = DateTime.UtcNow
        };
        await _context.Formulas.AddAsync(formula);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync("Existing Formula");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenFormulaNotExists()
    {
        // Act
        var result = await _repository.ExistsAsync("Nonexistent Formula");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveFormula_WhenExists()
    {
        // Arrange
        var formula = new Formula
        {
            Name = "Formula to Delete",
            TotalWeight = 100.0m,
            TotalCost = 50.0m,
            CreatedDate = DateTime.UtcNow
        };
        await _context.Formulas.AddAsync(formula);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(formula.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedFormula = await _context.Formulas.FindAsync(formula.Id);
        Assert.Null(deletedFormula);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateFormula_WhenExists()
    {
        // Arrange
        var formula = new Formula
        {
            Name = "Original Name",
            TotalWeight = 100.0m,
            TotalCost = 50.0m,
            CreatedDate = DateTime.UtcNow
        };
        await _context.Formulas.AddAsync(formula);
        await _context.SaveChangesAsync();

        // Act
        formula.Name = "Updated Name";
        formula.TotalCost = 75.0m;
        formula.LastModifiedDate = DateTime.UtcNow;
        await _repository.UpdateAsync(formula);
        await _context.SaveChangesAsync();

        // Assert
        var updatedFormula = await _context.Formulas.FindAsync(formula.Id);
        Assert.NotNull(updatedFormula);
        Assert.Equal("Updated Name", updatedFormula.Name);
        Assert.Equal(75.0m, updatedFormula.TotalCost);
        Assert.NotNull(updatedFormula.LastModifiedDate);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

public class RawMaterialRepositoryTests : IDisposable
{
    private readonly FormulaAnalyzerContext _context;
    private readonly RawMaterialRepository _repository;

    public RawMaterialRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FormulaAnalyzerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FormulaAnalyzerContext(options);
        _repository = new RawMaterialRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddRawMaterial_WhenValid()
    {
        // Arrange
        var rawMaterial = new RawMaterial
        {
            Name = "Test Material",
            PricePerKg = 10.0m,
            CreatedDate = DateTime.UtcNow
        };

        // Act
        var result = await _repository.AddAsync(rawMaterial);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Test Material", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnRawMaterial_WhenExists()
    {
        // Arrange
        var rawMaterial = new RawMaterial
        {
            Name = "Water",
            PricePerKg = 1.0m,
            CreatedDate = DateTime.UtcNow
        };
        await _context.RawMaterials.AddAsync(rawMaterial);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByNameAsync("Water");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Water", result.Name);
        Assert.Equal(1.0m, result.PricePerKg);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByNameAsync("Nonexistent Material");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRawMaterials()
    {
        // Arrange
        var materials = new[]
        {
            new RawMaterial { Name = "Water", PricePerKg = 1.0m, CreatedDate = DateTime.UtcNow },
            new RawMaterial { Name = "Glycerin", PricePerKg = 3.0m, CreatedDate = DateTime.UtcNow }
        };
        await _context.RawMaterials.AddRangeAsync(materials);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Name == "Water");
        Assert.Contains(result, r => r.Name == "Glycerin");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
