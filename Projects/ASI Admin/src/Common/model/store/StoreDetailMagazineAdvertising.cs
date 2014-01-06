using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.model.store
{

    public class StoreMagazineAdvertisingItem
    {

        public int Id { get; set; }

        public int OrderDetailId { get; set; }

        public MagazineIssue Issue { get; set; }

        public AdSize Size { get; set; }

        public AdPosition Position { get; set; }

        public string ArtWork { get; set; }

    }

    public class MagazineIssue
    {

        public int Id { get; set; }

        public Magazine MagazineNameEnum { get; set; }

        public DateTime Issue { get; set; }

    }

    public class AdPosition
    {
        public int Id { get; set; }

        public MagazineIssue Issue { get; set; }

        public string Description { get; set; }
    }

    public class AdSize
    {

        public int Id { get; set; }

        public MagazineIssue Issue { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }

    public enum Magazine: short
    {

        Counselor = 71,

        Advantages = 72,

        Stitches = 73,

        Wearables = 74,

        SGR = 75
    }
}