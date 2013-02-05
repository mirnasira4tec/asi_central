using System;
using System.Collections.Generic;

namespace asi.asicentral.model.store
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string LgImg { get; set; }
        public string LgImgHover { get; set; }
        public string MedImg { get; set; }
        public string MedImgHover { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> PriceUnitId { get; set; }
        public Nullable<bool> Display { get; set; }
        public Nullable<bool> Taxable { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<int> LocaleId { get; set; }
        public Nullable<int> TermId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public override string ToString()
        {
            return string.Format("Product of {0} - {1}", Id, Description);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            OrderProduct product = obj as OrderProduct;
            if (product != null) equals = product.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
