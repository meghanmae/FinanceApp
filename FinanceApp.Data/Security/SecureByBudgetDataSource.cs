namespace FinanceApp.Data.Security;
public class SecureByBudgetDataSource<T, TDbContext>(CrudContext<TDbContext> context) : StandardDataSource<T, TDbContext>(context) 
    where T : class
    where TDbContext : AppDbContext
{
    [Coalesce]
    public int? BudgetId { get; set; } = null;

    public override IQueryable<T> GetQuery(IDataSourceParameters parameters)
    {
        if (typeof(T).GetInterface(nameof(ISecuredByBudget)) is not null)
        {
            // If there is no provided BudgetId, return all resources for all of the user's budgets
            if (BudgetId is null)
            {
                IQueryable<int> userBudgetIds = Context.DbContext.BudgetUsers
                    .Where(x => x.ApplicationUserId == Context.User.UserId())
                    .Select(x => x.BudgetId);

                return QueryableExtensions.WhereBudgetMatches(base.GetQuery(parameters) as dynamic, userBudgetIds);
            }
            // If a BudgetId is provided, ensure the user is allowed to access it before querying by it
            if (BudgetId is not null && Context.DbContext.BudgetUsers.Any(x => x.BudgetId == BudgetId && x.ApplicationUserId == Context.User.UserId()))
            {
                return QueryableExtensions.WhereBudgetMatches(base.GetQuery(parameters) as dynamic, (int)BudgetId);
            }

            // If all else fails, return an empty queryable
            return Enumerable.Empty<T>().AsQueryable();
        }
        
        return base.GetQuery(parameters);
    }
}

public static class QueryableExtensions
{
    public static IQueryable<T> WhereBudgetMatches<T>(this IQueryable<T> query, IQueryable<int> budgetIds)
    where T : ISecuredByBudget
        => T.WhereBudgetMatches(query, budgetIds);

    public static IQueryable<T> WhereBudgetMatches<T>(this IQueryable<T> query, int budgetId)
    where T : ISecuredByBudget
        => T.WhereBudgetMatches(query, budgetId);
}