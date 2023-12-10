using FinanceApp.Data.Models;
using FinanceApp.Tests.Integration.Helpers;
using FinanceApp.Web.Models;
using FluentAssertions;
using IntelliTect.Coalesce.Models;
using System.Net;
using System.Net.Http.Json;

namespace FinanceApp.Tests.Integration;
public class BudgetTests : IntegrationTestsBase
{
    private const string prefix = "/api/Budget";

    #region DefaultDataSource
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
    #endregion

    #region BeforeSave Secured By BudgetId
    [Fact]
    public async Task BudgetsBeforeSave_SecuredByDefaultDatasource_UserCanNotEditBudgetTheyAreNotAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));

        await Db.SaveChangesAsync();

        BudgetDtoGen dto = new()
        {
            BudgetId = otherUsersBudget.BudgetId,
            Name = "Test budget name",
        };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);
        var result = await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Message.Should().Be($"Budget item with ID {otherUsersBudget.BudgetId} was not found.");
    }

    [Fact]
    public async Task BudgetsBeforeSave_SecuredByDefaultDatasource_UserCanEditBudgetTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        await Db.SaveChangesAsync();

        BudgetDtoGen dto = new()
        {
            BudgetId = callingUsersBudget.BudgetId,
            Name = "Test budget name",
        };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);
        await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    #endregion


    #region BeforeDelete 
    [Fact]
    public async Task BudgetsBeforeDelete_SecuredByDefaultDatasource_UserCanNotDeleteABudgetTheyAreNotAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsync($"{prefix}/delete/{otherUsersBudget.BudgetId}", null);
        var result = await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Message.Should().Be($"Budget item with ID {otherUsersBudget.BudgetId} was not found.");
    }

    [Fact]
    public async Task BudgetsBeforeDelete_SecuredByDefaultDatasource_UserCanNotDeleteABudgetTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsync($"{prefix}/delete/{callingUsersBudget.BudgetId}", null);
        await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    #endregion
}
