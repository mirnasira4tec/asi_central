using asi.asicentral.model.asicentral;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Models.asicentral
{
    public class ResearchDataUploadModel : PagerBase
    {
        public ResearchDataUploadModel()
        {
            this.ResultsPerPage = 20;
        }
        // Paging-related properties
        public int CurrentImportId { get; set; }

        [Required]
        public string ResearchName { get; set; }

        public List<SelectListItem> ResearchNames { get; set; }
        public List<ResearchImport> UploadedImports { get; set; }
    }
}