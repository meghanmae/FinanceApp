using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

public class Category : SecuredByBudgetBase
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
    public class CategoriesByBudget(CrudContext<AppDbContext> context) : SecureByBudgetDataSource<Category, AppDbContext>(context)
    {
        public override IQueryable<Category> GetQuery(IDataSourceParameters parameters)
        {
            return base.GetQuery(parameters);
        }
    }

    public class CategoryBehaviors(CrudContext<AppDbContext> context) : StandardBehaviors<Category, AppDbContext>(context)
    {
        public override ItemResult BeforeDelete(Category item)
        {
            // Remove assoicated things, like sub categories and transactions
            Db.SubCategories.RemoveRange(Db.SubCategories.Where(x => x.CategoryId == item.CategoryId));
            Db.Transactions.RemoveRange(Db.Transactions.Where(x => x.SubCategory!.CategoryId == item.CategoryId));
            Db.SubCategoryCustomCalculations.RemoveRange(Db.SubCategoryCustomCalculations.Where(x => x.SubCategory!.CategoryId == item.CategoryId));

            return base.BeforeDelete(item);
        }
    }
}
