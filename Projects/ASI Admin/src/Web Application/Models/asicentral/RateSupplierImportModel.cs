using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using asi.asicentral.web.models.show;
using asi.asicentral.model.asicentral;

namespace asi.asicentral.web.Models.asicentral
{
    public class RateSupplierImportModel : PagerModel
    {
        public RateSupplierImportModel()
        {
            this.PageSize = 20;
            Import = new RateSupplierImport();
        }
        // Paging-related properties
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public int ImportId { get; set; }
        public string DistCompanyName { get; set; }
        public string ASINumber { get; set; }
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
        public RateSupplierImport Import { get; set; }
    }
}