using System.ComponentModel.DataAnnotations;

namespace CoptisFormulaAnalyzer.Core.Entities;

public class FormulaComponent
{
    public int Id { get; set; }
    
    public int FormulaId { get; set; }
    
    public int RawMaterialId { get; set; }
    
    public decimal WeightInGrams { get; set; }
    
    public decimal Cost { get; set; }
    
    public virtual Formula Formula { get; set; } = null!;
    
    public virtual RawMaterial RawMaterial { get; set; } = null!;
}
