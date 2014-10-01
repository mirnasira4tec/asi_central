using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.findsupplier
{
    public class SupplierPhone
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string PhoneTypeCode { get; set; }
        public string Phone { get; set; }
        public bool IsPrimary { get; set; }
    }
}
