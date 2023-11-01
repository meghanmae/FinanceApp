using FinanceApp.Data.Helpers;
using FinanceApp.Data.Models;
using IntelliTect.Coalesce.Utilities;
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
