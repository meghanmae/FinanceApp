using FinanceApp.Data;
using FinanceApp.Data.Helpers;
using FinanceApp.Data.Models;
using IntelliTect.Coalesce;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceApp.Tests.Unit;
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

    protected UnitTestBase()
    {
        DbFixture = new();
        Context = new CrudContext<AppDbContext>(
            Db,
            () => new ClaimsPrincipal()
        );
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        DbFixture.Dispose();
        if(disposing) { }
    }

    protected void SetUserToContext(ApplicationUser applicationUser, int[]? managedProjectIds = null)
    {
        ClaimsPrincipal claimsPrincipal = new();
        claimsPrincipal.GetAndApplyUserClaims(applicationUser);

        Context = new CrudContext<AppDbContext>
        (
            Db,
            () => claimsPrincipal
        );
    }
}
