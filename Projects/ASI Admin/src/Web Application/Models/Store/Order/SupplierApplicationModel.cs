using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.model.store
{
    public class SupplierApplicationModel : StoreDetailSupplierMembership
    {
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
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
        public bool Completed { get; set; }
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
            Completed = order.IsCompleted;

            //fill in company fields
            if (order.Company != null)
            {
                this.Company = order.Company.Name;
                HasShipAddress = order.Company.HasShipAddress;
                Phone = order.Company.Phone;
                BillingWebUrl = order.Company.WebURL;
                StoreAddress companyAddress = order.Company.GetCompanyAddress();
                if (companyAddress != null)
                {
                    Address1 = companyAddress.Street1;
                    Address2 = companyAddress.Street2;
                    City = companyAddress.City;
                    Zip = companyAddress.Zip;
                    State = companyAddress.State;
                    Country = companyAddress.Country;
                }
                //set contact information
                Contacts = order.Company.Individuals;
            }
            //get billing information
            if (order.BillingIndividual != null)
            {
                BillingEmail = order.BillingIndividual.Email;
                BillingFax = order.BillingIndividual.Fax;
                BillingPhone = order.BillingIndividual.Phone;
                if (order.BillingIndividual.Address != null)
                {
                    HasBillAddress = true;
                    BillingAddress1 = order.BillingIndividual.Address.Street1;
                    BillingAddress2 = order.BillingIndividual.Address.Street2;
                    BillingCity = order.BillingIndividual.Address.City;
                    BillingState = order.BillingIndividual.Address.State;
                    BillingZip = order.BillingIndividual.Address.Zip;
                    BillingCountry = order.BillingIndividual.Address.Country;
                }
            }
            //get shipping information
            if (HasShipAddress)
            {
                StoreAddress address = order.Company.Addresses.Where(add => add.IsShipping).First().Address;
                ShippingCity = address.City;
                ShippingCountry = address.Country;
                ShippingState = address.State;
                ShippingStreet1 = address.Street1;
                ShippingStreet2 = address.Street2;
                ShippingZip = address.Zip;
            }
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
            Laser = HasDecorating(LookSupplierDecoratingType.DECORATION_ENGRAVING);
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
        public void SyncDecoratingTypes(IList<LookSupplierDecoratingType> decoratingTypes)
        {
            AddDecoratingType(this.Etching, LookSupplierDecoratingType.DECORATION_ETCHING, decoratingTypes);
            AddDecoratingType(this.HotStamping, LookSupplierDecoratingType.DECORATION_HOTSTAMPING, decoratingTypes);
            AddDecoratingType(this.SilkScreen, LookSupplierDecoratingType.DECORATION_SILKSCREEN, decoratingTypes);
            AddDecoratingType(this.PadPrint, LookSupplierDecoratingType.DECORATION_PADPRINT, decoratingTypes);
            AddDecoratingType(this.DirectEmbroidery, LookSupplierDecoratingType.DECORATION_DIRECTEMBROIDERY, decoratingTypes);
            AddDecoratingType(this.FoilStamping, LookSupplierDecoratingType.DECORATION_FOILSTAMPING, decoratingTypes);
            AddDecoratingType(this.Lithography, LookSupplierDecoratingType.DECORATION_LITHOGRAPHY, decoratingTypes);
            AddDecoratingType(this.Sublimination, LookSupplierDecoratingType.DECORATION_SUBLIMINATION, decoratingTypes);
            AddDecoratingType(this.FourColourProcess, LookSupplierDecoratingType.DECORATION_FOURCOLOR, decoratingTypes);
            AddDecoratingType(this.Engraving, LookSupplierDecoratingType.DECORATION_ENGRAVING, decoratingTypes);
            AddDecoratingType(this.Laser, LookSupplierDecoratingType.DECORATION_LASER, decoratingTypes);
            AddDecoratingType(this.Offset, LookSupplierDecoratingType.DECORATION_OFFSET, decoratingTypes);
            AddDecoratingType(this.Transfer, LookSupplierDecoratingType.DECORATION_TRANSFER, decoratingTypes);
            AddDecoratingType(this.FullColourProcess, LookSupplierDecoratingType.DECORATION_FULLCOLOR, decoratingTypes);
            AddDecoratingType(this.DieStamp, LookSupplierDecoratingType.DECORATION_DIESTAMP, decoratingTypes);
        }

        private void AddDecoratingType(bool selected, String typeName, IList<LookSupplierDecoratingType> decoratingTypes)
        {
            if (selected)
            {
                LookSupplierDecoratingType type = decoratingTypes.Where(decType => decType.Description == typeName).SingleOrDefault();
                if (type != null) DecoratingTypes.Add(type);
            }
        }
    }
}