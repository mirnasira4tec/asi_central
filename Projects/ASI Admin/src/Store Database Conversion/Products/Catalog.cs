using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Database_Conversion.Products
{
    class Catalog : BaseProductConvert
    {
        public override void Convert(asi.asicentral.model.store.StoreOrderDetail newOrderDetail, asi.asicentral.model.store.LegacyOrderDetail detail, asi.asicentral.database.StoreContext storeContext, asi.asicentral.database.ASIInternetContext asiInternetContext)
        {
            ILogService logService = LogService.GetLog(this.GetType());
            //get catalog information
            IList<LegacyOrderCatalogOption> options = asiInternetContext.OrderCatalogOptions.Where(option => option.COPS_OrderID == detail.OrderId && option.COPS_ProdID == detail.ProductId).ToList();
            LegacyOrderCatalog catalog = asiInternetContext.OrderCatalogs.Where(cat => cat.OrderID == detail.OrderId && cat.ProdID == detail.ProductId).FirstOrDefault();
            if (catalog != null)
            {
                StoreDetailCatalog newCatalog = new StoreDetailCatalog()
                {
                    BackLine1 = catalog.BackLine1,
                    BackLine2 = catalog.BackLine2,
                    BackLine3 = catalog.BackLine3,
                    BackLine4 = catalog.BackLine4,
                    IsArtworkToProof = catalog.ArtworkProof.HasValue ? catalog.ArtworkProof.Value : false,
                    Line1 = catalog.NewLine1,
                    Line2 = catalog.NewLine2,
                    Line3 = catalog.NewLine3,
                    Line4 = catalog.NewLine4,
                    Line5 = catalog.NewLine5,
                    Line6 = catalog.NewLine6,
                    LogoPath = catalog.Logo,
                    OrderDetailId = newOrderDetail.Id,  
                    CreateDate = newOrderDetail.CreateDate,
                    UpdateDate = newOrderDetail.CreateDate,
                    UpdateSource = "Migration Process - " + DateTime.Now,
                };
                //decoding area
                LegacyOrderCatalogOption temp = options.Where(opt => new int[] { 8, 9, 10, 25 }.Contains(opt.COPS_OptionID)).FirstOrDefault();
                if (temp != null) newCatalog.AreaId = temp.COPS_OptionID;
                //decoding color
                temp = options.Where(opt => new int[] { 11, 12, 13, 14, 15, 16, 17, 26 }.Contains(opt.COPS_OptionID)).FirstOrDefault();
                if (temp != null) newCatalog.ColorId = temp.COPS_OptionID;
                //decoding cover
                temp = options.Where(opt => new int[] { 1, 2, 3, 4, 5, 6, 7 }.Contains(opt.COPS_OptionID)).FirstOrDefault();
                if (temp != null) newCatalog.CoverId = temp.COPS_OptionID;
                //decoding imprint
                temp = options.Where(opt => new int[] { 18, 19, 20, 21 }.Contains(opt.COPS_OptionID)).FirstOrDefault();
                if (temp != null) newCatalog.ImprintId = temp.COPS_OptionID;
                //decoding supplement
                temp = options.Where(opt => new int[] { 22, 23, 24 }.Contains(opt.COPS_OptionID)).FirstOrDefault();
                if (temp != null) newCatalog.SupplementId = temp.COPS_OptionID;

                storeContext.StoreDetailCatalogs.Add(newCatalog);
            }
            else
            {
                logService.Debug("Could not find the catalog information for order detail:" + detail.OrderId + ", " + detail.ProductId);
            }
        }
    }
}
