using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class DistributorApplicationModel : DistributorMembershipApplication
    {
        public string BuisnessRevenue { set; get; }

        public bool Signs { set; get; }
        public bool TrophyAwards { set; get; }
        public bool Printing { set; get; }
        public bool ScreenPrinting { set; get; }
        public bool PromotionalProducts { set; get; }
        public bool Other { set; get; }
        
        public bool ProductA { get; set; }
        public bool ProductB { get; set; }
        public bool ProductC { get; set; }
        public bool Product1 { get; set; }
        public bool ProductD { get; set; }
        public bool ProductO { get; set; }
        public bool ProductY { get; set; }
        public bool ProductZ { get; set; }
        public bool ProductL { get; set; }
        public bool ProductG { get; set; }
        public bool ProductF { get; set; }
        public bool ProductI { get; set; }
        public bool ProductV { get; set; }
        public bool ProductJ { get; set; }
        public bool ProductH { get; set; }
        public bool ProductK { get; set; }
        public bool ProductU { get; set; }
        public bool ProductX { get; set; }
        public bool ProductM { get; set; }
        public bool ProductN { get; set; }
        public bool Product3 { get; set; }
        public bool Product4 { get; set; }
        public bool Product5 { get; set; }
        public bool ProductE { get; set; }
        public bool ProductP { get; set; }
        public bool ProductQ { get; set; }
        public bool ProductW { get; set; }
        public bool Product2 { get; set; }
        public bool ProductR { get; set; }
        public bool ProductS { get; set; }
        public bool ProductT { get; set; }

        public bool AccountA { get; set; }
        public bool AccountV { get; set; }
        public bool AccountK { get; set; }
        public bool AccountS { get; set; }
        public bool AccountB { get; set; }
        public bool AccountI { get; set; }
        public bool AccountJ { get; set; }
        public bool AccountC { get; set; }
        public bool AccountD { get; set; }
        public bool AccountT { get; set; }
        public bool AccountW { get; set; }
        public bool AccountF { get; set; }
        public bool AccountH { get; set; }
        public bool AccountO { get; set; }
        public bool AccountX { get; set; }
        public bool AccountY { get; set; }
        public bool AccountL { get; set; }
        public bool AccountM { get; set; }
        public bool AccountU { get; set; }
        public bool AccountN { get; set; }
        public bool AccountP { get; set; }
        public bool AccountE { get; set; }
        public bool AccountZ { get; set; }
        public bool AccountQ { get; set; }
        public bool Account1 { get; set; }
        public bool AccountG { get; set; }
        public bool AccountR { get; set; }

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