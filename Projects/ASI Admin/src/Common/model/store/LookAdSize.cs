using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{

    public class LookAdSize : IEquatable<LookAdSize>
    {
        public int Id { get; set; }
        public MagazineType MagazineId { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public string Color { get; set; }
        public bool Equals(LookAdSize o)
        {
            bool result = false;
            if ((object)o != null)
            {
                result = MagazineId == o.MagazineId && Description == o.Description && Color == o.Color;
            }
            return result;
        }

        public override bool Equals(object o)
        {
            bool result = false;
            if ((object)o != null && o is LookAdSize)
            {
                result = Equals(o as LookAdSize);
            }
            return false;
        }

        public static bool operator == (LookAdSize a, LookAdSize b)
        {
            bool result = false;
            if (object.ReferenceEquals(a, b)) result = true;
            if ((object)a == null || (object)b == null) result = false;
            result = a.Equals(b);
            return result;
        }

        public static bool operator !=(LookAdSize a, LookAdSize b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            int code = MagazineId.GetHashCode() ^ Description.GetHashCode();
            code = Color == null ? code : code ^ Color.GetHashCode();
            return code;
        }

        public override string ToString()
        {
            string size = string.Format("Ad Size: {0}", Description);
            if(!string.IsNullOrWhiteSpace(Color)) size = string.Format("{0}, Ad Color: {1}", size, Color);
            return size;
        }
    }
}
