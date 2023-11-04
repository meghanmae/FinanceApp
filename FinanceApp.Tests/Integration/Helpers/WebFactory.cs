using FinanceApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Tests.Integration.Helpers;
public sealed class WebFactory : WebApplicationFactory<Program>
{
    private SqliteConnection Connection { get; }

    public WebFactory()
    {
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();
    }

    // Allows us to have a fresh DB for every test method
    private readonly string _dbName = Guid.NewGuid().ToString();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Set up Sqlite DB connection
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            // Ensure the database is created
            ServiceProvider sp = services.BuildServiceProvider();
            using IServiceScope scope = sp.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            AppDbContext db = scopedServices.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }

    public AppDbContext GetDb()

    {
        IServiceScope scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            base.Dispose(disposing);
        }
    }
}
