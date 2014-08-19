using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.findsupplier
{
    public class SupplierPolicy
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int PloicyId { get; set; }
        public string Ploicy { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateSource { get; set; }
    }
}
