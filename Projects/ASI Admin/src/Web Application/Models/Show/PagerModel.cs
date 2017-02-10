using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.models.show
{
    public class PagerModel
    {
        public PagerModel()
        {
            // Define any default values here...
            this.PageSize = 20;
            this.NumericPageCount = 10;
        }

        // Paging-related properties
        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecordCount { get; set; }
        public int PageCount
        {
            get
            {
                return Math.Max(this.TotalRecordCount / this.PageSize, 1);
            }
        }
        public int NumericPageCount { get; set; }
        public string TabCompanyName { get; set; }
        public string TabMemberType { get; set; }
        public int? TabYear { get; set; }
        public int? TabShowTypeId { get; set; }
        public int ShowId { get; set; }
    }
}