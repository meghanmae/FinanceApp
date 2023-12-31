﻿using FinanceApp.Data.Models;
using FinanceApp.Tests.Unit.Helpers;
using FluentAssertions;
using IntelliTect.Coalesce.Api;

namespace FinanceApp.Tests.Unit;
public class CategoryTests : UnitTestBase
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Budgets_BudgetsForUserDataSource_ReturnsOnlyBudgetsUserIsAPartOf(bool specifyBudgetId)
    {
        // Arrange
        BudgetUser expectedBudgetUser = TestData.CreateTestBudgetUser();
        ApplicationUser callingUser = expectedBudgetUser.ApplicationUser!;
        Db.BudgetUsers.Add(expectedBudgetUser);
        Category expectedCategory = TestData.CreateTestCategory(expectedBudgetUser.Budget, expectedBudgetUser.ApplicationUser);
        Db.Categories.Add(expectedCategory);

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

        SetUserToContext(callingUser);
        Category.CategoriesByBudget context = new(Context);

        // Act
        context.BudgetId = specifyBudgetId ? expectedBudgetUser.BudgetId : null;
        var list = await context.GetListAsync(new ListParameters());
        IList<Category> result = list.List.List!;

        // Assert
        if (specifyBudgetId)
        {
            result.Count.Should().Be(1);
            result.First().CategoryId.Should().Be(expectedCategory.CategoryId);
        }
        else
        {
            result.Count.Should().Be(2);
        }
    }
}
