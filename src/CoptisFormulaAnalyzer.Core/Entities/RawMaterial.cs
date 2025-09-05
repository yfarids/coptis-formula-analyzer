using System.ComponentModel.DataAnnotations;

namespace CoptisFormulaAnalyzer.Core.Entities;

public class RawMaterial
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    
    public decimal PricePerKg { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public virtual ICollection<FormulaComponent> FormulaComponents { get; set; } = new List<FormulaComponent>();
}
