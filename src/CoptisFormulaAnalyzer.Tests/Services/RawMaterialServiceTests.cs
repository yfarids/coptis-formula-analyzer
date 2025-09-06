using CoptisFormulaAnalyzer.Application.Services;
using CoptisFormulaAnalyzer.Core.Entities;
using CoptisFormulaAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoptisFormulaAnalyzer.Tests.Services;

public class RawMaterialServiceTests
{
    private readonly Mock<IRawMaterialRepository> _mockRawMaterialRepository;
    private readonly Mock<ILogger<RawMaterialService>> _mockLogger;
    private readonly RawMaterialService _rawMaterialService;

    public RawMaterialServiceTests()
    {
        _mockRawMaterialRepository = new Mock<IRawMaterialRepository>();
        _mockLogger = new Mock<ILogger<RawMaterialService>>();
        _rawMaterialService = new RawMaterialService(
            _mockRawMaterialRepository.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllRawMaterialsAsync_ShouldReturnAllRawMaterials()
    {
        // Arrange
        var rawMaterials = new List<RawMaterial>
        {
            new RawMaterial { Id = 1, Name = "Water", PricePerKg = 1.0m },
            new RawMaterial { Id = 2, Name = "Glycerin", PricePerKg = 3.0m }
        };

        _mockRawMaterialRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(rawMaterials);

        // Act
        var result = await _rawMaterialService.GetAllRawMaterialsAsync();
        var resultList = result.ToList();

        // Assert
        Assert.Equal(2, resultList.Count);
        Assert.Equal("Water", resultList[0].Name);
        Assert.Equal(1.0m, resultList[0].PricePerKg);
        Assert.Equal("Glycerin", resultList[1].Name);
        Assert.Equal(3.0m, resultList[1].PricePerKg);
    }

    [Fact]
    public async Task UpdateRawMaterialPriceAsync_ShouldReturnTrue_WhenUpdateSuccessful()
    {
        // Arrange
        var rawMaterial = new RawMaterial { Id = 1, Name = "Water", PricePerKg = 1.0m };
        _mockRawMaterialRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(rawMaterial);
        _mockRawMaterialRepository.Setup(x => x.UpdateAsync(It.IsAny<RawMaterial>())).Returns(Task.CompletedTask);

        // Act
        var result = await _rawMaterialService.UpdateRawMaterialPriceAsync(1, 2.0m);

        // Assert
        Assert.True(result);
        Assert.Equal(2.0m, rawMaterial.PricePerKg);
        _mockRawMaterialRepository.Verify(x => x.UpdateAsync(rawMaterial), Times.Once);
    }

    [Fact]
    public async Task UpdateRawMaterialPriceAsync_ShouldReturnFalse_WhenRawMaterialNotFound()
    {
        // Arrange
        _mockRawMaterialRepository.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((RawMaterial?)null);

        // Act
        var result = await _rawMaterialService.UpdateRawMaterialPriceAsync(999, 2.0m);

        // Assert
        Assert.False(result);
        _mockRawMaterialRepository.Verify(x => x.UpdateAsync(It.IsAny<RawMaterial>()), Times.Never);
    }

    [Fact]
    public async Task UpdateRawMaterialPriceAsync_ShouldUpdateLastModifiedDate()
    {
        // Arrange
        var initialDate = DateTime.UtcNow.AddDays(-1);
        var rawMaterial = new RawMaterial 
        { 
            Id = 1, 
            Name = "Water", 
            PricePerKg = 1.0m,
            LastModifiedDate = initialDate
        };
        _mockRawMaterialRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(rawMaterial);
        _mockRawMaterialRepository.Setup(x => x.UpdateAsync(It.IsAny<RawMaterial>())).Returns(Task.CompletedTask);

        // Act
        var result = await _rawMaterialService.UpdateRawMaterialPriceAsync(1, 2.0m);

        // Assert
        Assert.True(result);
        Assert.True(rawMaterial.LastModifiedDate > initialDate);
        _mockRawMaterialRepository.Verify(x => x.UpdateAsync(rawMaterial), Times.Once);
    }

    [Fact]
    public async Task UpdateRawMaterialPriceAsync_ShouldRoundPriceToTwoDecimals()
    {
        // Arrange
        var rawMaterial = new RawMaterial { Id = 1, Name = "Water", PricePerKg = 1.0m };
        _mockRawMaterialRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(rawMaterial);
        _mockRawMaterialRepository.Setup(x => x.UpdateAsync(It.IsAny<RawMaterial>())).Returns(Task.CompletedTask);

        // Act
        var result = await _rawMaterialService.UpdateRawMaterialPriceAsync(1, 2.12345m);

        // Assert
        Assert.True(result);
        Assert.Equal(2.12m, rawMaterial.PricePerKg);
        _mockRawMaterialRepository.Verify(x => x.UpdateAsync(rawMaterial), Times.Once);
    }

    [Fact]
    public async Task GetAllRawMaterialsAsync_ShouldReturnEmptyList_WhenNoRawMaterials()
    {
        // Arrange
        _mockRawMaterialRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<RawMaterial>());

        // Act
        var result = await _rawMaterialService.GetAllRawMaterialsAsync();

        // Assert
        Assert.Empty(result);
    }
}
