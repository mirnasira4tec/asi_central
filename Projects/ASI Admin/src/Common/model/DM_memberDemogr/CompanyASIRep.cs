using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.DM_memberDemogr
{
    public class CompanyASIRep
    {
        [Column(Order = 0), Key]
        public int CompanyID { get; set; }
        [Column(Order = 1), Key]
        public int IndividualID { get; set; }
        public string CustomerStatusCode { get; set; }
        public int? MasterCustomerID { get; set; }
        public int? SubCustomerID { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MidleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string NickName { get; set; }
        public string SalutationFormal { get; set; }
        public string SalutationInformal { get; set; }
        [Column(Order = 2), Key]
        public string IndividualRoleCode { get; set; }
        public string RoleDescription { get; set; }
        public string PrimaryEmail { get; set; }
        public string PrimaryAreaCode { get; set; }
        public string PrimaryPhoneBase { get; set; }
        public string PrimaryPhoneExtension { get; set; }
        public string PrimaryPhoneFormatted { get; set; }

    }
}
