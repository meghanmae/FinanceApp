using FinanceApp.Data.Coalesce;
using System.Linq.Expressions;

namespace FinanceApp.Data.Models;
public class CustomCalculation : ISecureByBudget<CustomCalculation>
{
    Expression<Func<CustomCalculation, int>> ISecureByBudget<CustomCalculation>.GetBudget() => x => x.BudgetId;

    public int CustomCalculationId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public int BudgetId { get; set; }
    public Budget? Budget { get; set; }

    public ICollection<SubCategoryCustomCalculation> SubCategoryCustomCalculations { get; set; } = new List<SubCategoryCustomCalculation>();
}
