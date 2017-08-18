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
        [Test]
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
