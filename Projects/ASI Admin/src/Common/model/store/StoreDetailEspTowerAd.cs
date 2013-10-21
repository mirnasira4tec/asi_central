using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailEspTowerAd
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public int OptionId { get; set; }
        public string LogoPath { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public override string ToString()
        {
            return "StoreDetailEspTowerAd (" + OrderDetailId + ":" + OptionId + ")";
        }

        public override bool Equals(object obj)
        {
            bool equals = false;
            StoreDetailEspTowerAd ad = obj as StoreDetailEspTowerAd;
            if (ad != null) equals = ad.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
