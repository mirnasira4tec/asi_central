using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.database.mappings;
using asi.asicentral.model.store;

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
    }
}
