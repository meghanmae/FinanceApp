using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class SubCategoryDtoGen : GeneratedDto<FinanceApp.Data.Models.SubCategory>
    {
        public SubCategoryDtoGen() { }

        private int? _SubCategoryId;
        private string _Name;
        private string _Description;
        private decimal? _Allocation;
        private int? _CategoryId;
        private FinanceApp.Web.Models.CategoryDtoGen _Category;
        private System.Collections.Generic.ICollection<FinanceApp.Web.Models.SubCategoryCustomCalculationDtoGen> _SubCategoryCustomCalculations;
        private int? _BudgetId;

        public int? SubCategoryId
        {
            get => _SubCategoryId;
            set { _SubCategoryId = value; Changed(nameof(SubCategoryId)); }
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
        public decimal? Allocation
        {
            get => _Allocation;
            set { _Allocation = value; Changed(nameof(Allocation)); }
        }
        public int? CategoryId
        {
            get => _CategoryId;
            set { _CategoryId = value; Changed(nameof(CategoryId)); }
        }
        public FinanceApp.Web.Models.CategoryDtoGen Category
        {
            get => _Category;
            set { _Category = value; Changed(nameof(Category)); }
        }
        public System.Collections.Generic.ICollection<FinanceApp.Web.Models.SubCategoryCustomCalculationDtoGen> SubCategoryCustomCalculations
        {
            get => _SubCategoryCustomCalculations;
            set { _SubCategoryCustomCalculations = value; Changed(nameof(SubCategoryCustomCalculations)); }
        }
        public int? BudgetId
        {
            get => _BudgetId;
            set { _BudgetId = value; Changed(nameof(BudgetId)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.SubCategory obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.SubCategoryId = obj.SubCategoryId;
            this.Name = obj.Name;
            this.Description = obj.Description;
            this.Allocation = obj.Allocation;
            this.CategoryId = obj.CategoryId;
            this.BudgetId = obj.BudgetId;
            if (tree == null || tree[nameof(this.Category)] != null)
                this.Category = obj.Category.MapToDto<FinanceApp.Data.Models.Category, CategoryDtoGen>(context, tree?[nameof(this.Category)]);

            var propValSubCategoryCustomCalculations = obj.SubCategoryCustomCalculations;
            if (propValSubCategoryCustomCalculations != null && (tree == null || tree[nameof(this.SubCategoryCustomCalculations)] != null))
            {
                this.SubCategoryCustomCalculations = propValSubCategoryCustomCalculations
                    .OrderBy(f => f.SubCategoryCustomCalculationId)
                    .Select(f => f.MapToDto<FinanceApp.Data.Models.SubCategoryCustomCalculation, SubCategoryCustomCalculationDtoGen>(context, tree?[nameof(this.SubCategoryCustomCalculations)])).ToList();
            }
            else if (propValSubCategoryCustomCalculations == null && tree?[nameof(this.SubCategoryCustomCalculations)] != null)
            {
                this.SubCategoryCustomCalculations = new SubCategoryCustomCalculationDtoGen[0];
            }

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Models.SubCategory entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(SubCategoryId))) entity.SubCategoryId = (SubCategoryId ?? entity.SubCategoryId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(Allocation))) entity.Allocation = (Allocation ?? entity.Allocation);
            if (ShouldMapTo(nameof(CategoryId))) entity.CategoryId = (CategoryId ?? entity.CategoryId);
            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.SubCategory MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new FinanceApp.Data.Models.SubCategory()
            {
                Name = Name,
                Allocation = (Allocation ?? default),
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(SubCategoryId))) entity.SubCategoryId = (SubCategoryId ?? entity.SubCategoryId);
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(CategoryId))) entity.CategoryId = (CategoryId ?? entity.CategoryId);
            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);

            return entity;
        }
    }
}
