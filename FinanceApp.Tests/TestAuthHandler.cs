using FinanceApp.Data.Models;
using FinanceApp.Data.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FinanceApp.Tests;
public sealed class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<TestAuthHandlerOptions> options,
        ILoggerFactory logger,
        IHttpContextAccessor httpContextAccessor,
        UrlEncoder encoder
        ) : base(options, logger, encoder)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private readonly IHttpContextAccessor _httpContextAccessor;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsPrincipal principal = new();
        if (Options.AppUser is not null)
        {
            principal = principal.GetNewClaimsPrincipal(Options.AppUser, _httpContextAccessor);
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
