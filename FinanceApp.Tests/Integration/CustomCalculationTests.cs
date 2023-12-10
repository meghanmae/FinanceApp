using FinanceApp.Data.Models;
using FinanceApp.Tests.Integration.Helpers;
using FinanceApp.Web.Models;
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

    #region BeforeSave Secured By BudgetId
    [Fact]
    public async Task CustomCalculationsBeforeSave_SecuredByDefaultDatasource_UserCanNotEditDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        CustomCalculation otherBudgetsCustomCalculation = TestData.CreateTestCustomCalculation(otherUsersBudget);
        Db.CustomCalculations.Add(otherBudgetsCustomCalculation);

        await Db.SaveChangesAsync();

        CustomCalculationDtoGen dto = new()
        {
            CustomCalculationId = otherBudgetsCustomCalculation.CustomCalculationId,
            Name = "New test name"
        };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsFormDataAsync($"{prefix}/save", dto);
        var result = await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Message.Should().Be($"Custom Calculation item with ID {otherBudgetsCustomCalculation.CustomCalculationId} was not found.");
    }

    [Fact]
    public async Task CustomCalculationsBeforeSave_SecuredByDefaultDatasource_UserCanEditDataForBudgetsTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        CustomCalculation customCalculation = TestData.CreateTestCustomCalculation(callingUsersBudget.Budget!);
        Db.CustomCalculations.Add(customCalculation);

        await Db.SaveChangesAsync();

        CustomCalculationDtoGen dto = new()
        {
            CustomCalculationId = customCalculation.CustomCalculationId,
            Name = "New test name"
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
    public async Task CustomCalculationsBeforeDelete_SecuredByDefaultDatasource_UserCanNotDeleteDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        CustomCalculation otherBudgetsCustomCalculation = TestData.CreateTestCustomCalculation(otherUsersBudget);
        Db.CustomCalculations.Add(otherBudgetsCustomCalculation);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsync($"{prefix}/delete/{otherBudgetsCustomCalculation.CustomCalculationId}", null);
        var result = await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        result!.Message.Should().Be($"Custom Calculation item with ID {otherBudgetsCustomCalculation.CustomCalculationId} was not found.");
    }

    [Fact]
    public async Task CustomCalculationsBeforeDelete_SecuredByDefaultDatasource_UserCanNotDeleteDataForBudgetsTheyAreAPartOf()
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);

        CustomCalculation customCalculation = TestData.CreateTestCustomCalculation(callingUsersBudget.Budget!);
        Db.CustomCalculations.Add(customCalculation);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).PostAsync($"{prefix}/delete/{customCalculation.CustomCalculationId}", null);
        await response.Content.ReadFromJsonAsync<ItemResult>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    #endregion
}
