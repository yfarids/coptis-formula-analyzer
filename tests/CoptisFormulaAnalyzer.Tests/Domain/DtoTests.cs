using CoptisFormulaAnalyzer.Core.DTOs;

namespace CoptisFormulaAnalyzer.Tests.DTOs;

public class FormulaDtoTests
{
    [Fact]
    public void FormulaDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var formulaDto = new FormulaDto
        {
            Name = "Test Formula",
            Weight = 100.0m,
            WeightUnit = "g",
            RawMaterials = new List<RawMaterialDto>(),
            RawMaterialPercentages = new List<decimal>()
        };

        // Assert
        Assert.Equal("Test Formula", formulaDto.Name);
        Assert.Equal(100.0m, formulaDto.Weight);
        Assert.Equal("g", formulaDto.WeightUnit);
        Assert.NotNull(formulaDto.RawMaterials);
        Assert.NotNull(formulaDto.RawMaterialPercentages);
        Assert.Empty(formulaDto.RawMaterials);
        Assert.Empty(formulaDto.RawMaterialPercentages);
    }

    [Fact]
    public void FormulaDto_ShouldAllowComplexConstruction()
    {
        // Arrange & Act
        var formulaDto = new FormulaDto
        {
            Name = "Complex Formula",
            Weight = 500.0m,
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
                        new SubstanceDto { Name = "H2O" }
                    },
                    SubstancePercentages = new List<decimal> { 100.0m }
                }
            },
            RawMaterialPercentages = new List<decimal> { 80.0m }
        };

        // Assert
        Assert.Equal("Complex Formula", formulaDto.Name);
        Assert.Single(formulaDto.RawMaterials);
        Assert.Single(formulaDto.RawMaterialPercentages);
        Assert.Equal(80.0m, formulaDto.RawMaterialPercentages[0]);
        Assert.Equal("Water", formulaDto.RawMaterials[0].Name);
    }
}

public class RawMaterialDtoTests
{
    [Fact]
    public void RawMaterialDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var rawMaterialDto = new RawMaterialDto
        {
            Name = "Glycerin",
            Price = new RawMaterialPriceDto
            {
                Amount = 3.0m,
                Currency = "EUR",
                ReferenceUnit = "kg"
            },
            Substances = new List<SubstanceDto>
            {
                new SubstanceDto { Name = "Glycerol" }
            },
            SubstancePercentages = new List<decimal> { 100.0m }
        };

        // Assert
        Assert.Equal("Glycerin", rawMaterialDto.Name);
        Assert.NotNull(rawMaterialDto.Price);
        Assert.Equal(3.0m, rawMaterialDto.Price.Amount);
        Assert.Equal("EUR", rawMaterialDto.Price.Currency);
        Assert.Equal("kg", rawMaterialDto.Price.ReferenceUnit);
        Assert.Single(rawMaterialDto.Substances);
        Assert.Equal("Glycerol", rawMaterialDto.Substances[0].Name);
        Assert.Single(rawMaterialDto.SubstancePercentages);
        Assert.Equal(100.0m, rawMaterialDto.SubstancePercentages[0]);
    }
}

public class SubstanceAnalysisDtoTests
{
    [Fact]
    public void SubstanceAnalysisDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var analysisDto = new SubstanceAnalysisDto
        {
            Name = "Water",
            TotalWeight = 150.75m,
            NumberOfFormulas = 3
        };

        // Assert
        Assert.Equal("Water", analysisDto.Name);
        Assert.Equal(150.75m, analysisDto.TotalWeight);
        Assert.Equal(3, analysisDto.NumberOfFormulas);
    }

    [Fact]
    public void SubstanceAnalysisDto_ShouldHandleZeroValues()
    {
        // Arrange & Act
        var analysisDto = new SubstanceAnalysisDto
        {
            Name = "Unused Substance",
            TotalWeight = 0.0m,
            NumberOfFormulas = 0
        };

        // Assert
        Assert.Equal("Unused Substance", analysisDto.Name);
        Assert.Equal(0.0m, analysisDto.TotalWeight);
        Assert.Equal(0, analysisDto.NumberOfFormulas);
    }
}

public class RawMaterialPriceDtoTests
{
    [Fact]
    public void RawMaterialPriceDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var priceDto = new RawMaterialPriceDto
        {
            Amount = 25.50m,
            Currency = "USD",
            ReferenceUnit = "lb"
        };

        // Assert
        Assert.Equal(25.50m, priceDto.Amount);
        Assert.Equal("USD", priceDto.Currency);
        Assert.Equal("lb", priceDto.ReferenceUnit);
    }

    [Theory]
    [InlineData(0.01, "EUR", "g")]
    [InlineData(1000.00, "JPY", "kg")]
    [InlineData(99.99, "GBP", "oz")]
    public void RawMaterialPriceDto_ShouldHandleVariousValues(decimal amount, string currency, string unit)
    {
        // Arrange & Act
        var priceDto = new RawMaterialPriceDto
        {
            Amount = amount,
            Currency = currency,
            ReferenceUnit = unit
        };

        // Assert
        Assert.Equal(amount, priceDto.Amount);
        Assert.Equal(currency, priceDto.Currency);
        Assert.Equal(unit, priceDto.ReferenceUnit);
    }
}

public class SubstanceDtoTests
{
    [Fact]
    public void SubstanceDto_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var substanceDto = new SubstanceDto
        {
            Name = "Citral"
        };

        // Assert
        Assert.Equal("Citral", substanceDto.Name);
    }

    [Theory]
    [InlineData("Water")]
    [InlineData("Sodium Lauryl Sulfate")]
    [InlineData("Glycerin")]
    [InlineData("")]
    public void SubstanceDto_ShouldHandleVariousNames(string name)
    {
        // Arrange & Act
        var substanceDto = new SubstanceDto
        {
            Name = name
        };

        // Assert
        Assert.Equal(name, substanceDto.Name);
    }
}
