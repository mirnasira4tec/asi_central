using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.model.asicentral
{
    public class PagerBase
    {
        public PagerBase()
        {
            // Define any default values here...
            this.ResultsPerPage = 20;
            this.NumericPageCount = 10;
        }

        // Paging-related properties
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public int ResultsTotal { get; set; }
        public int PageCount
        {
            get
            {
                if (this.ResultsTotal % this.ResultsPerPage != 0)
                {
                    return Math.Max((this.ResultsTotal / this.ResultsPerPage) + 1, 1);
                }
                else
                {
                    return Math.Max(this.ResultsTotal / this.ResultsPerPage, 1);
                }
            }
        }
        public int NumericPageCount { get; set; }
        public Dictionary<string, string> q { get; set; }
    }
}
