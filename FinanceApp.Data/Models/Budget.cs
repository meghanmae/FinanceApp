namespace FinanceApp.Data.Models;
public class Budget
{
    public int BudgetId { get; set; }

    [Required]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<BudgetUser> BudgetUsers { get; set; } = new List<BudgetUser>();

    public ICollection<Category> Categories { get; set; } = new List<Category>();

}
