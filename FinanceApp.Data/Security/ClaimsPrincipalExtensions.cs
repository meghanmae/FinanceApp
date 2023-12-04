using FinanceApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FinanceApp.Data.Security;
public static class ClaimsPrincipalExtensions
{
    public static string UserId(this ClaimsPrincipal user)
    {
        return user.Claims.First(c => c.Type == nameof(ApplicationUser.ApplicationUserId)).Value;
    }

    public static ClaimsPrincipal GetNewClaimsPrincipal(this ClaimsPrincipal user, ApplicationUser appUser)
    {
        ClaimsIdentity? microsoftIdentity = user.Identities.ToList().Find(i => i.AuthenticationType == "AuthenticationTypes.Federation");

        ClaimsPrincipal newClaims = new();
        newClaims.GetAndApplyUserClaims(appUser);

        if (microsoftIdentity is not null)
        {
            newClaims.AddIdentity(microsoftIdentity);
        }

        return newClaims;
    }

    public static List<Claim> GetAndApplyUserClaims(this ClaimsPrincipal user, ApplicationUser applicationUser)
    {
        List<Claim> claims = new()
        {
            new Claim(nameof(ApplicationUser.Name), applicationUser.Name),
            new Claim(nameof(ApplicationUser.Email), applicationUser.Email),
            new Claim(nameof(ApplicationUser.ApplicationUserId), applicationUser.ApplicationUserId),
            new Claim(nameof(ApplicationUser.AzureObjectId), applicationUser.AzureObjectId),
        };

        user.AddIdentity(new(claims.ToList().AsEnumerable(), "FinanceApp"));

        return claims;
    }
}
