using Microsoft.EntityFrameworkCore;
using FinanceApp.Data.Models;

namespace FinanceApp.Data;

[Coalesce]
public class AppDbContext : DbContext
{
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<BudgetUser> BudgetUsers => Set<BudgetUser>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CustomCalculation> CustomCalculations => Set<CustomCalculation>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<SubCategoryCustomCalculation> SubCategoryCustomCalculations => Set<SubCategoryCustomCalculation>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Remove cascading deletes.
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
