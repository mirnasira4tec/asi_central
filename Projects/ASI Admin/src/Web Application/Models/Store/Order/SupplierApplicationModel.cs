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
        /// <summary>
        /// Required for MVC to rebuild the model
        /// </summary>
        /// 

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

        public SupplierApplicationModel()
        {
            //nothing to do
        }

        public SupplierApplicationModel(SupplierMembershipApplication application, asi.asicentral.model.store.Order order)
        {
            application.CopyTo(this);
            
            GetDecoratingTypesFrom();
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

        private void GetDecoratingTypesFrom()
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
        public void UpdateDecoratingTypes(IList<SupplierDecoratingType> decoratingTypes)
        {
            SyncDecoratingType(this.Etching, SupplierDecoratingType.DECORATION_ETCHING, decoratingTypes);
            SyncDecoratingType(this.HotStamping, SupplierDecoratingType.DECORATION_HOTSTAMPING, decoratingTypes);
            SyncDecoratingType(this.SilkScreen, SupplierDecoratingType.DECORATION_SILKSCREEN, decoratingTypes);
            SyncDecoratingType(this.PadPrint, SupplierDecoratingType.DECORATION_PADPRINT, decoratingTypes);
            SyncDecoratingType(this.DirectEmbroidery, SupplierDecoratingType.DECORATION_DIRECTEMBROIDERY, decoratingTypes);
            SyncDecoratingType(this.FoilStamping, SupplierDecoratingType.DECORATION_FOILSTAMPING, decoratingTypes);
            SyncDecoratingType(this.Lithography, SupplierDecoratingType.DECORATION_LITHOGRAPHY, decoratingTypes);
            SyncDecoratingType(this.Sublimination, SupplierDecoratingType.DECORATION_SUBLIMINATION, decoratingTypes);
            SyncDecoratingType(this.FourColourProcess, SupplierDecoratingType.DECORATION_FOURCOLOR, decoratingTypes);
            SyncDecoratingType(this.Engraving, SupplierDecoratingType.DECORATION_ENGRAVING, decoratingTypes);
            SyncDecoratingType(this.Laser, SupplierDecoratingType.DECORATION_LASER, decoratingTypes);
            SyncDecoratingType(this.Offset, SupplierDecoratingType.DECORATION_OFFSET, decoratingTypes);
            SyncDecoratingType(this.Transfer, SupplierDecoratingType.DECORATION_TRANSFER, decoratingTypes);
            SyncDecoratingType(this.FullColourProcess, SupplierDecoratingType.DECORATION_FULLCOLOR, decoratingTypes);
            SyncDecoratingType(this.DieStamp, SupplierDecoratingType.DECORATION_DIESTAMP, decoratingTypes);
        }

        private void SyncDecoratingType(bool selected, String typeName, IList<SupplierDecoratingType> decoratingTypes)
        {
            SupplierDecoratingType existing = DecoratingTypes.Where(type => type.Name == typeName).SingleOrDefault();
            if (selected && existing == null) DecoratingTypes.Add(decoratingTypes.Where(type => type.Name == typeName).SingleOrDefault());
            else if (!selected && existing != null) DecoratingTypes.Remove(existing);
        }

        public int OrderId { get; set; }
        public string ActionName { get; set; }
        public string ExternalReference { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}