using FinanceApp.Data.Helpers;
using IntelliTect.Coalesce.TypeDefinition;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Data.Coalesce;
public class FinanceAppDataSource<T, TDbContext> : StandardDataSource<T, TDbContext>
    where T : class
    where TDbContext : AppDbContext
{
    public FinanceAppDataSource(CrudContext<TDbContext> context) : base(context) { }

    public override IQueryable<T> GetQuery(IDataSourceParameters parameters)
    {
        return WhereVisible(base.GetQuery(parameters));
    }

    public IQueryable<T> WhereVisible(IQueryable<T> query)
    {
            Expression<Func<T, int>>? expr = hasBudget.GetBudget();
            IList<int> budgets = Db.BudgetUsers.Where(x => x.ApplicationUserId == User.UserId()).Select(x => x.BudgetId).ToList();

            Expression<Func<T, bool>> predicate = x => budgets.Contains(expr.Invoke(x));

            return query.Where(predicate.Expand());
    }

    public override string GetNotFoundMessage(object id)
    {
        string? msg = base.GetNotFoundMessage(id);
            return msg.Insert(msg.Length - 1, ", or you are nto allowed to view it.");
    }
}

public class FinanceAppDataSource<T> :  FinanceAppDataSource<T, AppDbContext>
    where T: class, new()
{
    public FinanceAppDataSource(CrudContext<AppDbContext> context) : base(context) { }
}
