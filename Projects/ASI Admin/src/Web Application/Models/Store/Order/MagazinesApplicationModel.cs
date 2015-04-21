using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;
using asi.asicentral.Resources;

namespace asi.asicentral.web.model.store
{
    public class MagazinesApplicationModel : MembershipModel
    {
        public IList<StoreMagazineSubscription> Subscriptions { get; set; }       
        public bool IsBillingEditable { get; set; }
        public bool IsShippingEditable { get; set; }
        public bool IsPrimaryEditable { get; set; }
        public bool IsSecondaryEditable { get; set; }
        public bool IsBillingContactEditable { get; set; }
        public bool IsCompanyAddressEditable { get; set; }
        public bool IsHallmarkProduct { get; set; }
        public IDictionary<string, string> hallmarkInformation { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public MagazinesApplicationModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            this.Subscriptions = new List<StoreMagazineSubscription>();
        }

        public MagazinesApplicationModel(StoreOrderDetail orderdetail, IStoreService storeService)
        {
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;
            if (orderdetail.MagazineSubscriptions != null && orderdetail.MagazineSubscriptions.Count > 0) Subscriptions = orderdetail.MagazineSubscriptions;
            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                if (ProductName == "Stitches" || ProductName == "Wearables") IsHallmarkProduct = true;
                else IsHallmarkProduct = false;
            }

            if (IsHallmarkProduct && OrderDetailId != 0)
                hallmarkInformation = GetHallmarkDetails(OrderDetailId, storeService);
            
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
            Price = order.Total;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
            FillEditableItemDetails(order, Subscriptions);
        }

        private IDictionary<string, string> GetHallmarkDetails(int orderDetailId, IStoreService storeService)
        {
            IDictionary<string, string> hallmarkInformation = null;
            StoreDetailHallmarkRequest hallmarkRequest = storeService.GetAll<StoreDetailHallmarkRequest>().Where(request => request.OrderDetailId == orderDetailId).SingleOrDefault();
            if (hallmarkRequest != null && !string.IsNullOrEmpty(hallmarkRequest.WebRequest))
            {
                var items = hallmarkRequest.WebRequest.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Split(new[] { '=' }));
                hallmarkInformation = new Dictionary<string, string>();
                foreach (var item in items) hallmarkInformation.Add(item[0], item[1]);
            }
            return hallmarkInformation;
        }

        private void FillEditableItemDetails(StoreOrder order, IList<StoreMagazineSubscription> subscriptions)
        {
            IsBillingEditable = true;
            IsShippingEditable = true;
            IsPrimaryEditable = true;
            IsSecondaryEditable = true;
            IsBillingContactEditable = true;
            IsCompanyAddressEditable = true;
            if (order != null && order.Company != null)
            {
                if(order.Company.Addresses != null && order.Company.Addresses.Count > 0)
                {
                    foreach (StoreCompanyAddress companyAddress in order.Company.Addresses)
                    {
                        if (subscriptions != null && subscriptions.Count > 0)
                        {
                            StoreMagazineSubscription subscription = subscriptions.Where(sub => sub.Contact.Address.Id == companyAddress.Address.Id).SingleOrDefault();
                            if (subscription != null)
                            {
                                if (companyAddress.IsBilling)
                                    IsBillingEditable = false;
                                else if (companyAddress.IsShipping)
                                    IsShippingEditable = false;

                                StoreAddress address = order.Company.GetCompanyAddress();
                                if (address != null && subscription.Contact != null && subscription.Contact.Address != null && address.Id == subscription.Contact.Address.Id)
                                    IsCompanyAddressEditable = false;

                            }
                        }
                    }
                }

                if (order.Company.Individuals != null && order.Company.Individuals.Count > 0)
                {
                    foreach (StoreIndividual individual in order.Company.Individuals)
                    {
                        if (subscriptions != null && subscriptions.Count > 0)
                        {
                            StoreMagazineSubscription subscription = subscriptions.Where(sub => sub.Contact.Id == individual.Id).SingleOrDefault();
                            if (subscription != null)
                            {
                                if (individual.IsPrimary)
                                    IsPrimaryEditable = false;
                                else 
                                    IsSecondaryEditable = false;

                                if (order.BillingIndividual.Id == individual.Id)
                                    IsBillingContactEditable = false;
                            }
                        }
                    }
                }
            }

        }
    }
}