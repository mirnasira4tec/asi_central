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
            List<ColorMapping> colorMappingList = null;
            if (ModelState.IsValid)
            {
                var resultColors = new ResultantColorMapping();
                var status = new List<KeyValuePair<string, string>>();
                try
                {
                    var colors = from colorData in GetEnumerator(MapDetails.ColorData)
                                 let indexValue = colorData.LastIndexOf(',')
                                 select new ColorMapping
                                 {
                                     SupplierColor = colorData.Substring(0, indexValue).Trim(),
                                     ColorGroup = colorData.Substring(indexValue + 1, (colorData.Length - (indexValue + 1))).Trim(),
                                     CompayId = MapDetails.CompanyId,
                                 };
                    if (colors.Any())
                    {
                        colorMappingList = new List<ColorMapping>();
                    }
                    
                    foreach (var color in colors)
                    {
                        try
                        {
                            var isColorMapped = _velocityService.MapColor(color);
                            color.Status = isColorMapped ? "success" : "already exists.";
                        }
                        catch (Exception ex)
                        {
                            color.Status = ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.Message : ex.Message;
                        }
                        colorMappingList.Add(color);
                    }
                }
                catch (Exception ex)
                {
                    resultColors.Status = ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) ? ex.InnerException.Message : ex.Message;
                }
                resultColors.ColorMappings = colorMappingList;
                TempData["ColorMapStatus"] = resultColors;
                return RedirectToAction("Result");
            }
            else { return View(MapDetails); }
        }
        public ActionResult Result()
        {
            ResultantColorMapping resultColors = null;
            if (TempData["ColorMapStatus"] != null)
            {
                resultColors = (ResultantColorMapping)TempData.Peek("ColorMapStatus");
            }
             return View(resultColors);
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
