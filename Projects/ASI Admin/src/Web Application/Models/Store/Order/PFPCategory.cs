using System;
using System.Collections.Generic;
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
        public string PaymentAmount { get; set; }
        public string Impressions { get; set; }
    }
}