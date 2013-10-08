using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class PFPCategory
    {
        public bool IsSelected { get; set; }
        public string CategoryName { get; set; }
        public int CPMOption { get; set; }
        public string PaymentOption { get; set; }
        [RegularExpression(@"^[1-9]\d*(\.\d+)?$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        public string PaymentAmount { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        public string Impressions { get; set; }
    }
}