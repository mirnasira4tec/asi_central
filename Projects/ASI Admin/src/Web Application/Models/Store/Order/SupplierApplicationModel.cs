using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class SupplierApplicationModel : SupplierMembershipApplication
    {
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 

        public List<SupplierMembershipApplicationContact> ModelContacts { set; get; }
        
        public SupplierApplicationModel()
        {
            //nothing to do
        }

        public List<SupplierMembershipApplicationContact> GetContactsFrom(SupplierMembershipApplication application)
        {
            List<SupplierMembershipApplicationContact> list = application.Contacts.ToList();
            return list;
        }
        
        public SupplierApplicationModel(SupplierMembershipApplication application, asi.asicentral.model.store.Order order)
        {
            this.ModelContacts = GetContactsFrom(application);

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