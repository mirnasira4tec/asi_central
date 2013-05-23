using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.model.store
{
    public class SupplierApplicationModel : LegacySupplierMembershipApplication
    {
        [Display(ResourceType = typeof(Resource), Name = "Etching")]
        public bool Etching { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "HotStamping")]
        public bool HotStamping { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "SilkScreen")]
        public bool SilkScreen { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "PadPrint")]
        public bool PadPrint { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "DirectEmbroidery")]
        public bool DirectEmbroidery { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "FoilStamping")]
        public bool FoilStamping { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Lithography")]
        public bool Lithography { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Sublimination")]
        public bool Sublimination { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "FourColourProcess")]
        public bool FourColourProcess { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Engraving")]
        public bool Engraving { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "ContactName")]
        public bool Laser { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Offset")]
        public bool Offset { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "Transfer")]
        public bool Transfer { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "FullColourProcess")]
        public bool FullColourProcess { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "DieStamp")]
        public bool DieStamp { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "OtherDecoratingMethod")]
        public bool OtherDecoratingMethod { set; get; }

        [Display(ResourceType = typeof(Resource), Name = "OtherDecoratingMethodName")]
        public string OtherDecoratingMethodName { set; get; }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public bool Completed { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 
        public SupplierApplicationModel() : base()
        {
            this.DecoratingTypes = new List<LegacySupplierDecoratingType>();
        }

        public SupplierApplicationModel(LegacySupplierMembershipApplication application, asi.asicentral.model.store.LegacyOrder order)
        {
            application.CopyTo(this);
            UpdateDecoratingTypesProperties();
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
            if (order.OrderDetails.Count == 1 && order.OrderDetails.ElementAt(0).Subtotal.HasValue)
                Price = order.OrderDetails.ElementAt(0).Subtotal.Value;
            else
                Price = 0m;
            Completed = order.Status.HasValue ? order.Status.Value : false;
        }

        private void UpdateDecoratingTypesProperties()
        {
            Etching = HasDecorating(LegacySupplierDecoratingType.DECORATION_ETCHING);
            HotStamping = HasDecorating(LegacySupplierDecoratingType.DECORATION_HOTSTAMPING);
            SilkScreen = HasDecorating(LegacySupplierDecoratingType.DECORATION_SILKSCREEN);
            PadPrint = HasDecorating(LegacySupplierDecoratingType.DECORATION_PADPRINT);
            DirectEmbroidery = HasDecorating(LegacySupplierDecoratingType.DECORATION_DIRECTEMBROIDERY);
            FoilStamping = HasDecorating(LegacySupplierDecoratingType.DECORATION_FOILSTAMPING);
            Lithography = HasDecorating(LegacySupplierDecoratingType.DECORATION_LITHOGRAPHY);
            Sublimination = HasDecorating(LegacySupplierDecoratingType.DECORATION_SUBLIMINATION);
            FourColourProcess = HasDecorating(LegacySupplierDecoratingType.DECORATION_FOURCOLOR);
            Engraving = HasDecorating(LegacySupplierDecoratingType.DECORATION_ENGRAVING);
            Laser = HasDecorating(LegacySupplierDecoratingType.DECORATION_ENGRAVING);
            Offset = HasDecorating(LegacySupplierDecoratingType.DECORATION_OFFSET);
            Transfer = HasDecorating(LegacySupplierDecoratingType.DECORATION_TRANSFER);
            FullColourProcess = HasDecorating(LegacySupplierDecoratingType.DECORATION_FULLCOLOR);
            DieStamp = HasDecorating(LegacySupplierDecoratingType.DECORATION_DIESTAMP);
        }

        private bool HasDecorating(string DecorationName)
        {
            return DecoratingTypes.Where(type => type.Name == DecorationName).Count() == 1;
        }

        /// <summary>
        /// Apply the extra bool values from the view model to the many to many
        /// </summary>
        /// <param name="StoreService"></param>
        public void SyncDecoratingTypes(IList<LegacySupplierDecoratingType> decoratingTypes)
        {
            AddDecoratingType(this.Etching, LegacySupplierDecoratingType.DECORATION_ETCHING, decoratingTypes);
            AddDecoratingType(this.HotStamping, LegacySupplierDecoratingType.DECORATION_HOTSTAMPING, decoratingTypes);
            AddDecoratingType(this.SilkScreen, LegacySupplierDecoratingType.DECORATION_SILKSCREEN, decoratingTypes);
            AddDecoratingType(this.PadPrint, LegacySupplierDecoratingType.DECORATION_PADPRINT, decoratingTypes);
            AddDecoratingType(this.DirectEmbroidery, LegacySupplierDecoratingType.DECORATION_DIRECTEMBROIDERY, decoratingTypes);
            AddDecoratingType(this.FoilStamping, LegacySupplierDecoratingType.DECORATION_FOILSTAMPING, decoratingTypes);
            AddDecoratingType(this.Lithography, LegacySupplierDecoratingType.DECORATION_LITHOGRAPHY, decoratingTypes);
            AddDecoratingType(this.Sublimination, LegacySupplierDecoratingType.DECORATION_SUBLIMINATION, decoratingTypes);
            AddDecoratingType(this.FourColourProcess, LegacySupplierDecoratingType.DECORATION_FOURCOLOR, decoratingTypes);
            AddDecoratingType(this.Engraving, LegacySupplierDecoratingType.DECORATION_ENGRAVING, decoratingTypes);
            AddDecoratingType(this.Laser, LegacySupplierDecoratingType.DECORATION_LASER, decoratingTypes);
            AddDecoratingType(this.Offset, LegacySupplierDecoratingType.DECORATION_OFFSET, decoratingTypes);
            AddDecoratingType(this.Transfer, LegacySupplierDecoratingType.DECORATION_TRANSFER, decoratingTypes);
            AddDecoratingType(this.FullColourProcess, LegacySupplierDecoratingType.DECORATION_FULLCOLOR, decoratingTypes);
            AddDecoratingType(this.DieStamp, LegacySupplierDecoratingType.DECORATION_DIESTAMP, decoratingTypes);
        }

        private void AddDecoratingType(bool selected, String typeName, IList<LegacySupplierDecoratingType> decoratingTypes)
        {
            if (selected)
            {
                LegacySupplierDecoratingType type = decoratingTypes.Where(decType => decType.Name == typeName).SingleOrDefault();
                if (type != null) DecoratingTypes.Add(type);
            }
        }
    }
}