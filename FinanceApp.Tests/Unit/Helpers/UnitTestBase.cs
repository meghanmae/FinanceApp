using FinanceApp.Data;
using FinanceApp.Data.Models;
using FinanceApp.Data.Security;
using IntelliTect.Coalesce;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.AutoMock;
using System.Security.Claims;

namespace FinanceApp.Tests.Unit.Helpers;
public class UnitTestBase : IDisposable
{
    protected AppDbContext Db
    {
        get => DbFixture.DbContext;
        set => DbFixture.DbContext = Db;
    }

    protected DbContextOptions<AppDbContext> Options
    {
        get => DbFixture.DbContextOptions;
    }

    public CrudContext<AppDbContext> Context { get; set; }
    private SqlTestDb DbFixture { get; }
    protected AutoMocker Mocker { get; } = new AutoMocker();

    protected UnitTestBase()
    {
        DbFixture = new();
        Context = new CrudContext<AppDbContext>(
            Db,
            () => new ClaimsPrincipal()
        );
        Mocker.Use(Db);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        DbFixture.Dispose();
        if (disposing) {
            DbFixture.Dispose();
        }
    }

    protected void SetUserToContext(ApplicationUser applicationUser)
    {
        ClaimsPrincipal claimsPrincipal = new();
        claimsPrincipal.GetAndApplyUserClaims(applicationUser);

        Context = new CrudContext<AppDbContext>
        (
            Db,
            () => claimsPrincipal
        );
    }

    private static void setRequestUrl(HttpRequest httpRequest, string url)
    {
        UriHelper
          .FromAbsolute(url, out var scheme, out var host, out var path, out var query,
            fragment: out var _);

        httpRequest.Scheme = scheme;
        httpRequest.Host = host;
        httpRequest.Path = path;
        httpRequest.QueryString = query;
    }
}
