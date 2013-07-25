using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.model.store
{
    public class MagazinesApplicationModel : IMembershipModel
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

        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        public IList<StoreMagazineSubscription> Subscriptions { get; set; }
        public StoreIndividual  BillingIndividual { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
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

        public MagazinesApplicationModel(StoreOrderDetail orderdetail)
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
            

            
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, order);
            FillEditableItemDetails(order, Subscriptions);
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