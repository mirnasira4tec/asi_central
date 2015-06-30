using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers
{
    public class CompanyValidationsController : Controller
    {
        public IObjectService ObjectService { get; set; }

        [HttpGet]
        public ActionResult List()
        {
            IList<CompanyValidation> exceptions = ObjectService.GetAll<CompanyValidation>().ToList();
            return View("../Store/CompanyValidations/List", exceptions);
        }

        [HttpPost]
        public ActionResult AddOrEdit(int Id, string Value, string Type)
        {
            CompanyValidation cv = null;
            if (Id != 0) cv = ObjectService.GetAll<CompanyValidation>().Where(d => d.Id == Id).SingleOrDefault();
            if (cv == null)
            {
                cv = new CompanyValidation();
                cv.CreateDate = DateTime.UtcNow;
                ObjectService.Add<CompanyValidation>(cv);
            }
            else
            {
                ObjectService.Update<CompanyValidation>(cv);
            }
            cv.Type = Type;
            cv.Value = Value;
            cv.UpdateDate = DateTime.UtcNow;
            cv.UpdateSource = "CompanyValidationsController - AddOrEdit";
            ObjectService.SaveChanges();
            return RedirectToAction("List", "CompanyValidations");
        }

        public ActionResult Delete(int Id)
        {
            CompanyValidation cv = ObjectService.GetAll<CompanyValidation>().Where(d => d.Id == Id).SingleOrDefault();
            if (cv != null)
            {
                ObjectService.Delete<CompanyValidation>(cv);
                ObjectService.SaveChanges();
            }
            return RedirectToAction("List", "CompanyValidations");
        }
    }
}
