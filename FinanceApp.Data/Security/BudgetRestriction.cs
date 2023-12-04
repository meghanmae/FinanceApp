using FinanceApp.Data.Models;
using System.Reflection;

namespace FinanceApp.Data.Security;

// Currently unused, but a good example for how to implement?
public class BudgetRestriction<T>(AppDbContext db) : IPropertyRestriction<T> where T : class
{
    public bool UserCanRead(IMappingContext context, string propertyName, T model)
        => GetBudgetUser(context, model) is not null;

    public bool UserCanWrite(IMappingContext context, string propertyName, T? model, object? incomingValue)
        => GetBudgetUser(context, model) is not null;

    public bool UserCanDelete(IMappingContext context, string propertyName, T model, object incomingValue)
    => GetBudgetUser(context, model) is not null;

    private BudgetUser? GetBudgetUser(IMappingContext context, T? model)
    {
        if (typeof(T).GetInterface(nameof(ISecuredByBudget)) is not null)
        {
            PropertyInfo? budgetIdProperty = typeof(T).GetProperty(nameof(ISecuredByBudget.BudgetId));
            if (budgetIdProperty is not null)
            {
                int budgetId = (int)(budgetIdProperty.GetValue(model) ?? 0);

                return db.BudgetUsers.FirstOrDefault(x => x.ApplicationUserId == context.User!.UserId() && x.BudgetId == budgetId);
            }
        }

        return null;
    }
}
