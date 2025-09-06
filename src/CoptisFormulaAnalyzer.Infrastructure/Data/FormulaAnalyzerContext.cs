using CoptisFormulaAnalyzer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoptisFormulaAnalyzer.Infrastructure.Data;

public class FormulaAnalyzerContext : DbContext
{
    public FormulaAnalyzerContext(DbContextOptions<FormulaAnalyzerContext> options) : base(options)
    {
    }

    // Parameterless constructor for design-time (EF migrations)
    public FormulaAnalyzerContext()
    {
    }

    public DbSet<Formula> Formulas { get; set; }
    public DbSet<RawMaterial> RawMaterials { get; set; }
    public DbSet<FormulaComponent> FormulaComponents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Formula entity
        modelBuilder.Entity<Formula>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.TotalWeight).HasPrecision(18, 2);
            entity.Property(e => e.TotalCost).HasPrecision(18, 2);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure RawMaterial entity
        modelBuilder.Entity<RawMaterial>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PricePerKg).HasPrecision(18, 2);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure FormulaComponent entity
        modelBuilder.Entity<FormulaComponent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.WeightInGrams).HasPrecision(18, 2);
            entity.Property(e => e.Cost).HasPrecision(18, 2);

            // Configure relationships
            entity.HasOne(e => e.Formula)
                  .WithMany(f => f.Components)
                  .HasForeignKey(e => e.FormulaId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.RawMaterial)
                  .WithMany(rm => rm.FormulaComponents)
                  .HasForeignKey(e => e.RawMaterialId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }
}
