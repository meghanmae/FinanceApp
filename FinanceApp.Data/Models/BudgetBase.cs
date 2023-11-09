﻿using FinanceApp.Data.Security;

namespace FinanceApp.Data.Models;
public abstract class BudgetBase : IBudgeted
{
    [InternalUse]
    [Required]
    public int BudgetId { get; set; }

    [InternalUse]
    public Budget? Budget { get; set; }
}

public abstract class BudgetInfrastructureBase : IHaveBudgetFk
{
    [Required]
    public int BudgetId { get; set; }

    public Budget? Budget { get; set; }
}
