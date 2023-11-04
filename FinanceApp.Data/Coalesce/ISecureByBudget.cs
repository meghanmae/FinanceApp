using System.Linq.Expressions;

namespace FinanceApp.Data.Coalesce;
internal interface ISecureByBudget<T>
{
    Expression<Func<T, int>> GetBudget();
}
