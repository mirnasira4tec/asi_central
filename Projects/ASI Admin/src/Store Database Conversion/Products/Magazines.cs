using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion.Products
{
    class Magazines : BaseProductConvert
    {
        public override void Convert(StoreOrderDetail newOrderDetail, asi.asicentral.model.store.LegacyOrderDetail detail, asi.asicentral.database.StoreContext storeContext, asi.asicentral.database.ASIInternetContext asiInternetContext)
        {
            StoreOrder neworder = newOrderDetail.Order;
            LegacyOrder order = detail.Order;
            //Process the addresses
            foreach (LegacyOrderAddress orderAddress in order.Addresses)
            {
                StoreIndividual newIndividual = new StoreIndividual()
                {
                    IsPrimary = false,
                    FirstName = orderAddress.SPAD_FirstName,
                    LastName = orderAddress.SPAD_LastName,
                    Email = orderAddress.SPAD_Email,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.CreateDate,
                    UpdateSource = "Migration Process - " + DateTime.Now,
                };
                storeContext.StoreIndividuals.Add(newIndividual);
                StoreMagazineSubscription subscription = new StoreMagazineSubscription()
                {
                    Contact = newIndividual,
                    LegacyId = orderAddress.SPAD_AddressID.ToString(),
                    LegacyTableId = "STOR_SPAddresses_SPAD",
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.CreateDate,
                    UpdateSource = "Migration Process - " + DateTime.Now,
                };
                storeContext.StoreMagazineSubscriptions.Add(subscription);
                newOrderDetail.MagazineSubscriptions.Add(subscription);
                if (!string.IsNullOrEmpty(orderAddress.SPAD_Street1) && !string.IsNullOrEmpty(orderAddress.SPAD_City))
                {
                    StoreAddress indivdualAddress = new StoreAddress()
                    {
                        Street1 = orderAddress.SPAD_Street1,
                        Street2 = orderAddress.SPAD_Street2,
                        City = orderAddress.SPAD_City,
                        State = orderAddress.SPAD_StateID,
                        Country = "USA",
                        Zip = orderAddress.SPAD_Zip,
                        CreateDate = newOrderDetail.CreateDate,
                        UpdateDate = newOrderDetail.CreateDate,
                        UpdateSource = "Migration Process - " + DateTime.Now,
                    };
                    storeContext.StoreAddresses.Add(indivdualAddress);
                    newIndividual.Address = indivdualAddress;
                }
            }
            List<LegacyOrderMagazineAddress> magazineAddresses = asiInternetContext.LegacyOrderMagazineAddresses.Where(t => t.OrderID == order.Id && t.ProdID == detail.ProductId).ToList();
            foreach (LegacyOrderMagazineAddress address in magazineAddresses)
            {
                LegacyMagazineAddress magazineAddress = address.Address;
                StoreIndividual newIndividual = new StoreIndividual()
                {
                    IsPrimary = false,
                    Title = magazineAddress.MAGA_Title,
                    FirstName = magazineAddress.MAGA_FName,
                    LastName = magazineAddress.MAGA_LName,
                    Email = magazineAddress.MAGA_Email,
                    Phone = magazineAddress.MAGA_Phone,
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.CreateDate,
                    UpdateSource = "Migration Process - " + DateTime.Now,
                };
                storeContext.StoreIndividuals.Add(newIndividual);
                StoreMagazineSubscription subscription = new StoreMagazineSubscription()
                {
                    Contact = newIndividual,
                    LegacyId = magazineAddress.MAGA_SubscribeID.ToString(),
                    LegacyTableId = "STOR_MagSbcrLong_MAGA",
                    CompanyName = magazineAddress.MAGA_Company,
                    IsDigitalVersion = (magazineAddress.MAGA_Digital.HasValue ? magazineAddress.MAGA_Digital.Value : false),
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.CreateDate,
                    UpdateSource = "Migration Process - " + DateTime.Now,
                };
                int asiNumber = 0;
                if (!string.IsNullOrEmpty(magazineAddress.MAGA_ASINo)) Int32.TryParse(magazineAddress.MAGA_ASINo, out asiNumber);
                if (asiNumber > 0) subscription.ASINumber = asiNumber;
                storeContext.StoreMagazineSubscriptions.Add(subscription);
                newOrderDetail.MagazineSubscriptions.Add(subscription);
                if (!string.IsNullOrEmpty(magazineAddress.MAGA_Street1) && !string.IsNullOrEmpty(magazineAddress.MAGA_City))
                {
                    StoreAddress indivdualAddress = new StoreAddress()
                    {
                        Street1 = magazineAddress.MAGA_Street1,
                        Street2 = magazineAddress.MAGA_Street2,
                        City = magazineAddress.MAGA_City,
                        State = magazineAddress.MAGA_State,
                        Country = magazineAddress.MAGA_Country,
                        Zip = magazineAddress.MAGA_Zip,
                        Phone = magazineAddress.MAGA_Phone,
                        CreateDate = newOrderDetail.CreateDate,
                        UpdateDate = newOrderDetail.CreateDate,
                        UpdateSource = "Migration Process - " + DateTime.Now,
                    };
                    storeContext.StoreAddresses.Add(indivdualAddress);
                    newIndividual.Address = indivdualAddress;
                }
            }
        }
    }
}
