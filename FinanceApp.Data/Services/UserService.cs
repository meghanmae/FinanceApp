using FinanceApp.Data.Models;
using FinanceApp.Data.Security;
using System.Security.Claims;
namespace FinanceApp.Data.Services;

[Coalesce, Service]
public class UserService
{
    [Coalesce]
    public ApplicationUser GetLoggedInUser(ClaimsPrincipal claim, [Inject] AppDbContext db)
    {
        return db.ApplicationUsers.Single(user => user.ApplicationUserId == claim.UserId());
    }
}
