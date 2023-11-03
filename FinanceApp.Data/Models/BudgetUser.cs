namespace FinanceApp.Data.Models;
public class BudgetUser
{
    public int BudgetUserId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser? ApplicationUser { get; set; }

    [Required]
    public int BudgetId { get; set; }
    public Budget? Budget { get; set; }
}
