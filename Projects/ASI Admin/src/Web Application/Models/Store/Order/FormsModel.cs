using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.model.store
{
    public class FormsModel : IMembershipModel
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
        public string StartDate { get; set; }
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
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(Resource))]
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
        public decimal Cost { get; set; }
        public bool IsStoreRequest { get; set; }
        
        public string ActionName { get; set; }
        public string AcceptedByName { get; set; }
        public StoreIndividual BillingIndividual { get; set; }
        public string ExternalReference { get; set; }
        public string BackendReference { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int? OptionId { get; set; }
        public decimal Price { get; set; }

        public IList<StoreIndividual> Contacts { get; set; }
        public FormInstance FormInstanceObject { get; set; }
        public int FormInstanceId { get; set; }
        
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public FormsModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public FormsModel(StoreOrderDetail orderdetail, IStoreService storeService)
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;
            this.AcceptedByName = orderdetail.AcceptedByName;
            if (orderdetail.OptionId.HasValue) this.OptionId = orderdetail.OptionId;
            if (orderdetail.DateOption.HasValue) this.StartDate = orderdetail.DateOption.Value.ToString("MM/dd/yyyy");
            this.Quantity = orderdetail.Quantity;
            this.AcceptedByName = orderdetail.AcceptedByName;
           
            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                ProductId = orderdetail.Product.Id;
                Cost = orderdetail.Cost;
            }

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            IsStoreRequest = order.IsStoreRequest;
            FormInstanceObject = storeService.GetAll<FormInstance>(false).SingleOrDefault(f => f.OrderDetailId == OrderDetailId);
            if(FormInstanceObject != null) FormInstanceId = FormInstanceObject.Id;

            MembershipModelHelper.PopulateModel(this, orderdetail);
        }
    }
}