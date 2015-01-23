using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreSupplierRepresentativeInformation
    {
        public static readonly string[] SUPPLIER_REPRESENTATIVES = { Resources.Resource.Executive, Resources.Resource.Sales, Resources.Resource.Orders, Resources.Resource.CreditManager, Resources.Resource.CustomerService, Resources.Resource.Marketing, Resources.Resource.Artwork };

        public int Id { get; set; }
        public string Role { get; set; }
        public virtual StoreOrderDetail OrderDetail { get; set; }
        public int? OrderDetailId { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Email { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Phone { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string Fax { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        
        public override string ToString()
        {
            return "Equipment Membership " + OrderDetailId;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreSupplierRepresentativeInformation equipment = obj as StoreSupplierRepresentativeInformation;
            if (equipment != null) equals = equipment.OrderDetailId == OrderDetailId;
            return equals;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + "StoreSupplierRepresentativeInformation".GetHashCode();
            hash = hash * 31 + OrderDetailId.GetHashCode();
            return hash;
        }
    }
}
