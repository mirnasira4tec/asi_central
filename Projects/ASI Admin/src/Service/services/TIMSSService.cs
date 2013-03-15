using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.model.timss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    //questions for gary
    //  CountryCode?
    //  Credit Date Created UTC?
    //  Contact Primary Key
    public class TIMSSService : IFulfilmentService
    {
        IObjectService _objectService;

        public TIMSSService(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public void Process(model.store.Order order, model.store.OrderDetailApplication application)
        {
            if (order == null || !order.UserId.HasValue || order.CreditCard == null || string.IsNullOrEmpty(order.CreditCard.ExternalReference)) throw new InvalidOperationException("You must pass a valid Order for this method");
            if (application == null) throw new InvalidOperationException("You must pass a valid Application for this method");
            TIMSSCompany company = new TIMSSCompany()
            {
                DAPP_UserId = order.UserId.Value,
                CustomerClass = "SUPPLIER",
                Name = application.Company,
                MasterCustomerId = order.ExternalReference,
                BillAddress1 = application.BillingAddress1,
                BillAddress2 = application.BillingAddress2,
                BillCity = application.BillingCity,
                BillPostalCode = application.BillingZip,
                BillState = application.BillingState,
                BillCountryCode = application.BillingCountry,
                ShipAddress1 = application.ShippingStreet1,
                ShipAddress2 = application.ShippingStreet2,
                ShipCity = application.ShippingCity,
                ShipPostalCode = application.ShippingZip,
                ShipCountryCode = application.ShippingCountry,
                ShipState = application.ShippingState,
                Url = application.BillingWebUrl,
                PhoneNumber = application.Phone,
            };
            _objectService.Add<TIMSSCompany>(company);
            TIMSSCreditInfo credit = new TIMSSCreditInfo()
            {
                DAPP_UserId = order.UserId.Value,
                Name = order.CreditCard.Name,
                FirstName = GetNamePart(order.CreditCard.Name, true),
                LastName = GetNamePart(order.CreditCard.Name, false),
                ExpirationMonth = order.CreditCard.ExpMonth,
                ExpirationYear = order.CreditCard.ExpYear,
                Type = order.CreditCard.Type,
                Number = order.CreditCard.ExternalReference,
                Street1 = application.BillingAddress1,
                Street2 = application.BillingAddress2,
                Zip = application.BillingZip,
                City = application.BillingCountry,
                State = application.BillingState,
                Country = application.BillingCountry,
                ExternalReference = new Guid(order.CreditCard.ExternalReference),
                DateCreated = DateTime.Now,
            };
            _objectService.Add<TIMSSCreditInfo>(credit);
            //add the contacts
            if (application is SupplierMembershipApplication)
            {
                SupplierMembershipApplication supplierApplication = application as SupplierMembershipApplication;
                foreach (SupplierMembershipApplicationContact contact in supplierApplication.Contacts)
                {
                    TIMSSContact timssContact = new TIMSSContact()
                    {
                        DAPP_UserId = order.UserId.Value,
                        FirstName = GetNamePart(contact.Name, true),
                        LastName = GetNamePart(contact.Name, false),
                        PhoneNumber = contact.Phone,
                        Title = contact.Title,
                        Email = contact.Email,
                        PrimaryFlag = contact.IsPrimary ? "T" : "F",
                    };
                    _objectService.Add<TIMSSContact>(timssContact);
                }
            }
            _objectService.SaveChanges();
        }

        private string GetNamePart(string name, bool first)
        {
            string[] nameParts = name.Split(' ');
            if (nameParts.Length > 0 && first) return nameParts[0].Trim();
            else if (!first && nameParts.Length > 0) return nameParts[nameParts.Length - 1].Trim();
            else return name;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            //no unmanaged resource to free at this point
        }

        #endregion IDisposable
    }
}
