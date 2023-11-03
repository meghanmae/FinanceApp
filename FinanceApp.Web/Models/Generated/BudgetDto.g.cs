using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class BudgetDtoGen : GeneratedDto<FinanceApp.Data.Models.Budget>
    {
        public BudgetDtoGen() { }

        private int? _BudgetId;
        private string _Name;
        private string _Description;
        private System.Collections.Generic.ICollection<FinanceApp.Web.Models.BudgetUserDtoGen> _BudgetUsers;
        private System.Collections.Generic.ICollection<FinanceApp.Web.Models.CategoryDtoGen> _Categories;

        public int? BudgetId
        {
            get => _BudgetId;
            set { _BudgetId = value; Changed(nameof(BudgetId)); }
        }
        public string Name
        {
            get => _Name;
            set { _Name = value; Changed(nameof(Name)); }
        }
        public string Description
        {
            get => _Description;
            set { _Description = value; Changed(nameof(Description)); }
        }
        public System.Collections.Generic.ICollection<FinanceApp.Web.Models.BudgetUserDtoGen> BudgetUsers
        {
            get => _BudgetUsers;
            set { _BudgetUsers = value; Changed(nameof(BudgetUsers)); }
        }
        public System.Collections.Generic.ICollection<FinanceApp.Web.Models.CategoryDtoGen> Categories
        {
            get => _Categories;
            set { _Categories = value; Changed(nameof(Categories)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.Budget obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.BudgetId = obj.BudgetId;
            this.Name = obj.Name;
            this.Description = obj.Description;
            var propValBudgetUsers = obj.BudgetUsers;
            if (propValBudgetUsers != null && (tree == null || tree[nameof(this.BudgetUsers)] != null))
            {
                this.BudgetUsers = propValBudgetUsers
                    .OrderBy(f => f.BudgetUserId)
                    .Select(f => f.MapToDto<FinanceApp.Data.Models.BudgetUser, BudgetUserDtoGen>(context, tree?[nameof(this.BudgetUsers)])).ToList();
            }
            else if (propValBudgetUsers == null && tree?[nameof(this.BudgetUsers)] != null)
            {
                this.BudgetUsers = new BudgetUserDtoGen[0];
            }

            var propValCategories = obj.Categories;
            if (propValCategories != null && (tree == null || tree[nameof(this.Categories)] != null))
            {
                this.Categories = propValCategories
                    .OrderBy(f => f.Name)
                    .Select(f => f.MapToDto<FinanceApp.Data.Models.Category, CategoryDtoGen>(context, tree?[nameof(this.Categories)])).ToList();
            }
            else if (propValCategories == null && tree?[nameof(this.Categories)] != null)
            {
                this.Categories = new CategoryDtoGen[0];
            }

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Models.Budget entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.Budget MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new FinanceApp.Data.Models.Budget()
            {
                Name = Name,
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;

            return entity;
        }
    }
}
