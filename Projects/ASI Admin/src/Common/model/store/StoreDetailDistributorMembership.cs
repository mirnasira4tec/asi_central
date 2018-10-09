﻿using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.store
{
    public class StoreDetailDistributorMembership : StoreDetailApplication
    {
        //In the below list of product id's 5 to 8, 81 are distributor products 
        //and 29 to 31 are Proforma products
        public static int[] Identifiers = new int[] { 5, 6, 7, 8, 81, 133, 134, 135, 136 };
        
        public StoreDetailDistributorMembership()
        {
            if (this.GetType() == typeof(StoreDetailDistributorMembership))
            {
                ProductLines = new List<LookProductLine>();
                AccountTypes = new List<LookDistributorAccountType>();
            }
        }
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
        [StringLength(250, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength250")]
        [Display(ResourceType = typeof(Resource), Name = "OtherBusinessRevenue")]
        public string OtherBusinessRevenue { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "YearEst")]
        public Nullable<DateTime> EstablishedDate { get; set; }
        public virtual LookDistributorRevenueType PrimaryBusinessRevenue { get; set; }
        public virtual IList<LookDistributorAccountType> AccountTypes{ get; set; }
        public virtual IList<LookProductLine> ProductLines { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "IsAuthorizedToBindCompany")]
        public bool? IsAuthorizedToBindCompany { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsForResale")]
        public bool? IsForResale { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsOnlyProfitReseller")]
        public bool? IsOnlyProfitReseller { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsDetailsProvider")]
        public bool? IsDetailsProvider { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsApplyingForMembership")]
        public bool? IsApplyingForMembership { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsChangesInformed")]
        public bool? IsChangesInformed { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "IsDataCertified")]
        public bool? IsDataCertified { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "OtherCompanyName")]
        public string OtherCompanyName { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "ApprovedSignature")]
        public string ApprovedSignature { get; set; }

        public override string ToString()
        {
            return "Distributor Membership " + OrderDetailId;
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            StoreDetailDistributorMembership distributor = obj as StoreDetailDistributorMembership;
            if (distributor != null) equals = distributor.OrderDetailId == OrderDetailId;
            return equals;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + "StoreDetailDistributorMembership".GetHashCode();
            hash = hash * 31 + OrderDetailId.GetHashCode();
            return hash;
        }

        public void CopyTo(StoreDetailDistributorMembership distributor)
        {
            base.CopyTo(distributor);
            distributor.AccountTypes = AccountTypes;
            distributor.AnnualSalesVolume = AnnualSalesVolume;
            distributor.AnnualSalesVolumeASP = AnnualSalesVolumeASP;
            distributor.ASIContactName = ASIContactName;
            distributor.Custom1 = Custom1;
            distributor.Custom2 = Custom2;
            distributor.Custom5 = Custom5;
            distributor.EstablishedDate = EstablishedDate;
            distributor.HasRecSpecials = HasRecSpecials;
            distributor.IsCorporateOfficer = IsCorporateOfficer;
            distributor.IsForProfit = IsForProfit;
            distributor.IsMajorForResale = IsMajorForResale;
            distributor.IsMajorityDistributeForResale = IsMajorityDistributeForResale;
            distributor.IsSolelyWork = IsSolelyWork;
            distributor.NumberOfEmployee = NumberOfEmployee;
            distributor.NumberOfSalesEmployee = NumberOfSalesEmployee;
            distributor.OtherBusinessRevenue = OtherBusinessRevenue;
            distributor.PrimaryBusinessRevenue = PrimaryBusinessRevenue;
            distributor.ProductLines = ProductLines;
            distributor.SolelyWorkName = SolelyWorkName;
        }
    }
}
