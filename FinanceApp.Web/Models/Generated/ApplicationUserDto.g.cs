using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class ApplicationUserDtoGen : GeneratedDto<FinanceApp.Data.Models.ApplicationUser>
    {
        public ApplicationUserDtoGen() { }

        private string _ApplicationUserId;
        private string _AzureObjectId;
        private string _Name;
        private string _Email;

        public string ApplicationUserId
        {
            get => _ApplicationUserId;
            set { _ApplicationUserId = value; Changed(nameof(ApplicationUserId)); }
        }
        public string AzureObjectId
        {
            get => _AzureObjectId;
            set { _AzureObjectId = value; Changed(nameof(AzureObjectId)); }
        }
        public string Name
        {
            get => _Name;
            set { _Name = value; Changed(nameof(Name)); }
        }
        public string Email
        {
            get => _Email;
            set { _Email = value; Changed(nameof(Email)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.ApplicationUser obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.ApplicationUserId = obj.ApplicationUserId;
            this.AzureObjectId = obj.AzureObjectId;
            this.Name = obj.Name;
            this.Email = obj.Email;
        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Models.ApplicationUser entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(ApplicationUserId))) entity.ApplicationUserId = ApplicationUserId;
            if (ShouldMapTo(nameof(AzureObjectId))) entity.AzureObjectId = AzureObjectId;
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Email))) entity.Email = Email;
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.ApplicationUser MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new FinanceApp.Data.Models.ApplicationUser()
            {
                AzureObjectId = AzureObjectId,
                Name = Name,
                Email = Email,
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(ApplicationUserId))) entity.ApplicationUserId = ApplicationUserId;

            return entity;
        }
    }
}
