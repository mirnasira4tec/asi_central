using asi.asicentral.web.Interface;
using asi.asicentral.web.Models.velocity;
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
            ColorMapData MapDetails = new ColorMapData();
            return View(MapDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ColorMapData MapDetails)
        {
            if (ModelState.IsValid)
            {
                var status = new List<KeyValuePair<string, string>>();
                try
                {

                    var colors = GetEnumerator(MapDetails.ColorData)
                                 .Select(l => l.Split(','))
                                    .Select(c => new ColorMapping
                                    {
                                        BaseColor = c[0],
                                        MappingColor = c[1],
                                        CompayId = MapDetails.CompanyId,
                                    }).ToList();

                    foreach (var color in colors)
                    {
                        try
                        {
                            var isColorMapped = _velocityService.MapColor(color);
                            status.Add(new KeyValuePair<string, string>(string.Format("{0},{1}", color.BaseColor, color.MappingColor), isColorMapped ? "success" : "already exists."));
                        }
                        catch (Exception ex)
                        {
                            status.Add(new KeyValuePair<string, string>(string.Format("{0},{1}", color.BaseColor, color.MappingColor), ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.Message : ex.Message));
                        }
                    }

                }
                catch (Exception ex)
                {
                    status.Add(new KeyValuePair<string, string>("Error occurred during update", ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.Message : ex.Message));
                }
                TempData["ColorMapStatus"] = status;
                return RedirectToAction("Result");
            }
            else { return View(MapDetails); }
        }
        public ActionResult Result()
        {
            List<KeyValuePair<string, string>> colorMapStatus = null;
            if (TempData["ColorMapStatus"] != null)
            {
                colorMapStatus = (List<KeyValuePair<string, string>>)TempData.Peek("ColorMapStatus");
            }

            return View(colorMapStatus);
        }

        private static IEnumerable<string> GetEnumerator(string data)
        {
            System.IO.StringReader reader = new System.IO.StringReader(data);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

    }
}
