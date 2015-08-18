using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.show
{
   public class ShowCompanyAddress
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
       // public int? AddressId { get; set; }
        public virtual ShowCompany Company { get; set; }
        public virtual ShowAddress Address { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
    }
}
