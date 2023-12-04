using FinanceApp.Data.Models;
using FinanceApp.Tests.Integration.Helpers;
using FluentAssertions;
using IntelliTect.Coalesce.Models;
using System.Net;
using System.Net.Http.Json;

namespace FinanceApp.Tests.Integration;
public class CustomCalculationTests : IntegrationTestsBase
{
    private const string prefix = "/api/CustomCalculation";

    #region DefaultDataSource Secured By BudgetId
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CustomCalculationsSecuredByBudget_DefaultDataSource_UserCanOnlyRetrieveDataFromBudgetsTheyAreAPartOf(bool specifyBudgetId)
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        CustomCalculation customCalculation1 = TestData.CreateTestCustomCalculation(callingUsersBudget.Budget!);
        CustomCalculation customCalculation2 = TestData.CreateTestCustomCalculation(callingUsersBudget.Budget!);
        Db.CustomCalculations.Add(customCalculation1);
        Db.CustomCalculations.Add(customCalculation2);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        CustomCalculation otherBudgetsCustomCalculation = TestData.CreateTestCustomCalculation(otherBudgetUser.Budget!);
        Db.CustomCalculations.Add(otherBudgetsCustomCalculation);

        Budget sharedBudget = TestData.CreateTestBudget();
        Db.Budgets.Add(sharedBudget);
        CustomCalculation sharedCustomCalculation = TestData.CreateTestCustomCalculation(sharedBudget);
        Db.CustomCalculations.Add(sharedCustomCalculation);
        BudgetUser sharedBudgetUser1 = TestData.CreateTestBudgetUser(sharedBudget, callingUser);
        BudgetUser sharedBudgetUser2 = TestData.CreateTestBudgetUser(sharedBudget, otherBudgetUser.ApplicationUser);
        Db.BudgetUsers.Add(sharedBudgetUser1);
        Db.BudgetUsers.Add(sharedBudgetUser2);

        await Db.SaveChangesAsync();

        var expectedCustomCalculations = new List<CustomCalculation> { customCalculation1, customCalculation2 };
        if (!specifyBudgetId) expectedCustomCalculations.Add(sharedCustomCalculation);

        // Act
        var suffix = specifyBudgetId ? $"?budgetId={callingUsersBudget.BudgetId}" : "";
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list{suffix}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<CustomCalculation>>();
        IList<CustomCalculation> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Select(x => x.CustomCalculationId).Should().BeEquivalentTo(expectedCustomCalculations.Select(x => x.CustomCalculationId));
    }

    [Fact]
    public async Task CustomCalculationsSecuredByBudget_DefaultDataSource_UserCanNotRetrieveDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        ApplicationUser callingUser = TestData.CreateTestApplicationUser();
        Db.ApplicationUsers.Add(callingUser);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        CustomCalculation otheCustomCalculation = TestData.CreateTestCustomCalculation(otherBudgetUser.Budget!);
        Db.CustomCalculations.Add(otheCustomCalculation);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list?budgetId={otherBudgetUser.BudgetId}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<CustomCalculation>>();
        IList<CustomCalculation> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Count().Should().Be(0);
    }
    #endregion
}
