
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
    [Route("api/Category")]
    [Authorize]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class CategoryController
        : BaseApiController<FinanceApp.Data.Models.Category, CategoryDtoGen, FinanceApp.Data.AppDbContext>
    {
        public CategoryController(CrudContext<FinanceApp.Data.AppDbContext> context) : base(context)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<FinanceApp.Data.Models.Category>();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public virtual Task<ItemResult<CategoryDtoGen>> Get(
            int id,
            DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.Category> dataSource)
            => GetImplementation(id, parameters, dataSource);

        [HttpGet("list")]
        [Authorize]
        public virtual Task<ListResult<CategoryDtoGen>> List(
            ListParameters parameters,
            IDataSource<FinanceApp.Data.Models.Category> dataSource)
            => ListImplementation(parameters, dataSource);

        [HttpGet("count")]
        [Authorize]
        public virtual Task<ItemResult<int>> Count(
            FilterParameters parameters,
            IDataSource<FinanceApp.Data.Models.Category> dataSource)
            => CountImplementation(parameters, dataSource);

        [HttpPost("save")]
        [Authorize]
        public virtual Task<ItemResult<CategoryDtoGen>> Save(
            [FromForm] CategoryDtoGen dto,
            [FromQuery] DataSourceParameters parameters,
            IDataSource<FinanceApp.Data.Models.Category> dataSource,
            IBehaviors<FinanceApp.Data.Models.Category> behaviors)
            => SaveImplementation(dto, parameters, dataSource, behaviors);

        [HttpPost("bulkSave")]
        [Authorize]
        public virtual Task<ItemResult<CategoryDtoGen>> BulkSave(
            [FromBody] BulkSaveRequest dto,
            [FromQuery] DataSourceParameters parameters,
            [FromServices] IDataSourceFactory dataSourceFactory,
            [FromServices] IBehaviorsFactory behaviorsFactory)
            => BulkSaveImplementation(dto, parameters, dataSourceFactory, behaviorsFactory);

        [HttpPost("delete/{id}")]
        [Authorize]
        public virtual Task<ItemResult<CategoryDtoGen>> Delete(
            int id,
            IBehaviors<FinanceApp.Data.Models.Category> behaviors,
            IDataSource<FinanceApp.Data.Models.Category> dataSource)
            => DeleteImplementation(id, new DataSourceParameters(), dataSource, behaviors);
    }
}
