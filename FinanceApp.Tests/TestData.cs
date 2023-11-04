using FinanceApp.Data.Models;

namespace FinanceApp.Tests;
public static class TestData
{
    public static ApplicationUser CreateTestApplicationUser()
    {
        return new ApplicationUser()
        {
            Name = "Test User",
            Email = "Test@FinanceApp.com",
            AzureObjectId = "123abc"
        };
    }

    public static Budget CreateTestBudget()
    {
        return new Budget()
        {
            Name = "Test Budget"
        };
    }

    public static BudgetUser CreateTestBudgetUser(Budget? budget = null, ApplicationUser? appUser = null)
    {
        budget = budget ?? CreateTestBudget();
        appUser = appUser ?? CreateTestApplicationUser();

        return new BudgetUser()
        {
            Budget = budget,
            ApplicationUser = appUser
        };
    }
}
