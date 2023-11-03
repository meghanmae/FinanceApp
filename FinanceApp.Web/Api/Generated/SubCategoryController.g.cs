
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
    [Route("api/SubCategory")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class SubCategoryController
        : BaseApiController<FinanceApp.Data.Models.SubCategory, SubCategoryDtoGen, FinanceApp.Data.AppDbContext>
    {
        public SubCategoryController(CrudContext<FinanceApp.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<FinanceApp.Data.Models.SubCategory>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<SubCategoryDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.SubCategory> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<SubCategoryDtoGen>> List(
            ListParameters parameters,
            IDataSource<FinanceApp.Data.Models.SubCategory> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<FinanceApp.Data.Models.SubCategory> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<SubCategoryDtoGen>> Save(
            [FromForm] SubCategoryDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.SubCategory> dataSource,
            IBehaviors<FinanceApp.Data.Models.SubCategory> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<SubCategoryDtoGen>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSourceFactory, behaviorsFactory);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<SubCategoryDtoGen>> Delete(
            int id,
            IBehaviors<FinanceApp.Data.Models.SubCategory> behaviors,
            IDataSource<FinanceApp.Data.Models.SubCategory> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
