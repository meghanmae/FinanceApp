using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Data.Security;
public class AppBehaviors<T, TDbContext> : StandardBehaviors<T, TDbContext>
    where T : class
    where TDbContext : AppDbContext
{
    public AppBehaviors(CrudContext<TDbContext> context) : base(context) { }

    public sealed override ItemResult BeforeSave(SaveKind kind, T? oldItem, T item)
    {
        throw new InvalidOperationException(
            "Do not call the non-async variant of BeforeSave. Data secured by budget logic is all implemented in BeforeSaveAsync.");
    }

    public sealed override async Task<ItemResult> BeforeSaveAsync(SaveKind kind, T? oldItem, T item)
    {
        // This is sealed so that it is much less likely to accidentally bypass budget checks.
        // A different overload (with a budgetId parameter) can be used in derived classes instead.

        var currentBudgetId = User!.BudgetId();

        if (kind == SaveKind.Create && item is IBudgeted budgetedItem)
        {
            // we don't have to worry about the BudgetId being changed,
            // since the BudgetId prop on all types is [InternalUse].
            // Since it is internal, though, the client can't set it (which we don't want),
            // so we'll set it to the current budget.
            budgetedItem.BudgetId = currentBudgetId;
        }

        return await BeforeSaveAsync(currentBudgetId, kind, oldItem, item);
    }

    public virtual Task<ItemResult> BeforeSaveAsync(int budgetId, SaveKind kind, T? oldItem, T item)
    {
        return Task.FromResult(new ItemResult(true));
    }
}

public class FinanceAppBehaviors<T> : AppBehaviors<T, AppDbContext> where T : class
{
    public FinanceAppBehaviors(CrudContext<AppDbContext> context) : base(context) { }
}
