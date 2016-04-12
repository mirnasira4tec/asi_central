using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;
using asi.asicentral.Resources;
using asi.asicentral.web.store.interfaces;

namespace asi.asicentral.web.model.store
{
    public class EquipmentApplicationModel : StoreDetailEquipmentMembership, IMembershipModel
    {
        [Display(ResourceType = typeof(Resource), Name = "CompanyName")]
        public string Company { get; set; }
        public string CompanyStatus { get; set; }
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

        public IList<StoreSupplierRepresentativeInformation> Representatives { get; set; }

        // What diffent types of equipments do you offer?
        [Display(ResourceType = typeof(Resource), Name = "Embroidery")]
        public bool Embroidery { set; get; }
        [Display(ResourceType = typeof(Resource), Name = "EquipmentScreenPrinting")]
        public bool ScreenPrinting { set; get; }
        [Display(ResourceType = typeof(Resource), Name = "HeatTransfer")]
        public bool HeatTransfer { set; get; }
        [Display(ResourceType = typeof(Resource), Name = "Digitizing")]
        public bool Digitizing { set; get; }
        [Display(ResourceType = typeof(Resource), Name = "Engraving")]
        public bool Engraving { set; get; }
        [Display(ResourceType = typeof(Resource), Name = "Sublimation")]
        public bool Sublimation { set; get; }
        [Display(ResourceType = typeof(Resource), Name = "Monogramming")]
        public bool Monogramming { set; get; }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public string BackendReference { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal Price { get; set; }

        public IList<StoreIndividual> Contacts { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public EquipmentApplicationModel() : base()
        {
            this.Representatives = new List<StoreSupplierRepresentativeInformation>();
            this.EquipmentTypes = new List<LookEquipmentType>();
            this.Contacts = new List<StoreIndividual>();
        }

        public EquipmentApplicationModel(StoreDetailEquipmentMembership application, StoreOrderDetail orderdetail, IStoreService storeService)
        {
            application.CopyTo(this);
            StoreOrder order = orderdetail.Order;
            UpdateEquipmentTypesProperties();
            IList<StoreSupplierRepresentativeInformation> representatives = storeService.GetAll<StoreSupplierRepresentativeInformation>(true).Where(rep => rep.OrderDetailId == orderdetail.Id).ToList();
            this.Representatives = new List<StoreSupplierRepresentativeInformation>();
            foreach (string rep in StoreSupplierRepresentativeInformation.SUPPLIER_REPRESENTATIVES)
            {
                StoreSupplierRepresentativeInformation newRep = null;
                if (representatives != null)
                {
                    newRep = representatives.SingleOrDefault(r => r.Role == rep);
                }
                if (newRep == null)
                {
                    newRep = new StoreSupplierRepresentativeInformation();
                    newRep.Role = rep;
                    newRep.OrderDetailId = orderdetail.Id;
                }
                Representatives.Add(newRep);
            }
            
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
        }

        private void UpdateEquipmentTypesProperties()
        {
            Embroidery = HasEquipment(LookEquipmentType.EMBROIDERY);
            ScreenPrinting = HasEquipment(LookEquipmentType.SCREENPRINTING);
            HeatTransfer = HasEquipment(LookEquipmentType.HEATTRANSFER);
            Digitizing = HasEquipment(LookEquipmentType.DIGITIZING);
            Engraving = HasEquipment(LookEquipmentType.ENGRAVING);
            Sublimation = HasEquipment(LookEquipmentType.SUBLIMITION);
            Monogramming = HasEquipment(LookEquipmentType.MONOGRAMING);
        }

        private bool HasEquipment(string EquipmentName)
        {
            return EquipmentTypes.Where(type => type.Description == EquipmentName).Count() == 1;
        }

        /// <summary>
        /// Apply the extra bool values from the view model to the many to many
        /// </summary>
        /// <param name="StoreService"></param>
        public void SyncEquipmentTypes(IList<LookEquipmentType> equipmentTypes, StoreDetailEquipmentMembership application)
        {
            AddEquipmentType(this.Embroidery, LookEquipmentType.EMBROIDERY, equipmentTypes, application);
            AddEquipmentType(this.ScreenPrinting, LookEquipmentType.SCREENPRINTING, equipmentTypes, application);
            AddEquipmentType(this.HeatTransfer, LookEquipmentType.HEATTRANSFER, equipmentTypes, application);
            AddEquipmentType(this.Digitizing, LookEquipmentType.DIGITIZING, equipmentTypes, application);
            AddEquipmentType(this.Engraving, LookEquipmentType.ENGRAVING, equipmentTypes, application);
            AddEquipmentType(this.Sublimation, LookEquipmentType.SUBLIMITION, equipmentTypes, application);
            AddEquipmentType(this.Monogramming, LookEquipmentType.MONOGRAMING, equipmentTypes, application);
        }

        private void AddEquipmentType(bool selected, String typeName, IList<LookEquipmentType> equipmentTypes, StoreDetailEquipmentMembership application)
        {
            LookEquipmentType existing = application.EquipmentTypes.Where(equType => equType.Description == typeName).SingleOrDefault();
            if (selected && existing == null) application.EquipmentTypes.Add(equipmentTypes.Where(type => type.Description == typeName).SingleOrDefault());
            else if (!selected && existing != null) application.EquipmentTypes.Remove(existing);
        }
    }
}