using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class RateSupplierImport
    {
        public int RateSupplierImportId { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public int NumberOfImports { get; set; }
        public bool IsActive { get; set; }
        public virtual List<RateSupplierForm> RateSupplierForms { get; set; }
    }
}
