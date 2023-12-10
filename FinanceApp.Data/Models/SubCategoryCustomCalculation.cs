using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

[Create(SecurityPermissionLevels.DenyAll)]
[Edit(SecurityPermissionLevels.DenyAll)]
[Read(SecurityPermissionLevels.DenyAll)]
[Delete(SecurityPermissionLevels.DenyAll)]
public class SubCategoryCustomCalculation : SecuredByBudgetBase
{
    public int SubCategoryCustomCalculationId { get; set; }

    [Required]
    public int SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    [Required]
    public int CustomCalculationId { get; set; }
    public CustomCalculation? CustomCalculation { get; set; }

    [DefaultDataSource]
    public class SubCategoriesByBudget(CrudContext<AppDbContext> context) : SecureByBudgetDataSource<SubCategoryCustomCalculation, AppDbContext>(context)
    {
        public override IQueryable<SubCategoryCustomCalculation> GetQuery(IDataSourceParameters parameters)
        {
            return base.GetQuery(parameters);
        }
    }
}
