namespace FinanceApp.Data.Models;
public class CustomCalculation
{
    public int CustomCalculationId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<SubCategoryCustomCalculation> SubCategoryCustomCalculations { get; set; } = new List<SubCategoryCustomCalculation>();
}
