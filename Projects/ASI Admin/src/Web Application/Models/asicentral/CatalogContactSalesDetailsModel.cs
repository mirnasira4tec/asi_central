using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.web.Models.asicentral
{
    
    public class CatalogContactSalesDetailsModel
    {
        public CatalogContactSalesDetailsModel()
        {
            CatalogContactSaleDetails = new List<CatalogContactSaleDetail>();
            CatalogContactSale = new List<CatalogContactSale>();
        }
        public List<CatalogContactSaleDetail> CatalogContactSaleDetails { get; set; }
        public List<CatalogContactSale> CatalogContactSale { get; set; }
        public CatalogContactSale CatalogRequested { get; set; }
    }
}