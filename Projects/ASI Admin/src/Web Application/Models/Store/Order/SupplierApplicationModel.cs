using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class SupplierApplicationModel : SupplierMembershipApplication
    {
        public SupplierApplicationModel(SupplierMembershipApplication application, asi.asicentral.model.store.Order order)
        {
            application.CopyTo(this);
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
        }

        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
    }
}