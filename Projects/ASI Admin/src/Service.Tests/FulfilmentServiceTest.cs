using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.model.store;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.database.mappings;
using System.Globalization;
using System.Collections.Generic;
using asi.asicentral.model.timss;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class FulfilmentServiceTest
    {
        [TestMethod]
        public void SupplierOrder()
        {
            #region create the order to store
            Order order = new Order()
            {
                BillFirstName = "Bill First Name",
                BillLastName = "Bill Last Name",
                BillStreet1 = "Bill Street1",
                BillStreet2 = "Bill Street2",
                BillCity = "Bill City",
                BillZip = "Bill Zip",
                BillState = "Bill State",
                BillCountry = "Bill Country",
                ExternalReference = "000008939544", //TIMSS ID
                UserId = Guid.NewGuid(),
                CreditCard = new OrderCreditCard()
                {
                    Type = "VISA",
                    Number = "***1234",
                    ExpMonth = "01",
                    ExpYear = "2016",
                    ExternalReference = Guid.NewGuid().ToString(),
                    Name = "First Last",
                    TotalAmount = 199,
                },
            };
            order.OrderDetails.Add(new OrderDetail()
            {
                ExternalReference = "192",
                ProductId = OrderProduct.SUPPLIER_APPLICATION,
                Quantity = 1,
            });
            //create the supplier application
            SupplierMembershipApplication application = new SupplierMembershipApplication()
            {
                Company = "Company Name",
                Address1 = "Address1",
                Address2 = "Address2",
                City = "City",
                Zip = "Zip",
                State = "State",
                Country = "USA",
                BillingAddress1 = "Bill Street1",
                BillingAddress2 = "Bill Street2",
                BillingCity = "Bill City",
                BillingZip = "Bill Zip",
                BillingState = "Bill State",
                BillingCountry = "USA",
                ShippingStreet1 = "Ship Street1",
                ShippingStreet2 = "Ship Street2",
                ShippingCity = "Ship City",
                ShippingZip = "Ship Zip",
                ShippingState = "Ship State",
                ShippingCountry = "USA",
                ContactName = "First Last",
                ContactTitle = "Contact Title",
                ContactEmail = "Contact@asi.com",
                ContactPhone = "123 123 1234",
                HasBillAddress = true,
                HasShipAddress = true,
                IsManufacturer = true,
                IsImporter = true,
                IsRetailer = false,
            };
            application.Contacts.Add(new SupplierMembershipApplicationContact()
            {
                IsPrimary = true,
                Name = "First Last",
                Title = "Contact Title",
                Phone = "123 123 1234",
                Email = "Contact@asi.com",
            });
            application.Contacts.Add(new SupplierMembershipApplicationContact()
            {
                IsPrimary = false,
                Name = "First2 Last2",
                Title = "Contact Title2",
                Phone = "987 987 9876",
                Email = "Contact2@asi.com",
            });
            #endregion create the order to store

            using (IFulfilmentService fulfilmentService = new TIMSSService(new ObjectService(new Container(new EFRegistry()))))
            {
                fulfilmentService.Process(order, application);
            }
            //check the database to see if the records were added
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                //make sure company record is created
                TIMSSCompany company = objectService.GetAll<TIMSSCompany>(true).Where(comp => comp.DAPP_UserId == order.UserId.Value).SingleOrDefault();
                Assert.IsNotNull(company);
                //make sure a credit card record is created
                TIMSSCreditInfo creditCard = objectService.GetAll<TIMSSCreditInfo>(true).Where(cc => cc.DAPP_UserId == order.UserId.Value).SingleOrDefault();
                Assert.IsNotNull(creditCard);
                //make sure 2 contact records are created
                IList<TIMSSContact> contacts = objectService.GetAll<TIMSSContact>(true).Where(cont => cont.DAPP_UserId == order.UserId.Value).ToList();
                Assert.AreEqual(2, contacts.Count);
            }
        }
    }
}
