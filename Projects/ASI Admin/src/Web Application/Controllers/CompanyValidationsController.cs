using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.model.store;
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
        public IStoreService StoreService { get; set; }

        [HttpGet]
        public ActionResult List(int Index = 0)
        {
            IList<CompanyValidation> validations = StoreService.GetAll<CompanyValidation>().ToList();
            CompanyValidationsModel model = new CompanyValidationsModel();
            model.CompanyValidations = validations;
            model.Index = Index;
            return View("../Store/CompanyValidations/List", model);
        }

        [HttpPost]
        public ActionResult AddOrEdit(int Id, string Value, string Type)
        {
            CompanyValidation cv = null;
            if (Id != 0) cv = StoreService.GetAll<CompanyValidation>().Where(d => d.Id == Id).SingleOrDefault();
            if (cv == null)
            {
                cv = new CompanyValidation();
                cv.CreateDate = DateTime.UtcNow;
                StoreService.Add<CompanyValidation>(cv);
            }
            else
            {
                StoreService.Update<CompanyValidation>(cv);
            }
            cv.Type = Type;
            cv.Value = Value;
            cv.UpdateDate = DateTime.UtcNow;
            cv.UpdateSource = "CompanyValidationsController - AddOrEdit";
            StoreService.SaveChanges();
            var index = GetIndexValue(cv.Type);
            return RedirectToAction("List", "CompanyValidations", new { Index = index });
        }

        public ActionResult Delete(int Id)
        {
            CompanyValidation cv = StoreService.GetAll<CompanyValidation>().Where(d => d.Id == Id).SingleOrDefault();
            var index = 0;
            if (cv != null)
            {
                index = GetIndexValue(cv.Type);
                StoreService.Delete<CompanyValidation>(cv);
                StoreService.SaveChanges();
            }
            return RedirectToAction("List", "CompanyValidations", new { Index = index });
        }

        private int GetIndexValue(string Type)
        {
            switch (Type)
            {
                case CompanyValidation.REGISTERED_TRADEMARKS:
                    return 1;
                case CompanyValidation.EMAIL_DOMAINS:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
