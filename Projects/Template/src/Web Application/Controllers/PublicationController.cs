using asi.asicentral.model;
using asi.asicentral.interfaces;
using asi.asicentral.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap.Attributes;

namespace asi.asicentral.web.Controllers
{
    public class PublicationController : Controller
    {
        IObjectService _objectService;

        /// <summary>
        /// Controller for any Publication related functionality
        /// </summary>
        /// <param name="objectService">Required as we will be retrieving records from the database</param>
        public PublicationController()
        {
            //_objectService = objectService;
        }

        public IObjectService ObjectService
        {
            get { return _objectService; }
            set { _objectService = value; }
        }

        public virtual ActionResult Index()
        {
            return List();
        }

        public virtual ActionResult List()
        {
            ViewBag.Title = "Publications";
            ViewBag.Message = "Publications stored in the database";
            return View("List", _objectService.GetAll<Publication>(true).OrderBy(pub => pub.Name).ToList());
        }

        [HttpGet]
        public virtual ActionResult Edit(int id)
        {
            Publication publication = _objectService.GetAll<Publication>(true).Where(pub => pub.PublicationId == id).FirstOrDefault();
            if (publication != null)
            {
                ViewBag.Title = String.Format(Resource.PublicationEditTitle, publication.Name);
                ViewBag.Message = Resource.PublicationEditDescription;
                PublicationView viewModel = PublicationView.CreateFromPublication(publication);

                IList<SelectListItem> colors = new List<SelectListItem>();
                colors.Add(new SelectListItem() { Text = "Blue", Value = "1", Selected = false });
                colors.Add(new SelectListItem() { Text = "Green", Value = "2", Selected = false });
                ViewBag.ColorList = new SelectList(colors, "Value", "Text");

                return View("Edit", viewModel);
            }
            else
                throw new Exception("Invalid identifier for a publication: " + id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(PublicationView publicationView)
        {
            var request = Request;
            if (ModelState.IsValid)
            {
                _objectService.Update<Publication>(publicationView.GetPublication());
                _objectService.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                //because we do not store the many-many in the input fields, the publication object is incomplete
                Publication original = _objectService.GetAll<Publication>(true).Where(pub => pub.PublicationId == publicationView.PublicationId).FirstOrDefault();
                if (original != null) publicationView.Issues = original.Issues;
            }
            ViewBag.Title = "Publication - " + publicationView.Name;
            ViewBag.Message = "Viewing the detailed information of a specific publication";
            return View("Edit", publicationView);
        }

        [HttpGet]
        /// <summary>
        /// Use this one to define default values. Could re-use edit but an Add could have specific reduced amount of fields from edit.
        /// </summary>
        public virtual ActionResult Add()
        {
            Publication publication = new Publication();
            ViewBag.Title = Resource.PublicationAddTitle;
            ViewBag.Message = Resource.PublicationAddDescription;
            return View("Add", publication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Could use Edit for this one but there might be validation specific to Add operation which does not apply to Edit
        /// </summary>
        public virtual ActionResult Add(Publication publication)
        {
            if (ModelState.IsValid)
            {
                _objectService.Add<Publication>(publication);
                _objectService.SaveChanges();
                //could return to edit 
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.Title = Resource.PublicationAddTitle;
                ViewBag.Message = Resource.PublicationAddDescription;
                return View("Add", publication);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
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
    }
}
