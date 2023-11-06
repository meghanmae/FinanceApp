namespace FinanceApp.Data.Models;
public class SubCategory : BudgetBase
{
    public int SubCategoryId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Budget { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<SubCategoryCustomCalculation> SubCategoryCustomCalculations { get; set; } = new List<SubCategoryCustomCalculation>();
}
