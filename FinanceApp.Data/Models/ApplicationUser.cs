namespace FinanceApp.Data.Models;

#nullable disable

public class ApplicationUser
{
    public int ApplicationUserId { get; set; }

    [Required]
    public string Name { get; set; }

#nullable restore

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
