using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.Resources;
using asi.asicentral.util.store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class MagazinesAdvertisingApplicationModel : MembershipModel
    {
        #region Magazine Advertising information
        public int Id { get; set; }

        public int MagOrderDetailId { get; set; }

        public IList<MagazineAdvertisingItem> MagAdItem { get; set; }

        public int Sequence { get; set; }

        #endregion Magazine Advertising information

       public IList<int> ids { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public MagazinesAdvertisingApplicationModel()
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
        }

        public MagazinesAdvertisingApplicationModel(StoreOrderDetail orderdetail, IList<StoreDetailMagazineAdvertisingItem> magazineAdvertising, IStoreService storeService)
            : base()
        {
            this.Contacts = new List<StoreIndividual>();
            StoreOrder order = orderdetail.Order;
            BillingIndividual = order.BillingIndividual;
            OrderDetailId = orderdetail.Id;

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            if (orderdetail.Product != null)
            {
                ProductName = orderdetail.Product.Name;
                ProductId = orderdetail.Product.Id;
            }
            
            MagazineType magType = (MagazineType)ProductId;
            var issue = storeService.GetAll<LookMagazineIssue>(true).Where(item => item.ProductId == magType).GroupBy(iss => new { iss.MaterialDeadline, iss.MailingDate, iss.ReservationDeadline }).ToList();
            ids = new List<int>();
            foreach (var item in issue)
            {
                foreach (var subitem in item)
                {
                    if (item.Count() == 2)
                    {
                        ids.Add(subitem.Id);
                    }
                }
            }
            #region Fill Magazine Advertising details based on product
            switch (ProductId)
            {
               
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                    magazineAdvertising = magazineAdvertising.OrderBy(item => item.Sequence).ThenBy(item => item.Issue.Id).ToList();
                 
                    MagAdItem = new List<MagazineAdvertisingItem>();
                    for (int i = 0; i < magazineAdvertising.Count; i++)
                    {
                        MagazineAdvertisingItem magAdItem = new MagazineAdvertisingItem();
                        magAdItem.Position = magazineAdvertising[i].Position;
                        magAdItem.Size = magazineAdvertising[i].Size;
                        magAdItem.Issue = magazineAdvertising[i].Issue;
                        magAdItem.id = magazineAdvertising[i].Id;
                        magAdItem.Sequence = magazineAdvertising[i].Sequence;
                        MagazinesAdvertisingHelper magazinesAdvertisingHelper = new MagazinesAdvertisingHelper();
                        if (magazineAdvertising[i].ArtWork)
                            magAdItem.ArtWork = magazinesAdvertisingHelper.MAGAZINESADVERTISING_ARTWORK[0];
                        else
                            magAdItem.ArtWork = magazinesAdvertisingHelper.MAGAZINESADVERTISING_ARTWORK[1];
                        MagAdItem.Add(magAdItem);
                       
                    }
                    break;
            }
            #endregion

            OrderId = order.Id;
            Price = order.Total;
            OrderStatus = order.ProcessStatus;
            IsCompleted = order.IsCompleted;
            MembershipModelHelper.PopulateModel(this, orderdetail);
        }
    }

    public class MagazineAdvertisingItem
    {
        [Display(ResourceType = typeof(Resource), Name = "Issue")]
        public LookMagazineIssue Issue { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Size")]
        public LookAdSize Size { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "Position")]
        public LookAdPosition Position { get; set; }

        public string ArtWork { get; set; }

        public int id { get; set; }

        public int Sequence { get; set; }
    }
}