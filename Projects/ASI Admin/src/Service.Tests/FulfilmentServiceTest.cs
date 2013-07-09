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
            StoreOrder order = CreateOrder();

            order.ExternalReference = "8939514";
            //create the supplier application
            StoreDetailSupplierMembership application = new StoreDetailSupplierMembership()
            {
                LineNames = "Product Line Names",
                YearEstablished = 1992,
                YearEnteredAdvertising = 1997,
                WomanOwned = false,
                IsMinorityOwned = false,
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
            PopulateOrder(order);
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                //method of imprinting and decorating
                IList<LookSupplierDecoratingType> decoratingTypes = objectService.GetAll<LookSupplierDecoratingType>().ToList();
                Assert.IsTrue(decoratingTypes.Count > 1);
                application.DecoratingTypes.Add(decoratingTypes[0]);
                application.DecoratingTypes.Add(decoratingTypes[1]);
            }

            #endregion create the order to store
            Guid newIdentifier;
            using (IFulfilmentService fulfilmentService = new TIMSSService(new ObjectService(new Container(new EFRegistry()))))
            {
                newIdentifier = fulfilmentService.Process(order, application);
            }
            //check the database to see if the records were added
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                GenericAsserts(objectService, order, newIdentifier);
            }
        }

        [TestMethod]
        public void DistributorOrder()
        {
            StoreOrder order = CreateOrder();
            order.ExternalReference = "8939541";
            //create the supplier application
            StoreDetailDistributorMembership application = new StoreDetailDistributorMembership()
            {
                AnnualSalesVolume = "A lot more",
                AnnualSalesVolumeASP = "A lot",
                EstablishedDate = DateTime.Now,
                NumberOfEmployee = 2,
                NumberOfSalesEmployee = 1,
                OtherBusinessRevenue = "Checking",
            };
            PopulateOrder(order);
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                IList<LookDistributorAccountType> accountTypes = objectService.GetAll<LookDistributorAccountType>().ToList();
                Assert.IsTrue(accountTypes.Count > 1);
                for (int i = 0; i < 2; i++) application.AccountTypes.Add(accountTypes[i]);
                IList<LookProductLine> productLines = objectService.GetAll<LookProductLine>().ToList();
                Assert.IsTrue(productLines.Count > 1);
                for (int i = 0; i < 2; i++) application.ProductLines.Add(productLines[i]);
            }
            Guid newIdentifier;
            using (IFulfilmentService fulfilmentService = new TIMSSService(new ObjectService(new Container(new EFRegistry()))))
            {
                newIdentifier = fulfilmentService.Process(order, application);
            }
            //check the database to see if the records were added
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                GenericAsserts(objectService, order, newIdentifier);
                IList<TIMSSAccountType> accountTypes = objectService.GetAll<TIMSSAccountType>(true).Where(accType => accType.DAPP_UserId == newIdentifier).ToList();
                Assert.AreEqual(2, accountTypes.Count);
                IList<TIMSSProductType> productTypes = objectService.GetAll<TIMSSProductType>(true).Where(prodType => prodType.DAPP_UserId == newIdentifier).ToList();
                Assert.AreEqual(2, productTypes.Count);
            }
        }

        /// <summary>
        /// Asserts common to both Supplier and Distributor
        /// </summary>
        /// <param name="objectService"></param>
        /// <param name="order"></param>
        private void GenericAsserts(IObjectService objectService, StoreOrder order, Guid newIdentifier)
        {
            //make sure company record is created
            TIMSSCompany company = objectService.GetAll<TIMSSCompany>(true).Where(comp => comp.DAPP_UserId == newIdentifier).SingleOrDefault();
            Assert.IsNotNull(company);
            //make sure a credit card record is created
            TIMSSCreditInfo creditCard = objectService.GetAll<TIMSSCreditInfo>(true).Where(cc => cc.DAPP_UserId == newIdentifier).SingleOrDefault();
            Assert.IsNotNull(creditCard);
            //make sure 2 contact records are created
            IList<TIMSSContact> contacts = objectService.GetAll<TIMSSContact>(true).Where(cont => cont.DAPP_UserId == newIdentifier).ToList();
            Assert.AreEqual(2, contacts.Count);
        }

        private StoreOrder CreateOrder()
        {
            StoreAddress address = new StoreAddress()
            {
                Street1 = "Bill Street1",
                Street2 = "Bill Street 2",
                City = "Bill City",
                State = "Bill State",
                Zip = "Bill Zip",
                Country = "USA",
            };
            StoreIndividual individual = new StoreIndividual()
            {
                LastName = "Bill Last Name",
                FirstName = "Bill First Name",
                Phone = "123 123 1234",
                Address = address,
            };

            StoreOrder order = new StoreOrder()
            {
                ExternalReference = "000008939544", //TIMSS ID
                Total = 299,
                BillingIndividual = individual,
                CreditCard = new StoreCreditCard()
                {
                    CardType = "VISA",
                    CardNumber = "***1234",
                    ExpMonth = "01",
                    ExpYear = "2016",
                    ExternalReference = Guid.NewGuid().ToString(),
                    CardHolderName = "First Last",
                },
            };
            ContextProduct product = new ContextProduct()
            {
                Id = 2,
                IsSubscription = true,
                Name = "Supplier Product",
                ApplicationCost = 255,
                Cost = 299,
            };
            order.OrderDetails.Add(new StoreOrderDetail()
            {
                Cost = 199,
                LegacyProductId = LegacyOrderProduct.SUPPLIER_APPLICATION,
                Product = product,
                Quantity = 1,
            });
            return order;
        }

        private void PopulateOrder(StoreOrder order)
        {
            StoreAddress shipAddress = new StoreAddress()
            {
                Street1 = "Address1",
                Street2 = "Address2",
                City = "City",
                State = "State",
                Country = "USA",
            };
            StoreCompany company = new StoreCompany()
            {
                Name = "Company Name",
                WebURL = "http://www.server.com/page",
                Phone = "123 123 1234",
            };
            company.Addresses.Add(new StoreCompanyAddress()
            {
                Address = shipAddress,
                IsShipping = true,
                
            });
            order.Company = company;
            if (order.BillingIndividual != null && order.BillingIndividual.Address != null)
            {
                order.Company.Individuals.Add(order.BillingIndividual);
                order.BillingIndividual.IsPrimary = true;
            }
            else
            {
                StoreAddress billAddress = new StoreAddress()
                {
                    Street1 = "Bill Street1",
                    Street2 = "Bill Street2",
                    City = "Bill City",
                    State = "Bill State",
                    Country = "USA",
                };
                company.Addresses.Add(new StoreCompanyAddress()
                {
                    Address = billAddress,
                    IsBilling = true,

                });
                StoreIndividual primary = new StoreIndividual()
                {
                    IsPrimary = true,
                    FirstName = "First",
                    LastName = "Last",
                    Title = "Contact Title",
                    Phone = "123 123 1234",
                    Email = "Contact@asi.com",
                };
                company.Individuals.Add(primary);
            }
            //add one more contact
            company.Individuals.Add(new StoreIndividual()
            {
                IsPrimary = false,
                FirstName = "First2",
                LastName = "Last2",
                Title = "Contact Title2",
                Phone = "987 987 9876",
                Email = "Contact2@asi.com",
            });
        }
    }
}
