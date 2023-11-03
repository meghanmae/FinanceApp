using IntelliTect.Coalesce;
using IntelliTect.Coalesce.Mapping;
using IntelliTect.Coalesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FinanceApp.Web.Models
{
    public partial class TransactionDtoGen : GeneratedDto<FinanceApp.Data.Models.Transaction>
    {
        public TransactionDtoGen() { }

        private long? _TransactionId;
        private string _Description;
        private decimal? _Amount;
        private int? _SubCategoryId;
        private FinanceApp.Web.Models.SubCategoryDtoGen _SubCategory;

        public long? TransactionId
        {
            get => _TransactionId;
            set { _TransactionId = value; Changed(nameof(TransactionId)); }
        }
        public string Description
        {
            get => _Description;
            set { _Description = value; Changed(nameof(Description)); }
        }
        public decimal? Amount
        {
            get => _Amount;
            set { _Amount = value; Changed(nameof(Amount)); }
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

        /// <summary>
        /// Map from the domain object to the properties of the current DTO instance.
        /// </summary>
        public override void MapFrom(FinanceApp.Data.Models.Transaction obj, IMappingContext context, IncludeTree tree = null)
        {
            if (obj == null) return;
            var includes = context.Includes;

            this.TransactionId = obj.TransactionId;
            this.Description = obj.Description;
            this.Amount = obj.Amount;
            this.SubCategoryId = obj.SubCategoryId;
            if (tree == null || tree[nameof(this.SubCategory)] != null)
                this.SubCategory = obj.SubCategory.MapToDto<FinanceApp.Data.Models.SubCategory, SubCategoryDtoGen>(context, tree?[nameof(this.SubCategory)]);

        }

        /// <summary>
        /// Map from the current DTO instance to the domain object.
        /// </summary>
        public override void MapTo(FinanceApp.Data.Models.Transaction entity, IMappingContext context)
        {
            var includes = context.Includes;

            if (OnUpdate(entity, context)) return;

            if (ShouldMapTo(nameof(TransactionId))) entity.TransactionId = (TransactionId ?? entity.TransactionId);
            if (ShouldMapTo(nameof(Description))) entity.Description = Description;
            if (ShouldMapTo(nameof(Amount))) entity.Amount = (Amount ?? entity.Amount);
            if (ShouldMapTo(nameof(SubCategoryId))) entity.SubCategoryId = (SubCategoryId ?? entity.SubCategoryId);
        }

        /// <summary>
        /// Map from the current DTO instance to a new instance of the domain object.
        /// </summary>
        public override FinanceApp.Data.Models.Transaction MapToNew(IMappingContext context)
        {
            var includes = context.Includes;

            var entity = new FinanceApp.Data.Models.Transaction()
            {
                Description = Description,
                Amount = (Amount ?? default),
            };

            if (OnUpdate(entity, context)) return entity;
            if (ShouldMapTo(nameof(TransactionId))) entity.TransactionId = (TransactionId ?? entity.TransactionId);
            if (ShouldMapTo(nameof(SubCategoryId))) entity.SubCategoryId = (SubCategoryId ?? entity.SubCategoryId);

            return entity;
        }
    }
}
