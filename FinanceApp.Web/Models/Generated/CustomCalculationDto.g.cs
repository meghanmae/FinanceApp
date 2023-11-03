using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class CustomCalculationDtoGen : GeneratedDto<FinanceApp.Data.Models.CustomCalculation>
    {
        public CustomCalculationDtoGen() { }

        private int? _CustomCalculationId;
        private string _Name;
        private string _Description;
        private System.Collections.Generic.ICollection<FinanceApp.Web.Models.SubCategoryCustomCalculationDtoGen> _SubCategoryCustomCalculations;

        public int? CustomCalculationId
        {
            get => _CustomCalculationId;
            set { _CustomCalculationId = value; Changed(nameof(CustomCalculationId)); }
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
        public System.Collections.Generic.ICollection<FinanceApp.Web.Models.SubCategoryCustomCalculationDtoGen> SubCategoryCustomCalculations
        {
            get => _SubCategoryCustomCalculations;
            set { _SubCategoryCustomCalculations = value; Changed(nameof(SubCategoryCustomCalculations)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.CustomCalculation obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.CustomCalculationId = obj.CustomCalculationId;
            this.Name = obj.Name;
            this.Description = obj.Description;
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
        public override void MapTo(FinanceApp.Data.Models.CustomCalculation entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(CustomCalculationId))) entity.CustomCalculationId = (CustomCalculationId ?? entity.CustomCalculationId);
            if (ShouldMapTo(nameof(Name))) entity.Name = Name;
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.CustomCalculation MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new FinanceApp.Data.Models.CustomCalculation()
            {
                Name = Name,
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(CustomCalculationId))) entity.CustomCalculationId = (CustomCalculationId ?? entity.CustomCalculationId);
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;

            return entity;
        }
    }
}
