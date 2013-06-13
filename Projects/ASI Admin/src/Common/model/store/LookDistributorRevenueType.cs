using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LookDistributorRevenueType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public override string ToString()
        {
            return Id + " - " + Name;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            LookDistributorRevenueType lookup = obj as LookDistributorRevenueType;
            if (lookup != null) equals = lookup.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
