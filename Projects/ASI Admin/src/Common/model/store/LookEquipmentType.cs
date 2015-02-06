using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LookEquipmentType
    {
        public const string EMBROIDERY = "Embroidery";
        public const string SCREENPRINTING = "Screen Printing";
        public const string HEATTRANSFER = "Heat Transfer";
        public const string DIGITIZING = "Digitizing";
        public const string ENGRAVING = "Engraving";
        public const string SUBLIMITION = "Sublimation";
        public const string MONOGRAMING = "Monogramming";
        
        public int Id { get; set; }
        public string Description { get; set; }
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

            LookEquipmentType lookup = obj as LookEquipmentType;
            if (lookup != null) equals = lookup.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
