using FinanceApp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FinanceApp.Data.Security
{
    public class FinanceAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public FinanceAppClaimsPrincipalFactory(
            AppDbContext db,
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> options,
            IHttpContextAccessor httpContextAccessor
        ) : base(userManager, options)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            user = await _db.ApplicationUsers
                .Include(u => u.BudgetUsers)
                .SingleAsync(f => f.ApplicationUserId == user.ApplicationUserId);

            // Parse the budget ID from the URL
            var displayUrl = _httpContextAccessor.HttpContext?.Request.GetDisplayUrl();
            Regex budgetUrlRegex = new Regex(@"\/budgets\/(\d+)");
            Match match = budgetUrlRegex.Match(displayUrl ?? "");
            int budgetId = -1;
            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, out budgetId);
            }

            var budget = _db.Budgets.Where(b => b.BudgetId == budgetId);
            if (budget is not null)
            {
                identity.AddClaim(new Claim(nameof(Budget.BudgetId), budgetId.ToString()));
                // TODO: Set budget permissions here - eventually
            }

            return identity;
        }
    }
}
