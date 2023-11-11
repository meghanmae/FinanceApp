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

    #region DefaultDatasource Securied By BudgetId
    [Fact]
    public async Task SubCategoriesSecuredByBudget_DefaultDataSource_UserCanOnlyRetrieveDataFromBudgetsTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        SubCategory callingUsersSubCategory1 = TestData.CreateTestSubCategory(callingUsersBudget.Budget);
        SubCategory callingUsersSubCategory2 = TestData.CreateTestSubCategory(callingUsersBudget.Budget);
        Db.SubCategories.Add(callingUsersSubCategory1);
        Db.SubCategories.Add(callingUsersSubCategory2);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        SubCategory otherBudgetsSubCategory = TestData.CreateTestSubCategory(otherUsersBudget);
        Db.SubCategories.Add(otherBudgetsSubCategory);

        await Db.SaveChangesAsync();

        var expectedSubCategories = new List<SubCategory> { callingUsersSubCategory1, callingUsersSubCategory2 };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser, callingUsersBudget.BudgetId).GetAsync($"{prefix}/list");
        var result = await response.Content.ReadFromJsonAsync<ListResult<SubCategory>>();
        IList<SubCategory> subCategories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        subCategories.Select(x => x.SubCategoryId).Should().BeEquivalentTo(expectedSubCategories.Select(x => x.SubCategoryId));
    }
    #endregion
}
