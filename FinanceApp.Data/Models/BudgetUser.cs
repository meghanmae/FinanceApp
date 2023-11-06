namespace FinanceApp.Data.Models;

[Index(nameof(ApplicationUserId), nameof(BudgetId), IsUnique = true)]
[Create(SecurityPermissionLevels.DenyAll)]
[Edit(SecurityPermissionLevels.DenyAll)]
[Read(SecurityPermissionLevels.DenyAll)]
[Delete(SecurityPermissionLevels.DenyAll)]
public class BudgetUser : BudgetInfrastructureBase
{
    public int BudgetUserId { get; set; }

    [Required]
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser? ApplicationUser { get; set; }
}
