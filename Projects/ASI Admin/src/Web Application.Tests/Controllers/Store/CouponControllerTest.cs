using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace asi.asicentral.WebApplication.Tests.Controllers.Store
{
    [TestClass]
    public class CouponControllerTest
    {
        [TestMethod]
        public void SaveCouponDetails()
        {
            Coupon product = new Coupon();

            Mock<IStoreService> mockObjectService = new Mock<IStoreService>();
                        mockObjectService.Setup(objectService => objectService.Add<Coupon>(It.IsAny<Coupon>()))
                            .Callback<Coupon>((thecoupon) => product = thecoupon);

            //For Coupons
            CouponModel information = new CouponModel();
            information.CouponCode = "testCaseCoupon";
            information.Description = "testDescription";
            information.DiscountPercentage = "0";
            information.IsFixedAmount = true;
            information.DiscountAmount = "20";
            information.IsSubscription = true;
            information.IsProduct = true;
            information.ProductId = 3;

            CouponController controller = new CouponController();
            controller.StoreService = mockObjectService.Object;

            RedirectResult actionResult = controller.SaveCouponDetails(information) as RedirectResult;

            Assert.IsNotNull(product);
            Assert.IsNotNull(product.CouponCode);
            Assert.IsNotNull(product.Description);
            Assert.IsNotNull(product.IsFixedAmount);
            Assert.IsNotNull(product.DiscountPercentage);
            Assert.IsNotNull(product.DiscountAmount);
            Assert.IsNotNull(product.IsSubscription);
            Assert.IsNotNull(product.ProductId);


          
            Assert.AreEqual(product.CouponCode, information.CouponCode);
            Assert.AreEqual(product.Description, information.Description);
            Assert.AreEqual(product.DiscountPercentage, Convert.ToInt32(information.DiscountPercentage));
            Assert.AreEqual(product.DiscountAmount,Convert.ToDecimal(information.DiscountAmount));
            Assert.AreEqual(product.IsFixedAmount, information.IsFixedAmount);
            Assert.AreEqual(product.IsSubscription, information.IsSubscription);
            Assert.AreEqual(product.ProductId, information.ProductId);

            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));

        }
    }
}
