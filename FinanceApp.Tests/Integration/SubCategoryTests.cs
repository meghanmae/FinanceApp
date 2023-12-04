using FinanceApp.Data.Models;
using FinanceApp.Tests.Integration.Helpers;
using FluentAssertions;
using IntelliTect.Coalesce.Models;
using System.Net;
using System.Net.Http.Json;

namespace FinanceApp.Tests.Integration;
public class SubCategoryTests : IntegrationTestsBase
{
    private const string prefix = "/api/SubCategory";

    #region DefaultDataSource Secured By BudgetId
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SubCategoriesSecuredByBudget_DefaultDataSource_UserCanOnlyRetrieveDataFromBudgetsTheyAreAPartOf(bool specifyBudgetId)
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        SubCategory subCategory1 = TestData.CreateTestSubCategory(callingUsersBudget.Budget!);
        SubCategory subCategory2 = TestData.CreateTestSubCategory(callingUsersBudget.Budget!);
        Db.SubCategories.Add(subCategory1);
        Db.SubCategories.Add(subCategory2);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        SubCategory otherBudgetsSubCateogry = TestData.CreateTestSubCategory(otherBudgetUser.Budget!);
        Db.SubCategories.Add(otherBudgetsSubCateogry);

        Budget sharedBudget = TestData.CreateTestBudget();
        Db.Budgets.Add(sharedBudget);
        SubCategory sharedSubCategory = TestData.CreateTestSubCategory(sharedBudget);
        Db.SubCategories.Add(sharedSubCategory);
        BudgetUser sharedBudgetUser1 = TestData.CreateTestBudgetUser(sharedBudget, callingUser);
        BudgetUser sharedBudgetUser2 = TestData.CreateTestBudgetUser(sharedBudget, otherBudgetUser.ApplicationUser);
        Db.BudgetUsers.Add(sharedBudgetUser1);
        Db.BudgetUsers.Add(sharedBudgetUser2);

        await Db.SaveChangesAsync();

        var expectedSubCategories = new List<SubCategory> { subCategory1, subCategory2 };
        if (!specifyBudgetId) expectedSubCategories.Add(sharedSubCategory);

        // Act
        var suffix = specifyBudgetId ? $"?budgetId={callingUsersBudget.BudgetId}" : "";
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list{suffix}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<SubCategory>>();
        IList<SubCategory> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Select(x => x.SubCategoryId).Should().BeEquivalentTo(expectedSubCategories.Select(x => x.SubCategoryId));
    }

    [Fact]
    public async Task SubCategoriesSecuredByBudget_DefaultDataSource_UserCanNotRetrieveDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        ApplicationUser callingUser = TestData.CreateTestApplicationUser();
        Db.ApplicationUsers.Add(callingUser);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        SubCategory otherSubCategory = TestData.CreateTestSubCategory(otherBudgetUser.Budget!);
        Db.SubCategories.Add(otherSubCategory);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list?budgetId={otherBudgetUser.BudgetId}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<SubCategory>>();
        IList<SubCategory> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Count().Should().Be(0);
    }
    #endregion
}
