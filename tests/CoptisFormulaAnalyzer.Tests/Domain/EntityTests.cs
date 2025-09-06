using CoptisFormulaAnalyzer.Core.Entities;

namespace CoptisFormulaAnalyzer.Tests.Entities;

public class FormulaTests
{
    [Fact]
    public void Formula_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var formula = new Formula
        {
            Id = 1,
            Name = "Test Formula",
            TotalWeight = 100.50m,
            TotalCost = 25.75m,
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow,
            IsPriceUpdated = true
        };

        // Assert
        Assert.Equal(1, formula.Id);
        Assert.Equal("Test Formula", formula.Name);
        Assert.Equal(100.50m, formula.TotalWeight);
        Assert.Equal(25.75m, formula.TotalCost);
        Assert.True(formula.IsPriceUpdated);
        Assert.NotNull(formula.Components);
        Assert.Empty(formula.Components);
    }

    [Fact]
    public void Formula_Components_ShouldBeInitialized()
    {
        // Arrange & Act
        var formula = new Formula();

        // Assert
        Assert.NotNull(formula.Components);
        Assert.Empty(formula.Components);
    }

    [Fact]
    public void Formula_ShouldAllowAddingComponents()
    {
        // Arrange
        var formula = new Formula();
        var component = new FormulaComponent
        {
            Id = 1,
            FormulaId = formula.Id,
            RawMaterialId = 1,
            WeightInGrams = 50.0m,
            Cost = 10.0m
        };

        // Act
        formula.Components.Add(component);

        // Assert
        Assert.Single(formula.Components);
        Assert.Equal(component, formula.Components.First());
    }
}

public class RawMaterialTests
{
    [Fact]
    public void RawMaterial_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var rawMaterial = new RawMaterial
        {
            Id = 1,
            Name = "Water",
            PricePerKg = 1.50m,
            CreatedDate = DateTime.UtcNow,
            LastModifiedDate = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(1, rawMaterial.Id);
        Assert.Equal("Water", rawMaterial.Name);
        Assert.Equal(1.50m, rawMaterial.PricePerKg);
        Assert.NotNull(rawMaterial.FormulaComponents);
        Assert.Empty(rawMaterial.FormulaComponents);
    }

    [Fact]
    public void RawMaterial_FormulaComponents_ShouldBeInitialized()
    {
        // Arrange & Act
        var rawMaterial = new RawMaterial();

        // Assert
        Assert.NotNull(rawMaterial.FormulaComponents);
        Assert.Empty(rawMaterial.FormulaComponents);
    }
}

public class FormulaComponentTests
{
    [Fact]
    public void FormulaComponent_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var component = new FormulaComponent
        {
            Id = 1,
            FormulaId = 10,
            RawMaterialId = 20,
            WeightInGrams = 75.25m,
            Cost = 15.50m
        };

        // Assert
        Assert.Equal(1, component.Id);
        Assert.Equal(10, component.FormulaId);
        Assert.Equal(20, component.RawMaterialId);
        Assert.Equal(75.25m, component.WeightInGrams);
        Assert.Equal(15.50m, component.Cost);
    }

    [Fact]
    public void FormulaComponent_ShouldAllowNavigationProperties()
    {
        // Arrange
        var formula = new Formula { Id = 1, Name = "Test Formula" };
        var rawMaterial = new RawMaterial { Id = 1, Name = "Water", PricePerKg = 1.0m };
        
        var component = new FormulaComponent
        {
            Id = 1,
            FormulaId = formula.Id,
            RawMaterialId = rawMaterial.Id,
            WeightInGrams = 50.0m,
            Cost = 10.0m,
            Formula = formula,
            RawMaterial = rawMaterial
        };

        // Assert
        Assert.Equal(formula, component.Formula);
        Assert.Equal(rawMaterial, component.RawMaterial);
        Assert.Equal("Test Formula", component.Formula.Name);
        Assert.Equal("Water", component.RawMaterial.Name);
    }
}
