using FinanceApp.Data.Security;
using IntelliTect.Coalesce.Models;
using System.Security.Claims;

namespace FinanceApp.Data.Services;

[Coalesce, Service]
public class TransactionsService(AppDbContext db)
{
    [Coalesce]
    [Execute(PermissionLevel = SecurityPermissionLevels.AllowAuthorized)]
    public ItemResult<IEnumerable<MonthlyTransactionsDto>> HistoricalTransactions(ClaimsPrincipal claim, int budgetId, int years = 3)
    {
        // Make sure the user is allowed to query transactions for the provided budget id
        if (!db.BudgetUsers.Any(x => x.BudgetId == budgetId && x.ApplicationUserId == claim.UserId()))
        {
            return "You do not have permission to query information for this budget";
        }

        DateOnly today = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        DateOnly past = today.AddYears(-years);

        IEnumerable<MonthlyTransactionsDto> transactions = db.Transactions
            .Where(t => t.TransactionDate >= past)
            .ToList()
            .GroupBy(
                t => new { t.SubCategoryId },
                t => t)
            .Select(group1 => new MonthlyTransactionsDto()
            {
                StartOfMonth = group1.GroupBy(
                    transaction => new
                    {
                        transaction.TransactionDate.Month,
                        transaction.TransactionDate.Year
                    })
                    .Select(group2 => new DateOnly(group2.Key.Year, group2.Key.Month, 1))
                    .ToList(),
                Amount = group1.GroupBy(
                    transaction => new
                    {
                        transaction.TransactionDate.Month,
                        transaction.TransactionDate.Year
                    },
                    transaction => transaction.Amount)
                    .Select(group2 => group2.Sum())
                    .ToList(),
                SubCategoryName = db.SubCategories.First(x => x.SubCategoryId == group1.Key.SubCategoryId).Name,
                CategoryColor = db.SubCategories.Include(x => x.Category).First(x => x.SubCategoryId == group1.Key.SubCategoryId).Category!.Color,
            });

        return new ItemResult<IEnumerable<MonthlyTransactionsDto>>()
        {
            Object = transactions,
            WasSuccessful = true
        };

    }
}

public class MonthlyTransactionsDto()
{
    public required List<DateOnly> StartOfMonth { get; set; }
    public required List<decimal> Amount { get; set; }
    public required string SubCategoryName { get; set; }
    public required string CategoryColor { get; set; }

}
