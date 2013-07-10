﻿using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public interface IMembershipModel
    {
        string Company { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string Zip { get; set; }
        string State { get; set; }
        string Country { get; set; }
        string Phone { get; set; }
        string InternationalPhone { get; set; }
        bool HasShipAddress { get; set; }
        bool HasBillAddress { get; set; }
        IList<StoreIndividual> Contacts { get; set; }

        #region Billing information

        string BillingTollFree { get; set; }
        string BillingFax { get; set; }
        string BillingAddress1 { get; set; }
        string BillingAddress2 { get; set; }
        string BillingCity { get; set; }
        string BillingState { get; set; }
        string BillingZip { get; set; }
        string BillingCountry { get; set; }
        string BillingPhone { get; set; }
        string BillingEmail { get; set; }
        string BillingWebUrl { get; set; }

        #endregion Billing information

        #region shipping information

        string ShippingStreet1 { get; set; }
        string ShippingStreet2 { get; set; }
        string ShippingCity { get; set; }
        string ShippingState { get; set; }
        string ShippingZip { get; set; }
        string ShippingCountry { get; set; }

        #endregion shipping information
    }

    /// <summary>
    /// Populates the membership model classes based on order data
    /// </summary>
    public class MembershipModelHelper
    {
        public static void PopulateModel(IMembershipModel model, StoreOrder order)
        {
            //fill in company fields
            if (order.Company != null)
            {
                model.Company = order.Company.Name;
                model.HasShipAddress = order.Company.HasShipAddress;
                model.Phone = order.Company.Phone;
                model.BillingWebUrl = order.Company.WebURL;
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
        }
    }
}