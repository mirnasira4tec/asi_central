using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.model.asicentral;
using asi.asicentral.web.models.show;

namespace asi.asicentral.web.Models.asicentral
{
    public class CatalogContactModel : PagerBase
    {
        public CatalogContactModel()
        {
            this.PageSize = 5;
            Contacts = new List<CatalogContact>();
            States = new List<SelectListItem>();
            Counties = new List<SelectListItem>();
        }
        // Paging-related properties
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public int ContatactId { get; set; }
        public int PageCount
        {
            get
            {
                if (this.TotalRecordCount % this.PageSize != 0)
                {
                    return Math.Max((this.TotalRecordCount / this.PageSize) + 1, 1);
                }
                else
                {
                    return Math.Max(this.TotalRecordCount / this.PageSize, 1);
                }
            }
        }
        public int NumericPageCount { get; set; }
        public List<CatalogContact> Contacts { get; set; }
        public int ImportId { get; set; }
        public string CatalogName { get; set; }
        public string Industry { get; set; }
        public IList<SelectListItem> States { get; set; }
        public List<SelectListItem> Counties { get; set; }

    }

    public class CatalogCountyInfo
    {
        public int CatalogContactId { get; set; }
        public int RemaingContacts { get; set; }
        public string County { get; set; }
    }
}