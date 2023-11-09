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

    public static int BudgetId(this ClaimsPrincipal user)
    {
        return int.Parse(user.Claims.FirstOrDefault(c => c.Type == nameof(Budget.BudgetId))?.Value ?? "-1");
    }

    public static ClaimsPrincipal GetNewClaimsPrincipal(this ClaimsPrincipal user, ApplicationUser appUser, IHttpContextAccessor httpContextAccessor)
    {
        ClaimsIdentity? microsoftIdentity = user.Identities.ToList().Find(i => i.AuthenticationType == "AuthenticationTypes.Federation");

        ClaimsPrincipal newClaims = new();
        newClaims.GetAndApplyUserClaims(appUser, httpContextAccessor);

        if (microsoftIdentity is not null)
        {
            newClaims.AddIdentity(microsoftIdentity);
        }

        return newClaims;
    }

    public static List<Claim> GetAndApplyUserClaims(this ClaimsPrincipal user, ApplicationUser applicationUser, IHttpContextAccessor httpContextAccessor)
    {
        List<Claim> claims = new()
        {
            new Claim(nameof(ApplicationUser.Name), applicationUser.Name),
            new Claim(nameof(ApplicationUser.Email), applicationUser.Email),
            new Claim(nameof(ApplicationUser.ApplicationUserId), applicationUser.ApplicationUserId),
            new Claim(nameof(ApplicationUser.AzureObjectId), applicationUser.AzureObjectId),
        };

        // Add budget claims
        var displayUrl = httpContextAccessor.HttpContext?.Request.GetDisplayUrl();
        Regex budgetUrlRegex = new Regex(@"\/budgets\/(\d+)");
        Match match = budgetUrlRegex.Match(displayUrl ?? "");
        if (match.Success)
        {
            int.TryParse(match.Groups[1].Value, out var budgetId);
            claims.Add(new Claim(nameof(Budget.BudgetId), budgetId.ToString()));
            // TODO: Set budget permissions here - eventually
        }

        user.AddIdentity(new(claims.ToList().AsEnumerable(), "FinanceApp"));

        return claims;
    }
}
