using System.ComponentModel.DataAnnotations;

namespace CoptisFormulaAnalyzer.Core.Entities;

public class Formula
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    
    public decimal TotalWeight { get; set; }
    
    public decimal TotalCost { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public DateTime? LastModifiedDate { get; set; }
    
    public bool IsPriceUpdated { get; set; }
    
    public virtual ICollection<FormulaComponent> Components { get; set; } = new List<FormulaComponent>();
}
