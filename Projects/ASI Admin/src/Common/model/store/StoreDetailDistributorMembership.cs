using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailDistributorMembership
    {
        public int OrderDetailId { get; set; }
        public string LegacyApplicationId { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ApplicationStatus")]
        public int? AppStatusId { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "NumberOfEmployee")]
        public int? NumberOfEmployee { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "NumberOfSalesEmployee")]
        public int? NumberOfSalesEmployee { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "AnnualSalesVolume")]
        public string AnnualSalesVolume { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "AnnualSalesVolumeASP")]
        public string AnnualSalesVolumeASP { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ASIContact")]
        public string ASIContactName { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "CorporateOfficer")]
        public bool? IsCorporateOfficer { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsMajorForResale")]
        public bool? IsMajorForResale { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsForProfit")]
        public bool? IsForProfit { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsSolelyWork")]
        public bool? IsSolelyWork { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "SolelyWorkName")]
        public string SolelyWorkName { get; set; }
        public bool? HasRecSpecials { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsMajorityDistributeForResale")]
        public bool? IsMajorityDistributeForResale { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom5 { get; set; }
        public LookDistributorAccountType BusinessRevenue { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "OtherBusinessRevenue")]
        public string OtherBusinessRevenue { get; set; }
        public Nullable<DateTime> EstablishedDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateSource { get; set; }
        //@todo this is a relationship
        public string ProductLines { get; set; }
    }
}
