using asi.asicentral.model.store;
using asi.asicentral.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class CompanyValidationsModel
    {
        public IList<CompanyValidation> CompanyValidations { get; set; }
        public int Index { get; set; }
        
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NameOrKeyword { get; set; }
    }
}