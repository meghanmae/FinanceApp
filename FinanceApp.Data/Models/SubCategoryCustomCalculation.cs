using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

[Create(SecurityPermissionLevels.DenyAll)]
[Edit(SecurityPermissionLevels.DenyAll)]
[Read(SecurityPermissionLevels.DenyAll)]
[Delete(SecurityPermissionLevels.DenyAll)]
public class SubCategoryCustomCalculation : BudgetBase
{
    public int SubCategoryCustomCalculationId { get; set; }

    [Required]
    public int SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    [Required]
    public int CustomCalculationId { get; set; }
    public CustomCalculation? CustomCalculation { get; set; }

    [DefaultDataSource]
    public class SubCategoriesByBudget(CrudContext<AppDbContext> context) : FinanceAppDataSource<SubCategoryCustomCalculation>(context)
    {
        public override IQueryable<SubCategoryCustomCalculation> GetQuery(IDataSourceParameters parameters)
        {
            return base.GetQuery(parameters);
        }
    }
}
