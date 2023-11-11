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

    #region DefaultDatasource Securied By BudgetId
    [Fact]
    public async Task CustomCalculationsSecuredByBudget_DefaultDataSource_UserCanOnlyRetrieveDataFromBudgetsTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        CustomCalculation customCalculation1 = TestData.CreateTestCustomCalculation(callingUsersBudget.Budget!);
        CustomCalculation customCalculation2 = TestData.CreateTestCustomCalculation(callingUsersBudget.Budget!);
        Db.CustomCalculations.Add(customCalculation1);
        Db.CustomCalculations.Add(customCalculation2);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        CustomCalculation otherBudgetsCustomCalculation = TestData.CreateTestCustomCalculation(otherUsersBudget);
        Db.CustomCalculations.Add(otherBudgetsCustomCalculation);

        await Db.SaveChangesAsync();

        var expectedCustomCalculations = new List<CustomCalculation> { customCalculation1, customCalculation2 };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser, callingUsersBudget.BudgetId).GetAsync($"{prefix}/list");
        var result = await response.Content.ReadFromJsonAsync<ListResult<CustomCalculation>>();
        IList<CustomCalculation> customCalculations = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        customCalculations.Select(x => x.CustomCalculationId).Should().BeEquivalentTo(expectedCustomCalculations.Select(x => x.CustomCalculationId));
    }
    #endregion
}
