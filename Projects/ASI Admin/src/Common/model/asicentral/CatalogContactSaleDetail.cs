using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class CatalogContactSaleDetail
    {
        public int CatalogContactSaleDetailId { get; set; }
        public int CatalogContactSaleId { get; set; }
        public int CatalogContactId { get; set; }
        public int ContactsRequested { get; set; }
        public int? ContactsApproved { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public virtual CatalogContactSale CatalogContactSale { get; set; }
        public virtual CatalogContact CatalogContacts { get; set; }

    }
}
