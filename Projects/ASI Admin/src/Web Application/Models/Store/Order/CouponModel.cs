using asi.asicentral.interfaces;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.model.store
{
    public class CouponModel
    {
        public int Id { get; set; }
        public bool IsProduct { get; set; }
        public int? ProductId { get; set; }
        public int? ContextId { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CouponCode { get; set; }

        public bool IsSubscription { get; set; }
        public bool IsFixedAmount { get; set; }
        [RegularExpression(@"^[0-9]\d*(\.\d+)?$", ErrorMessageResourceName = "FieldInvalidNumber1", ErrorMessageResourceType = typeof(Resource))]
        public string DiscountAmount { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber1", ErrorMessageResourceType = typeof(Resource))]
        public string DiscountPercentage { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime ValidUpto { get; set; }

        public IList<SelectListItem> Products { get; set; }
        public IList<SelectListItem> Contexts { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string ActionName { get; set; }
    }
}