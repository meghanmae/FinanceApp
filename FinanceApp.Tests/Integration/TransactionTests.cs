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

    #region DefaultDatasource Securied By BudgetId
    [Fact]
    public async Task TransactionsSecuredByBudget_DefaultDataSource_UserCanOnlyRetrieveDataFromBudgetsTheyAreAPartOf()
    {
        BudgetUser callingUsersBudget = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = callingUsersBudget.ApplicationUser!;
        Db.BudgetUsers.Add(callingUsersBudget);
        Transaction transaction1 = TestData.CreateTestTransaction(callingUsersBudget.Budget!);
        Transaction transaction2 = TestData.CreateTestTransaction(callingUsersBudget.Budget!);
        Db.Transactions.Add(transaction1);
        Db.Transactions.Add(transaction2);

        Budget otherUsersBudget = TestData.CreateTestBudget();
        Db.BudgetUsers.Add(TestData.CreateTestBudgetUser(otherUsersBudget));
        Transaction otherTransaction = TestData.CreateTestTransaction(otherUsersBudget);
        Db.Transactions.Add(otherTransaction);

        await Db.SaveChangesAsync();

        var expectedTransactions = new List<Transaction> { transaction1, transaction2 };

        // Act
        HttpResponseMessage response = await GetAuthClient(callingUser, callingUsersBudget.BudgetId).GetAsync($"{prefix}/list");
        var result = await response.Content.ReadFromJsonAsync<ListResult<Transaction>>();
        IList<Transaction> customCalculations = result!.List!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        customCalculations.Select(x => x.TransactionId).Should().BeEquivalentTo(expectedTransactions.Select(x => x.TransactionId));
    }
    #endregion
}