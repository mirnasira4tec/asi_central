using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using asi.asicentral.util.store;
using asi.asicentral.util.store.catalogadvertising;
using asi.asicentral.util.store.magazinesadvertising;
using asi.asicentral.web.Controllers.Store;
using asi.asicentral.web.model.store;
using Castle.DynamicProxy.Contributors;

namespace asi.asicentral.web.model.store
{
    public class CatalogAdvertisingApplicationModel : MembershipModel
    {
        #region Catalog Advertising information
        public int Id { get; set; }

        public IList<CatalogAdvertisingItem> CatalogAdvertisingItems { get; set; }

        public IList<CatalogAdvertisingTieredProductPricing> Prices { get; set; }

        public int Sequence { get; set; }

        #endregion Catalog Advertising information

        public IList<int> ids { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public CatalogAdvertisingApplicationModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public CatalogAdvertisingApplicationModel(StoreOrderDetail orderdetail, IList<StoreDetailCatalogAdvertisingItem> catalogAdvertisingItems, IStoreService storeService)
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            if (orderdetail.Product != null)
            {
                ProductName = HttpUtility.HtmlDecode(orderdetail.Product.Name);
                ProductId = orderdetail.Product.Id;
            }
            else
            {
                throw new Exception("Order detail doesn't exist.");
            }

            #region Fill Catalog Advertising details

            catalogAdvertisingItems = catalogAdvertisingItems.OrderBy(item => item.Id).ToList();
            CatalogAdvertisingItems = new List<CatalogAdvertisingItem>();
            foreach (var item in catalogAdvertisingItems)
            {
                CatalogAdvertisingItems.Add(CatalogAdvertisingItem.GenerateFrom(item));
            }
            Prices = CatalogAdvertisingTieredProductPricing.GetTieredProductPricing(ProductId);

            #endregion

            OrderId = order.Id;
            Price = order.Total;
            OrderStatus = order.ProcessStatus;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
        }
    }

    public class CatalogAdvertisingItem
    {
        public int Id { get; set; }

        public CatalogAdvertisingUpload ProductType { get; set; }

        [DisplayName("Ad Size")]
        public string AdSize { get; set; }

        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        [DisplayName("Product Pricing")]
        public string ProductPricing { get; set; }

        [DisplayName("Website")]
        public string Website { get; set; }

        [DisplayName("Product Number")]
        public string ProductNumber { get; set; }

        [DisplayName("ESP Number")]
        public string ESPNumber { get; set; }

        [DisplayName("Product Image")]
        public string ProductImage { get; set; }

        public int Sequence { get; set; }

        public static CatalogAdvertisingItem GenerateFrom(StoreDetailCatalogAdvertisingItem item)
        {
            var result = new CatalogAdvertisingItem();
            result.ProductType = item.ProductType;
            result.Id = item.Id;
            result.AdSize = item.AdSize;
            result.ProductDescription = item.ProductDescription;
            result.ProductPricing = item.ProductPricing;
            result.Website = item.Website;
            result.ProductNumber = item.ProductNumber;
            result.ESPNumber = item.ESPNumber;
            result.ProductImage = item.ProductImage;
            result.Sequence = item.Sequence;
            return result;
        }

        public void CopyTo(StoreDetailCatalogAdvertisingItem item)
        {
            if (item == null || item.Id != Id) return;
            item.AdSize = AdSize;
            item.ProductDescription = ProductDescription;
            item.ProductPricing = ProductPricing;
            item.Website = Website;
            item.ProductNumber = ProductNumber;
            item.ESPNumber = ESPNumber;
            item.ProductImage = ProductImage;
            item.UpdateDateUTCAndSource();
        }
    }
}