using CoptisFormulaAnalyzer.Application.Services;
using CoptisFormulaAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoptisFormulaAnalyzer.Tests.Services;

public class FileImportServiceTests
{
    private readonly Mock<IFormulaService> _mockFormulaService;
    private readonly Mock<ILogger<FileImportService>> _mockLogger;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly FileImportService _fileImportService;

    public FileImportServiceTests()
    {
        _mockFormulaService = new Mock<IFormulaService>();
        _mockLogger = new Mock<ILogger<FileImportService>>();
        _mockConfiguration = new Mock<IConfiguration>();
        
        // Setup default configuration
        _mockConfiguration.Setup(x => x["ImportFolder"]).Returns("./TestImportFolder");
        
        _fileImportService = new FileImportService(
            _mockFormulaService.Object,
            _mockLogger.Object,
            _mockConfiguration.Object);
    }

    [Fact]
    public async Task ImportFromJsonAsync_ShouldReturnTrue_WhenValidJson()
    {
        // Arrange
        var validJson = @"{
            ""Name"": ""Test Formula"",
            ""Weight"": 100.0,
            ""WeightUnit"": ""g"",
            ""RawMaterials"": [
                {
                    ""Name"": ""Water"",
                    ""Price"": {
                        ""Amount"": 1.0,
                        ""Currency"": ""EUR"",
                        ""ReferenceUnit"": ""kg""
                    },
                    ""Substances"": [
                        {
                            ""Name"": ""Water""
                        }
                    ],
                    ""SubstancePercentages"": [100]
                }
            ],
            ""RawMaterialPercentages"": [100]
        }";

        _mockFormulaService.Setup(x => x.ImportFormulaAsync(It.IsAny<Core.DTOs.FormulaDto>()))
                          .ReturnsAsync(true);

        // Act
        var result = await _fileImportService.ImportFromJsonAsync(validJson);

        // Assert
        Assert.True(result);
        _mockFormulaService.Verify(x => x.ImportFormulaAsync(It.IsAny<Core.DTOs.FormulaDto>()), Times.Once);
    }

    [Fact]
    public async Task ImportFromJsonAsync_ShouldReturnFalse_WhenInvalidJson()
    {
        // Arrange
        var invalidJson = @"{
            ""Name"": ""Test Formula"",
            ""Weight"": ""invalid_weight"",
            ""WeightUnit"": ""g""
        }";

        // Act
        var result = await _fileImportService.ImportFromJsonAsync(invalidJson);

        // Assert
        Assert.False(result);
        _mockFormulaService.Verify(x => x.ImportFormulaAsync(It.IsAny<Core.DTOs.FormulaDto>()), Times.Never);
    }

    [Fact]
    public async Task ImportFromJsonAsync_ShouldReturnFalse_WhenEmptyJson()
    {
        // Arrange
        var emptyJson = "";

        // Act
        var result = await _fileImportService.ImportFromJsonAsync(emptyJson);

        // Assert
        Assert.False(result);
        _mockFormulaService.Verify(x => x.ImportFormulaAsync(It.IsAny<Core.DTOs.FormulaDto>()), Times.Never);
    }

    [Fact]
    public async Task ImportFromJsonAsync_ShouldReturnFalse_WhenFormulaServiceFails()
    {
        // Arrange
        var validJson = @"{
            ""Name"": ""Test Formula"",
            ""Weight"": 100.0,
            ""WeightUnit"": ""g"",
            ""RawMaterials"": [],
            ""RawMaterialPercentages"": []
        }";

        _mockFormulaService.Setup(x => x.ImportFormulaAsync(It.IsAny<Core.DTOs.FormulaDto>()))
                          .ReturnsAsync(false);

        // Act
        var result = await _fileImportService.ImportFromJsonAsync(validJson);

        // Assert
        Assert.False(result);
        _mockFormulaService.Verify(x => x.ImportFormulaAsync(It.IsAny<Core.DTOs.FormulaDto>()), Times.Once);
    }

    [Fact]
    public void GetWatchFolderPath_ShouldReturnConfiguredPath()
    {
        // Act
        var path = _fileImportService.GetWatchFolderPath();

        // Assert
        Assert.NotNull(path);
        Assert.Contains("TestImportFolder", path);
    }

    [Theory]
    [InlineData(@"{""Name"":""Valid"",""Weight"":100,""WeightUnit"":""g"",""RawMaterials"":[],""RawMaterialPercentages"":[]}", true)]
    [InlineData(@"{""invalid"":""json""", false)]
    [InlineData(@"not json at all", false)]
    [InlineData(@"", false)]
    [InlineData(null, false)]
    public async Task ImportFromJsonAsync_ShouldHandleVariousInputs(string? jsonInput, bool expectedResult)
    {
        // Arrange
        if (expectedResult)
        {
            _mockFormulaService.Setup(x => x.ImportFormulaAsync(It.IsAny<Core.DTOs.FormulaDto>()))
                              .ReturnsAsync(true);
        }

        // Act
        var result = await _fileImportService.ImportFromJsonAsync(jsonInput ?? "");

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
