using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

[Create(SecurityPermissionLevels.AllowAuthorized)]
[Edit(SecurityPermissionLevels.AllowAuthorized)]
[Read(SecurityPermissionLevels.AllowAuthorized)]
[Delete(SecurityPermissionLevels.AllowAuthorized)]
public class Budget
{
    public int BudgetId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Color { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Allocation { get; set; }

    public string? Description { get; set; }

    public ICollection<BudgetUser> BudgetUsers { get; set; } = new List<BudgetUser>();

    public ICollection<Category> Categories { get; set; } = new List<Category>();

    [DefaultDataSource]
    public class BudgetsForUser(CrudContext<AppDbContext> context) : StandardDataSource<Budget, AppDbContext>(context)
    {
        public override IQueryable<Budget> GetQuery(IDataSourceParameters parameters)
        {
            // Only return budgets that this user is a member of
            return Db.BudgetUsers
                .Include(x => x.Budget)
                .Where(x => x.ApplicationUserId == Context.User!.UserId())
                // We use `!` here because we've specified that we are including the budget nav property above,
                // so this should never be null since it's FK relationship is required on the BudgetUser model.
                .Select(x => x.Budget!);
        }
    }

    public class BudgetBehaviors(CrudContext<AppDbContext> context) : StandardBehaviors<Budget, AppDbContext>(context)
    {
        public override ItemResult BeforeSave(SaveKind kind, Budget? oldItem, Budget item)
        {
            // Make sure the calling user is allowed to update this budget
            // Only users assigned to the budget may do so
            if (kind == SaveKind.Update && !Db.BudgetUsers.Where(x => x.BudgetId == item.BudgetId && x.ApplicationUserId == Context.User.UserId()).Any())
            {
                return "You are not allowed to modify this budget";
            }

            // Set up the BudgetUser for a newly created budget
            if (kind == SaveKind.Create)
            {
                BudgetUser budgetUser = new()
                {
                    Budget = item,
                    ApplicationUserId = Context.User!.UserId()
                };
                Db.BudgetUsers.Add(budgetUser);
            }

            return base.BeforeSave(kind, oldItem, item);
        }

        public override ItemResult BeforeDelete(Budget item)
        {
            // Make sure the calling user is allowed to delete this budget
            // Only users assigned to the budget may do so
            if (!Db.BudgetUsers.Where(x => x.BudgetId == item.BudgetId && x.ApplicationUserId == Context.User!.UserId()).Any())
            {
                return "You are not allowed to delete this budget";
            }

            // Remove associated budget users
            Db.BudgetUsers.RemoveRange(Db.BudgetUsers.Where(x => x.BudgetId == item.BudgetId));

            // Remove assoicated things, like categories
            Db.Categories.RemoveRange(Db.Categories.Where(x => x.BudgetId == item.BudgetId));
            Db.CustomCalculations.RemoveRange(Db.CustomCalculations.Where(x => x.BudgetId == item.BudgetId));
            Db.SubCategories.RemoveRange(Db.SubCategories.Where(x => x.BudgetId == item.BudgetId));
            Db.SubCategoryCustomCalculations.RemoveRange(Db.SubCategoryCustomCalculations.Where(x => x.BudgetId == item.BudgetId));
            Db.Transactions.RemoveRange(Db.Transactions.Where(x => x.BudgetId == item.BudgetId));

            return base.BeforeDelete(item);
        }
    }
}
