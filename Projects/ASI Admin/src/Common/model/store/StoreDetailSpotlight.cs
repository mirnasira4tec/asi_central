using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailSpotlight
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public string ItemNumber { get; set; }
        public string SmallImagePath { get; set; }
        public string TextUnderImage { get; set; }
        public string LargeImagePath { get; set; }
        public string TextAroundImage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
