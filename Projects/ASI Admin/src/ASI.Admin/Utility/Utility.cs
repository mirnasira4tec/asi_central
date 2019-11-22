using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.model.asicentral;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using asi.asicentral.database.mappings;
using asi.asicentral.services;
using System.Web.Mvc;

namespace asi.asicentral.web.Utility
{
    public class Utility
    {
        public static List<CatalogContactSaleDetail> IsImportContactReserved(int importId)
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);

      
          List<CatalogContactSaleDetail> reservedContact= new List<CatalogContactSaleDetail>();
            using (var objectContext = new ObjectService(container))
            {
                reservedContact = objectContext.GetAll<CatalogContactSaleDetail>("CatalogContacts.CatalogContactSaleDetails.CatalogContactSale")
                                        .Where(m => m.CatalogContacts.CatalogContactImport.CatalogContactImportId == importId).ToList();
            }
            return reservedContact;
        }

        public static string GetSateFullNameBySortName(string sortName, IList<SelectListItem>stateList)
        {
            string stateFullName = string.Empty;
            if (stateList != null && stateList.Count>0)
            {
                stateFullName = stateList.Where(m => m.Value == sortName).Select(m => m.Text).FirstOrDefault();
            }
            return stateFullName;
        }
        
        public static string GetArtWorkFileName(string mediaLink)
        {
                var fileName = string.Empty;
                if (!string.IsNullOrWhiteSpace(mediaLink))
                {
                    var dashIndex = mediaLink.LastIndexOf("-");
                    var dotIndex = mediaLink.LastIndexOf(".");
                    var slashIndex = mediaLink.LastIndexOf("\\");
                    fileName = mediaLink.Substring(slashIndex + 1, dashIndex - slashIndex - 1) + mediaLink.Substring(dotIndex);
                }
                return fileName;
            }
        }
}