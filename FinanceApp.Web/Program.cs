using FinanceApp.Data;
using FinanceApp.Data.Models;
using FinanceApp.Data.Security;
using FinanceApp.Data.Services;
using IntelliTect.Coalesce;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Explicit declaration prevents ASP.NET Core from erroring if wwwroot doesn't exist at startup:
    WebRootPath = "wwwroot"
});

var services = builder.Services;

builder.Logging
    .AddConsole()
    // Filter out Request Starting/Request Finished noise:
    .AddFilter<ConsoleLoggerProvider>("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>(true)
    .AddEnvironmentVariables();

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ??
    builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container
services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.GetSection("AzureAd").Bind(options);

        options.Events.OnTicketReceived = (TicketReceivedContext trc) =>
        {
            // Create a new app user for the logging in user
            AppDbContext db = trc.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            var azureObjectId = trc.Principal?.Identities.FirstOrDefault()?.Claims.FirstOrDefault(claim => claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            if (string.IsNullOrWhiteSpace(azureObjectId))
            {
                trc.Fail("Invalid login, an azure object id is required");
                return Task.CompletedTask;
            }

            var appUser = db.ApplicationUsers.FirstOrDefault(appUser => appUser.AzureObjectId == azureObjectId);
            if (appUser is null)
            {
                // Create a new user
                var name = trc.Principal?.Identities.First().Claims.First(claim => claim.Type == "name").Value
                    ?? throw new InvalidOperationException("Principal first name is unexpectedly null");

                var email = trc.Principal?.Identity?.Name
                    ?? throw new InvalidOperationException("Principal email is unexpectedly null");

                appUser = new ApplicationUser()
                {
                    Name = name,
                    Email = email,
                    AzureObjectId = azureObjectId
                };

                db.ApplicationUsers.Add(appUser);
                db.SaveChanges();
            }

            trc.Principal = trc.Principal?.GetNewClaimsPrincipal(appUser);

            trc.Success();
            Console.WriteLine($"Successfully Logged in user: {appUser.Name}");

            return Task.CompletedTask;
        };

    })
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
    .AddInMemoryTokenCaches();

services.AddRazorPages().AddMicrosoftIdentityUI();

services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according the the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});


#region Configure Services
services.AddSwaggerGen();

services.AddDbContext<AppDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), opt => opt
        .EnableRetryOnFailure()
        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    )
    // Ignored because it interferes with the construction of Coalesce IncludeTrees via .Include()
    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored))
);

services.AddCoalesce<AppDbContext>();

services.AddScoped<UserService>();

services
    .AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

#endregion



#region Configure HTTP Pipeline

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseViteDevelopmentServer(c =>
    {
        c.DevServerPort = 5002;
    });

    app.MapCoalesceSecurityOverview("coalesce-security");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger(OptionsBuilderConfigurationExtensions => OptionsBuilderConfigurationExtensions.SerializeAsV2 = true);
app.UseSwaggerUI();

var containsFileHashRegex = new Regex(@"\.[0-9a-fA-F]{8}\.[^\.]*$", RegexOptions.Compiled);
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // vite puts 8-hex-char hashes before the file extension.
        // Use this to determine if we can send a long-term cache duration.
        if (containsFileHashRegex.IsMatch(ctx.File.Name))
        {
            ctx.Context.Response.GetTypedHeaders().CacheControl =
                new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromDays(30) };
        }
    }
});

// For all requests that aren't to static files, disallow caching by default.
// Individual endpoints may override this.
app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new CacheControlHeaderValue { NoCache = true, NoStore = true, };

    await next();
});

app.MapControllers();

// API fallback to prevent serving SPA fallback to 404 hits on API endpoints.
app.Map("/api/{**any}", () => Results.NotFound());

app.MapFallbackToController("Index", "Home");

#endregion



#region Launch

// Initialize/migrate database.
using (var scope = app.Services.CreateScope())
{
    var serviceScope = scope.ServiceProvider;

    // Run database migrations.
    using var db = serviceScope.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();

#endregion

public partial class Program { }
