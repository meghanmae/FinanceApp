using FinanceApp.Data.Models;

namespace FinanceApp.Data.Security;

public interface ISecuredByBudget
{
    public int BudgetId { get; set; }
    public Budget? Budget { get; set; }

    static virtual IQueryable<T> WhereBudgetMatches<T>(IQueryable<T> query, IQueryable<int> budgetIds)
    where T : ISecuredByBudget
    => query.Where(x => budgetIds.Contains(x.BudgetId));

    static virtual IQueryable<T> WhereBudgetMatches<T>(IQueryable<T> query, int budgetId)
    where T : ISecuredByBudget
    => query.Where(x => x.BudgetId == budgetId);
}

