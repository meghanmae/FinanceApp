using FinanceApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Data.Security;

/// <summary>
/// The model is parented to a budget and has a FK to that budget.
/// This inclues budget infrastructure, which isn't filtered when queried by a site admin.
/// </summary>
public interface IHaveBudgetFk
{
    public int BudgetId { get; set; }  
    public Budget? Budget { get; set; }
}

/// <summary>
/// The model belongs to a budget and should be filtered to the budget of the current user/HTTP request.
/// </summary>
public interface IBudgeted : IHaveBudgetFk
{
    static virtual IQueryable<T> WhereTenantMatches<T>(IQueryable<T> query, int budgetId)
        where T : IBudgeted
        => query.Where(x => x.BudgetId == budgetId);
}
