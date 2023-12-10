using FinanceApp.Data.Models;
using FinanceApp.Data.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FinanceApp.Tests;
public sealed class TestAuthHandler(
        IOptionsMonitor<TestAuthHandlerOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
        ) : AuthenticationHandler<TestAuthHandlerOptions>(options, logger, encoder)
{

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsPrincipal principal = new();
        if (Options.AppUser is not null)
        {
            principal = principal.GetNewClaimsPrincipal(Options.AppUser);
        }
        // Manually tack on the budgetId we are testing with
        if (Options.BudgetId is not null)
        {
            var identity = principal.Identity as ClaimsIdentity;

            var budgetIdClaim = new Claim(nameof(Budget.BudgetId), $"{Options.BudgetId}");
            identity?.AddClaim(budgetIdClaim);
        }
        AuthenticationTicket ticket = new(principal, "FakeAuth");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public ApplicationUser? AppUser { get; set; }
    public int? BudgetId { get; set; }
}
