using FinanceApp.Data.Models;
using FluentAssertions;
using IntelliTect.Coalesce.Api;

namespace FinanceApp.Tests.Unit;

public class BudgetTests : UnitTestBase
{
    [Fact]
    public async Task Budgets_DefaultDataSource_ReturnsOnlyBudgetsUserIsAPartOf()
    {
        // Arrange
        BudgetUser expectedBudgetUser = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = expectedBudgetUser.ApplicationUser!;
        Db.BudgetUsers.Add(expectedBudgetUser);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        ApplicationUser otherUser = otherBudgetUser.ApplicationUser!;
        Db.BudgetUsers.Add(otherBudgetUser);

        Budget expectedSharedBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(expectedSharedBudget, callingUser));
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(expectedSharedBudget, otherUser));

        await Db.SaveChangesAsync();

        SetUserToContext(callingUser);
        Budget.BudgetsForUser budgetsForUserDataSource = new(Context);

        var expectedBudgetIds = new List<Budget> { expectedBudgetUser.Budget!, expectedSharedBudget };

        // Act
        var list = await budgetsForUserDataSource.GetListAsync(new ListParameters());
        IList<Budget> result = list.List.List!;

        // Assert
        result.Should().BeEquivalentTo(expectedBudgetIds);
    }
}