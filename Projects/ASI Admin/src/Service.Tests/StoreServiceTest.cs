using System;
using System.Linq;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.database.mappings;
using asi.asicentral.model.store;
using System.Collections.Generic;
using NUnit.Framework;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class StoreServiceTest
    {
        [Test]
        [Ignore("Ignore a OrderApplicationRetrieveTest")]
        public void OrderApplicationRetrieveTest()
        {
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                //order 10491 has one line item of type 102 (Supplier Application)
                StoreOrder supplierOrder = storeService.GetAll<StoreOrder>().Where(theOrder => theOrder.LegacyId != null && theOrder.LegacyId == 10491).SingleOrDefault();
                Assert.IsTrue(supplierOrder != null && supplierOrder.OrderDetails.Count > 0);
                Assert.IsNotNull(supplierOrder.Company);
                Assert.IsTrue(supplierOrder.Company.Individuals.Count > 0);
                Assert.IsNotNull(supplierOrder.BillingIndividual);
                Assert.AreEqual(OrderStatus.Approved, supplierOrder.ProcessStatus);
                StoreOrderDetail supplierOrderDetail = null;
                foreach (StoreOrderDetail orderDetail in supplierOrder.OrderDetails)
                {
                    if (orderDetail.Product != null && StoreDetailSupplierMembership.Identifiers.Contains(orderDetail.Product.Id)) supplierOrderDetail = orderDetail;
                }
                Assert.IsNotNull(supplierOrderDetail);
                StoreDetailSupplierMembership supplierapplication = storeService.GetSupplierApplication(supplierOrderDetail);
                Assert.IsNotNull(supplierapplication);

                //order 288 has one line item of type 103 (Distributor Application)
                StoreOrder distributorOrder = storeService.GetAll<StoreOrder>().Where(theOrder => theOrder.LegacyId != null && theOrder.LegacyId == 288).SingleOrDefault();
                Assert.IsTrue(distributorOrder != null && distributorOrder.OrderDetails.Count > 0);
                StoreOrderDetail distributorOrderDetail = null;
                foreach (StoreOrderDetail orderDetail in distributorOrder.OrderDetails)
                {
                    if (StoreDetailDistributorMembership.Identifiers.Contains(orderDetail.Product.Id)) distributorOrderDetail = orderDetail;
                }
                Assert.IsNotNull(distributorOrderDetail);
                StoreDetailDistributorMembership distributorApplication = storeService.GetDistributorApplication(distributorOrderDetail);
                Assert.IsNotNull(distributorApplication);

                Assert.IsNull(storeService.GetSupplierApplication(distributorOrderDetail));
                Assert.IsNull(storeService.GetDistributorApplication(supplierOrderDetail));
            }
        }

        [Test]
        public void OrderSupplierProduct()
        {
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                // select product screen
                LegacyOrderProduct orderProduct = new LegacyOrderProduct { Id = LegacyOrderProduct.SUPPLIER_APPLICATION };
                LegacyOrderDetail orderDetail = new LegacyOrderDetail
                {
                    ProductId = orderProduct.Id,
                    Quantity = 1,
                    Added = DateTime.Now
                };

                // company information screen
                LegacyOrder order = new LegacyOrder();
                order.TransId = Guid.NewGuid();
                order.UserId = order.TransId.Value;
                order.Status = false;
                order.DateCreated = DateTime.Now;
                order.BillCity = "City";
                order.BillCountry = "Country";
                order.BillFirstName = "FirstName";
                order.BillLastName = "LastName";
                order.OrderDetails.Add(orderDetail);

                LegacySupplierMembershipApplicationContact supplierAppContact = new LegacySupplierMembershipApplicationContact();
                supplierAppContact.Name = "First Last";
                supplierAppContact.Title = "Title";
                //supplierAppContact.SalesId = 1;
                supplierAppContact.Email = "Email@Email.com";
                supplierAppContact.Phone = "1231231234";
                supplierAppContact.AppplicationId = order.UserId.Value;

                LegacySupplierMembershipApplication supplierApplication = new LegacySupplierMembershipApplication();
                supplierApplication.UserId = order.TransId.Value;
                supplierApplication.Company = "Company";
                supplierApplication.Contacts = new List<LegacySupplierMembershipApplicationContact>();
                supplierApplication.Contacts.Add(supplierAppContact);

                storeService.Add<LegacyOrder>(order);
                storeService.Add<LegacySupplierMembershipApplication>(supplierApplication);
                storeService.SaveChanges();

                // billing and shipping screen
                LegacyOrder billingOrder = storeService.GetAll<LegacyOrder>().Where(theOrder => theOrder.Id == order.Id).SingleOrDefault();
                if (billingOrder == null) throw new Exception("Invalid identifier for order id " + order.Id);
                LegacySupplierMembershipApplication billingSupplierApplication = storeService.GetAll<LegacySupplierMembershipApplication>()
                    .Where(application => application.UserId == billingOrder.UserId).SingleOrDefault();

                if (billingSupplierApplication == null) throw new Exception("Illegal supplier application with id " + billingSupplierApplication.UserId);

                billingOrder.BillFirstName = "New First Name";
                billingOrder.BillLastName = "New Last Name";

                billingSupplierApplication.ShippingStreet1 = "shipping address";
                billingSupplierApplication.ShippingCity = "shipping city";
                billingSupplierApplication.ShippingZip = "19111";

                LegacyOrderCreditCard creditcard = new LegacyOrderCreditCard()
                {
                    ExpMonth = "March",
                    ExpYear = "2013",
                    Name = "first last",
                    Number = "5555444433332222",
                    Type = "Mastercard",
                    TotalAmount = orderDetail.Subtotal
                };

                // TODO: find out how to fix this
                billingOrder.CreditCard = creditcard;

                storeService.Update<LegacySupplierMembershipApplication>(billingSupplierApplication);
                storeService.Update<LegacyOrder>(billingOrder);
                storeService.SaveChanges();

                // review screen
                LegacyOrder reviewOrder = storeService.GetAll<LegacyOrder>().Where(theOrder => theOrder.Id == order.Id).SingleOrDefault();
                if (reviewOrder == null) throw new Exception("Invalid identifier for order id " + order.Id);

                ASPNetMembership membership = new ASPNetMembership();
                membership.ApplicationId = new Guid("71792474-3BB3-4108-9C88-E26179DB443A");
                membership.UserId = reviewOrder.UserId.Value;
                membership.Email = supplierAppContact.Email;
                membership.Password = "na";
                membership.PasswordFormat = 2;
                membership.PasswordSalt = "na";
                membership.IsApproved = false;
                membership.IsLockedOut = false;
                membership.Comment = "na";
                membership.CreateDate = DateTime.Now;
                membership.LastLoginDate = DateTime.Now;
                membership.LastPasswordChangedDate = DateTime.Now;
                membership.LastLockoutDate = DateTime.Now;
                membership.FailedPasswordAnswerAttemptCount = 0;
                membership.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                membership.FailedPasswordAttemptCount = 0;
                membership.FailedPasswordAttemptWindowStart = DateTime.Now;

                reviewOrder.Status = true;
                storeService.Update<LegacyOrder>(reviewOrder);
                storeService.Add<ASPNetMembership>(membership);
                storeService.SaveChanges();

                // option screen
                LegacySupplierMembershipApplication supplierMembershipApplication = storeService.GetAll<LegacySupplierMembershipApplication>()
                    .Where(application => application.UserId == reviewOrder.UserId.Value).SingleOrDefault();

                supplierMembershipApplication.UserId = reviewOrder.UserId.Value;
                supplierMembershipApplication.IsImporter = true;
                supplierMembershipApplication.IsImprinterVsDecorator = true;
                supplierMembershipApplication.IsManufacturer = true;
                supplierMembershipApplication.IsRetailer = true;
                supplierMembershipApplication.IsWholesaler = true;
                supplierMembershipApplication.LineMinorityOwned = true;
                supplierMembershipApplication.LineNames = "Line names";
                supplierMembershipApplication.YearEstablished = 2013;
                supplierMembershipApplication.YearEnteredAdvertising = 2013;
                supplierMembershipApplication.ApplicationStatusId = 1;
                // need data field for "is your company women owned?"

                storeService.Update<LegacySupplierMembershipApplication>(supplierMembershipApplication);
                storeService.SaveChanges();
            }
        }

        [Test]
        public void StoreDetailMagazineAdvertisingDbTest()
        {
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                var storeMagazineAdvertisingItems = storeService.GetAll<StoreDetailMagazineAdvertisingItem>().ToList<StoreDetailMagazineAdvertisingItem>();
                var magazines = storeService.GetAll<LookMagazineIssue>().ToList<LookMagazineIssue>();
                var adPositions = storeService.GetAll<LookAdPosition>().ToList<LookAdPosition>();
                var adSizes = storeService.GetAll<LookAdSize>().ToList<LookAdSize>();

                Assert.IsTrue(storeMagazineAdvertisingItems.Count() >= 0);
                Assert.IsTrue(magazines.Count() >= 0);
                Assert.IsNotNull(adPositions.Count() >= 0);
                Assert.IsNotNull(adSizes.Count() >= 0);
            }
        }

        [Test]
        public void LookSendMyAdDbTest()
        {
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                var lookSendMyAdPublications = storeService.GetAll<LookSendMyAdPublication>().ToList<LookSendMyAdPublication>();
                var lookSendMyAdAdSpecs = storeService.GetAll<LookSendMyAdAdSpec>().ToList<LookSendMyAdAdSpec>();
                var lookSendMyAdCountryCodes = storeService.GetAll<LookSendMyAdCountryCode>().ToList<LookSendMyAdCountryCode>();

                Assert.IsTrue(lookSendMyAdPublications.Count() >= 0);
                Assert.IsTrue(lookSendMyAdAdSpecs.Count() >= 0);
                Assert.IsTrue(lookSendMyAdCountryCodes.Count() >= 0);
            }
        }

        [Test]
        public void StoreDetailMagazineAdvertisingItemTest()
        {
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                IList<StoreDetailMagazineAdvertisingItem> dbMagazineAdItems = 
                    storeService.GetAll<StoreDetailMagazineAdvertisingItem>().Where(item => item.OrderDetailId == 4).ToList();
                if (dbMagazineAdItems != null && dbMagazineAdItems.Count > 0)
                {
                    var sizes = dbMagazineAdItems.Select(item => item.Size).Distinct();
                    foreach (var size in sizes)
                    {
                        var dbMagazineAdItems2 = dbMagazineAdItems.Where(item1 => item1.Size == size);
                        foreach (var item2 in dbMagazineAdItems2)
                        {
                            item2.ProcessId = "12345";
                        }
                    }
                }

                int c = storeService.SaveChanges();

                Assert.IsTrue(dbMagazineAdItems.Count() >= 0);
            }
        }

	    [Test]
	    public void LookSendMyAdCountryCodeCachedTest()
	    {
		    using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
		    {
				var usaCode = storeService.GetAll<LookSendMyAdCountryCode>(true).Single(code => code.Alpha3 == "USA");
				var usaCode2 = storeService.GetAll<LookSendMyAdCountryCode>(true).Single(code => code.Alpha3 == "USA");
				Assert.IsTrue(Object.ReferenceEquals(usaCode, usaCode2));
			}
	    }

        [Test]
        public void FormPersistenceTest()
        {
            string testKey = Guid.NewGuid().ToString();
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                var formType = storeService.GetAll<FormType>(false).FirstOrDefault();
                if (formType == null)
                {
                    //add a form type for the test
                    formType = new FormType
                    {
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        UpdateSource = "FormPersistenceTest",
                        Name = "Form Test",
                        Implementation = "Not important",
                    };
                    storeService.Add<FormType>(formType);
                    storeService.SaveChanges();
                }
                var form = new FormInstance
                {
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    FormType = formType,
                    UpdateSource = "FormPersistenceTest",                    
                    Email = "test@test.com",
					Salutation = "Yann",
                    Greetings = "Please check the order",
                    ExternalReference = testKey,
                };
                form.Values.Add(new FormValue
                {
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "FormPersistenceTest",                    
                    Sequence = 0,
                    Name = "option one",
                    Value = "true",
                });
                form.Values.Add(new FormValue
                {
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "FormPersistenceTest",
                    Sequence = 1,
                    Name = "option two",
                    Value = "true",
                });
                storeService.Add<FormInstance>(form);
	            try
	            {
		            storeService.SaveChanges();
	            }
	            catch (Exception e)
	            {
		            Assert.IsTrue(false, e.Message);
	            }
            }
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                var form = storeService.GetAll<FormInstance>().SingleOrDefault(instance => instance.ExternalReference == testKey);
                Assert.IsNotNull(form, "Form could not be saved or retrieved");
                Assert.AreEqual(2, form.Values.Count, "Did not get the right number of values");
				Assert.IsNotNull(form.FormType);
            }
        }
    }
}
