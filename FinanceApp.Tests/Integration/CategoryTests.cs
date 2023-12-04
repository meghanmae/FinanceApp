using FinanceApp.Data.Models;
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

    //[Fact]
    //public async Task CategoriesSecuredByBudget_BeforeSave_UserCanNotEditDataForBudgetsTheyAreNotAPartOf()
    //{
    //    // Arrange
    //    BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
    //    ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
    //    Db.BudgetUsers.Add(callingUsersBudget);

    //    Budget otherUsersBudget = TestData.CreateTestBudget();
    //    Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
    //    Category otherBudgetsCategory = TestData.CreateTestCategory(otherUsersBudget);
    //    Db.Categories.Add(otherBudgetsCategory);

    //    await Db.SaveChangesAsync();

    //    CategoryDtoGen dto = new()
    //    {
    //        CategoryId = otherBudgetsCategory.CategoryId,
    //        Name = otherBudgetsCategory.Name,
    //        Color = "blue",
    //        Icon = "test"
    //    };

    //    // Act
    //    HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //}

    //[Fact]
    //public async Task CategoriesSecuredByBudget_BeforeSave_UserCanEditDataForBudgetsTheyAreAPartOf()
    //{
    //    // Arrange
    //    BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
    //    ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
    //    Db.BudgetUsers.Add(callingUsersBudget);
    //    Category callingUsersCategory = TestData.CreateTestCategory(callingUsersBudget.Budget!);
    //    Db.Categories.Add(callingUsersCategory);

    //    await Db.SaveChangesAsync();

    //    CategoryDtoGen dto = new()
    //    {
    //        CategoryId = callingUsersCategory.CategoryId,
    //        Name = callingUsersCategory.Name,
    //        Color = "blue",
    //        Icon = "test"
    //    };

    //    // Act
    //    HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);
    //    var result = await response.Content.ReadFromJsonAsync<Category>();
    //    Category category = result!;

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //    category.BudgetId.Should().Be(callingUsersBudget.BudgetId);
    //}

    //[Fact]
    //public async Task CategoriesSecuredByBudget_BeforeSave_UserAddsDataForBudgetsAutoSetsBudgetId()
    //{
    //    // Arrange
    //    BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
    //    ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
    //    Db.BudgetUsers.Add(callingUsersBudget);

    //    await Db.SaveChangesAsync();

    //    CategoryDtoGen dto = new()
    //    {
    //        Name = "New Category",
    //        Color = "blue",
    //        Icon = "test"
    //    };

    //    // Act
    //    HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);

    //    // Assert
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //}
}
