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

namespace asi.asicentral.web.Models.Store.Order
{
    public class CatalogAdvertisingApplicationModel : IMembershipModel
    {
        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        public string Company { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Street1")]
        public string Address1 { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Street2")]
        public string Address2 { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "City")]
        public string City { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Zipcode")]
        public string Zip { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "State")]
        public string State { get; set; }
        [Display(ResourceType = typeof(Resource), Name = "Country")]
        public string Country { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        public string Phone { get; set; }
        public string InternationalPhone { get; set; }
        [RegularExpression(@"^[1-9][0-9]{3,5}$", ErrorMessageResourceName = "FieldInvalidASINumber", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(6, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldLength")]
        public string ASINumber { get; set; }
        public bool HasShipAddress { get; set; }
        public bool HasBillAddress { get; set; }
        public bool HasBankInformation { get; set; }

        #region Billing information

        [Display(ResourceType = typeof(Resource), Name = "BillingTollPhone")]
        public string BillingTollFree { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Fax")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string BillingFax { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Street1")]
        public string BillingAddress1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Street2")]
        public string BillingAddress2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "City")]
        public string BillingCity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "State")]
        public string BillingState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Zipcode")]
        public string BillingZip { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Country")]
        public string BillingCountry { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Phone")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
        public string BillingPhone { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string BillingEmail { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "WebUrl")]
        [DataType(DataType.Url)]
        public string BillingWebUrl { get; set; }

        #endregion Billing information

        #region shipping information

        [Display(ResourceType = typeof(Resource), Name = "ShippingAddress")]
        public string ShippingStreet1 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingAddress2")]
        public string ShippingStreet2 { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingCity")]
        public string ShippingCity { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingState")]
        public string ShippingState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "ShippingZip")]
        public string ShippingZip { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Country")]
        public string ShippingCountry { get; set; }

        #endregion shipping information

        #region Cost information
        public decimal ItemsCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal ApplicationFeeCost { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal SubscriptionCost { get; set; }
        public string SubscriptionFrequency { get; set; }
        public int Quantity { get; set; }
        public decimal PromotionalDiscount { get; set; }
        #endregion

        #region Bank information
        public string BankName { get; set; }
        public string BankState { get; set; }
        public string BankCity { get; set; }
        #endregion

        #region Catalog Advertising information
        public int Id { get; set; }

        public IList<CatalogAdvertisingItem> CatalogAdvertisingItems { get; set; }

        public IList<CatalogAdvertisingTieredProductPricing> Prices { get; set; }

        public int Sequence { get; set; }

        #endregion Catalog Advertising information

        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public StoreIndividual BillingIndividual { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public string BackendReference { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }

        public IList<int> ids { get; set; }

        public IList<StoreIndividual> Contacts { get; set; }

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