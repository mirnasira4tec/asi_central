using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestFixture]
    public class CouponControllerTest
    {
        public ContextProduct CreateProduct(int id = 1)
        {
            ContextProduct product = new ContextProduct()
            {
                Name = "test product" + id.ToString(),
                Cost = 100,
                Id = id,
                Origin = "ASI"
            };
            return product;
        }   
        [Test]
        [Ignore("Ignore a test")]
        public void SaveCouponDetails()
        {
            Coupon product = new Coupon();
            StoreIndividual individual = new StoreIndividual() { LastName = "Last", FirstName = "First" };
            StoreOrder order = new StoreOrder() { Id = 0, BillingIndividual = individual };
            StoreOrderDetail detail = new StoreOrderDetail { Id = 3, Order = order, };
            List<StoreOrderDetail> details = new List<StoreOrderDetail>();
            details.Add(detail);
            detail.Order = order;
            detail.Quantity = 30;
            detail.Product = CreateProduct(3);
            detail.ShippingMethod = "UPS2DAY";

            order.Company = new StoreCompany();
            order.Company.Email = "asi@asi.com";
            order.Company.WebURL = "http://asicentral.com";

            Mock<IStoreService> mockObjectService = new Mock<IStoreService>();
                        mockObjectService.Setup(objectService => objectService.Add<Coupon>(It.IsAny<Coupon>()))
                            .Callback<Coupon>((thecoupon) => product = thecoupon);
            mockObjectService.Setup(service => service.GetAll<StoreOrderDetail>(false)).Returns(details.AsQueryable());
            //For Coupons
            CouponModel information = new CouponModel();
            information.CouponCode = "testCaseCoupon";
            information.Description = "testDescription";
            information.IsProduct = true;
            information.ProductId = 3;
            //information.MonthlyCost = 50;

            CouponController controller = new CouponController();
            controller.StoreService = mockObjectService.Object;

            RedirectResult actionResult = controller.SaveCouponDetails(information) as RedirectResult;

            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));

        }
    }
}
