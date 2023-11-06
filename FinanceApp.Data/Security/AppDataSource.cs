using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Data.Security;
public class AppDataSource<T, TDbContext> : StandardDataSource<T, TDbContext>
    where T : class
    where TDbContext : AppDbContext
{
    public AppDataSource(CrudContext<TDbContext> context) : base(context) { }

    public override IQueryable<T> GetQuery(IDataSourceParameters parameters)
    {
        return QueryableExtensions.WhereBudgetMatches_Internal(base.GetQuery(parameters), User!.)
    }
}
