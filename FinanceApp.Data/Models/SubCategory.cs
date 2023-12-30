using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

public class SubCategory : SecuredByBudgetBase
{
    public int SubCategoryId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public required decimal Allocation { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    /// <summary>
    /// A category that would not have transactions assoicated with it
    /// </summary>
    public bool IsStatic { get; set; }

    public ICollection<SubCategoryCustomCalculation> SubCategoryCustomCalculations { get; set; } = new List<SubCategoryCustomCalculation>();

    [DefaultDataSource]
    public class SubCategoriesByBudget(CrudContext<AppDbContext> context) : SecureByBudgetDataSource<SubCategory, AppDbContext>(context)
    {
        [Coalesce]
        public int? CategoryId { get; set; } = null;

        public override IQueryable<SubCategory> GetQuery(IDataSourceParameters parameters)
        {
            var query = base.GetQuery(parameters);
            if (CategoryId is not null)
            {
                query = query.Where(x => x.CategoryId == CategoryId);
            }
            return query;
        }
    }

    public class SubCategoryBehaviors(CrudContext<AppDbContext> context) : StandardBehaviors<SubCategory, AppDbContext>(context)
    {
        public override ItemResult BeforeDelete(SubCategory item)
        {
            // Remove assoicated things, like transactions
            Db.Transactions.RemoveRange(Db.Transactions.Where(x => x.SubCategoryId == item.SubCategoryId));
            Db.SubCategoryCustomCalculations.RemoveRange(Db.SubCategoryCustomCalculations.Where(x => x.SubCategoryId == item.SubCategoryId));

            return base.BeforeDelete(item);
        }
    }
}
