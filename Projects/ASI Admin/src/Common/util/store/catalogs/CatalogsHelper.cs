﻿using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using asi.asicentral.interfaces;
using System.Web.Mvc;
using asi.asicentral.Resources;

namespace asi.asicentral.util.store
{
    public class CatalogsHelper
    {
        //Supplement options
        public readonly int[] CATALOG_SUPPLEMENT_PRODUCT_39 = { 23, 24 };
        //Cover options
        public static readonly int[] CATALOG_COVER_PRODUCT_35 = { 1, 6 };
        public static readonly int[] CATALOG_COVER_PRODUCT_36_38 = { 1 };
        public static readonly int[] CATALOG_COVER_PRODUCT_37 = { 1, 7 };
        public static readonly int[] CATALOG_COVER_PRODUCT_39_40 = { 1, 2, 3, 4 ,5 };
        //Area options
        public static readonly int[] CATALOG_AREA_PRODUCT_35_37_38 = { 8 };
        public static readonly int[] CATALOG_AREA_PRODUCT_36_39_40 = { 8, 9, 25 };
        //color options
        public static readonly int[] CATALOG_COLOR_PRODUCT_35_36_37_38_39_40 = { 11, 26 };
        public static readonly int[] CATALOG_IMPRINT_PRODUCT_35_36_37_38_39_40 = { 18, 19, 20, 21 };

        private IStoreService storeService;
        private int productId;
        private IList<LookCatalogOption> catalogOptions { get; set; }

        public CatalogsHelper(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        public CatalogsHelper(IStoreService storeService, int productId, IList<LookCatalogOption> catalogOptions)
        {
            this.storeService = storeService;
            this.productId = productId;
            this.catalogOptions = catalogOptions;
        }

        public IList<SelectListItem> GetShippingOptions(string origin, string country)
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
                            text = Resource.UPS2Day;
                            break;
                        case "UPSGround":
                            text = Resource.UPSGround;
                            break;
                        case "UPSOvernight":
                            text = Resource.UPSOvernight;
                            break;
                    }
                    if (!string.IsNullOrEmpty(text))
                        dropdownOptions.Add(new SelectListItem() { Text = text, Value = option.ShippingMethod, Selected = false });
                }
            }
            return dropdownOptions;
        }

        public IList<SelectListItem> GetOptionsByCategory(int categoryId)
        {
            IList<SelectListItem> selectedItems = null;
            IList<LookCatalogOption> optionsList = null;
            if (catalogOptions != null) optionsList = catalogOptions.Where(options => options.CategoryId == categoryId).ToList();

            if (optionsList != null)
            {
                selectedItems = new List<SelectListItem>();
                foreach (LookCatalogOption option in optionsList)
                {
                    switch (categoryId)
                    {
                        case 1:
                            switch (productId)
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
                                    if (CATALOG_COVER_PRODUCT_39_40.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                                case 40:
                                    if (CATALOG_COVER_PRODUCT_39_40.Contains(option.Id))
                                        selectedItems.Add(new SelectListItem() { Text = option.Name, Value = option.Id.ToString(), Selected = false });
                                    break;
                            }
                            break;
                        case 2:
                            switch (productId)
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
                            switch (productId)
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
                            switch (productId)
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
                            switch (productId)
                            {
                                case 39:
                                    if (CATALOG_SUPPLEMENT_PRODUCT_39.Contains(option.Id))
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
    }
}
