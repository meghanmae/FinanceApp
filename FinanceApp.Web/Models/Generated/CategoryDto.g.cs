using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class CategoryDtoGen : GeneratedDto<FinanceApp.Data.Models.Category>
    {
        public CategoryDtoGen() { }

        private int? _CategoryId;
        private string _Name;
        private string _Description;
        private string _Color;
        private string _Icon;
        private System.Collections.Generic.ICollection<FinanceApp.Web.Models.SubCategoryDtoGen> _SubCategories;
        private int? _BudgetId;

        public int? CategoryId
        {
            get => _CategoryId;
            set { _CategoryId = value; Changed(nameof(CategoryId)); }
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
        public string Color
        {
            get => _Color;
            set { _Color = value; Changed(nameof(Color)); }
        }
        public string Icon
        {
            get => _Icon;
            set { _Icon = value; Changed(nameof(Icon)); }
        }
        public System.Collections.Generic.ICollection<FinanceApp.Web.Models.SubCategoryDtoGen> SubCategories
        {
            get => _SubCategories;
            set { _SubCategories = value; Changed(nameof(SubCategories)); }
        }
        public int? BudgetId
        {
            get => _BudgetId;
            set { _BudgetId = value; Changed(nameof(BudgetId)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.Category obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.CategoryId = obj.CategoryId;
            this.Name = obj.Name;
            this.Description = obj.Description;
            this.Color = obj.Color;
            this.Icon = obj.Icon;
            this.BudgetId = obj.BudgetId;
            var propValSubCategories = obj.SubCategories;
            if (propValSubCategories != null && (tree == null || tree[nameof(this.SubCategories)] != null))
            {
                this.SubCategories = propValSubCategories
                    .OrderBy(f => f.Name)
                    .Select(f => f.MapToDto<FinanceApp.Data.Models.SubCategory, SubCategoryDtoGen>(context, tree?[nameof(this.SubCategories)])).ToList();
            }
            else if (propValSubCategories == null && tree?[nameof(this.SubCategories)] != null)
            {
                this.SubCategories = new SubCategoryDtoGen[0];
            }

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Models.Category entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(CategoryId))) entity.CategoryId = (CategoryId ?? entity.CategoryId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(Color))) entity.Color = Color;
            if (ShouldMapTo(nameof(Icon))) entity.Icon = Icon;
            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.Category MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new FinanceApp.Data.Models.Category()
            {
                Name = Name,
                Color = Color,
                Icon = Icon,
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(CategoryId))) entity.CategoryId = (CategoryId ?? entity.CategoryId);
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);

            return entity;
        }
    }
}
