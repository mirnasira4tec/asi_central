using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LookProductLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string MemberType { get; set; }
        public string SubCode { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }

        public override string ToString()
        {
            return Id + " - " + Description;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            LookProductLine lookup = obj as LookProductLine;
            if (lookup != null) equals = lookup.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
