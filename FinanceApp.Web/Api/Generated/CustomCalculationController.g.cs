
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
    [Route("api/CustomCalculation")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class CustomCalculationController
        : BaseApiController<FinanceApp.Data.Models.CustomCalculation, CustomCalculationDtoGen, FinanceApp.Data.AppDbContext>
    {
        public CustomCalculationController(CrudContext<FinanceApp.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<FinanceApp.Data.Models.CustomCalculation>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<CustomCalculationDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.CustomCalculation> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<CustomCalculationDtoGen>> List(
            ListParameters parameters,
            IDataSource<FinanceApp.Data.Models.CustomCalculation> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<FinanceApp.Data.Models.CustomCalculation> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<CustomCalculationDtoGen>> Save(
            [FromForm] CustomCalculationDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.CustomCalculation> dataSource,
            IBehaviors<FinanceApp.Data.Models.CustomCalculation> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<CustomCalculationDtoGen>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSourceFactory, behaviorsFactory);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<CustomCalculationDtoGen>> Delete(
            int id,
            IBehaviors<FinanceApp.Data.Models.CustomCalculation> behaviors,
            IDataSource<FinanceApp.Data.Models.CustomCalculation> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
