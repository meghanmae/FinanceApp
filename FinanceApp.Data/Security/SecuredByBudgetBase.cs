using FinanceApp.Data.Models;

namespace FinanceApp.Data.Security;
public abstract class SecuredByBudgetBase : ISecuredByBudget
{
    [InternalUse]
    [Required]
    public int BudgetId { get; set; }

    [InternalUse]
    public Budget? Budget { get; set; }
}
