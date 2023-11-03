
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
    [Route("api/BudgetUser")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class BudgetUserController
        : BaseApiController<FinanceApp.Data.Models.BudgetUser, BudgetUserDtoGen, FinanceApp.Data.AppDbContext>
    {
        public BudgetUserController(CrudContext<FinanceApp.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<FinanceApp.Data.Models.BudgetUser>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<BudgetUserDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.BudgetUser> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<BudgetUserDtoGen>> List(
            ListParameters parameters,
            IDataSource<FinanceApp.Data.Models.BudgetUser> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<FinanceApp.Data.Models.BudgetUser> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<BudgetUserDtoGen>> Save(
            [FromForm] BudgetUserDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.BudgetUser> dataSource,
            IBehaviors<FinanceApp.Data.Models.BudgetUser> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<BudgetUserDtoGen>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSourceFactory, behaviorsFactory);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<BudgetUserDtoGen>> Delete(
            int id,
            IBehaviors<FinanceApp.Data.Models.BudgetUser> behaviors,
            IDataSource<FinanceApp.Data.Models.BudgetUser> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
