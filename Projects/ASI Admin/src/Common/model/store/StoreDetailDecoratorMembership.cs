using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailDecoratorMembership : StoreDetailApplication
    {
        //In the below list of product id's 5 to 8 are distributor products and 29 to 31 are Proforma products
        public static int[] Identifiers = new int[] { 66, 67, 68, 69 };

        public StoreDetailDecoratorMembership()
        {
            if (this.GetType() == typeof(StoreDetailDecoratorMembership))
            {
                ImprintTypes = new List<LookDecoratorImprintingType>();
            }
        }

        public int? BestDescribesOption { get; set; }
        public string BestDescribesOtherDesc { get; set; }
        public bool IsUnionMember { get; set; }
        public virtual IList<LookDecoratorImprintingType> ImprintTypes { get; set; }

        public override string ToString()
        {
            return "Decorator Membership " + OrderDetailId;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreDetailDecoratorMembership decorator = obj as StoreDetailDecoratorMembership;
            if (decorator != null) equals = decorator.OrderDetailId == OrderDetailId;
            return equals;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + "StoreDetailDecoratorMembership".GetHashCode();
            hash = hash * 31 + OrderDetailId.GetHashCode();
            return hash;
        }

        public void CopyTo(StoreDetailDecoratorMembership decorator)
        {
            base.CopyTo(decorator);
            decorator.IsUnionMember = IsUnionMember;
            decorator.BestDescribesOption = BestDescribesOption;
            decorator.BestDescribesOtherDesc = BestDescribesOtherDesc;
            decorator.ImprintTypes = ImprintTypes;
        }
    }
}
