using FinanceApp.Data.Coalesce;
using System.Linq.Expressions;

namespace FinanceApp.Data.Models;
public class SubCategoryCustomCalculation : ISecureByBudget<SubCategoryCustomCalculation>
{
    Expression<Func<SubCategoryCustomCalculation, int>> ISecureByBudget<SubCategoryCustomCalculation>.GetBudget() => x => x.SubCategory!.Category!.BudgetId;

    public int SubCategoryCustomCalculationId { get; set; }

    [Required]
    public int SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    [Required]
    public int CustomCalculationId { get; set; }
    public CustomCalculation? CustomCalculation { get; set; }
}
