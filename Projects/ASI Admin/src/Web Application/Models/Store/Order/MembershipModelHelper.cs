using asi.asicentral.model.store;
using asi.asicentral.services;
using asi.asicentral.util.store;
using asi.asicentral.web.store.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class MembershipModelHelper
    {
        public static void PopulateModel(IMembershipModel model, StoreOrderDetail orderDetail)
        {
            if (orderDetail == null || orderDetail.Order == null) return;
            StoreOrder order = orderDetail.Order;

            if (order != null && order.Context != null) model.ContextId = order.Context.Id;
            //fill in company fields
            if (order.Company != null)
            {
                model.Company = order.Company.Name;
                model.CompanyEmail = order.Company.Email;
                model.CompanyStatus = order.Company.MemberStatus;
                model.HasShipAddress = order.Company.HasShipAddress;
                model.Phone = order.Company.Phone;
                model.BillingWebUrl = order.Company.WebURL;
                model.ASINumber = order.Company.ASINumber;
                model.BankName = order.Company.BankName;
                model.BankCity = order.Company.BankCity;
                model.BankState = order.Company.BankState;

                StoreAddress companyAddress = order.Company.GetCompanyAddress();
                if (companyAddress != null)
                {
                    model.Address1 = companyAddress.Street1;
                    model.Address2 = companyAddress.Street2;
                    model.City = companyAddress.City;
                    model.Zip = companyAddress.Zip;
                    model.State = companyAddress.State;
                    model.Country = companyAddress.Country;
                }
                //set contact information
                model.Contacts = order.Company.Individuals;
            }
            //get billing information
            if (order.BillingIndividual != null)
            {
                model.BillingEmail = order.BillingIndividual.Email;
                model.BillingFax = order.BillingIndividual.Fax;
                model.BillingPhone = order.BillingIndividual.Phone;
                if (order.BillingIndividual.Address != null)
                {
                    model.HasBillAddress = true;
                    model.BillingAddress1 = order.BillingIndividual.Address.Street1;
                    model.BillingAddress2 = order.BillingIndividual.Address.Street2;
                    model.BillingCity = order.BillingIndividual.Address.City;
                    model.BillingState = order.BillingIndividual.Address.State;
                    model.BillingZip = order.BillingIndividual.Address.Zip;
                    model.BillingCountry = order.BillingIndividual.Address.Country;
                }
            }
            //get shipping information
            if (model.HasShipAddress)
            {
                StoreAddress address = order.Company.Addresses.Where(add => add.IsShipping).First().Address;
                model.ShippingCity = address.City;
                model.ShippingCountry = address.Country;
                model.ShippingState = address.State;
                model.ShippingStreet1 = address.Street1;
                model.ShippingStreet2 = address.Street2;
                model.ShippingZip = address.Zip;
            }

            //get cost information
            model.ItemsCost = orderDetail.Cost;
            model.Quantity = orderDetail.Quantity;
            model.ApplicationFeeCost = orderDetail.ApplicationCost;
            model.TaxCost = orderDetail.TaxCost;
            model.ShippingCost = orderDetail.ShippingCost;
            model.PromotionalDiscount = orderDetail.DiscountAmount;
            model.TotalCost = order.Total;
            model.OptionId = (orderDetail.OptionId.HasValue) ? orderDetail.OptionId.Value : 0;

            decimal cost = orderDetail.Cost;
            int quantity = orderDetail.Quantity;

            if (orderDetail.Product != null)
            {
                model.HasBankInformation = orderDetail.Product.HasBankInformation;
                model.SubscriptionCost += (cost * quantity) + orderDetail.TaxCost + orderDetail.ShippingCost;

                model.SubscriptionFrequency = (!string.IsNullOrEmpty(orderDetail.Product.SubscriptionFrequency) ? (orderDetail.Product.SubscriptionFrequency == "M" ? "monthly" : "yearly") : string.Empty);
                if (orderDetail.Product.HasBackEndIntegration && !string.IsNullOrEmpty(order.BackendReference))
                {
                    model.BackendReference = order.BackendReference;
                }
            }
        }
    }
}