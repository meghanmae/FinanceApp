
using FinanceApp.Web.Models;
using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Api;
using IntelliTect.Coalesce.Api.Behaviors;
using IntelliTect.Coalesce.Api.Controllers;
using IntelliTect.Coalesce.Api.DataSources;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Mapping.IncludeTrees;
using IntelliTect.Coalesce.Models;
using IntelliTect.Coalesce.TypeDefinition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinanceApp.Web.Api
{
    [Route("api/TransactionsService")]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class TransactionsServiceController : Controller
    {
        protected ClassViewModel GeneratedForClassViewModel { get; }
        protected FinanceApp.Data.Services.TransactionsService Service { get; }
        protected CrudContext Context { get; }

        public TransactionsServiceController(CrudContext context, FinanceApp.Data.Services.TransactionsService service)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<FinanceApp.Data.Services.TransactionsService>();
            Service = service;
            Context = context;
        }

        /// <summary>
        /// Method: HistoricalTransactions
        /// </summary>
        [HttpPost("HistoricalTransactions")]
        [Authorize]
        public virtual ItemResult<System.Collections.Generic.ICollection<MonthlyTransactionsDtoDtoGen>> HistoricalTransactions(
            [FromForm(Name = "budgetId")] int budgetId,
            [FromForm(Name = "years")] int years = 3)
        {
            var _params = new
            {
                budgetId = budgetId,
                years = years
            };

            if (Context.Options.ValidateAttributesForMethods)
            {
                var _validationResult = ItemResult.FromParameterValidation(
                    GeneratedForClassViewModel!.MethodByName("HistoricalTransactions"), _params, HttpContext.RequestServices);
                if (!_validationResult.WasSuccessful) return new ItemResult<System.Collections.Generic.ICollection<MonthlyTransactionsDtoDtoGen>>(_validationResult);
            }

            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(Context);
            var _methodResult = Service.HistoricalTransactions(
                User,
                _params.budgetId,
                _params.years
            );
            var _result = new ItemResult<System.Collections.Generic.ICollection<MonthlyTransactionsDtoDtoGen>>(_methodResult);
            _result.Object = _methodResult.Object?.ToList().Select(o => Mapper.MapToDto<FinanceApp.Data.Services.MonthlyTransactionsDto, MonthlyTransactionsDtoDtoGen>(o, _mappingContext, includeTree ?? _methodResult.IncludeTree)).ToList();
            return _result;
        }
    }
}
