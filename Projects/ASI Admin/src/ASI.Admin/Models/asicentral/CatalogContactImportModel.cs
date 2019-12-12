using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using asi.asicentral.model.asicentral;
namespace asi.asicentral.web.Models.asicentral
{
    public class CatalogContactImportModel : PagerBase
    {
        public CatalogContactImportModel()
        {
            this.ResultsPerPage = 20;
            //  Import = new CatalogContactImport();
            Imports = new List<CatalogContactImport>();
            States = new List<SelectListItem>();
            Counties = new List<SelectListItem>();
        }
        // Paging-related properties
        public int ImportId { get; set; }

        [Required]
        public string Industry { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Counties { get; set; }
        public List<SelectListItem> Industries { get; set; }
        public List<CatalogContactImport> Imports { get; set; }
        public Dictionary<int, KeyValuePair<bool, int>> contactSalesInfo { get; set; }
        //public CatalogContactImport Import { get; set; }
    }
}