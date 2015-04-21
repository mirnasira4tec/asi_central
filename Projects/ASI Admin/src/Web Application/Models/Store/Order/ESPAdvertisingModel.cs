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
    public class ESPAdvertisingModel : MembershipModel
    { 
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

        public IList<StoreDetailESPAdvertisingItem> Videos{ get; set; }
        public IList<StoreDetailEspTowerAd> ESPTowerAds { get; set; }
      

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