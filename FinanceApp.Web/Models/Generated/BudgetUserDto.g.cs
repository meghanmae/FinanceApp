using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class BudgetUserDtoGen : GeneratedDto<FinanceApp.Data.Models.BudgetUser>
    {
        public BudgetUserDtoGen() { }

        private int? _BudgetUserId;
        private string _ApplicationUserId;
        private FinanceApp.Web.Models.ApplicationUserDtoGen _ApplicationUser;
        private int? _BudgetId;

        public int? BudgetUserId
        {
            get => _BudgetUserId;
            set { _BudgetUserId = value; Changed(nameof(BudgetUserId)); }
        }
        public string ApplicationUserId
        {
            get => _ApplicationUserId;
            set { _ApplicationUserId = value; Changed(nameof(ApplicationUserId)); }
        }
        public FinanceApp.Web.Models.ApplicationUserDtoGen ApplicationUser
        {
            get => _ApplicationUser;
            set { _ApplicationUser = value; Changed(nameof(ApplicationUser)); }
        }
        public int? BudgetId
        {
            get => _BudgetId;
            set { _BudgetId = value; Changed(nameof(BudgetId)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.BudgetUser obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.BudgetUserId = obj.BudgetUserId;
            this.ApplicationUserId = obj.ApplicationUserId;
            this.BudgetId = obj.BudgetId;
            if (tree == null || tree[nameof(this.ApplicationUser)] != null)
                this.ApplicationUser = obj.ApplicationUser.MapToDto<FinanceApp.Data.Models.ApplicationUser, ApplicationUserDtoGen>(context, tree?[nameof(this.ApplicationUser)]);

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Models.BudgetUser entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(BudgetUserId))) entity.BudgetUserId = (BudgetUserId ?? entity.BudgetUserId);
            if (ShouldMapTo(nameof(ApplicationUserId))) entity.ApplicationUserId = ApplicationUserId;
            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.BudgetUser MapToNew(IMappingContext context)
        {
            var entity = new FinanceApp.Data.Models.BudgetUser();
            MapTo(entity, context);
            return entity;
        }
    }
}
