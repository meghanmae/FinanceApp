namespace FinanceApp.Data.Models;

[Create(SecurityPermissionLevels.DenyAll)]
[Edit(SecurityPermissionLevels.DenyAll)]
[Read(SecurityPermissionLevels.DenyAll)]
[Delete(SecurityPermissionLevels.DenyAll)]
public class ApplicationUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ApplicationUserId { get; set; } = null!;

    [Required]
    public required string AzureObjectId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Email { get; set; }

    public ICollection<BudgetUser> BudgetUsers { get; set; } = new List<BudgetUser>();
}
