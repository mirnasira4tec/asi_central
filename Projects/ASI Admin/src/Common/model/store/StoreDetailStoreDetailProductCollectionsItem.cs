using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailProductCollectionItem
    {
        public int ItemId { get; set; }
        public int ItemMonthId { get; set; }
        public int Sequence { get; set; }
        public string Collection { get; set; }
        public string ItemNumbers { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
