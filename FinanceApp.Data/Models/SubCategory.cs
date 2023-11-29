using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

[Delete(SecurityPermissionLevels.DenyAll)]
public class SubCategory : BudgetBase
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

    public ICollection<SubCategoryCustomCalculation> SubCategoryCustomCalculations { get; set; } = new List<SubCategoryCustomCalculation>();

    [DefaultDataSource]
    public class SubCategoriesByBudget(CrudContext<AppDbContext> context) : FinanceAppDataSource<SubCategory>(context)
    {
        public override IQueryable<SubCategory> GetQuery(IDataSourceParameters parameters)
        {
            return base.GetQuery(parameters);
        }
    }
}
