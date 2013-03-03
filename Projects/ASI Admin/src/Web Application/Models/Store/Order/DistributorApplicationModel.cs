using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class DistributorApplicationModel : DistributorMembershipApplication
    {
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        public DistributorApplicationModel()
        {
            //nothing to do
        }

        public DistributorApplicationModel(DistributorMembershipApplication application, asi.asicentral.model.store.Order order)
        {
            application.CopyTo(this);
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
        }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}