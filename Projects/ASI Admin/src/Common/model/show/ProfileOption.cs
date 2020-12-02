using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ProfileOption
    {
        public int Id { get; set; }
        public string ProfileOptionName { get; set; }
        public int Sequence { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public bool IsDMHP { get; set; }        
        public string OptionRule { get; set; }
        public bool IsRequired { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public string Label { get; set; }
        public virtual IList<ProfileOptionValue> ProfileOptionValues { get; set; }
    }
}
