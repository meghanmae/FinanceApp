using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;

[Delete(SecurityPermissionLevels.DenyAll)]
public class CustomCalculation : BudgetBase
{
    public int CustomCalculationId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<SubCategoryCustomCalculation> SubCategoryCustomCalculations { get; set; } = new List<SubCategoryCustomCalculation>();

    [DefaultDataSource]
    public class CustomCalculationsByBudget(CrudContext<AppDbContext> context) : FinanceAppDataSource<CustomCalculation>(context)
    {
        public override IQueryable<CustomCalculation> GetQuery(IDataSourceParameters parameters)
        {
            return base.GetQuery(parameters);
        }
    }
}
