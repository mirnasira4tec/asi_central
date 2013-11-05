using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailProductCollection
    {
        public StoreDetailProductCollection()
        {
            if (this.GetType() == typeof(StoreDetailProductCollection))
            {
                ProductCollectionItems = new List<StoreDetailProductCollectionItem>();
            }
        }
        public int ItemMonthId { get; set; }
        public int OrderDetailId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public virtual IList<StoreDetailProductCollectionItem> ProductCollectionItems { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
