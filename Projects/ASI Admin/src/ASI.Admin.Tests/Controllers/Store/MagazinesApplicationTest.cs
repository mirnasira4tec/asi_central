using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using asi.asicentral.model.store;
using asi.asicentral.interfaces;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using System.Web.Mvc;
using NUnit.Framework;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestFixture]
    public class MagazinesApplicationTest
    {
        [Test]
        [Ignore("Ignore a test")]
        public void EditMagazines()
        {
            Guid guid = Guid.NewGuid();
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 0, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            List<StoreMagazineSubscription> subscriptions = new List<StoreMagazineSubscription>();
            details.Add(detail);
            detail.Order = order;
            detail.MagazineSubscriptions = new List<StoreMagazineSubscription>();
            StoreMagazineSubscription subscription = new StoreMagazineSubscription();
            subscription.ASINumber = 1234;
            subscription.CompanyName = "ASI";
            subscription.Id = 100;
            subscription.IsDigitalVersion = false;
            subscription.OrderDetailId = detail.Id;
            subscription.PrimaryBusiness = "Business";
            subscription.Contact = new StoreIndividual();
            subscription.Contact.FirstName = "first name";
            subscription.Contact.LastName = "last name";
            subscription.Contact.Email = "asi@asi.com";
            subscription.Contact.Title = "Mr.";
            subscription.Contact.Phone = "9745579890";
            subscription.Contact.Fax = "9745579890";
            subscription.Contact.Department = "department";

            subscription.Contact.Address = new StoreAddress();
            subscription.Contact.Address.Street1 = "street1";
            subscription.Contact.Address.Street2 = "street2";
            subscription.Contact.Address.City = "city";
            subscription.Contact.Address.State = "state";
            subscription.Contact.Address.Zip = "515004";
            subscription.Contact.Address.Country = "country";
            subscription.Id = 14;
            detail.MagazineSubscriptions.Add(subscription);
            subscriptions.Add(subscription);

            order.CreditCard = new StoreCreditCard() { ExternalReference = "111" };
            StoreOrder orderRef = order;
            StoreCompany company = new StoreCompany()
            {
                Name = "Company",
            };
            order.Company = company;
            company.Individuals.Add(new StoreIndividual() { FirstName = "First1", LastName = "Last1", Id = 0 });
            company.Individuals.Add(new StoreIndividual() { FirstName = "First2", LastName = "Last2", Id = 1 });
            
            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            mockStoreService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());
            mockStoreService.Setup(service => service.GetAll<StoreMagazineSubscription>(false)).Returns(subscriptions.AsQueryable());

            ApplicationController controller = new ApplicationController();
            controller.StoreService = mockStoreService.Object;

            MagazinesApplicationModel model = new MagazinesApplicationModel(detail, controller.StoreService);
            model.ExternalReference = "102";
            model.ActionName = ApplicationController.COMMAND_SAVE;
            
            model.Subscriptions = new List<StoreMagazineSubscription>();
            StoreMagazineSubscription subscription1 = new StoreMagazineSubscription();
            subscription1.ASINumber = 12345;
            subscription1.CompanyName = "ASI1";
            subscription1.IsDigitalVersion = true;

            subscription1.Id = 14;
            subscription1.PrimaryBusiness = "Business1";
            subscription1.Contact = new StoreIndividual();
            subscription1.Contact.FirstName = "first name1";
            subscription1.Contact.LastName = "last name1";
            subscription1.Contact.Email = "asi1@asi.com";
            subscription1.Contact.Title = "Mr1.";
            subscription1.Contact.Phone = "97455798901";
            subscription1.Contact.Fax = "97455798901";
            subscription1.Contact.Department = "department1";

            subscription1.Contact.Address = new StoreAddress();
            subscription1.Contact.Address.Street1 = "street11";
            subscription1.Contact.Address.Street2 = "street21";
            subscription1.Contact.Address.City = "city1";
            subscription1.Contact.Address.State = "state1";
            subscription1.Contact.Address.Zip = "5150041";
            subscription1.Contact.Address.Country = "country1";

            model.Subscriptions.Add(subscription1);
            
            // user selects imprinting methods and clicks save - order should be updated wih externalreference, imprinting methods should be saved.
            RedirectToRouteResult result = controller.EditMagazines(model) as RedirectToRouteResult;
            Assert.AreEqual(result.RouteValues["controller"], "Application");
            Assert.AreEqual(orderRef.ExternalReference, model.ExternalReference);
            Assert.IsNotNull(detail.MagazineSubscriptions);
            Assert.IsTrue(detail.MagazineSubscriptions.Count > 0);
            StoreMagazineSubscription sub1 = detail.MagazineSubscriptions.ElementAt(0);
            StoreMagazineSubscription sub2 = model.Subscriptions.ElementAt(0);
            Assert.AreEqual(sub1.ASINumber, sub2.ASINumber);
            Assert.AreEqual(sub1.CompanyName, sub2.CompanyName);
            Assert.AreEqual(sub1.PrimaryBusiness, sub2.PrimaryBusiness);
            Assert.AreEqual(sub1.IsDigitalVersion, sub2.IsDigitalVersion);

            Assert.IsNotNull(sub1.Contact);
            Assert.AreEqual(sub1.Contact.FirstName, sub2.Contact.FirstName);
            Assert.AreEqual(sub1.Contact.LastName, sub2.Contact.LastName);
            Assert.AreEqual(sub1.Contact.Email, sub2.Contact.Email);
            Assert.AreEqual(sub1.Contact.Title, sub2.Contact.Title);
            Assert.AreEqual(sub1.Contact.Phone, sub2.Contact.Phone);
            Assert.AreEqual(sub1.Contact.Fax, sub2.Contact.Fax);
            Assert.AreEqual(sub1.Contact.Department, sub2.Contact.Department);

            Assert.IsNotNull(sub1.Contact.Address);
            Assert.AreEqual(sub1.Contact.Address.Street1, sub2.Contact.Address.Street1);
            Assert.AreEqual(sub1.Contact.Address.Street2, sub2.Contact.Address.Street2);
            Assert.AreEqual(sub1.Contact.Address.City, sub2.Contact.Address.City);
            Assert.AreEqual(sub1.Contact.Address.State, sub2.Contact.Address.State);
            Assert.AreEqual(sub1.Contact.Address.Zip, sub2.Contact.Address.Zip);
            Assert.AreEqual(sub1.Contact.Address.Country, sub2.Contact.Address.Country);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(1));

            // user clicks reject - order should be updated to reject
            model.ActionName = ApplicationController.COMMAND_REJECT;
            RedirectToRouteResult result2 = controller.EditMagazines(model) as RedirectToRouteResult;
            Assert.AreEqual(result2.RouteValues["controller"], "Orders");
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Rejected);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(2));

            // user clicks approve with reference id
            model.ActionName = ApplicationController.COMMAND_ACCEPT;
            RedirectToRouteResult result3 = controller.EditMagazines(model) as RedirectToRouteResult;
            Assert.AreEqual(orderRef.ProcessStatus, OrderStatus.Approved);
            mockStoreService.Verify(service => service.SaveChanges(), Times.Exactly(3));
        }
    }
}
