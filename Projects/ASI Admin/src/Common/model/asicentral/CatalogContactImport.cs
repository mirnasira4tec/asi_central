using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class CatalogContactImport
    {
        public int CatalogContactImportId { get; set; }
        public string IndustryName { get; set; }
        public string ImportedBy { get; set; }
        public bool IsActive { get; set; }
        public string CatalogName { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public virtual ICollection<CatalogContact> CatalogContacts { get; set; }
    }
}
