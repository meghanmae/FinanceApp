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

    public class ApplicationUserBehaviors : StandardBehaviors<ApplicationUser, AppDbContext>
    {
        public ApplicationUserBehaviors(CrudContext<AppDbContext> context) : base(context) { }

        public override ItemResult BeforeSave(SaveKind kind, ApplicationUser? oldItem, ApplicationUser item)
        {
            var claimsInfo = Context.User?.Claims;

            return base.BeforeSave(kind, oldItem, item);
        }
    }

}
