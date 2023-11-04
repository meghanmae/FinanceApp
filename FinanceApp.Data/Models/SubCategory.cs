using FinanceApp.Data.Coalesce;
using System.Linq.Expressions;

namespace FinanceApp.Data.Models;
public class SubCategory : ISecureByBudget<SubCategory>
{
    Expression<Func<SubCategory, int>> ISecureByBudget<SubCategory>.GetBudget() => x => x.Category!.BudgetId;

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
