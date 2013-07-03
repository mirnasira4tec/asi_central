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
            LegacyOrder order = CreateOrder();

            order.ExternalReference = "8939514";
            //create the supplier application
            LegacySupplierMembershipApplication application = new LegacySupplierMembershipApplication()
            {
                ContactName = "First Last",
                ContactTitle = "Contact Title",
                ContactEmail = "Contact@asi.com",
                ContactPhone = "123 123 1234",
                LineNames = "Product Line Names",
                YearEstablished = 1992,
                YearEnteredAdvertising = 1997,
                WomanOwned = false,
                LineMinorityOwned = false,
                NumberOfEmployee = "500+",
                HasAmericanProducts = true,
                BusinessHours = "Very Flexible Hours",
                ProductionTime = "Very Fast",
                IsRushServiceAvailable = false,
                IsImprinterVsDecorator = true,
                IsImporter = true,
                IsManufacturer = true,
                IsRetailer = false,
                IsWholesaler = false,
            };
            PopulateApplication(application);
            application.Contacts.Add(new LegacySupplierMembershipApplicationContact()
            {
                IsPrimary = true,
                Name = "First Last",
                Title = "Contact Title",
                Phone = "123 123 1234",
                Email = "Contact@asi.com",
            });
            application.Contacts.Add(new LegacySupplierMembershipApplicationContact()
            {
                IsPrimary = false,
                Name = "First2 Last2",
                Title = "Contact Title2",
                Phone = "987 987 9876",
                Email = "Contact2@asi.com",
            });
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                //method of imprinting and decorating
                IList<LegacySupplierDecoratingType> decoratingTypes = objectService.GetAll<LegacySupplierDecoratingType>().ToList();
                Assert.IsTrue(decoratingTypes.Count > 1);
                application.DecoratingTypes.Add(decoratingTypes[0]);
                application.DecoratingTypes.Add(decoratingTypes[1]);
            }

            #endregion create the order to store
            using (IFulfilmentService fulfilmentService = new TIMSSService(new ObjectService(new Container(new EFRegistry()))))
            {
                fulfilmentService.Process(order, application);
            }
            //check the database to see if the records were added
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                GenericAsserts(objectService, order);
            }
        }

        [TestMethod]
        public void DistributorOrder()
        {
            LegacyOrder order = CreateOrder();
            order.ExternalReference = "8939541";
            //create the supplier application
            LegacyDistributorMembershipApplication application = new LegacyDistributorMembershipApplication()
            {
                AnnualSalesVolume = "A lot more",
                AnnualSalesVolumeASP = "A lot",
                EstablishedDate = DateTime.Now,
                NumberOfEmployee = 2,
                NumberOfSalesEmployee = 1,
                OtherBusinessRevenue = "Checking",
            };
            PopulateApplication(application);
            application.Contacts.Add(new LegacyDistributorMembershipApplicationContact()
            {
                IsPrimary = true,
                Name = "First Last",
                Title = "Contact Title",
                Phone = "123 123 1234",
                Email = "Contact@asi.com",
            });
            application.Contacts.Add(new LegacyDistributorMembershipApplicationContact()
            {
                IsPrimary = false,
                Name = "First2 Last2",
                Title = "Contact Title2",
                Phone = "987 987 9876",
                Email = "Contact2@asi.com",
            });
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                IList<LegacyDistributorAccountType> accountTypes = objectService.GetAll<LegacyDistributorAccountType>().ToList();
                Assert.IsTrue(accountTypes.Count > 1);
                for (int i = 0; i < 2; i++) application.AccountTypes.Add(accountTypes[i]);
                IList<LegacyDistributorProductLine> productLines = objectService.GetAll<LegacyDistributorProductLine>().ToList();
                Assert.IsTrue(productLines.Count > 1);
                for (int i = 0; i < 2; i++) application.ProductLines.Add(productLines[i]);
            }

            using (IFulfilmentService fulfilmentService = new TIMSSService(new ObjectService(new Container(new EFRegistry()))))
            {
                fulfilmentService.Process(order, application);
            }
            //check the database to see if the records were added
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                GenericAsserts(objectService, order);
                IList<TIMSSAccountType> accountTypes = objectService.GetAll<TIMSSAccountType>(true).Where(accType => accType.DAPP_UserId == order.UserId.Value).ToList();
                Assert.AreEqual(2, accountTypes.Count);
                IList<TIMSSProductType> productTypes = objectService.GetAll<TIMSSProductType>(true).Where(prodType => prodType.DAPP_UserId == order.UserId.Value).ToList();
                Assert.AreEqual(2, productTypes.Count);
            }
        }

        /// <summary>
        /// Asserts common to both Supplier and Distributor
        /// </summary>
        /// <param name="objectService"></param>
        /// <param name="order"></param>
        private void GenericAsserts(IObjectService objectService, LegacyOrder order)
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

        private LegacyOrder CreateOrder()
        {
            LegacyOrder order = new LegacyOrder()
            {
                UserId = Guid.NewGuid(),
                ExternalReference = "000008939544", //TIMSS ID
                BillFirstName = "Bill First Name",
                BillLastName = "Bill Last Name",
                BillStreet1 = "Bill Street1",
                BillStreet2 = "Bill Street2",
                BillCity = "Bill City",
                BillZip = "Bill Zip",
                BillState = "Bill State",
                BillCountry = "Bill Country",
                BillPhone = "123 123 1234",
                CreditCard = new LegacyOrderCreditCard()
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
            order.OrderDetails.Add(new LegacyOrderDetail()
            {
                ExternalReference = "192",
                ProductId = LegacyOrderProduct.SUPPLIER_APPLICATION,
                Quantity = 1,
            });
            return order;
        }

        private void PopulateApplication(LegacyOrderDetailApplication application)
        {
            application.Company = "Company Name";
            application.Address1 = "Address1";
            application.Address2 = "Address2";
            application.City = "City";
            application.Zip = "Zip";
            application.State = "State";
            application.Country = "USA";
            application.BillingAddress1 = "Bill Street1";
            application.BillingAddress2 = "Bill Street2";
            application.BillingCity = "Bill City";
            application.BillingZip = "Bill Zip";
            application.BillingState = "Bill State";
            application.BillingCountry = "USA";
            application.ShippingStreet1 = "Ship Street1";
            application.ShippingStreet2 = "Ship Street2";
            application.ShippingCity = "Ship City";
            application.ShippingZip = "Ship Zip";
            application.ShippingState = "Ship State";
            application.ShippingCountry = "USA";
            application.BillingWebUrl = "http://www.server.com/page";
            application.BillingPhone = "123 123 1234";
            application.HasBillAddress = true;
            application.HasShipAddress = true;
        }
    }
}
