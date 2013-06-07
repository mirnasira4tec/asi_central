using asi.asicentral.database;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion.Products
{
    class DistributorMembership : BaseProductConvert
    {
        public override void Convert(StoreOrderDetail newOrderDetail, LegacyOrderDetail detail, StoreContext storeContext, ASIInternetContext asiInternetContext)
        {
            //retrieve the current application
            LegacyDistributorMembershipApplication application = asiInternetContext.DistributorMembershipApplications.Where(app => app.UserId == detail.Order.UserId).SingleOrDefault();
            if (application == null && !IgnoreOrderIssues(detail))
            {
                ILogService logService = LogService.GetLog(this.GetType());
                logService.Error("Expected to find and application and could not retrieve it: " + detail.Order.Id);
            }
            else if (application != null)
            {
                //creating a new application
                StoreDetailDistributorMembership newMembership = new StoreDetailDistributorMembership()
                {
                    OrderDetailId = newOrderDetail.Id,
                    AnnualSalesVolume = application.AnnualSalesVolume,
                    AnnualSalesVolumeASP = application.AnnualSalesVolumeASP,
                    AppStatusId = application.ApplicationStatusId,
                    ASIContactName = application.ASIContact,
                    //@todo BusinessRevenue
                    Custom1 = application.Custom1,
                    Custom2 = application.Custom2,
                    Custom5 = application.Custom5,
                    EstablishedDate = application.EstablishedDate,
                    //@todo missing from Legacy? HasRecSpecials
                    IsCorporateOfficer = application.CorporateOfficer,
                    IsForProfit = application.IsForProfit,
                    IsMajorForResale = application.IsMajorForResale,
                    IsMajorityDistributeForResale = application.IsMajorityDistributeForResale,
                    IsSolelyWork = application.IsSolelyWork,
                    LegacyApplicationId = application.Id.ToString(),
                    NumberOfEmployee = application.NumberOfEmployee,
                    NumberOfSalesEmployee = application.NumberOfSalesEmployee,
                    OtherBusinessRevenue = application.OtherBusinessRevenue,
                    //@todo ProductLines = application.ProductLines,
                    SolelyWorkName = application.SolelyWorkName,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,
                };
                storeContext.StoreDetailDistributorMemberships.Add(newMembership);
            }
        }
    }
}
