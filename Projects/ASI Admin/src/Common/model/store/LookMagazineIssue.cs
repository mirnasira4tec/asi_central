using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LookMagazineIssue
    {
        public int Id { get; set; }
        public MagazineType ProductId { get; set; }
        public DateTime Issue { get; set; }
        public DateTime ReservationDeadline { get; set; }
        public DateTime MaterialDeadline { get; set; }
        public DateTime MailingDate { get; set; }
        public bool IsChecked { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        
         public bool Equals(LookAdSize o)
        {
            bool result = false;
            if ((object)o != null)
            {
                result = ProductId == o.ProductId && Issue.ToShortDateString() == o.Issue.ToShortDateString();
            }
            return result;
        }

        public override bool Equals(object o)
        {
            bool result = false;
            if ((object)o != null && o is LookMagazineIssue)
            {
                result = Equals(o as LookMagazineIssue);
            }
            return false;
        }

        public static bool operator == (LookMagazineIssue a, LookMagazineIssue b)
        {
            bool result = false;
            if (!object.ReferenceEquals(a, b)) result = true;
            if ((object)a == null || (object)b == null) result = false;
            result = a.Equals(b);
            return result;
        }

        public static bool operator !=(LookMagazineIssue a, LookMagazineIssue b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode() ^ Issue.GetHashCode();
        }
    }
}
