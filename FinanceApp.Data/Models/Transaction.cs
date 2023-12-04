using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

[Delete(SecurityPermissionLevels.DenyAll)]
public class Transaction : SecuredByBudgetBase
{
    public long TransactionId { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Amount { get; set; }

    [Required]
    public int SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    [DefaultDataSource]
    public class TransactionsByBudget(CrudContext<AppDbContext> context) : SecureByBudgetDataSource<Transaction, AppDbContext>(context)
    {
        public override IQueryable<Transaction> GetQuery(IDataSourceParameters parameters)
        {
            return base.GetQuery(parameters);
        }
    }
}
