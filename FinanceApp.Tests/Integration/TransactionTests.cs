using FinanceApp.Data.Models;
using FinanceApp.Tests.Integration.Helpers;
using FluentAssertions;
using IntelliTect.Coalesce.Models;
using System.Net;
using System.Net.Http.Json;

namespace FinanceApp.Tests.Integration;
public class TransactionTests : IntegrationTestsBase
{
    private const string prefix = "/api/Transaction";

    #region DefaultDataSource Secured By BudgetId
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task TransactionsSecuredByBudget_DefaultDataSource_UserCanOnlyRetrieveDataFromBudgetsTheyAreAPartOf(bool specifyBudgetId)
    {
        // Arrange
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        Transaction transaction1 = TestData.CreateTestTransaction(callingUsersBudget.Budget!);
        Transaction transaction2 = TestData.CreateTestTransaction(callingUsersBudget.Budget!);
        Db.Transactions.Add(transaction1);
        Db.Transactions.Add(transaction2);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        Transaction otherBudgetsTransaction = TestData.CreateTestTransaction(otherBudgetUser.Budget!);
        Db.Transactions.Add(otherBudgetsTransaction);

        Budget sharedBudget = TestData.CreateTestBudget();
        Db.Budgets.Add(sharedBudget);
        Transaction sharedTransaction = TestData.CreateTestTransaction(sharedBudget);
        Db.Transactions.Add(sharedTransaction);
        BudgetUser sharedBudgetUser1 = TestData.CreateTestBudgetUser(sharedBudget, callingUser);
        BudgetUser sharedBudgetUser2 = TestData.CreateTestBudgetUser(sharedBudget, otherBudgetUser.ApplicationUser);
        Db.BudgetUsers.Add(sharedBudgetUser1);
        Db.BudgetUsers.Add(sharedBudgetUser2);

        await Db.SaveChangesAsync();

        var expectedSubCategories = new List<Transaction> { transaction1, transaction2 };
        if (!specifyBudgetId) expectedSubCategories.Add(sharedTransaction);

        // Act
        var suffix = specifyBudgetId ? $"?budgetId={callingUsersBudget.BudgetId}" : "";
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list{suffix}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Transaction>>();
        IList<Transaction> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Select(x => x.TransactionId).Should().BeEquivalentTo(expectedSubCategories.Select(x => x.TransactionId));
    }

    [Fact]
    public async Task TransactionsSecuredByBudget_DefaultDataSource_UserCanNotRetrieveDataForBudgetsTheyAreNotAPartOf()
    {
        // Arrange
        ApplicationUser callingUser = TestData.CreateTestApplicationUser();
        Db.ApplicationUsers.Add(callingUser);

        BudgetUser otherBudgetUser = TestData.CreateTestBudgetUser();
        Db.BudgetUsers.Add(otherBudgetUser);
        Transaction otherTransaction = TestData.CreateTestTransaction(otherBudgetUser.Budget!);
        Db.Transactions.Add(otherTransaction);

        await Db.SaveChangesAsync();

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser).GetAsync($"{prefix}/list?budgetId={otherBudgetUser.BudgetId}");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Transaction>>();
        IList<Transaction> categories = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Count().Should().Be(0);
    }
    #endregion
}