using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ProfilePackageOption
    {
        public int Id { get; set; }
        public int ProfilePackageId { get; set; }
        public int ProfileOptionId { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime UpdateDateUTC { get; set; }
        public string UpdateSource { get; set; }
        public virtual ProfilePackage ProfilePackage { get; set; }
        public virtual ProfileOption ProfileOption { get; set; }
    }
}
