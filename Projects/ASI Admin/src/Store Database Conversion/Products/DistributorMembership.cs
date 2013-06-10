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
                    SolelyWorkName = application.SolelyWorkName,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,
                };
                //update the product line - ids match between legacy records and new db ones
                foreach (var product in application.ProductLines)
                {
                    LookProductLine newProduct = storeContext.LookProductLines.Where(t => t.Id == product.Id).First();
                    newMembership.ProductLines.Add(newProduct);
                }
                //update the account type - ids match between legacy records and new db ones
                foreach (var accountType in application.AccountTypes)
                {
                    LookDistributorAccountType account = storeContext.LookDistributorAccountTypes.Where(t => t.Id == accountType.Id).First();
                    newMembership.AccountTypes.Add(account);
                }
                //update primary business revenue - ids should match
                if (application.PrimaryBusinessRevenue != null)
                {
                    LookDistributorRevenueType revenue = storeContext.LookDistributorRevenueTypes.Where(t => t.Id == application.PrimaryBusinessRevenue.Id).First();
                    newMembership.PrimaryBusinessRevenue = revenue;
                }
                //always create a new company for transfer
                StoreCompany company = new StoreCompany()
                {
                    Name = application.Company,
                    MemberType = "Distributor",
                    Phone = application.Phone,
                    WebURL = application.BillingWebUrl,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.UpdateDate,
                    UpdateSource = newOrderDetail.UpdateSource,                    
                };
                newOrderDetail.Order.Company = company;
                //@todo we have 3 addresses, they might all be the same. We already have a billing address stored with the order through billing individual
                //@todo need to add the contacts - avoid duplicates
                storeContext.StoreDetailDistributorMemberships.Add(newMembership);
            }
        }
    }
}
