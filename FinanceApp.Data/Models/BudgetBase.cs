using FinanceApp.Data.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Data.Models;
public abstract class BudgetBase : IBudgeted
{
    [InternalUse]
    public int BudgetId { get; set; }
    
    [InternalUse]
    public Budget? Budget { get; set; }
}

public abstract class BudgetInfrastructureBase : IHaveBudgetFk
{
    public int BudgetId { get; set; }

    public Budget? Budget { get; set; }
}
