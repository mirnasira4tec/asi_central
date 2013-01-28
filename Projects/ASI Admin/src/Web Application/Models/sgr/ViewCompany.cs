using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using asi.asicentral.model.sgr;

namespace asi.asicentral.web.Models.sgr
{
    public class ViewCompany : Company
    {
        public int CategoryID { set; get; }

        public static ViewCompany CreateFromCompany(Company company)
        {
            ViewCompany view = new ViewCompany();
            company.CopyTo(view);
            return view;
        }

        public Company GetCompany()
        {
            Company company = new Company();
            this.CopyTo(company);
            return company;
        }
    }
}