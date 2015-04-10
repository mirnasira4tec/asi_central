using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;
using asi.asicentral.Resources;

namespace asi.asicentral.web.model.store
{
    public class MagazinesApplicationModel : IMembershipModel
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
        public int? OptionId { get; set; }
        public int ContextId { get; set; }

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
        public bool HasBankInformation { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "BankName")]
        public string BankName { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "BankState")]
        public string BankState { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "BankCity")]
        public string BankCity { get; set; }
        #endregion

        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public IList<StoreMagazineSubscription> Subscriptions { get; set; }
        public StoreIndividual  BillingIndividual { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public string BackendReference { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public bool IsBillingEditable { get; set; }
        public bool IsShippingEditable { get; set; }
        public bool IsPrimaryEditable { get; set; }
        public bool IsSecondaryEditable { get; set; }
        public bool IsBillingContactEditable { get; set; }
        public bool IsCompanyAddressEditable { get; set; }
        public bool IsHallmarkProduct { get; set; }

        public IList<StoreIndividual> Contacts { get; set; }
        public IDictionary<string, string> hallmarkInformation { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public MagazinesApplicationModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            this.Subscriptions = new List<StoreMagazineSubscription>();
        }

        public MagazinesApplicationModel(StoreOrderDetail orderdetail, IStoreService storeService)
        {
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;
            if (orderdetail.MagazineSubscriptions != null && orderdetail.MagazineSubscriptions.Count > 0) Subscriptions = orderdetail.MagazineSubscriptions;
            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                if (ProductName == "Stitches" || ProductName == "Wearables") IsHallmarkProduct = true;
                else IsHallmarkProduct = false;
            }

            if (IsHallmarkProduct && OrderDetailId != 0)
                hallmarkInformation = GetHallmarkDetails(OrderDetailId, storeService);
            
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
            FillEditableItemDetails(order, Subscriptions);
        }

        private IDictionary<string, string> GetHallmarkDetails(int orderDetailId, IStoreService storeService)
        {
            IDictionary<string, string> hallmarkInformation = null;
            StoreDetailHallmarkRequest hallmarkRequest = storeService.GetAll<StoreDetailHallmarkRequest>().Where(request => request.OrderDetailId == orderDetailId).SingleOrDefault();
            if (hallmarkRequest != null && !string.IsNullOrEmpty(hallmarkRequest.WebRequest))
            {
                var items = hallmarkRequest.WebRequest.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { '=' }));
                hallmarkInformation = new Dictionary<string, string>();
                foreach (var item in items) hallmarkInformation.Add(item[0], item[1]);
            }
            return hallmarkInformation;
        }

        private void FillEditableItemDetails(StoreOrder order, IList<StoreMagazineSubscription> subscriptions)
        {
            IsBillingEditable = true;
            IsShippingEditable = true;
            IsPrimaryEditable = true;
            IsSecondaryEditable = true;
            IsBillingContactEditable = true;
            IsCompanyAddressEditable = true;
            if (order != null && order.Company != null)
            {
                if(order.Company.Addresses != null && order.Company.Addresses.Count > 0)
                {
                    foreach (StoreCompanyAddress companyAddress in order.Company.Addresses)
                    {
                        if (subscriptions != null && subscriptions.Count > 0)
                        {
                            StoreMagazineSubscription subscription = subscriptions.Where(sub => sub.Contact.Address.Id == companyAddress.Address.Id).SingleOrDefault();
                            if (subscription != null)
                            {
                                if (companyAddress.IsBilling)
                                    IsBillingEditable = false;
                                else if (companyAddress.IsShipping)
                                    IsShippingEditable = false;

                                StoreAddress address = order.Company.GetCompanyAddress();
                                if (address != null && subscription.Contact != null && subscription.Contact.Address != null && address.Id == subscription.Contact.Address.Id)
                                    IsCompanyAddressEditable = false;

                            }
                        }
                    }
                }

                if (order.Company.Individuals != null && order.Company.Individuals.Count > 0)
                {
                    foreach (StoreIndividual individual in order.Company.Individuals)
                    {
                        if (subscriptions != null && subscriptions.Count > 0)
                        {
                            StoreMagazineSubscription subscription = subscriptions.Where(sub => sub.Contact.Id == individual.Id).SingleOrDefault();
                            if (subscription != null)
                            {
                                if (individual.IsPrimary)
                                    IsPrimaryEditable = false;
                                else 
                                    IsSecondaryEditable = false;

                                if (order.BillingIndividual.Id == individual.Id)
                                    IsBillingContactEditable = false;
                            }
                        }
                    }
                }
            }

        }
    }
}