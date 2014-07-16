using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class ESPAdvertisingModel : IMembershipModel
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

        #region ESP Advertising information
        [Display(ResourceType = typeof(Resource), Name = "NumberOfItems")]
        public string NumberOfItems_First { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfItems")]
        public string NumberOfItems_Second { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfItems")]
        public string NumberOfItems_Third { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfProducts")]
        public int Products_OptionId_First { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfProducts")]
        public int Products_OptionId_Second { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "NumberOfProducts")]
        public int Products_OptionId_Third { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "LogoPath")]
        public string LogoPath { get; set; }

        public string Video { get; set; }
        
        public string LoginScreen_Dates { get; set; }
        public IList<EventDetailsModel> Events { get; set; }

        #endregion ESP Advertising information

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
        public IList<StoreDetailESPAdvertisingItem> Videos{ get; set; }
        public IList<StoreDetailEspTowerAd> ESPTowerAds { get; set; }
        public IList<StoreIndividual> Contacts { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public ESPAdvertisingModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public ESPAdvertisingModel(StoreOrderDetail orderdetail, StoreDetailESPAdvertising espAdvertising, IStoreService storeService)
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
                ProductName = orderdetail.Product.Name;
                ProductId = orderdetail.Product.Id;
            }

            #region Fill ESP Advertising details based on product
            switch (ProductId)
            {
                case 48:
                    ESPTowerAds = storeService.GetAll<StoreDetailEspTowerAd>(true).Where(towerAd => towerAd.OrderDetailId == OrderDetailId).ToList();
                    break;
                case 49:
                    List<StoreDetailESPAdvertisingItem> dbEvents = storeService.GetAll<StoreDetailESPAdvertisingItem>().Where(model => model.OrderDetailId == OrderDetailId).OrderBy(model => model.Sequence).ToList();
                    if (dbEvents != null && dbEvents.Count > 0)
                    {
                        List<LookEventMerchandiseProduct> dbAllEvents = storeService.GetAll<LookEventMerchandiseProduct>().Where(model => !model.Deleted).OrderBy(model => model.Sequence).ToList();
                        Events = new List<EventDetailsModel>();
                        LookEventMerchandiseProduct eventProduct = null;
                        foreach (StoreDetailESPAdvertisingItem item in dbEvents)
                        {
                            EventDetailsModel modelEvent = new EventDetailsModel();
                            modelEvent.OptionId = item.OptionID;
                            modelEvent.ItemNumbers = item.ItemList;
                            eventProduct = dbAllEvents.Where(dbEvent => dbEvent.Id == item.OptionID).SingleOrDefault();
                            if (eventProduct != null) modelEvent.EventName = eventProduct.Name;
                            Events.Add(modelEvent);
                        }
                    }
                    break;
                case 50:
                    NumberOfItems_First = espAdvertising.FirstItemList;
                    Products_OptionId_First = 1;
                    NumberOfItems_Second = espAdvertising.SecondItemList;
                    Products_OptionId_Second = 1;
                    NumberOfItems_Third = espAdvertising.ThirdItemList;
                    Products_OptionId_Third = 1;
                    break;
                case 51:
                    LogoPath = espAdvertising.LogoPath;
                    break;
                case 52:
                        string LoginScreen_Dates = string.Empty;
                        List<StoreDetailESPAdvertisingItem> items = storeService.GetAll<StoreDetailESPAdvertisingItem>().Where(model => model.OrderDetailId == OrderDetailId).OrderBy(model => model.Sequence).ToList();
                        foreach (var dateitem in items)
                            LoginScreen_Dates += dateitem.AdSelectedDate.Date.ToString("dd-MM-yyyy") + "\r\n";
                     this.LoginScreen_Dates = LoginScreen_Dates;
                     this.LogoPath = espAdvertising.LogoPath;
                    break;
                case 53:
                    if (espAdvertising.ESPAdvertisingItems != null && espAdvertising.ESPAdvertisingItems.Count > 0)
                    {
                        Videos = new List<StoreDetailESPAdvertisingItem>();
                        foreach (StoreDetailESPAdvertisingItem item in espAdvertising.ESPAdvertisingItems)
                        {
                            Videos.Add(item);
                        }
                    }
                    break;
               
            }
            #endregion

            OrderId = order.Id;
            Price = order.Total;
            OrderStatus = order.ProcessStatus;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
        }
    }
}