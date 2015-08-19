using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.util.show;
using asi.asicentral.web.models.show;
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
        public ActionResult ShowAdd()
        {
            ShowModel show = new ShowModel();
            show.ShowType = GetShowType();
            show.StartDate = DateTime.UtcNow;
            show.EndDate = DateTime.UtcNow;
            return View("../Show/ShowAdd", show);
        }

        [HttpPost]
        public ActionResult ShowAdd(ShowModel show)
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
                return View("../Show/ShowAdd", show);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostShowAttendeeInformation(ShowCompaniesModel attendeeInfo)
        {
            ShowAttendee showAttendee = null;
            int companyId = 0;
            int showId = 0;
            if (attendeeInfo != null && attendeeInfo.ShowAttendees != null && attendeeInfo.ShowAttendees.Count > 0)
            {
                showAttendee = attendeeInfo.ShowAttendees.ElementAt(0);
                companyId = (showAttendee.CompanyId != null && showAttendee.CompanyId.HasValue) ? showAttendee.CompanyId.Value : 0;
                showId = (showAttendee.ShowId != null && showAttendee.ShowId.HasValue) ? showAttendee.ShowId.Value : 0;
            }
            ShowAttendee existingAttendee = ObjectService.GetAll<ShowAttendee>().SingleOrDefault(attendee => attendee.ShowId == showId && attendee.CompanyId == companyId);
            if (showAttendee != null)
            {
                if (existingAttendee == null) existingAttendee = new ShowAttendee();
                existingAttendee.CompanyId = companyId;
                existingAttendee.ShowId = showId;
                existingAttendee.IsAttending = showAttendee.IsAttending;
                existingAttendee.IsSponsor = showAttendee.IsSponsor;
                existingAttendee.IsExhibitDay = showAttendee.IsExhibitDay;
                existingAttendee.BoothNumber = showAttendee.BoothNumber;
                existingAttendee.UpdateDate = DateTime.UtcNow;
                existingAttendee.UpdateSource = "ShowController - PostShowAttendeeInformation";
                ShowAttendee attendeeToSave = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, existingAttendee);

                foreach (EmployeeAttendance employeeAttendance in attendeeInfo.ShowEmployees)
                {
                    bool isAdd = employeeAttendance.IsAttending;
                    ShowHelper.AddOrDeleteShowEmployeeAttendance(ObjectService, attendeeToSave, employeeAttendance.Employee, isAdd, "ShowController - PostShowAttendeeInformation");
                }
                ObjectService.SaveChanges();
            }
            return new RedirectResult("/ShowCompany/GetCompanyList?showId=" + attendeeInfo.Show.Id);
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
