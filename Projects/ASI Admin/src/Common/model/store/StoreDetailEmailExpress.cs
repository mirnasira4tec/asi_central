using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailEmailExpress
    {
        public StoreDetailEmailExpress()
        {
            if (this.GetType() == typeof(StoreDetailEmailExpress))
            {
                EmailExpressItems = new List<StoreDetailEmailExpressItem>();
            }
        }

        public int OrderDetailId { get; set; }
        public int ItemTypeId { get; set; }
        public int NumberOfDates { get; set; }
        public virtual IList<StoreDetailEmailExpressItem> EmailExpressItems { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
