using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class SubCategoryCustomCalculationDtoGen : GeneratedDto<FinanceApp.Data.Models.SubCategoryCustomCalculation>
    {
        public SubCategoryCustomCalculationDtoGen() { }

        private int? _SubCategoryCustomCalculationId;
        private int? _SubCategoryId;
        private FinanceApp.Web.Models.SubCategoryDtoGen _SubCategory;
        private int? _CustomCalculationId;
        private FinanceApp.Web.Models.CustomCalculationDtoGen _CustomCalculation;
        private int? _BudgetId;

        public int? SubCategoryCustomCalculationId
        {
            get => _SubCategoryCustomCalculationId;
            set { _SubCategoryCustomCalculationId = value; Changed(nameof(SubCategoryCustomCalculationId)); }
        }
        public int? SubCategoryId
        {
            get => _SubCategoryId;
            set { _SubCategoryId = value; Changed(nameof(SubCategoryId)); }
        }
        public FinanceApp.Web.Models.SubCategoryDtoGen SubCategory
        {
            get => _SubCategory;
            set { _SubCategory = value; Changed(nameof(SubCategory)); }
        }
        public int? CustomCalculationId
        {
            get => _CustomCalculationId;
            set { _CustomCalculationId = value; Changed(nameof(CustomCalculationId)); }
        }
        public FinanceApp.Web.Models.CustomCalculationDtoGen CustomCalculation
        {
            get => _CustomCalculation;
            set { _CustomCalculation = value; Changed(nameof(CustomCalculation)); }
        }
        public int? BudgetId
        {
            get => _BudgetId;
            set { _BudgetId = value; Changed(nameof(BudgetId)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.SubCategoryCustomCalculation obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.SubCategoryCustomCalculationId = obj.SubCategoryCustomCalculationId;
            this.SubCategoryId = obj.SubCategoryId;
            this.CustomCalculationId = obj.CustomCalculationId;
            this.BudgetId = obj.BudgetId;
            if (tree == null || tree[nameof(this.SubCategory)] != null)
                this.SubCategory = obj.SubCategory.MapToDto<FinanceApp.Data.Models.SubCategory, SubCategoryDtoGen>(context, tree?[nameof(this.SubCategory)]);

            if (tree == null || tree[nameof(this.CustomCalculation)] != null)
                this.CustomCalculation = obj.CustomCalculation.MapToDto<FinanceApp.Data.Models.CustomCalculation, CustomCalculationDtoGen>(context, tree?[nameof(this.CustomCalculation)]);

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Models.SubCategoryCustomCalculation entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(SubCategoryCustomCalculationId))) entity.SubCategoryCustomCalculationId = (SubCategoryCustomCalculationId ?? entity.SubCategoryCustomCalculationId);
            if (ShouldMapTo(nameof(SubCategoryId))) entity.SubCategoryId = (SubCategoryId ?? entity.SubCategoryId);
            if (ShouldMapTo(nameof(CustomCalculationId))) entity.CustomCalculationId = (CustomCalculationId ?? entity.CustomCalculationId);
            if (ShouldMapTo(nameof(BudgetId))) entity.BudgetId = (BudgetId ?? entity.BudgetId);
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.SubCategoryCustomCalculation MapToNew(IMappingContext context)
        {
            var entity = new FinanceApp.Data.Models.SubCategoryCustomCalculation();
            MapTo(entity, context);
            return entity;
        }
    }
}
