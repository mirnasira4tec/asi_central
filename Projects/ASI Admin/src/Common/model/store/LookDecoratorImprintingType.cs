﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class LookDecoratorImprintingType
    {
        public const string IMPRINT_EMBROIDERY = "Embroidery";
        public const string IMPRINT_SCREENPRINTING = "Screen Printing";
        public const string IMPRINT_HEATTRANSFER = "Heat Transfer";
        public const string IMPRINT_DIGITIZING = "Digitizing";
        public const string IMPRINT_ENGRAVING = "Engraving";
        public const string IMPRINT_SUBLIMINATION = "Sublimation";
        public const string IMPRINT_MONOGRAMMING = "Monogramming";
        public const string IMPRINT_OTHER = "Other";

        public int Id { get; set; }
        public string Description { get; set; }
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

            LookDecoratorImprintingType lookup = obj as LookDecoratorImprintingType;
            if (lookup != null) equals = lookup.Id == Id;
            return equals;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
