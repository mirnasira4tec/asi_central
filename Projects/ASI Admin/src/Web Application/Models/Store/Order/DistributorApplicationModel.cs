using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class DistributorApplicationModel : DistributorMembershipApplication
    {
        public string PrimaryBusinessRevenue_modelview { set; get; }
        public bool Signs { set; get; }
        public bool TrophyAwards { set; get; }
        public bool Printing { set; get; }
        public bool ScreenPrinting { set; get; }
        public bool PromotionalProducts { set; get; }
        public bool Other { set; get; }

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
            GetPrimaryBusinessRevenue();
            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
        }

        private void GetPrimaryBusinessRevenue()
        {
            Signs = HasPrimaryBuisnessRevenue(DistributorBusinessRevenue.BUSINESSREVENUE_SIGNS);
            TrophyAwards = HasPrimaryBuisnessRevenue(DistributorBusinessRevenue.BUSINESSREVENUE_TROPHYAWARDS);
            Printing = HasPrimaryBuisnessRevenue(DistributorBusinessRevenue.BUSINESSREVENUE_PRINTING);
            ScreenPrinting = HasPrimaryBuisnessRevenue(DistributorBusinessRevenue.BUSINESSREVENUE_SCREENPRINTING);
            PromotionalProducts = HasPrimaryBuisnessRevenue(DistributorBusinessRevenue.BUSINESSREVENUE_PROMOTIONALPRODUCTS);
            Other = string.IsNullOrEmpty(this.OtherBusinessRevenue) ? false : true;
        }

        private bool HasPrimaryBuisnessRevenue(string name)
        {
            if (this.PrimaryBusinessRevenue != null) return (this.PrimaryBusinessRevenue.Name == name);
            else return false;
        }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}