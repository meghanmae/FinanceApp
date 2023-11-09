using FinanceApp.Data.Models;
using FinanceApp.Data.Security;
using Microsoft.AspNetCore.Authentication;
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
        UrlEncoder encoder
        ) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsPrincipal principal = new();
        if (Options.AppUser is not null)
        {
            principal = principal.GetNewClaimsPrincipal(Options.AppUser);
        }
        AuthenticationTicket ticket = new(principal, "FakeAuth");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public ApplicationUser? AppUser { get; set; }
}
