using FinanceApp.Data;
using FinanceApp.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq.AutoMock;

namespace FinanceApp.Tests.Integration.Helpers;
public class IntegrationTestsBase : IDisposable
{
    private bool disposedValue;

    protected WebFactory WebFactory { get; private set; }

    protected AppDbContext Db { get; }

    protected AutoMocker Mocker { get; } = new AutoMocker();

    protected IntegrationTestsBase()
    {
        WebFactory = new WebFactory();
        Db = WebFactory.GetDb();
        Mocker.Use(Db);
    }

    public void ResetContext()
    {
        WebFactory = new WebFactory();
    }

    /// <summary>
    /// Client with no signed-in user
    /// </summary>
    protected HttpClient AnnoymousClient
    {
        get
        {
            HttpClient client = WebFactory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => ConfigureServices(services)))
                .CreateClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", "XmlHttpRequest");
            return client;
        }
    }

    /// <summary>
    /// Client with a signed-in user 
    /// </summary>
    protected HttpClient GetAuthClient(ApplicationUser? appUser = null, int? budgetId = null)
    {
        return WebFactory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("FakeAuth")
                        .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(
                            "FakeAuth",
                            options =>
                            {
                                options.AppUser = appUser;
                                options.BudgetId = budgetId;
                            }
                        );
                    ConfigureServices(services);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        // Override in test classes to put mocks into the service container
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                WebFactory.Dispose();
            }

            disposedValue = true;
        }
    }

    ~IntegrationTestsBase()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
