
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
    [Route("api/UserService")]
    [ServiceFilter(typeof(IApiActionFilter))]
    public partial class UserServiceController : Controller
    {
        protected ClassViewModel GeneratedForClassViewModel { get; }
        protected FinanceApp.Data.Services.UserService Service { get; }
        protected CrudContext Context { get; }

        public UserServiceController(CrudContext context, FinanceApp.Data.Services.UserService service)
        {
            GeneratedForClassViewModel = context.ReflectionRepository.GetClassViewModel<FinanceApp.Data.Services.UserService>();
            Service = service;
            Context = context;
        }

        /// <summary>
        /// Method: GetLoggedInUser
        /// </summary>
        [HttpPost("GetLoggedInUser")]
        [Authorize]
        public virtual ItemResult<ApplicationUserDtoGen> GetLoggedInUser(
            [FromServices] FinanceApp.Data.AppDbContext db)
        {
            IncludeTree includeTree = null;
            var _mappingContext = new MappingContext(Context);
            var _methodResult = Service.GetLoggedInUser(
                User,
                db
            );
            var _result = new ItemResult<ApplicationUserDtoGen>();
            _result.Object = Mapper.MapToDto<FinanceApp.Data.Models.ApplicationUser, ApplicationUserDtoGen>(_methodResult, _mappingContext, includeTree);
            return _result;
        }
    }
}
