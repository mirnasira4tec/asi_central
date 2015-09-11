using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.util.show;
using asi.asicentral.web.models.show;
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
        public ActionResult AddShow()
        {
            ShowModel show = new ShowModel();
            show.ShowType = GetShowType();
            show.StartDate = DateTime.UtcNow;
            show.EndDate = DateTime.UtcNow;
            return View("../Show/AddShow", show);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddShow(ShowModel show)
        {
            if (ModelState.IsValid)
            {
                IQueryable<ShowASI> showName = ObjectService.GetAll<ShowASI>().Where(item => item.Name == show.Name
                && item.StartDate == show.StartDate && item.EndDate == show.EndDate);
                if (showName.Any())
                {
                    ModelState.AddModelError("Name", "The show name already exists");
                    show.ShowType = GetShowType();
                    return View("../Show/AddShow", show);
                }
                else
                {
                    ShowASI objShow = new ShowASI();
                    objShow.Id = show.Id;
                    objShow.Name = show.Name;
                    objShow.Address = show.Address;
                    objShow.ShowTypeId = show.ShowTypeId;
                    objShow.StartDate = show.StartDate;
                    objShow.EndDate = show.EndDate;
                    objShow.UpdateSource = "ShowController - AddShow";
                    objShow = ShowHelper.CreateOrUpdateShow(ObjectService, objShow);
                    ObjectService.SaveChanges();
                }
                return RedirectToAction("ShowList");
            }
            else
            {
                if (show != null)
                {
                    show.ShowType = GetShowType();
                }
                return View("../Show/AddShow", show);
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
            return new RedirectResult("/ShowCompany/GetAttendeeCompany?showId=" + attendeeInfo.Show.Id);
        }

        public ActionResult ShowList(String showTab, int? ShowTypeId, int? year)
        {
            var show = new ShowModel();
            show.ShowType = GetShowType();
            IQueryable<ShowASI> showList = ObjectService.GetAll<ShowASI>(true);
            if (string.IsNullOrEmpty(showTab)) showTab = ShowModel.TAB_SHOWTYPE;
            if (ShowTypeId != null && year != null)
            {
                showList = showList.Where(item => (item.StartDate.Year != null
                && item.StartDate.Year == year && item.ShowTypeId != null && item.ShowTypeId == ShowTypeId));
            }
            else if (ShowTypeId != null || year != null )
            {
                showList = showList.Where(item => (item.ShowTypeId != null 
                && item.ShowTypeId == ShowTypeId) || ( item.StartDate.Year != null 
                 && item.StartDate.Year == year));
            }
            show.ShowTab = showTab;
            show.Show = showList.OrderByDescending(form => form.StartDate).ToList();
            return View("../Show/ShowList", show);
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ShowModel show = new ShowModel();
            show.ShowType = GetShowType();
            if (id != 0)
            {
                ShowASI ShowModel = ObjectService.GetAll<ShowASI>().Where(item => item.Id == id).FirstOrDefault();
                if (show != null)
                {
                    show.Id = id;
                    show.Name = ShowModel.Name;
                    show.Address = ShowModel.Address;
                    show.StartDate = ShowModel.StartDate;
                    show.EndDate = ShowModel.EndDate;
                    show.ShowTypeId = ShowModel.ShowTypeId;
                }
            }
            else
            {
                show.StartDate = DateTime.UtcNow;
                show.EndDate = DateTime.UtcNow;
            }
            return View("../Show/AddShow", show);
        }

        public ActionResult Delete(int id)
        {
            ShowASI show = ObjectService.GetAll<ShowASI>().Where(item => item.Id == id).FirstOrDefault();
            IList<ShowEmployeeAttendee> employeeAttendees = null;
            if (show != null)
            {
                int attendeeCount = show.Attendees.Count();
                if (attendeeCount > 0)
                {
                    for (int attendee = attendeeCount; attendee > 0; attendee--)
                    {
                        foreach (var employeeAttendee in show.Attendees)
                        {
                            employeeAttendees = ObjectService.GetAll<ShowEmployeeAttendee>().Where(item => item.AttendeeId == employeeAttendee.Id).ToList();
                            int employeeAttendeeCount = employeeAttendees.Count();
                            if (employeeAttendeeCount >0)
                            {
                                for (int employee = employeeAttendeeCount; employee > 0; employee--)
                                {
                                    ObjectService.Delete<ShowEmployeeAttendee>(employeeAttendees.ElementAt(employee - 1));
                                }
                            }

                        }
                        ObjectService.Delete<ShowAttendee>(show.Attendees.ElementAt(attendee - 1));
                    }
                }

                ObjectService.Delete<ShowASI>(show);
                ObjectService.SaveChanges();
            }
            return Redirect("ShowList");
        }

        public ActionResult AttendeeList()
        {
            IList<ShowEmployeeAttendee> attendees = ObjectService.GetAll<ShowEmployeeAttendee>(true).ToList();
            return View("../Show/Attendees", attendees);
        }
    }
}
