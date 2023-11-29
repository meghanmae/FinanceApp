using FinanceApp.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Tests;
public class SqlTestDb : IDisposable
{
    public AppDbContext DbContext { get; set; }
    public DbContextOptions<AppDbContext> DbContextOptions { get; }
    public SqliteConnection SqliteConnection { get; set; }

    public SqlTestDb()
    {
        SqliteConnection = new SqliteConnection("DataSource=:memory:");
        SqliteConnection.Open();
        DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(SqliteConnection)
            .Options;
        DbContext = new(DbContextOptions);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DbContext.Dispose();
            SqliteConnection.Close();
            SqliteConnection.Dispose();
        }
    }
}
