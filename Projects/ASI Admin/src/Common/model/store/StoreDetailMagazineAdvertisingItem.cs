using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.model.store
{

    public class StoreDetailMagazineAdvertisingItem
    {

        public int Id { get; set; }

        public int OrderDetailId { get; set; }

        public LookMagazineIssue Issue { get; set; }

        public LookAdSize Size { get; set; }

        public LookAdPosition Position { get; set; }

        public string ArtWork { get; set; }

    }

    public enum MagazineType: short
    {

        Counselor = 72,

        Advantages = 73,

        Stitches = 74,

        Wearables = 75,

        SGR = 76
    }
}