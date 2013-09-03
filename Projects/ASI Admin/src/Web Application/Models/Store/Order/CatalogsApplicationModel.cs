using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;
using System.Web.Mvc;
using asi.asicentral.util.store;

namespace asi.asicentral.web.model.store
{
    public class CatalogsApplicationModel : IMembershipModel
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

        #region Catalog information

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Cover")]
        public string Cover { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Area")]
        public string Area { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Color")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "ImprintInstruction")]
        public string Imprint { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Supplement")]
        public string Supplement { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line1")]
        public string Line1 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line2")]
        public string Line2 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line3")]
        public string Line3 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line4")]
        public string Line4 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line5")]
        public string Line5 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line6")]
        public string Line6 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line1")]
        public string BackLine1 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line2")]
        public string BackLine2 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line3")]
        public string BackLine3 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Line4")]
        public string BackLine4 { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "Artwork")]
        public string ArtworkOption { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "LogoPath")]
        public string LogoPath { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "IsArtworkToProof")]
        public bool IsArtworkToProof { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "IsUploadImageTobeUsed")]
        public bool IsUploadImageTobeUsed { get; set; }

        [Display(ResourceType = typeof(asi.asicentral.web.Resource), Name = "ShippingMethod")]
        public string ShippingMethod { get; set; }

        IList<SelectListItem> coverOptions { get; set; }
        public IList<SelectListItem> CoverOptions
        {
            get { return coverOptions; }
        }

        IList<SelectListItem> areaOptions { get; set; }
        public IList<SelectListItem> AreaOptions
        {
            get { return areaOptions; }
        }

        IList<SelectListItem> colorOptions { get; set; }
        public IList<SelectListItem> ColorOptions
        {
            get { return colorOptions; }
        }

        IList<SelectListItem> imprintOptions { get; set; }
        public IList<SelectListItem> ImprintOptions
        {
            get { return imprintOptions; }
        }

        IList<SelectListItem> supplementOptions { get; set; }
        public IList<SelectListItem> SupplementOptions
        {
            get { return supplementOptions; }
        }

        IList<SelectListItem> shippingOptions { get; set; }
        public IList<SelectListItem> ShippingOptions
        {
            get { return shippingOptions; }
        }

        #endregion

        public int OrderId { get; set; }
        public int OrderDetailId { get; set; }
        [RegularExpression(@"^(?=[^0-9]*[0-9])[0-9\s!@#$%^&*()_\-+]+$", ErrorMessageResourceName = "FieldInvalidNumber", ErrorMessageResourceType = typeof(asi.asicentral.Resource))]
        public string Quantity { get; set; }
        public StoreDetailCatalog StoreDetailCatalog { get; set; }
        public StoreIndividual BillingIndividual { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public bool IsCompleted { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public IStoreService StoreService { get; set; }
        public IList<StoreIndividual> Contacts { get; set; }
        
        private CatalogsHelper catalogsHelper { get; set; }
        private IList<LookCatalogOption> catalogOptions { get; set; }
        
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public CatalogsApplicationModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public CatalogsApplicationModel (StoreOrderDetail orderdetail, StoreDetailCatalog storeDetailCatalog, IStoreService storeService): base()
        {
            string origin = string.Empty;
            string country = string.Empty;
            this.Contacts = new List<StoreIndividual>();
            this.StoreService = storeService;
            this.catalogOptions = StoreService.GetAll<LookCatalogOption>(true).Where(option => option.Id != 0).ToList();
            if(orderdetail == null || storeDetailCatalog == null) throw new Exception("Invalid catalog request");
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;
            ShippingMethod = orderdetail.ShippingMethod;
            this.Quantity = orderdetail.Quantity.ToString();
            this.StoreDetailCatalog = storeDetailCatalog;

            this.Cover = this.StoreDetailCatalog.CoverId.ToString();
            this.Area = this.StoreDetailCatalog.AreaId.ToString();
            this.Color = this.StoreDetailCatalog.ColorId.ToString();
            this.Imprint = this.StoreDetailCatalog.ImprintId.ToString();
            if (this.StoreDetailCatalog.ImprintId == 21) this.ArtworkOption = this.StoreDetailCatalog.ArtworkOption;

            if (storeDetailCatalog.ImprintId != 18) this.IsArtworkToProof = storeDetailCatalog.IsArtworkToProof;
            if ((storeDetailCatalog.AreaId == 8 || storeDetailCatalog.AreaId == 25) && (storeDetailCatalog.ImprintId == 20 || (storeDetailCatalog.ImprintId == 21 && storeDetailCatalog.ArtworkOption == "PRINT")))
            {
                this.Line1 = storeDetailCatalog.Line1;
                this.Line2 = storeDetailCatalog.Line2;
                this.Line3 = storeDetailCatalog.Line3;
                this.Line4 = storeDetailCatalog.Line4;
                this.Line5 = storeDetailCatalog.Line5;
                this.Line6 = storeDetailCatalog.Line6;
            }

            if ((storeDetailCatalog.AreaId == 9 || storeDetailCatalog.AreaId == 25) && (storeDetailCatalog.ImprintId == 20 || (storeDetailCatalog.ImprintId == 21 && storeDetailCatalog.ArtworkOption == "PRINT")))
            {
                this.BackLine1 = storeDetailCatalog.BackLine1;
                this.BackLine2 = storeDetailCatalog.BackLine2;
                this.BackLine3 = storeDetailCatalog.BackLine3;
                this.BackLine4 = storeDetailCatalog.BackLine4;
            }
            this.IsUploadImageTobeUsed = storeDetailCatalog.IsUploadImageTobeUsed;
            this.LogoPath = storeDetailCatalog.LogoPath;

            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                ProductId = orderdetail.Product.Id;
                origin = orderdetail.Product.Origin;
                if (orderdetail.Product.Id == 39) this.Supplement = storeDetailCatalog.SupplementId.ToString();
                catalogsHelper = new CatalogsHelper(storeService, this.ProductId, this.catalogOptions);
            }

            this.coverOptions = catalogsHelper.GetOptionsByCategory(1);
            this.areaOptions = catalogsHelper.GetOptionsByCategory(2);
            this.colorOptions = catalogsHelper.GetOptionsByCategory(3);
            this.imprintOptions = catalogsHelper.GetOptionsByCategory(4);
            this.supplementOptions = catalogsHelper.GetOptionsByCategory(5);

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, order);
            if(order.Company != null) country = order.Company.GetCompanyShippingAddress().Country;
            this.shippingOptions = catalogsHelper.GetShippingOptions(origin, country);
        }
    }
}