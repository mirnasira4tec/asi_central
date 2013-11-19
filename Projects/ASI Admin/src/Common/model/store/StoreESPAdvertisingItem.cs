using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailESPAdvertisingItem
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public string ItemList { get; set; }
        public string LogoPath { get; set; }
        public string FileName { get; set; }
        public int Sequence { get; set; }
        public DateTime AdSelectedDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
