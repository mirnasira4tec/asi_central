using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
   public class ProfileOptionValue
    {
        public int Id { get; set; }
        public int ProfileOptionId { get; set; }
        public string Value { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public virtual ProfileOption ProfileOption { get; set; }
    }
}
