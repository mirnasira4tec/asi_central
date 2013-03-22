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
        public bool Etching { set; get; }
        public bool HotStamping { set; get; }
        public bool SilkScreen { set; get; }
        public bool PadPrint { set; get; }
        public bool DirectEmbroidery { set; get; }
        public bool FoilStamping { set; get; }
        public bool Lithography { set; get; }
        public bool Sublimination { set; get; }
        public bool FourColourProcess { set; get; }
        public bool Engraving { set; get; }
        public bool Laser { set; get; }
        public bool Offset { set; get; }
        public bool Transfer { set; get; }
        public bool FullColourProcess { set; get; }
        public bool DieStamp { set; get; }
        public bool OtherDecoratingMethod { set; get; }
        public string OtherDecoratingMethodName { set; get; }

        public SupplierApplicationModel()
        {
            //nothing to do
        }

        public SupplierApplicationModel(SupplierMembershipApplication application, asi.asicentral.model.store.Order order)
        {
            application.CopyTo(this);
            GetDecoratingTypesFrom(this.DecoratingTypes);

            if (!String.IsNullOrEmpty(OtherDec))
            {
                OtherDecoratingMethod = true;
                OtherDecoratingMethodName = this.OtherDec;
            }
            else OtherDecoratingMethod = false;

            ActionName = "Approve";
            ExternalReference = order.ExternalReference;
            OrderId = order.Id;
            OrderStatus = order.ProcessStatus;
        }

        public void SaveDecoratingTypesTo(SupplierMembershipApplication application)
        {
            // TODO save decorating types to the applicatiojn
        }

        public void GetDecoratingTypesFrom(ICollection<SupplierDecoratingType> decorationTypes)
        {
            Etching = HasDecoration(SupplierDecoratingType.DECORATION_ETCHING, decorationTypes);
            HotStamping = HasDecoration(SupplierDecoratingType.DECORATION_HOTSTAMPING, decorationTypes);
            SilkScreen = HasDecoration(SupplierDecoratingType.DECORATION_SILKSCREEN, decorationTypes);
            PadPrint = HasDecoration(SupplierDecoratingType.DECORATION_PADPRINT, decorationTypes);
            DirectEmbroidery = HasDecoration(SupplierDecoratingType.DECORATION_DIRECTEMBROIDERY, decorationTypes);
            FoilStamping = HasDecoration(SupplierDecoratingType.DECORATION_FOILSTAMPING, decorationTypes);
            Lithography = HasDecoration(SupplierDecoratingType.DECORATION_LITHOGRAPHY, decorationTypes);
            Sublimination = HasDecoration(SupplierDecoratingType.DECORATION_SUBLIMINATION, decorationTypes);
            FourColourProcess = HasDecoration(SupplierDecoratingType.DECORATION_FOURCOLOR, decorationTypes);
            Engraving = HasDecoration(SupplierDecoratingType.DECORATION_ENGRAVING, decorationTypes);
            Laser = HasDecoration(SupplierDecoratingType.DECORATION_ENGRAVING, decorationTypes);
            Offset = HasDecoration(SupplierDecoratingType.DECORATION_OFFSET, decorationTypes);
            Transfer = HasDecoration(SupplierDecoratingType.DECORATION_TRANSFER, decorationTypes);
            FullColourProcess = HasDecoration(SupplierDecoratingType.DECORATION_FULLCOLOR, decorationTypes);
            DieStamp = HasDecoration(SupplierDecoratingType.DECORATION_DIESTAMP, decorationTypes);
        }

        public bool HasDecoration(string DecorationName, ICollection<SupplierDecoratingType> decorationTypes)
        {
            return decorationTypes.Where(type => type.Name == DecorationName).SingleOrDefault() == null ? false : true;
        }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}