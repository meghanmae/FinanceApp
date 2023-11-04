using FinanceApp.Data.Coalesce;
using System.Linq.Expressions;

namespace FinanceApp.Data.Models;
public class Category : ISecureByBudget<Category>
{
    Expression<Func<Category, int>> ISecureByBudget<Category>.GetBudget() => x => x.BudgetId;

    public int CategoryId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public required string Color { get; set; }

    [Required]
    public required string Icon { get; set; }

    [Required]
    public int BudgetId { get; set; }
    public Budget? Budget { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
