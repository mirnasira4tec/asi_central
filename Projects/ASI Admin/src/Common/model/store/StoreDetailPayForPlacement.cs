using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailPayForPlacement
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public string CategoryName { get; set; }
        public int CPMOption { get; set; }
        public string PaymentType { get; set; }
        public int ImpressionsRequested { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
