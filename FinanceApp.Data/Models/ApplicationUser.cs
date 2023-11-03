namespace FinanceApp.Data.Models;

public class ApplicationUser
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ApplicationUserId { get; set; } = null!;

    [Required]
    public required string AzureObjectId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Email { get; set; }

    public ICollection<BudgetUser> Budgets { get; set; } = new List<BudgetUser>();
}
