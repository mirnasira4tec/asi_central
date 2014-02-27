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
    public class SupplierApplicationModel : StoreDetailSupplierMembership, IMembershipModel
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

        [Display(ResourceType = typeof(Resource), Name = "Etching")]
        public bool Etching { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "HotStamping")]
        public bool HotStamping { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "SilkScreen")]
        public bool SilkScreen { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "PadPrint")]
        public bool PadPrint { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "DirectEmbroidery")]
        public bool DirectEmbroidery { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "FoilStamping")]
        public bool FoilStamping { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Lithography")]
        public bool Lithography { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Sublimination")]
        public bool Sublimination { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "FourColourProcess")]
        public bool FourColourProcess { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Engraving")]
        public bool Engraving { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "ContactName")]
        public bool Laser { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Offset")]
        public bool Offset { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Transfer")]
        public bool Transfer { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "FullColourProcess")]
        public bool FullColourProcess { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "DieStamp")]
        public bool DieStamp { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "OtherDecoratingMethod")]
        public bool OtherDecoratingMethod { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "OtherDecoratingMethodName")]
        public string OtherDecoratingMethodName { set; get; }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal Price { get; set; }

        public IList<StoreIndividual> Contacts { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public SupplierApplicationModel() : base()
        {
            this.DecoratingTypes = new List<LookSupplierDecoratingType>();
            this.Contacts = new List<StoreIndividual>();
        }

        public SupplierApplicationModel(StoreDetailSupplierMembership application, StoreOrderDetail orderdetail)
        {
            application.CopyTo(this);
            StoreOrder order = orderdetail.Order;
            UpdateDecoratingTypesProperties();
            if (!String.IsNullOrEmpty(OtherDec))
            {
                OtherDecoratingMethod = true;
                OtherDecoratingMethodName = this.OtherDec;
            }
            else OtherDecoratingMethod = false;

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
        }

        private void UpdateDecoratingTypesProperties()
        {
            Etching = HasDecorating(LookSupplierDecoratingType.DECORATION_ETCHING);
            HotStamping = HasDecorating(LookSupplierDecoratingType.DECORATION_HOTSTAMPING);
            SilkScreen = HasDecorating(LookSupplierDecoratingType.DECORATION_SILKSCREEN);
            PadPrint = HasDecorating(LookSupplierDecoratingType.DECORATION_PADPRINT);
            DirectEmbroidery = HasDecorating(LookSupplierDecoratingType.DECORATION_DIRECTEMBROIDERY);
            FoilStamping = HasDecorating(LookSupplierDecoratingType.DECORATION_FOILSTAMPING);
            Lithography = HasDecorating(LookSupplierDecoratingType.DECORATION_LITHOGRAPHY);
            Sublimination = HasDecorating(LookSupplierDecoratingType.DECORATION_SUBLIMINATION);
            FourColourProcess = HasDecorating(LookSupplierDecoratingType.DECORATION_FOURCOLOR);
            Engraving = HasDecorating(LookSupplierDecoratingType.DECORATION_ENGRAVING);
            Laser = HasDecorating(LookSupplierDecoratingType.DECORATION_LASER);
            Offset = HasDecorating(LookSupplierDecoratingType.DECORATION_OFFSET);
            Transfer = HasDecorating(LookSupplierDecoratingType.DECORATION_TRANSFER);
            FullColourProcess = HasDecorating(LookSupplierDecoratingType.DECORATION_FULLCOLOR);
            DieStamp = HasDecorating(LookSupplierDecoratingType.DECORATION_DIESTAMP);
        }

        private bool HasDecorating(string DecorationName)
        {
            return DecoratingTypes.Where(type => type.Description == DecorationName).Count() == 1;
        }

        /// <summary>
        /// Apply the extra bool values from the view model to the many to many
        /// </summary>
        /// <param name="StoreService"></param>
        public void SyncDecoratingTypes(IList<LookSupplierDecoratingType> decoratingTypes, StoreDetailSupplierMembership application)
        {
            AddDecoratingType(this.Etching, LookSupplierDecoratingType.DECORATION_ETCHING, decoratingTypes, application);
            AddDecoratingType(this.HotStamping, LookSupplierDecoratingType.DECORATION_HOTSTAMPING, decoratingTypes, application);
            AddDecoratingType(this.SilkScreen, LookSupplierDecoratingType.DECORATION_SILKSCREEN, decoratingTypes,application);
            AddDecoratingType(this.PadPrint, LookSupplierDecoratingType.DECORATION_PADPRINT, decoratingTypes, application);
            AddDecoratingType(this.DirectEmbroidery, LookSupplierDecoratingType.DECORATION_DIRECTEMBROIDERY, decoratingTypes, application);
            AddDecoratingType(this.FoilStamping, LookSupplierDecoratingType.DECORATION_FOILSTAMPING, decoratingTypes, application);
            AddDecoratingType(this.Lithography, LookSupplierDecoratingType.DECORATION_LITHOGRAPHY, decoratingTypes, application);
            AddDecoratingType(this.Sublimination, LookSupplierDecoratingType.DECORATION_SUBLIMINATION, decoratingTypes, application);
            AddDecoratingType(this.FourColourProcess, LookSupplierDecoratingType.DECORATION_FOURCOLOR, decoratingTypes, application);
            AddDecoratingType(this.Engraving, LookSupplierDecoratingType.DECORATION_ENGRAVING, decoratingTypes, application);
            AddDecoratingType(this.Laser, LookSupplierDecoratingType.DECORATION_LASER, decoratingTypes, application);
            AddDecoratingType(this.Offset, LookSupplierDecoratingType.DECORATION_OFFSET, decoratingTypes, application);
            AddDecoratingType(this.Transfer, LookSupplierDecoratingType.DECORATION_TRANSFER, decoratingTypes, application);
            AddDecoratingType(this.FullColourProcess, LookSupplierDecoratingType.DECORATION_FULLCOLOR, decoratingTypes, application);
            AddDecoratingType(this.DieStamp, LookSupplierDecoratingType.DECORATION_DIESTAMP, decoratingTypes, application);
        }

        private void AddDecoratingType(bool selected, String typeName, IList<LookSupplierDecoratingType> decoratingTypes, StoreDetailSupplierMembership application)
        {
            LookSupplierDecoratingType existing = application.DecoratingTypes.Where(decType => decType.Description == typeName).SingleOrDefault();
            if (selected && existing == null) application.DecoratingTypes.Add(decoratingTypes.Where(type => type.Description == typeName).SingleOrDefault());
            else if (!selected && existing != null) application.DecoratingTypes.Remove(existing);
        }
    }
}