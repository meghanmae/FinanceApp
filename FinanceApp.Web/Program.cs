using IntelliTect.Coalesce;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using FinanceApp.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IntelliTect.Coalesce.Models;
using FinanceApp.Data.Models;
using FinanceApp.Data.Helpers;
using FinanceApp.Data.Services;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Explicit declaration prevents ASP.NET Core from erroring if wwwroot doesn't exist at startup:
    WebRootPath = "wwwroot"
});

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
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options => {
        builder.Configuration.GetSection("AzureAd").Bind(options);

        options.Events.OnRedirectToIdentityProvider = (context) =>
        {
            if ("XmlHttpRequest".Equals(context.Request.Headers.XRequestedWith, StringComparison.OrdinalIgnoreCase))
            {
                // Don't redirect AJAX/API requests. Just return a plan Unauthorized response.
                context.Response.StatusCode = 401;
                context.Response.WriteAsJsonAsync<ItemResult>("You are not signed in.");
                context.HandleResponse();
            }
            return Task.CompletedTask;
        };

        options.Events.OnTicketReceived = (TicketReceivedContext trc) =>
        {
            // Create a new app user for the logging in user
            AppDbContext db = trc.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            string? email = trc.Principal?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
            {
                trc.Fail("Invalid login, an email is required");
                return Task.CompletedTask;
            }

            var appUser = db.ApplicationUsers.FirstOrDefault(appUser => appUser.Email == email);
            if(appUser is null)
            {
                // Create a new user

                var name = trc.Principal?.Identities.First().Claims.First(claim => claim.Type == "name").Value
                    ?? throw new InvalidOperationException("Principal first name is unexpectedly null");

                appUser = new ApplicationUser()
                {
                    Name = name,
                    Email = email,
                };

                db.ApplicationUsers.Add(appUser);
                db.SaveChanges();
            }

            trc.Success();
            trc.Principal = trc.Principal.GetNewClaimsPrincipal(appUser);
            Console.WriteLine($"Successfully Logged in user: {trc.Principal!.Identity!.Name}");

            return Task.CompletedTask;
        };

    })
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
    .AddInMemoryTokenCaches();

builder.Services.AddRazorPages().AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according the the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});


#region Configure Services

builder.Services.AddSwaggerGen();

var services = builder.Services;

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
