using asi.asicentral.interfaces;
using asi.asicentral.model.timss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class TIMSSService : IFulfilmentService
    {
        IObjectService _objectService;

        public TIMSSService(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public void Process(model.store.Order order, model.store.OrderDetailApplication application)
        {
            if (order == null || !order.TransId.HasValue) throw new InvalidOperationException("You must pass a valid Order for this method");
            if (application == null) throw new InvalidOperationException("You must pass a valid Application for this method");
            TIMSSCompany company = new TIMSSCompany()
            {
                CustomerClass = "SUPPLIER",
                Name = application.Company,
                ASINumber = order.ExternalReference,
            };
            _objectService.Add<TIMSSCompany>(company);
            TIMSSCreditInfo credit = new TIMSSCreditInfo()
            {
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
            };
            _objectService.Add<TIMSSCreditInfo>(credit);
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
                _objectService.Dispose();
            }
            //no unmanaged resource to free at this point
        }

        #endregion IDisposable
    }
}
