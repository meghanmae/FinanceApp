using FinanceApp.Data;
using FinanceApp.Data.Models;
using FinanceApp.Data.Security;
using FinanceApp.Tests.Integration.Helpers;
using FinanceApp.Web.Models;
using FluentAssertions;
using IntelliTect.Coalesce.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;

namespace FinanceApp.Tests.Integration;
public class CategoryTests : IntegrationTestsBase
{
    private const string prefix = "/api/Category";

    #region DefaultDataSource Secured By BudgetId
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CategoriesSecuredByBudget_DefaultDataSource_UserCanOnlyRetrieveDataFromBudgetsTheyAreAPartOf(bool specifyBudgetId)
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        Category callingUsersCategory1 = TestData.CreateTestCategory(callingUsersBudget.Budget!);
        Category callingUsersCategory2 = TestData.CreateTestCategory(callingUsersBudget.Budget!);
        Db.Categories.Add(callingUsersCategory1);
        Db.Categories.Add(callingUsersCategory2);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        Category otherCategory = TestData.CreateTestCategory(otherBudgetUser.Budget, otherBudgetUser.ApplicationUser);
        Db.Categories.Add(otherCategory);

        Budget sharedBudget = TestData.CreateTestBudget();
        Db.Budgets.Add(sharedBudget);
        Category sharedCategory = TestData.CreateTestCategory(sharedBudget);
        Db.Categories.Add(sharedCategory);
        BudgetUser sharedBudgetUser1 = TestData.CreateTestBudgetUser(sharedBudget, callingUser);
        BudgetUser sharedBudgetUser2 = TestData.CreateTestBudgetUser(sharedBudget, otherBudgetUser.ApplicationUser);
        Db.BudgetUsers.Add(sharedBudgetUser1);
        Db.BudgetUsers.Add(sharedBudgetUser2);

        await Db.SaveChangesAsync();

        var expectedCategories = new List<Category> { callingUsersCategory1, callingUsersCategory2 };
        if (!specifyBudgetId) expectedCategories.Add(sharedCategory);

        // Act
        var suffix = specifyBudgetId ? $"?budgetId={callingUsersBudget.BudgetId}" : "";
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list{suffix}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Category>>();
        IList<Category> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Select(x => x.CategoryId).Should().BeEquivalentTo(expectedCategories.Select(x => x.CategoryId));
    }

    [Fact]
    public async Task CategoriesSecuredByBudget_DefaultDataSource_UserCanNotRetrieveDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        ApplicationUser callingUser = TestData.CreateTestApplicationUser();
        Db.ApplicationUsers.Add(callingUser);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        Category otherCategory = TestData.CreateTestCategory(otherBudgetUser.Budget, otherBudgetUser.ApplicationUser);
        Db.Categories.Add(otherCategory);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list?budgetId={otherBudgetUser.BudgetId}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Category>>();
        IList<Category> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Count().Should().Be(0);
    }
    #endregion

    #region BeforeSave Secured By BudgetId
    [Fact]
    public async Task CategoriesBeforeSave_SecuredByDefaultDatasource_UserCanNotEditDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        Category otherBudgetsCategory = TestData.CreateTestCategory(otherUsersBudget);
        Db.Categories.Add(otherBudgetsCategory);

        await Db.SaveChangesAsync();

        CategoryDtoGen dto = new()
        {
            CategoryId = otherBudgetsCategory.CategoryId,
            Name = otherBudgetsCategory.Name,
            Color = "blue",
            Icon = "test"
        };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);
        var result = await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Message.Should().Be($"Category item with ID {otherBudgetsCategory.CategoryId} was not found.");
    }

    [Fact]
    public async Task CategoriesBeforeSave_SecuredByDefaultDatasource_UserCanEditDataForBudgetsTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        Category category = TestData.CreateTestCategory(callingUsersBudget.Budget!);
        Db.Categories.Add(category);

        await Db.SaveChangesAsync();

        CategoryDtoGen dto = new()
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Color = "blue",
            Icon = "test"
        };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);
        await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    #endregion

    #region BeforeDelete Secured By BudgetId
    [Fact]
    public async Task CategoriesBeforeDelete_SecuredByDefaultDatasource_UserCanNotDeleteDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        Category otherBudgetsCategory = TestData.CreateTestCategory(otherUsersBudget);
        Db.Categories.Add(otherBudgetsCategory);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsync($"{prefix}/delete/{otherBudgetsCategory.CategoryId}", null);
        var result = await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Message.Should().Be($"Category item with ID {otherBudgetsCategory.CategoryId} was not found.");
    }

    [Fact]
    public async Task CategoriesBeforeDelete_SecuredByDefaultDatasource_UserCanNotDeleteDataForBudgetsTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        Category category = TestData.CreateTestCategory(callingUsersBudget.Budget!);
        Db.Categories.Add(category);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsync($"{prefix}/delete/{category.CategoryId}", null);
        await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    #endregion
}
