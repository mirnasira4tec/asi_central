using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.web.store.interfaces
{
    public interface IMembershipModel
    {
        string Company { get; set; }
        string CompanyStatus { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string Zip { get; set; }
        string State { get; set; }
        string Country { get; set; }
        string Phone { get; set; }
        string InternationalPhone { get; set; }
        string CompanyEmail { get; set; }
        string ASINumber { get; set; }
        bool HasShipAddress { get; set; }
        bool HasBillAddress { get; set; }
        bool HasBankInformation { get; set; }
        int? OptionId { get; set; }
        int ContextId { get; set; }
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

        #region Cost information
        decimal ItemsCost { get; set; }
        decimal TaxCost { get; set; }
        decimal ApplicationFeeCost { get; set; }
        decimal ShippingCost { get; set; }
        decimal TotalCost { get; set; }
        decimal SubscriptionCost { get; set; }
        string SubscriptionFrequency { get; set; }
        int Quantity { get; set; }
        decimal PromotionalDiscount { get; set; }
        #endregion

        #region OrderInformation
        int OrderId { get; set; }
        string ActionName { get; set; }
        string ExternalReference { get; set; }
        string BackendReference { get; set; }
        OrderStatus OrderStatus { get; set; }
        bool IsCompleted { get; set; }
        #endregion

        #region Bank information
        string BankName { get; set; }
        string BankState { get; set; }
        string BankCity { get; set; }
        #endregion
    }
}
