using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.database.mappings;
using asi.asicentral.model.store;
using System.Collections.Generic;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class StoreServiceTest
    {
        [TestMethod]
        public void OrderApplicationRetrieveTest()
        {
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                //order 10491 has one line item of type 102 (Supplier Application)
                Order supplierOrder = storeService.GetAll<Order>().Where(theOrder => theOrder.Id == 10491).SingleOrDefault();
                Assert.IsTrue(supplierOrder != null && supplierOrder.OrderDetails.Count > 0);
                Assert.IsNotNull(supplierOrder.Membership);
                Assert.AreEqual(OrderStatus.Approved, supplierOrder.ProcessStatus);
                OrderDetail supplierOrderDetail = null;
                foreach (OrderDetail orderDetail in supplierOrder.OrderDetails)
                {
                    if (orderDetail.ProductId == OrderProduct.SUPPLIER_APPLICATION) supplierOrderDetail = orderDetail;
                }
                Assert.IsNotNull(supplierOrderDetail);
                SupplierMembershipApplication supplierapplication = storeService.GetSupplierApplication(supplierOrderDetail);
                Assert.IsNotNull(supplierapplication);
                Assert.IsTrue(supplierapplication.Contacts.Count > 0);

                //order 288 has one line item of type 103 (Distributor Application)
                Order distributorOrder = storeService.GetAll<Order>().Where(theOrder => theOrder.Id == 288).SingleOrDefault();
                Assert.IsTrue(distributorOrder != null && distributorOrder.OrderDetails.Count > 0);
                OrderDetail distributorOrderDetail = null;
                foreach (OrderDetail orderDetail in distributorOrder.OrderDetails)
                {
                    if (orderDetail.ProductId == OrderProduct.DISTRIBUTOR_APPLICATION) distributorOrderDetail = orderDetail;
                }
                Assert.IsNotNull(distributorOrderDetail);
                DistributorMembershipApplication distributorApplication = storeService.GetDistributorApplication(distributorOrderDetail);
                Assert.IsNotNull(distributorApplication);

                Assert.IsNull(storeService.GetSupplierApplication(distributorOrderDetail));
                Assert.IsNull(storeService.GetDistributorApplication(supplierOrderDetail));
            }
        }

        [TestMethod]
        public void OrderSupplierProduct()
        {
            using (IStoreService storeService = new StoreService(new Container(new EFRegistry())))
            {
                // select product screen
                OrderProduct orderProduct = new OrderProduct { Id = OrderProduct.SUPPLIER_APPLICATION };
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId = orderProduct.Id,
                    Quantity = 1,
                    Added = DateTime.Now
                };

                // company information screen
                Order order = new Order();
                order.TransId = Guid.NewGuid();
                order.UserId = order.TransId.Value;
                order.Status = false;
                order.DateCreated = DateTime.Now;
                order.BillCity = "City";
                order.BillCountry = "Country";
                order.BillFirstName = "FirstName";
                order.BillLastName = "LastName";
                order.OrderDetails.Add(orderDetail);

                SupplierMembershipApplicationContact supplierAppContact = new SupplierMembershipApplicationContact();
                supplierAppContact.Name = "First Last";
                supplierAppContact.Title = "Title";
                //supplierAppContact.SalesId = 1;
                supplierAppContact.Email = "Email@Email.com";
                supplierAppContact.Phone = "1231231234";
                supplierAppContact.AppplicationId = order.UserId.Value;

                SupplierMembershipApplication supplierApplication = new SupplierMembershipApplication();
                supplierApplication.UserId = order.TransId.Value;
                supplierApplication.Company = "Company";
                supplierApplication.Contacts = new List<SupplierMembershipApplicationContact>();
                supplierApplication.Contacts.Add(supplierAppContact);

                storeService.Add<Order>(order);
                storeService.Add<SupplierMembershipApplication>(supplierApplication);
                storeService.SaveChanges();

                // billing and shipping screen
                Order billingOrder = storeService.GetAll<Order>().Where(theOrder => theOrder.Id == order.Id).SingleOrDefault();
                if (billingOrder == null) throw new Exception("Invalid identifier for order id " + order.Id);
                SupplierMembershipApplication billingSupplierApplication = storeService.GetAll<SupplierMembershipApplication>()
                    .Where(application => application.UserId == billingOrder.UserId).SingleOrDefault();

                if (billingSupplierApplication == null) throw new Exception("Illegal supplier application with id " + billingSupplierApplication.UserId);

                billingOrder.BillFirstName = "New First Name";
                billingOrder.BillLastName = "New Last Name";

                billingSupplierApplication.ShippingStreet1 = "shipping address";
                billingSupplierApplication.ShippingCity = "shipping city";
                billingSupplierApplication.ShippingZip = "19111";

                OrderCreditCard creditcard = new OrderCreditCard()
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

                storeService.Update<SupplierMembershipApplication>(billingSupplierApplication);
                storeService.Update<Order>(billingOrder);
                storeService.SaveChanges();

                // review screen
                Order reviewOrder = storeService.GetAll<Order>().Where(theOrder => theOrder.Id == order.Id).SingleOrDefault();
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
                storeService.Update<Order>(reviewOrder);
                storeService.Add<ASPNetMembership>(membership);
                storeService.SaveChanges();

                // option screen
                SupplierMembershipApplication supplierMembershipApplication = storeService.GetAll<SupplierMembershipApplication>()
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

                storeService.Update<SupplierMembershipApplication>(supplierMembershipApplication);
                storeService.SaveChanges();
            }
        }
    }
}
