using FinanceApp.Data.Models;
using System.Security.Claims;

namespace FinanceApp.Data.Security;
public class AppDataSource<T, TDbContext> : StandardDataSource<T, TDbContext>
    where T : class
    where TDbContext : AppDbContext
{
    public AppDataSource(CrudContext<TDbContext> context) : base(context) { }

    public override IQueryable<T> GetQuery(IDataSourceParameters parameters)
    {
        return QueryableExtensions.WhereBudgetMatches_Internal(base.GetQuery(parameters), User!.BudgetId());
    }
}

public class FinanceAppDataSource<T> : AppDataSource<T, AppDbContext>
    where T : class
{
    public FinanceAppDataSource(CrudContext<AppDbContext> context) : base(context) { }
}

public static class QueryableExtensions
{
    internal static IQueryable<T> WhereBudgetMatches_Internal<T>(IQueryable<T> query, int budgetId)
        where T : class
    {
        if (typeof(T).GetInterface("IHaveBudgetFk") != null)
        {
            return WhereBudgetMatches(query as dynamic, budgetId);
        }

        return query;
    }

    public static IQueryable<ApplicationUser> WhereBudgetMatches(this IQueryable<ApplicationUser> query, int budgetId)
        => query.Where(u => u.BudgetUsers.Any(b => b.BudgetId == budgetId));

    public static IQueryable<T> WhereBudgetMatches<T>(this IQueryable<T> query, int budgetId)
        where T : IBudgeted
        => T.WhereBudgetMatches(query, budgetId);

    public static IQueryable<T> WhereBudgetMatches<T>(this IQueryable<T> query, ClaimsPrincipal user)
        where T : IBudgeted
        => query.WhereBudgetMatches(user.BudgetId());
}
