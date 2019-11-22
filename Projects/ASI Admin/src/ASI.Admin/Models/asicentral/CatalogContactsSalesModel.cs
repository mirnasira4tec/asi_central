using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.web.Models.asicentral
{
    public class CatalogContactsSalesModel : PagerBase
    {
        public CatalogContactsSalesModel()
        {
            this.ResultsPerPage = 20;
            Sales = new List<CatalogSales>();
            Industries = new List<SelectListItem>();
            States = new List<SelectListItem>();
            Counties = new List<SelectListItem>();
        }
        // Paging-related properties
        public int ImportId { get; set; }
        public int ContactId { get; set; }
        public string CatalogName { get; set; }
        public string IndustryName { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<SelectListItem> Industries { get; set; }
        public List<CatalogSales> Sales { get; set; }
    }

    public class CatalogSales
    {
        public int CatalogContactSaleId { get; set; }
        public string CompanyName { get; set; }
        public string ASINumber { get; set; }
        public string Email { get; set; }
        public string ASIRep { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public bool IsApproved { get; set; }
        public string Industry { get; set; }
        public int PendingContact { get; set; }
        public int ApprovedContact { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public int PendingSales { get; set; }
        public string OtherOption { get; set; }
       public List<CatalogSalesDetails> salesDetails { get; set; }
    }

    public class CatalogSalesDetails
    {
        public string State { get; set; }
        public string County { get; set; }
        public int PendingSales { get; set; }
        public bool IsPending { get; set; }
    }
}