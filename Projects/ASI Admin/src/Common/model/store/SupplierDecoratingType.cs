using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class SupplierDecoratingType
    {
        public const string DECORATION_ETCHING = "Etching";
        public const string DECORATION_HOTSTAMPING = "const";
        public const string DECORATION_SILKSCREEN = "Silkscreen";
        public const string DECORATION_PADPRINT = "Pad Print";
        public const string DECORATION_DIRECTEMBROIDERY = "Direct Embroidery";
        public const string DECORATION_FOILSTAMPING = "Foil Stamping";
        public const string DECORATION_LITHOGRAPHY = "Lithography";
        public const string DECORATION_SUBLIMINATION = "Sublimation";
        public const string DECORATION_FOURCOLOR = "Four Color Process";
        public const string DECORATION_ENGRAVING = "Engraving";
        public const string DECORATION_LASER = "Laser";
        public const string DECORATION_OFFSET = "Offset";
        public const string DECORATION_TRANSFER = "Transfer";
        public const string DECORATION_FULLCOLOR = "Full Color Process";
        public const string DECORATION_DIESTAMP = "Die Stamp";

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<SupplierMembershipApplication> SupplierApplications { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
