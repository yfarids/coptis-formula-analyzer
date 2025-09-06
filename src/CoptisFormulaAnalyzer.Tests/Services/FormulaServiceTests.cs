using CoptisFormulaAnalyzer.Application.Services;
using CoptisFormulaAnalyzer.Core.DTOs;
using CoptisFormulaAnalyzer.Core.Entities;
using CoptisFormulaAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoptisFormulaAnalyzer.Tests.Services;

public class FormulaServiceTests
{
    private readonly Mock<IFormulaRepository> _mockFormulaRepository;
    private readonly Mock<IRawMaterialRepository> _mockRawMaterialRepository;
    private readonly Mock<ILogger<FormulaService>> _mockLogger;
    private readonly FormulaService _formulaService;

    public FormulaServiceTests()
    {
        _mockFormulaRepository = new Mock<IFormulaRepository>();
        _mockRawMaterialRepository = new Mock<IRawMaterialRepository>();
        _mockLogger = new Mock<ILogger<FormulaService>>();
        _formulaService = new FormulaService(
            _mockFormulaRepository.Object,
            _mockRawMaterialRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllFormulasAsync_ShouldReturnFormulas_WhenFormulasExist()
    {
        // Arrange
        var formulas = new List<Formula>
        {
            new Formula
            {
                Id = 1,
                Name = "Test Formula",
                TotalWeight = 100.0m,
                TotalCost = 50.0m,
                Components = new List<FormulaComponent>
                {
                    new FormulaComponent
                    {
                        Id = 1,
                        WeightInGrams = 100.0m,
                        RawMaterial = new RawMaterial
                        {
                            Id = 1,
                            Name = "Water",
                            PricePerKg = 1.0m
                        }
                    }
                }
            }
        };

        _mockFormulaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(formulas);

        // Act
        var result = await _formulaService.GetAllFormulasAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Formula", result.First().Name);
        Assert.Equal(100.0m, result.First().Weight);
    }

    [Fact]
    public async Task ImportFormulaAsync_ShouldReturnTrue_WhenFormulaIsValid()
    {
        // Arrange
        var formulaDto = new FormulaDto
        {
            Name = "New Formula",
            Weight = 100.0m,
            WeightUnit = "g",
            RawMaterials = new List<RawMaterialDto>
            {
                new RawMaterialDto
                {
                    Name = "Water",
                    Price = new RawMaterialPriceDto
                    {
                        Amount = 1.0m,
                        Currency = "EUR",
                        ReferenceUnit = "kg"
                    },
                    Substances = new List<SubstanceDto>
                    {
                        new SubstanceDto { Name = "Water" }
                    },
                    SubstancePercentages = new List<decimal> { 100 }
                }
            },
            RawMaterialPercentages = new List<decimal> { 100 }
        };

        var rawMaterial = new RawMaterial
        {
            Id = 1,
            Name = "Water",
            PricePerKg = 1.0m
        };

        _mockFormulaRepository.Setup(x => x.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _mockRawMaterialRepository.Setup(x => x.GetByNameAsync("Water")).ReturnsAsync(rawMaterial);
        _mockFormulaRepository.Setup(x => x.AddAsync(It.IsAny<Formula>())).ReturnsAsync(new Formula());

        // Act
        var result = await _formulaService.ImportFormulaAsync(formulaDto);

        // Assert
        Assert.True(result);
        _mockFormulaRepository.Verify(x => x.AddAsync(It.IsAny<Formula>()), Times.Once);
    }

    [Fact]
    public async Task ImportFormulaAsync_ShouldReturnFalse_WhenFormulaAlreadyExists()
    {
        // Arrange
        var formulaDto = new FormulaDto
        {
            Name = "Existing Formula",
            Weight = 100.0m,
            WeightUnit = "g",
            RawMaterials = new List<RawMaterialDto>(),
            RawMaterialPercentages = new List<decimal>()
        };

        _mockFormulaRepository.Setup(x => x.ExistsAsync("Existing Formula")).ReturnsAsync(true);

        // Act
        var result = await _formulaService.ImportFormulaAsync(formulaDto);

        // Assert
        Assert.False(result);
        _mockFormulaRepository.Verify(x => x.AddAsync(It.IsAny<Formula>()), Times.Never);
    }

    [Fact]
    public async Task DeleteFormulaByNameAsync_ShouldReturnTrue_WhenFormulaExists()
    {
        // Arrange
        var formula = new Formula
        {
            Id = 1,
            Name = "Test Formula",
            Components = new List<FormulaComponent>
            {
                new FormulaComponent { RawMaterialId = 1 }
            }
        };

        var formulas = new List<Formula> { formula };

        _mockFormulaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(formulas);
        _mockFormulaRepository.Setup(x => x.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _formulaService.DeleteFormulaByNameAsync("Test Formula");

        // Assert
        Assert.True(result);
        _mockFormulaRepository.Verify(x => x.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteFormulaByNameAsync_ShouldReturnFalse_WhenFormulaDoesNotExist()
    {
        // Arrange
        var formulas = new List<Formula>();
        _mockFormulaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(formulas);

        // Act
        var result = await _formulaService.DeleteFormulaByNameAsync("Nonexistent Formula");

        // Assert
        Assert.False(result);
        _mockFormulaRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetSubstanceAnalysisByWeightAsync_ShouldReturnCorrectAnalysis()
    {
        // Arrange
        var formulas = new List<Formula>
        {
            new Formula
            {
                Id = 1,
                Name = "Formula 1",
                Components = new List<FormulaComponent>
                {
                    new FormulaComponent
                    {
                        FormulaId = 1,
                        WeightInGrams = 50.0m,
                        RawMaterial = new RawMaterial { Name = "Water" }
                    },
                    new FormulaComponent
                    {
                        FormulaId = 1,
                        WeightInGrams = 30.0m,
                        RawMaterial = new RawMaterial { Name = "Glycerin" }
                    }
                }
            },
            new Formula
            {
                Id = 2,
                Name = "Formula 2",
                Components = new List<FormulaComponent>
                {
                    new FormulaComponent
                    {
                        FormulaId = 2,
                        WeightInGrams = 40.0m,
                        RawMaterial = new RawMaterial { Name = "Water" }
                    }
                }
            }
        };

        _mockFormulaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(formulas);

        // Act
        var result = await _formulaService.GetSubstanceAnalysisByWeightAsync();
        var resultList = result.ToList();

        // Assert
        Assert.Equal(2, resultList.Count);
        
        // Water should be first (90g total)
        var waterAnalysis = resultList.First(x => x.Name == "Water");
        Assert.Equal(90.0m, waterAnalysis.TotalWeight);
        Assert.Equal(2, waterAnalysis.NumberOfFormulas);

        // Glycerin should be second (30g total)
        var glycerinAnalysis = resultList.First(x => x.Name == "Glycerin");
        Assert.Equal(30.0m, glycerinAnalysis.TotalWeight);
        Assert.Equal(1, glycerinAnalysis.NumberOfFormulas);
    }

    [Fact]
    public async Task GetSubstanceAnalysisByUsageAsync_ShouldReturnCorrectAnalysis()
    {
        // Arrange
        var formulas = new List<Formula>
        {
            new Formula
            {
                Id = 1,
                Name = "Formula 1",
                Components = new List<FormulaComponent>
                {
                    new FormulaComponent
                    {
                        FormulaId = 1,
                        WeightInGrams = 10.0m,
                        RawMaterial = new RawMaterial { Name = "Water" }
                    },
                    new FormulaComponent
                    {
                        FormulaId = 1,
                        WeightInGrams = 90.0m,
                        RawMaterial = new RawMaterial { Name = "Rare Ingredient" }
                    }
                }
            },
            new Formula
            {
                Id = 2,
                Name = "Formula 2",
                Components = new List<FormulaComponent>
                {
                    new FormulaComponent
                    {
                        FormulaId = 2,
                        WeightInGrams = 50.0m,
                        RawMaterial = new RawMaterial { Name = "Water" }
                    }
                }
            }
        };

        _mockFormulaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(formulas);

        // Act
        var result = await _formulaService.GetSubstanceAnalysisByUsageAsync();
        var resultList = result.ToList();

        // Assert
        Assert.Equal(2, resultList.Count);
        
        // Water should be first (used in 2 formulas)
        var waterAnalysis = resultList[0];
        Assert.Equal("Water", waterAnalysis.Name);
        Assert.Equal(2, waterAnalysis.NumberOfFormulas);
        Assert.Equal(60.0m, waterAnalysis.TotalWeight);

        // Rare Ingredient should be second (used in 1 formula, despite higher weight)
        var rareAnalysis = resultList[1];
        Assert.Equal("Rare Ingredient", rareAnalysis.Name);
        Assert.Equal(1, rareAnalysis.NumberOfFormulas);
        Assert.Equal(90.0m, rareAnalysis.TotalWeight);
    }
}
