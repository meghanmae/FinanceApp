using FinanceApp.Data.Coalesce;
using FinanceApp.Data.Helpers;
using System.Linq.Expressions;

namespace FinanceApp.Data.Models;
public class Budget : ISecureByBudget<Budget>
{
    Expression<Func<Budget, int>> ISecureByBudget<Budget>.GetBudget() => x => x.BudgetId;

    public int BudgetId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<BudgetUser> BudgetUsers { get; set; } = new List<BudgetUser>();

    public ICollection<Category> Categories { get; set; } = new List<Category>();

    [DefaultDataSource]
    public class BudgetsForUser : FinanceAppDataSource<Budget, AppDbContext>
    {
        public BudgetsForUser(CrudContext<AppDbContext> context) : base(context) { }

        public override IQueryable<Budget> GetQuery(IDataSourceParameters parameters)
        {
            // Only return budgets that this user is a member of
            return Db.BudgetUsers
                .Include(x => x.Budget)
                .Where(x => x.ApplicationUserId == Context.User.UserId())
                // We use `!` here because we've specified that we are including the budget nav property above,
                // so this should never be null since it's FK relationship is required on the BudgetUser model.
                .Select(x => x.Budget!); 
        }
    }

    public class BudgetBehaviors : StandardBehaviors<Budget, AppDbContext>
    {
        public BudgetBehaviors(CrudContext<AppDbContext> context) : base(context) { }

        public override ItemResult BeforeSave(SaveKind kind, Budget? oldItem, Budget item)
        {
            // Make sure the calling user is allowed to update this budget
            // Only users assigned to the budget may do so
            if (kind == SaveKind.Update && !Db.BudgetUsers.Where(x => x.BudgetId == item.BudgetId && x.ApplicationUserId == Context.User.UserId()).Any())
            {
                return "You are not allowed to modify this budget";
            }

            return base.BeforeSave(kind, oldItem, item);
        }

        public override ItemResult BeforeDelete(Budget item)
        {
            // Make sure the calling user is allowed to delete this budget
            // Only users assigned to the budget may do so
            if(!Db.BudgetUsers.Where(x => x.BudgetId == item.BudgetId && x.ApplicationUserId == Context.User.UserId()).Any())
            {
                return "You are not allowed to delete this budget";
            }

            return base.BeforeDelete(item);
        }
    }
}
