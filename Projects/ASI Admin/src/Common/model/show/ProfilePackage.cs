using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
    public class ProfilePackage
    {
        public int Id { get; set; }
        public string ProfilePackageName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual IList<ProfilePackageOption> ProfilePackageOptions { get; set; }
    }
}
