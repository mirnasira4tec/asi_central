using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.util.show;
using asi.asicentral.web.Models.Show;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.Show
{
    public class ShowController : Controller
    {
        public IObjectService ObjectService { get; set; }
        [HttpGet]
        public ActionResult Show()
        {
            ShowModel show = new ShowModel();
            show.ShowType = GetShowType();
            show.StartDate = DateTime.UtcNow;
            show.EndDate = DateTime.UtcNow;
            return View("../Show/Show", show);
        }

        [HttpPost]
        public ActionResult Show(ShowModel show)
        {
            if (ModelState.IsValid)
            {
                ShowASI objShow = new ShowASI();
                objShow.Name = show.Name;
                objShow.Address = show.Address;
                objShow.ShowTypeId = show.ShowTypeId;
                objShow.StartDate = show.StartDate;
                objShow.EndDate = show.EndDate;
                objShow.UpdateSource = "ShowController - Show";
                objShow = ShowHelper.CreateOrUpdateShow(ObjectService, objShow);
                ObjectService.SaveChanges();
                return RedirectToAction("ShowList");
            }
            else
            {
                if (show != null)
                {
                    show.ShowType = GetShowType();
                }
                return View("../Show/Show", show);
            }
        }

        public ActionResult ShowList()
        {
            IList<ShowASI> showList = ObjectService.GetAll<ShowASI>(true).ToList();
            return View("../Show/ShowList", showList);
        }
        private IList<SelectListItem> GetShowType()
        {
            IList<SelectListItem> typeList = null;
            IList<ShowType> types = ObjectService.GetAll<ShowType>(true).ToList();
            if (types != null && types.Count > 0)
            {
                typeList = new List<SelectListItem>();
                string text = string.Empty;
                foreach (ShowType type in types)
                {
                    text = type.Type;
                    typeList.Add(new SelectListItem() { Text = text, Value = type.Id.ToString(), Selected = false });
                }
            }
            return typeList;
        }

    }
}
