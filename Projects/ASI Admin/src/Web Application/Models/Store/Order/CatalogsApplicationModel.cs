using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;
using System.Web.Mvc;

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
        public IList<LookCatalogOption> CatalogOptions { get; set; }
        public IStoreService StoreService { get; set; }

        public IList<StoreIndividual> Contacts { get; set; }

        public readonly int[] CATALOG_SUPPLIMENT_PRODUCT_39 = { 23, 24 };

        private readonly int[] CATALOG_COVER_PRODUCT_35 = { 1, 6 };
        private readonly int[] CATALOG_COVER_PRODUCT_36_38 = { 1 };
        private readonly int[] CATALOG_COVER_PRODUCT_37 = { 1, 7 };
        private readonly int[] CATALOG_COVER_PRODUCT_39 = { 1, 2 };
        private readonly int[] CATALOG_COVER_PRODUCT_40 = { 1, 2, 3, 4, 5 };

        private readonly int[] CATALOG_AREA_PRODUCT_35_37_38 = { 8 };
        private readonly int[] CATALOG_AREA_PRODUCT_36_39_40 = { 8, 9, 25 };

        private readonly int[] CATALOG_COLOR_PRODUCT_35_36_37_38_39_40 = { 11, 26 };

        private readonly int[] CATALOG_IMPRINT_PRODUCT_35_36_37_38_39_40 = { 18, 19, 20, 21 };

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
            this.CatalogOptions = StoreService.GetAll<LookCatalogOption>(true).Where(option => option.Id != 0).ToList();
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
            }

            this.coverOptions = GetOptionsByCategory(1);
            this.areaOptions = GetOptionsByCategory(2);
            this.colorOptions = GetOptionsByCategory(3);
            this.imprintOptions = GetOptionsByCategory(4);
            this.supplementOptions = GetOptionsByCategory(5);

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, order);
            if(order.Company != null) country = order.Company.GetCompanyShippingAddress().Country;
            this.shippingOptions = GetShippingOptions(this.StoreService, origin, country);
        }

        private IList<SelectListItem> GetOptionsByCategory(int categoryId)
        {
            IList<SelectListItem> selectedItems = null;
            IList<LookCatalogOption> optionsList = null;
            if (CatalogOptions != null) optionsList = CatalogOptions.Where(options => options.CategoryId == categoryId).ToList();

            if (optionsList != null)
            {
                selectedItems = new List<SelectListItem>();
                foreach (LookCatalogOption option in optionsList)
                {
                    switch (categoryId)
                    {
                        case 1:
                            switch (ProductId)
                            {
                                case 35:
                                    if (CATALOG_COVER_PRODUCT_35.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                                case 36:
                                case 38:
                                    if (CATALOG_COVER_PRODUCT_36_38.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                                case 37:
                                    if (CATALOG_COVER_PRODUCT_37.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                                case 39:
                                    if (CATALOG_COVER_PRODUCT_39.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                                case 40:
                                    if (CATALOG_COVER_PRODUCT_40.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                            }
                            break;
                        case 2:
                            switch (ProductId)
                            {
                                case 35:
                                case 37:
                                case 38:
                                    if (CATALOG_AREA_PRODUCT_35_37_38.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                                case 36:
                                case 39:
                                case 40:
                                    if (CATALOG_AREA_PRODUCT_36_39_40.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                            }
                            break;
                        case 3:
                            switch (ProductId)
                            {
                                case 35:
                                case 36:
                                case 37:
                                case 38:
                                case 39:
                                case 40:
                                    if (CATALOG_COLOR_PRODUCT_35_36_37_38_39_40.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                            }
                            break;
                        case 4:
                            switch (ProductId)
                            {
                                case 35:
                                case 36:
                                case 37:
                                case 38:
                                case 39:
                                case 40:
                                    if (CATALOG_IMPRINT_PRODUCT_35_36_37_38_39_40.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                            }
                            break;
                        case 5:
                            switch (ProductId)
                            {
                                case 39:
                                    if (CATALOG_SUPPLIMENT_PRODUCT_39.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                            }
                            break;
                        default:
                            selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                            break;
                    }
                }
            }
            return selectedItems;
        }

        private IList<SelectListItem> GetShippingOptions(IStoreService storeService, string origin, string country)
        {
            IList<SelectListItem> dropdownOptions = new List<SelectListItem>();
            IList<LookProductShippingRate> shippingOptions = storeService.GetAll<LookProductShippingRate>().Where(item => item.Origin == origin && item.Country == country).Distinct().ToList();
            if (shippingOptions != null && shippingOptions.Count > 0)
            {
                foreach (LookProductShippingRate option in shippingOptions)
                {
                    string text = string.Empty;
                    switch (option.ShippingMethod)
                    {
                        case "UPS2Day":
                            text = asi.asicentral.web.Resource.UPS2Day;
                            break;
                        case "UPSGround":
                            text = asi.asicentral.web.Resource.UPSGround;
                            break;
                        case "UPSOvernight":
                            text = asi.asicentral.web.Resource.UPSOvernight;
                            break;
                    }
                    if (!string.IsNullOrEmpty(text))
                        dropdownOptions.Add(new SelectListItem() { Text = text, Value = option.ShippingMethod, Selected = false });
                }
            }
            return dropdownOptions;
        }
    }
}