using FinanceApp.Data.Models;
using FinanceApp.Tests.Integration.Helpers;
using FluentAssertions;
using IntelliTect.Coalesce.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Tests.Integration;
public class CategoryTests : IntegrationTestsBase
{
    private const string prefix = "/api/Category";

    #region DefaultDatasource Budget Security
    [Fact]
    public async Task Categories_DefaultDataSource_UserWithBudgetCanNotReadOtherBudgetsCategories()
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
        HttpResponseMessage response = await GetAuthClient(callingUser, callingUsersBudget.BudgetId).GetAsync($"{prefix}/get/{otherBudgetsCategory.CategoryId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Categories_DefaultDataSource_UserWithBiBudgetReturnNoCategories()
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
        HttpResponseMessage response = await GetAuthClient(callingUser, callingUsersBudget.BudgetId).GetAsync($"{prefix}/list");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Category>>();
        IList<Category> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Should().BeEmpty();
    }

    [Fact]
    public async Task Categories_DefaultDataSource_UserCanOnlyRetrieveCategoriesFromBudgetsTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        Category callingUsersCategory1 = TestData.CreateTestCategory(callingUsersBudget.Budget!);
        Category callingUsersCategory2 = TestData.CreateTestCategory(callingUsersBudget.Budget!);
        Db.Categories.Add(callingUsersCategory1);
        Db.Categories.Add(callingUsersCategory2);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        Category otherBudgetsCategory = TestData.CreateTestCategory(otherUsersBudget);
        Db.Categories.Add(otherBudgetsCategory);

        await Db.SaveChangesAsync();

        var expectedCategories = new List<Category> { callingUsersCategory1, callingUsersCategory2 };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser, callingUsersBudget.BudgetId).GetAsync($"{prefix}/list");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Category>>();
        IList<Category> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Select(x => x.CategoryId).Should().BeEquivalentTo(expectedCategories.Select(x => x.CategoryId));
    }
    #endregion
}
