using asi.asicentral.model.store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.model.store
{
    public class CompanyValidationsModel
    {
        public IList<CompanyValidation> CompanyValidations { get; set; }
        public int Index { get; set; }
    }
}