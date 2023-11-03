namespace FinanceApp.Data.Models;
public class SubCategoryCustomCalculation
{
    public int SubCategoryCustomCalculationId { get; set; }

    [Required]
    public int SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    [Required]
    public int CustomCalculationId { get; set; }
    public CustomCalculation? CustomCalculation { get; set; }
}
