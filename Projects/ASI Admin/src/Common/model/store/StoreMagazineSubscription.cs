using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.Resources;

namespace asi.asicentral.model.store
{
    public class StoreMagazineSubscription
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public string Signature { get; set; }
        public string LegacyTableId { get; set; }
        public string LegacyId { get; set; }
        public int ASINumber { get; set; }
        public string CompanyName { get; set; }
        public bool IsDigitalVersion { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "PrimaryBusiness")]
        public string PrimaryBusiness { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "PrimaryBusinessOtherDescription")]
        public string PrimaryBusinessOtherDesc { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        public virtual StoreIndividual Contact { get; set; }
    }
}
