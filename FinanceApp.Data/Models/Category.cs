using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

[Delete(SecurityPermissionLevels.DenyAll)]
public class Category : BudgetBase
{
    public int CategoryId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public required string Color { get; set; }

    [Required]
    public required string Icon { get; set; }

    public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

    [DefaultDataSource]
    public class CategoriesByBudget : FinanceAppDataSource<Category>
    {
        public CategoriesByBudget(CrudContext<AppDbContext> context) : base(context) { }

        public override IQueryable<Category> GetQuery(IDataSourceParameters parameters)
        {
            return base.GetQuery(parameters);
        }
    }

    public class Behaviors : FinanceAppBehaviors<Category>
    {
        public Behaviors(CrudContext<AppDbContext> context) : base(context) { }

        public override async Task<ItemResult> BeforeSaveAsync(int budgetId, SaveKind kind, Category? oldItem, Category item)
        {
            return await base.BeforeSaveAsync(budgetId, kind, oldItem, item);
        }
    }
}
