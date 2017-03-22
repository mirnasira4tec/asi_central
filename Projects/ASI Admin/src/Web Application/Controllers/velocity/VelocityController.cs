using asi.asicentral.web.Interface;
using asi.asicentral.web.model.velocity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.velocity
{
    public class VelocityController : Controller
    {
        private readonly IVelocityService _velocityService;

        public VelocityController(IVelocityService velocityService)
        {
            _velocityService = velocityService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ColorMapping MapDetails = new ColorMapping();
            
            if (TempData["MapDetails"] != null)
                MapDetails = (ColorMapping)TempData.Peek("MapDetails");

            return View(MapDetails);
        }

        [HttpPost]
        public ActionResult Index(ColorMapping MapDetails)
        {
            var isColorMapped = _velocityService.MapColor(MapDetails);
            MapDetails.Status = isColorMapped ? "Color mapped sucessfully." : "Color already exists in database.";
            TempData["MapDetails"] = MapDetails;
            return RedirectToAction("Index");
        }

    }
}
