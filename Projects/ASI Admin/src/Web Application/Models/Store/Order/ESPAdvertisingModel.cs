using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class ESPAdvertisingModel : IMembershipModel
    {
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "CompanyName")]
        public string Company { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street1")]
        public string Address1 { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street2")]
        public string Address2 { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "City")]
        public string City { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Zipcode")]
        public string Zip { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "State")]
        public string State { get; set; }
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Country")]
        public string Country { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Phone")]
        public string Phone { get; set; }
        public string InternationalPhone { get; set; }
        [RegularExpression(@"^[1-9][0-9]{3,5}$", ErrorMessageResourceName = "FieldInvalidASINumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        [StringLength(6, ErrorMessageResourceType = typeof(asi.asicentral.Resource), ErrorMessageResourceName = "FieldLength")]
        public string ASINumber { get; set; }
        public bool HasShipAddress { get; set; }
        public bool HasBillAddress { get; set; }

        #region Billing information

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "BillingTollPhone")]
        public string BillingTollFree { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Fax")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        public string BillingFax { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street1")]
        public string BillingAddress1 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Street2")]
        public string BillingAddress2 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "City")]
        public string BillingCity { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "State")]
        public string BillingState { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Zipcode")]
        public string BillingZip { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Country")]
        public string BillingCountry { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Phone")]
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        public string BillingPhone { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string BillingEmail { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "WebUrl")]
        [DataType(DataType.Url)]
        public string BillingWebUrl { get; set; }

        #endregion Billing information

        #region shipping information

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingAddress")]
        public string ShippingStreet1 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingAddress2")]
        public string ShippingStreet2 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingCity")]
        public string ShippingCity { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingState")]
        public string ShippingState { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "ShippingZip")]
        public string ShippingZip { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.Resource), Name = "Country")]
        public string ShippingCountry { get; set; }

        #endregion shipping information

        #region ESP Advertising information
        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "NumberOfItems")]
        public string NumberOfItems_First { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "NumberOfItems")]
        public string NumberOfItems_Second { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "NumberOfItems")]
        public string NumberOfItems_Third { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "NumberOfProducts")]
        public int Products_OptionId_First { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "NumberOfProducts")]
        public int Products_OptionId_Second { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "NumberOfProducts")]
        public int Products_OptionId_Third { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "LogoPath")]
        public string LogoPath { get; set; }

        public string LoginScreen_Dates { get; set; }

        #endregion ESP Advertising information

        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public StoreIndividual BillingIndividual { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public IList<StoreDetailESPAdvertisingItem> Videos{ get; set; }
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
                    Products_OptionId_First = espAdvertising.FirstOptionId;
                    LogoPath = espAdvertising.LogoPath;
                    break;
                case 49:
                    NumberOfItems_First = espAdvertising.FirstItemList;
                    break;
                case 50:
                    NumberOfItems_First = espAdvertising.FirstItemList;
                    Products_OptionId_First = espAdvertising.FirstOptionId;
                    NumberOfItems_Second = espAdvertising.SecondItemList;
                    Products_OptionId_Second = espAdvertising.SecondOptionId;
                    NumberOfItems_Third = espAdvertising.ThirdItemList;
                    Products_OptionId_Third = espAdvertising.ThirdOptionId;
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
                case 54:
                    Products_OptionId_First = espAdvertising.FirstOptionId - 1;
                    LogoPath = espAdvertising.LogoPath;
                    break;
            }
            #endregion

            OrderId = order.Id;
            Price = order.Total;
            OrderStatus = order.ProcessStatus;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, order);
        }
    }
}