using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FinanceApp.Data.Security;
public class RefreshClaimsMiddleware
{
    private readonly RequestDelegate _next;

    public RefreshClaimsMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context, AppDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        var principal = context.User;
        await RefreshClaimsAsync(principal, db, httpContextAccessor);

        await _next(context);
    }

    private async Task RefreshClaimsAsync(ClaimsPrincipal principal, AppDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        var appUser = await db.ApplicationUsers.FirstAsync(x => x.ApplicationUserId == principal.UserId());
        principal.GetAndApplyUserClaims(appUser, httpContextAccessor);
    }
}

public static class RefreshClaimsMiddlewareExtensions
{
    public static IApplicationBuilder UseRefreshClaimsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RefreshClaimsMiddleware>();
    }
}
