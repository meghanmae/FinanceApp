
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
    [Route("api/Budget")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class BudgetController
        : BaseApiController<FinanceApp.Data.Models.Budget, BudgetDtoGen, FinanceApp.Data.AppDbContext>
    {
        public BudgetController(CrudContext<FinanceApp.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<FinanceApp.Data.Models.Budget>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<BudgetDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.Budget> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<BudgetDtoGen>> List(
            ListParameters parameters,
            IDataSource<FinanceApp.Data.Models.Budget> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<FinanceApp.Data.Models.Budget> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<BudgetDtoGen>> Save(
            [FromForm] BudgetDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.Budget> dataSource,
            IBehaviors<FinanceApp.Data.Models.Budget> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<BudgetDtoGen>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSourceFactory, behaviorsFactory);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<BudgetDtoGen>> Delete(
            int id,
            IBehaviors<FinanceApp.Data.Models.Budget> behaviors,
            IDataSource<FinanceApp.Data.Models.Budget> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
