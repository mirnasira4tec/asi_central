using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.services;
using asi.asicentral.web.Models.Show;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace asi.asicentral.web.Controllers
{
    public class RedirectionSurfaceController : Controller
    {
        public IObjectService ObjectService { get; set; }
        [HttpPost]
        public ActionResult IsValidCompany(string name)
        {
            IQueryable<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true);
            companyList = companyList.Where(item => item.Name != null
                 && item.Name.Contains(name));
            return Json(companyList);
        }

    }
}
