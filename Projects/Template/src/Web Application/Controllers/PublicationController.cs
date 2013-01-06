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

        public PublicationController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Publications";
            ViewBag.Message = "Publications stored in the database";
            ViewBag.Publications = _objectService.GetAll<Publication>(true).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Publication publication = _objectService.GetAll<Publication>().Where(pub => pub.PublicationId == id).FirstOrDefault();
            if (publication != null)
            {
                ViewBag.Title = "Publication - " + publication.Name;
                ViewBag.Message = "Viewing the detailed information of a specific publication";
                return View(publication);
            }
            else
                throw new Exception("Invalid identifier for a publication: " + id);
        }

        [HttpPost]
        public ActionResult Edit(Publication publication)
        {
            if (ModelState.IsValid)
            {
                //because the model is not simple (has relationships), it cannot be added directly using update
                Publication original = _objectService.GetAll<Publication>().Where(pub => pub.PublicationId == publication.PublicationId).FirstOrDefault();
                if (original != null)
                {
                    original.Name = publication.Name;
                    _objectService.SaveChanges();
                }
                else
                {
                    throw new Exception("Invalid Identifier for the model: " + publication.PublicationId);
                }
            }
            return View(publication);
        }

    }
}
