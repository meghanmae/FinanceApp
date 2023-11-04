using FinanceApp.Data.Coalesce;
using System.Linq.Expressions;

namespace FinanceApp.Data.Models;
public class BudgetUser : ISecureByBudget<BudgetUser>
{
    Expression<Func<BudgetUser, int>> ISecureByBudget<BudgetUser>.GetBudget() => x => x.BudgetId;

    public int BudgetUserId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser? ApplicationUser { get; set; }

    [Required]
    public int BudgetId { get; set; }
    public Budget? Budget { get; set; }
}
