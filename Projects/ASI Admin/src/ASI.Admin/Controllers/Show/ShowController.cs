using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.oauth;
using asi.asicentral.util.show;
using asi.asicentral.web.models.show;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var show = new ShowModel();
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
                if (show.Id == 0 && showName.Any())
                {
                    ModelState.AddModelError("Name", "The show name already exists");
                    show.ShowType = GetShowType();
                    return View("../Show/AddShow", show);
                }
                else
                {
                    var objShow = new ShowASI();
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
            int? priorityOrder = null;
            ShowAttendee existingAttendee = ObjectService.GetAll<ShowAttendee>().SingleOrDefault(attendee => attendee.ShowId == showId && attendee.CompanyId == companyId);
            if (showAttendee != null)
            {
                if (existingAttendee == null) existingAttendee = new ShowAttendee();
                existingAttendee.CompanyId = companyId;
                existingAttendee.ShowId = showId;
                existingAttendee.IsSponsor = showAttendee.IsSponsor;
                existingAttendee.IsExhibitDay = showAttendee.IsExhibitDay;
                existingAttendee.BoothNumber = showAttendee.BoothNumber;
                existingAttendee.IsPresentation = showAttendee.IsPresentation;
                existingAttendee.IsExisting = showAttendee.IsExisting;
                existingAttendee.IsCatalog = showAttendee.IsCatalog;
                existingAttendee.UpdateDate = DateTime.UtcNow;
                existingAttendee.UpdateSource = "ShowController - PostShowAttendeeInformation";
                existingAttendee.IsNew = showAttendee.IsNew;
                ShowAttendee attendeeToSave = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, existingAttendee);
                if (attendeeInfo != null && attendeeInfo.ShowAttendees != null && attendeeInfo.ShowAttendees.Count > 0 &&
                    existingAttendee.EmployeeAttendees != null && existingAttendee.EmployeeAttendees.Count > 0)
                {
                    for (int i = 0; i < existingAttendee.EmployeeAttendees.Count; i++)
                    {
                        var employeeInfo = attendeeInfo.ShowEmployees.FirstOrDefault(m => m.Employee.Id == existingAttendee.EmployeeAttendees[i].EmployeeId);
                        existingAttendee.EmployeeAttendees[i].PriorityOrder = employeeInfo.PriorityOrder;
                    }
                }
                foreach (EmployeeAttendance employeeAttendance in attendeeInfo.ShowEmployees)
                {
                    bool isAdd = employeeAttendance.IsAttending;
                    ShowHelper.AddOrDeleteShowEmployeeAttendance(ObjectService, attendeeToSave, employeeAttendance.Employee, isAdd, "ShowController - PostShowAttendeeInformation");
                }


                ObjectService.SaveChanges();
            }
            return new RedirectResult("/ShowCompany/GetAttendeeCompany?showId=" + attendeeInfo.Show.Id);
        }

        public ActionResult ShowList(String showTab, int? ShowTypeId, int? year, int page = 1, int pageSize = 20)
        {
            var show = new ShowModel();
            show.ShowType = GetShowType();
            show.CurrentPageIndex = page;
            show.PageSize = pageSize;
            var showList = ObjectService.GetAll<ShowASI>(true);
            if (string.IsNullOrEmpty(showTab)) showTab = ShowModel.TAB_SHOWTYPE;
            if (ShowTypeId != null)
            {
                showList = showList.Where(item => item.ShowTypeId == ShowTypeId);
            }
            if (year != null)
            {
                showList = showList.Where(item => item.StartDate.Year == year);
            }
            show.TotalRecordCount = showList.Count();
            showList = showList.OrderByDescending(form => form.StartDate);
            show.ShowTab = showTab;
            show.TabShowTypeId = ShowTypeId;
            show.TabYear = year;
            show.Show = showList.Skip((show.CurrentPageIndex - 1) * show.PageSize)
                                            .Take(show.PageSize).ToList();
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
            var show = new ShowModel();
            show.ShowType = GetShowType();
            if (id != 0)
            {
                ShowASI ShowModel = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Id == id);
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
            ShowASI show = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Id == id);
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
                            if (employeeAttendeeCount > 0)
                            {
                                for (int employee = employeeAttendeeCount; employee > 0; employee--)
                                {
                                    ObjectService.Delete(employeeAttendees.ElementAt(employee - 1));
                                }
                            }

                        }
                        ObjectService.Delete(show.Attendees.ElementAt(attendee - 1));
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

        [HttpGet]
        public ActionResult ValidateAttendeeEmails(int id)
        {
            var validationDetails = new List<Tuple<ShowEmployeeAttendee, string>>();
            var empAttendees = ObjectService.GetAll<ShowEmployeeAttendee>("Employee").Where(empAttendee => empAttendee.Attendee.ShowId == id).OrderBy(empAttendee => empAttendee.Attendee.Company.Name).ToList();
            if (empAttendees != null && empAttendees.Count > 0)
            {                
                //Parallel.ForEach(empAttendees, empAttendee =>
                foreach (var empAttendee in empAttendees)
                {
                    var errorMessage = string.Empty;
                    var inValidEmails = new List<string>();
                    var email = empAttendee.Employee.Email;
                    var user = ASIOAuthClient.GetUserByEmail(email);
                    if (user == null)
                    {
                        errorMessage = "Invalid Email";
                    }
                    else
                    {
                        if ((string.Compare(user.StatusCode, "DELISTED") == 0))
                        {
                            if (!string.IsNullOrWhiteSpace(errorMessage))
                                errorMessage += "<br/>";
                            errorMessage = "Invalid Company Status - <b>DELISTED</b>";
                        }
                        if ((string.Compare(user.StatusCode, "TERMINATED") == 0))
                        {
                            if (!string.IsNullOrWhiteSpace(errorMessage))
                                errorMessage += "<br/>";
                            errorMessage = "Invalid Company Status - <b>TERMINATED</b>";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        validationDetails.Add(new Tuple<ShowEmployeeAttendee, string>(empAttendee, errorMessage));
                    }
                }
               //});
            }
            return View("../Show/ValidateAttendeeEmails", validationDetails);
        }
    }
}
