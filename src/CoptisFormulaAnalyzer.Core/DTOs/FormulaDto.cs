namespace CoptisFormulaAnalyzer.Core.DTOs;

public record FormulaDto
{
    public string Name { get; set; } = default!;
    public decimal Weight { get; set; }
    public string WeightUnit { get; set; } = "g";
    public List<RawMaterialDto> RawMaterials { get; set; } = [];

    /// <summary>
    /// Percentages ordered by raw material list
    /// </summary>
    public List<decimal> RawMaterialPercentages { get; set; } = [];
}

public record RawMaterialDto
{
    public string Name { get; set; } = default!;
    public RawMaterialPriceDto Price { get; set; } = new RawMaterialPriceDto();

    public List<SubstanceDto> Substances { get; set; } = [];

    /// <summary>
    /// Percentages ordered by substance list
    /// </summary>
    public List<decimal> SubstancePercentages { get; set; } = [];
}

public record SubstanceDto
{
    public string Name { get; set; } = default!;
}

public record RawMaterialPriceDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public string ReferenceUnit { get; set; } = "kg";
}
