using asi.asicentral.model.store;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.interfaces;

namespace asi.asicentral.web.model.store
{
    public class SupplierApplicationModel : SupplierMembershipApplication
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
            this.DecoratingTypes = new List<SupplierDecoratingType>();
        }

        public SupplierApplicationModel(SupplierMembershipApplication application, asi.asicentral.model.store.Order order)
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
            Etching = HasDecorating(SupplierDecoratingType.DECORATION_ETCHING);
            HotStamping = HasDecorating(SupplierDecoratingType.DECORATION_HOTSTAMPING);
            SilkScreen = HasDecorating(SupplierDecoratingType.DECORATION_SILKSCREEN);
            PadPrint = HasDecorating(SupplierDecoratingType.DECORATION_PADPRINT);
            DirectEmbroidery = HasDecorating(SupplierDecoratingType.DECORATION_DIRECTEMBROIDERY);
            FoilStamping = HasDecorating(SupplierDecoratingType.DECORATION_FOILSTAMPING);
            Lithography = HasDecorating(SupplierDecoratingType.DECORATION_LITHOGRAPHY);
            Sublimination = HasDecorating(SupplierDecoratingType.DECORATION_SUBLIMINATION);
            FourColourProcess = HasDecorating(SupplierDecoratingType.DECORATION_FOURCOLOR);
            Engraving = HasDecorating(SupplierDecoratingType.DECORATION_ENGRAVING);
            Laser = HasDecorating(SupplierDecoratingType.DECORATION_ENGRAVING);
            Offset = HasDecorating(SupplierDecoratingType.DECORATION_OFFSET);
            Transfer = HasDecorating(SupplierDecoratingType.DECORATION_TRANSFER);
            FullColourProcess = HasDecorating(SupplierDecoratingType.DECORATION_FULLCOLOR);
            DieStamp = HasDecorating(SupplierDecoratingType.DECORATION_DIESTAMP);
        }

        private bool HasDecorating(string DecorationName)
        {
            return DecoratingTypes.Where(type => type.Name == DecorationName).Count() == 1;
        }

        /// <summary>
        /// Apply the extra bool values from the view model to the many to many
        /// </summary>
        /// <param name="StoreService"></param>
        public void SyncDecoratingTypes(IList<SupplierDecoratingType> decoratingTypes)
        {
            AddDecoratingType(this.Etching, SupplierDecoratingType.DECORATION_ETCHING, decoratingTypes);
            AddDecoratingType(this.HotStamping, SupplierDecoratingType.DECORATION_HOTSTAMPING, decoratingTypes);
            AddDecoratingType(this.SilkScreen, SupplierDecoratingType.DECORATION_SILKSCREEN, decoratingTypes);
            AddDecoratingType(this.PadPrint, SupplierDecoratingType.DECORATION_PADPRINT, decoratingTypes);
            AddDecoratingType(this.DirectEmbroidery, SupplierDecoratingType.DECORATION_DIRECTEMBROIDERY, decoratingTypes);
            AddDecoratingType(this.FoilStamping, SupplierDecoratingType.DECORATION_FOILSTAMPING, decoratingTypes);
            AddDecoratingType(this.Lithography, SupplierDecoratingType.DECORATION_LITHOGRAPHY, decoratingTypes);
            AddDecoratingType(this.Sublimination, SupplierDecoratingType.DECORATION_SUBLIMINATION, decoratingTypes);
            AddDecoratingType(this.FourColourProcess, SupplierDecoratingType.DECORATION_FOURCOLOR, decoratingTypes);
            AddDecoratingType(this.Engraving, SupplierDecoratingType.DECORATION_ENGRAVING, decoratingTypes);
            AddDecoratingType(this.Laser, SupplierDecoratingType.DECORATION_LASER, decoratingTypes);
            AddDecoratingType(this.Offset, SupplierDecoratingType.DECORATION_OFFSET, decoratingTypes);
            AddDecoratingType(this.Transfer, SupplierDecoratingType.DECORATION_TRANSFER, decoratingTypes);
            AddDecoratingType(this.FullColourProcess, SupplierDecoratingType.DECORATION_FULLCOLOR, decoratingTypes);
            AddDecoratingType(this.DieStamp, SupplierDecoratingType.DECORATION_DIESTAMP, decoratingTypes);
        }

        private void AddDecoratingType(bool selected, String typeName, IList<SupplierDecoratingType> decoratingTypes)
        {
            if (selected)
            {
                SupplierDecoratingType type = decoratingTypes.Where(decType => decType.Name == typeName).SingleOrDefault();
                if (type != null) DecoratingTypes.Add(type);
            }
        }
    }
}