using FinanceApp.Data.Models;
using FinanceApp.Tests.Integration.Helpers;
using FluentAssertions;
using IntelliTect.Coalesce.Models;
using System.Net;
using System.Net.Http.Json;

namespace FinanceApp.Tests.Integration;
public class BudgetTests : IntegrationTestsBase
{
    private const string prefix = "/api/Budget";

    [Fact]
    public async Task Budgets_DefaultDataSource_UserCanNotReadOtherUsersBudgets()
    {
        // Arrange
        ApplicationUser callingUser = TestData.CreateTestApplicationUser();
        Db.ApplicationUsers.Add(callingUser);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/get/{otherUsersBudget.BudgetId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Budgets_DefaultDataSource_UserWithNoBudgetsReturnsNull()
    {
        // Arrange
        ApplicationUser callingUser = TestData.CreateTestApplicationUser();
        Db.ApplicationUsers.Add(callingUser);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Budget>>();
        IList<Budget> budgets = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        budgets.Should().BeEmpty();
    }

    [Fact]
    public async Task Budgets_DefaultDataSource_UserCanOnlyRetrieveBudgetsTheyAreAPartOf()
    {
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

        var expectedBudgets = new List<Budget> { expectedBudgetUser.Budget!, expectedSharedBudget };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Budget>>();
        IList<Budget> budgets = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        budgets.Select(x => x.BudgetId).Should().BeEquivalentTo(expectedBudgets.Select(x => x.BudgetId));
    }
}
