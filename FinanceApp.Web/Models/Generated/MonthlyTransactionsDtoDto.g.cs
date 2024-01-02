using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class MonthlyTransactionsDtoDtoGen : GeneratedDto<FinanceApp.Data.Services.MonthlyTransactionsDto>
    {
        public MonthlyTransactionsDtoDtoGen() { }

        private System.Collections.Generic.ICollection<System.DateOnly> _StartOfMonth;
        private System.Collections.Generic.ICollection<decimal> _Amount;
        private string _SubCategoryName;
        private string _CategoryColor;

        public System.Collections.Generic.ICollection<System.DateOnly> StartOfMonth
        {
            get => _StartOfMonth;
            set { _StartOfMonth = value; Changed(nameof(StartOfMonth)); }
        }
        public System.Collections.Generic.ICollection<decimal> Amount
        {
            get => _Amount;
            set { _Amount = value; Changed(nameof(Amount)); }
        }
        public string SubCategoryName
        {
            get => _SubCategoryName;
            set { _SubCategoryName = value; Changed(nameof(SubCategoryName)); }
        }
        public string CategoryColor
        {
            get => _CategoryColor;
            set { _CategoryColor = value; Changed(nameof(CategoryColor)); }
        }

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Services.MonthlyTransactionsDto obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.StartOfMonth = obj.StartOfMonth;
            this.Amount = obj.Amount;
            this.SubCategoryName = obj.SubCategoryName;
            this.CategoryColor = obj.CategoryColor;
        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Services.MonthlyTransactionsDto entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(StartOfMonth))) entity.StartOfMonth = StartOfMonth?.ToList();
            if (ShouldMapTo(nameof(Amount))) entity.Amount = Amount?.ToList();
            if (ShouldMapTo(nameof(SubCategoryName))) entity.SubCategoryName = SubCategoryName;
            if (ShouldMapTo(nameof(CategoryColor))) entity.CategoryColor = CategoryColor;
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Services.MonthlyTransactionsDto MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new FinanceApp.Data.Services.MonthlyTransactionsDto()
            {
                StartOfMonth = StartOfMonth?.ToList(),
                Amount = Amount?.ToList(),
                SubCategoryName = SubCategoryName,
                CategoryColor = CategoryColor,
            };

            if (OnUpdate(entity, context)) return entity;

            return entity;
        }
    }
}
