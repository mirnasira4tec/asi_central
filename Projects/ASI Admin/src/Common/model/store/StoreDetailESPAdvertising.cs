using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailESPAdvertising
    {
        public StoreDetailESPAdvertising()
        {
            if (this.GetType() == typeof(StoreDetailESPAdvertising))
            {
                ESPAdvertisingItems = new List<StoreDetailESPAdvertisingItem>();
            }
        }

        public int OrderDetailId { get; set; }
        public int FirstOptionId { get; set; }
        public string FirstItemList { get; set; }
        public int SecondOptionId { get; set; }
        public string SecondItemList { get; set; }
        public int ThirdOptionId { get; set; }
        public string ThirdItemList { get; set; }
        public string LogoPath { get; set; }
        [DataType(DataType.Date)]
        public DateTime? AdSelectedDate { get; set; }
        public virtual IList<StoreDetailESPAdvertisingItem> ESPAdvertisingItems { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
