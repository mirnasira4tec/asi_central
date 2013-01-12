using asi.asicentral.model;
using asi.asicentral.services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers
{
    public class PublicationController : Controller
    {
        IObjectService _objectService;

        /// <summary>
        /// Controller for any Publication related functionality
        /// </summary>
        /// <param name="objectService">Required as we will be retrieving records from the database</param>
        public PublicationController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult Index()
        {
            return List();
        }

        public ActionResult List()
        {
            ViewBag.Title = "Publications";
            ViewBag.Message = "Publications stored in the database";
            return View("List", _objectService.GetAll<Publication>(true).ToList());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Publication publication = _objectService.GetAll<Publication>(true).Where(pub => pub.PublicationId == id).FirstOrDefault();
            if (publication != null)
            {
                ViewBag.Title = String.Format(Resource.PublicationEditTitle, publication.Name);
                ViewBag.Message = Resource.PublicationEditDescription;
                return View("Edit", publication);
            }
            else
                throw new Exception("Invalid identifier for a publication: " + id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Publication publication = _objectService.GetAll<Publication>().Where(pub => pub.PublicationId == id).FirstOrDefault();
            if (publication != null)
            {
                _objectService.Delete(publication);
                _objectService.SaveChanges();
                return RedirectToAction("List");
            }
            else
                throw new Exception("Invalid identifier for a publication: " + id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Publication publication)
        {
            if (ModelState.IsValid)
            {
                _objectService.Update<Publication>(publication);
                _objectService.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                //because we do not store the many-many in the input fields, the publication object is incomplete
                Publication original = _objectService.GetAll<Publication>(true).Where(pub => pub.PublicationId == publication.PublicationId).FirstOrDefault();
                if (original != null) publication.Issues = original.Issues;
            }
            ViewBag.Title = "Publication - " + publication.Name;
            ViewBag.Message = "Viewing the detailed information of a specific publication";
            return View("Edit", publication);
        }

    }
}
