using asi.asicentral.model.store;
using asi.asicentral.Resources;
using asi.asicentral.web.store.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class DecoratorApplicationModel : StoreDetailDecoratorMembership, IMembershipModel
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
        [Display(ResourceType = typeof(Resource), Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string CompanyEmail { get; set; }
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

        public IList<StoreIndividual> Contacts { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal Price { get; set; }
       
        public bool Embroidery { get; set; }
        public bool ScreenPrinting { get; set; }
        public bool HeatTransfer { get; set; }
        public bool Digitziing { get; set; }
        public bool Engraving { get; set; }
        public bool Sublimation { get; set; }
        public bool Monogramming { get; set; }
        public bool Other { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        public DecoratorApplicationModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            this.ImprintTypes = new List<LookDecoratorImprintingType>();
        }

        public DecoratorApplicationModel(StoreDetailDecoratorMembership application, StoreOrderDetail orderDetail)
        {
            StoreOrder order = orderDetail.Order;
            application.CopyTo(this);
            GetImprintTypes();
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            IsCompleted = order.IsCompleted;
            Price = order.Total;
            MonthlyPrice = (order.Total - order.AnnualizedTotal) / 11;
            MembershipModelHelper.PopulateModel(this, orderDetail);
        }

        public void SyncImprintingTypesToApplication(IList<LookDecoratorImprintingType> imprintingType, StoreDetailDecoratorMembership application)
        {
            AddImprintingType(Embroidery, "A", imprintingType, application);
            AddImprintingType(ScreenPrinting, "B", imprintingType, application);
            AddImprintingType(HeatTransfer, "C", imprintingType, application);
            AddImprintingType(Digitziing, "D", imprintingType, application);
            AddImprintingType(Engraving, "E", imprintingType, application);
            AddImprintingType(Sublimation, "F", imprintingType, application);
            AddImprintingType(Monogramming, "G", imprintingType, application);
            AddImprintingType(Other, "H", imprintingType, application);
        }

        private void GetImprintTypes()
        {
            Embroidery = HasImprintingType("A");
            ScreenPrinting = HasImprintingType("B");
            HeatTransfer = HasImprintingType("C");
            Digitziing = HasImprintingType("D");
            Engraving = HasImprintingType("E");
            Sublimation = HasImprintingType("F");
            Monogramming = HasImprintingType("G");
            Other = HasImprintingType("H");
        }

        private bool HasImprintingType(string code)
        {
            return (this.ImprintTypes.Where(imprintingTypes => imprintingTypes.SubCode == code).Count() == 1);
        }

        private void AddImprintingType(bool selected, String codeName, IList<LookDecoratorImprintingType> imprintingTypes, StoreDetailDecoratorMembership application)
        {
            LookDecoratorImprintingType reference = application.ImprintTypes.Where(type => type.SubCode == codeName).SingleOrDefault();
            if (selected && reference == null) application.ImprintTypes.Add(imprintingTypes.Where(type => type.SubCode == codeName).SingleOrDefault());
            else if (!selected && reference != null) application.ImprintTypes.Remove(reference);
        }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string BackendReference { get; set; }
        public string ExternalReference { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsCompleted { get; set; }
    }
}