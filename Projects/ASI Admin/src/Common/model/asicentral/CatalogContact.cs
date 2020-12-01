using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class CatalogContact
    {
        public int CatalogContactId { get; set; }
        public int CatalogContactImportId { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public decimal? Percentage { get; set; }
        public int OriginalContacts { get; set; }
        public int ManualReservedContacts { get; set; }
        public int RemainingContacts { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public string Note { get; set; }
        public virtual CatalogContactImport CatalogContactImport { get; set; }
        public virtual ICollection<CatalogContactSaleDetail> CatalogContactSaleDetails { get; set; }
    }
}
