using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.model.store
{

    public class StoreDetailMagazineAdvertisingItem
    {

        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public virtual LookMagazineIssue Issue { get; set; }
        public virtual LookAdSize Size { get; set; }
        public virtual LookAdPosition Position { get; set; }
        public bool ArtWork { get; set; }
        public int Sequence { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string ProcessId { get; set; }

        public string GetAdSpec()
        {
            string adSpec = string.Empty;
            if (Size != null && Position != null)
            {
                adSpec += string.Format("Ad Size: {0}, Ad Position: {1}", Size.Description, Position.Description);
            }
            return adSpec;
        }

        public override string ToString()
        {
            string result = string.Empty;
            if (Id > 0)
            {
                if (!string.IsNullOrWhiteSpace(GetAdSpec())) result += GetAdSpec();
                if (ProcessId != null)
                {
                    string url = System.Configuration.ConfigurationManager.AppSettings["SendMyAdBaseAddress"] + "/index.php?action=drawAd&processId=";
                    result = string.Format("{0}<br/><a href=\"{1}{2}\">{3}</a>", result, url, ProcessId, Issue.ToString());
                }
                else
                {
                    result = string.Format("{0}<br/>{1}", result, Issue.ToString());
                }
            }
            return result;
        }
    }

    public enum MagazineType : short
    {
        Counselor = 72,
        Advantages = 73,
        Stitches = 74,
        Wearables = 75,
        SGR = 76
    }
}