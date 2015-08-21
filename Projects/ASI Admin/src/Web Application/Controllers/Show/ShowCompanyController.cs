using asi.asicentral.model.show;
using asi.asicentral.services;
using asi.asicentral.util.show;
using asi.asicentral.web.models.show;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace asi.asicentral.web.Controllers.Show
{
    public class ShowCompanyController : Controller
    {
        public IObjectService ObjectService { get; set; }
        IList<string> messages = new List<string>();

        public ActionResult CompanyList(String companyTab, string companyName, string MemberType)
        {
            var company = new CompanyModel();
            IQueryable<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true);
            if (string.IsNullOrEmpty(companyTab)) companyTab = CompanyModel.TAB_COMPANYNAME;
            if (companyTab == CompanyModel.TAB_COMPANYNAME && !string.IsNullOrEmpty(companyName))
            {
                companyList = companyList.Where(item => item.Name != null
                 && item.Name.Contains(companyName));
            }
            else if (companyTab == CompanyModel.TAB_MEMBERTYPE && !string.IsNullOrEmpty(MemberType))
            {
                companyList = companyList.Where(item => item.MemberType != null
                 && item.MemberType.Contains(MemberType));
            }

            company.CompanyTab = companyTab;
            company.company = companyList.OrderByDescending(form => form.CreateDate).ToList();
            return View("../Show/Company/CompanyList", company);
        }

        [HttpGet]
        public virtual ActionResult AddCompany()
        {
            CompanyModel company = new CompanyModel();
            return View("../Show/Company/AddCompany", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddCompany(CompanyModel company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ShowCompany objCompany = new ShowCompany();
                    ShowAddress objAddress = new ShowAddress();
                    ShowCompanyAddress objCompanyAddress = new ShowCompanyAddress();
                    objCompany.Name = company.Name;
                    objCompany.WebUrl = company.Url;
                    objCompany.MemberType = company.MemberType;
                    objCompany.ASINumber = company.ASINumber;
                    objCompany.UpdateSource = "ShowCompanyController - Add";
                    objCompany = ShowHelper.CreateOrUpdateCompany(ObjectService, objCompany);

                    objAddress.Phone = company.Phone;
                    objAddress.PhoneAreaCode = company.PhoneAreaCode;
                    objAddress.FaxAreaCode = company.FaxAreaCode;
                    objAddress.Fax = company.Fax;
                    objAddress.Street1 = company.Address1;
                    objAddress.Street2 = company.Address2;
                    objAddress.Zip = company.Zip;
                    objAddress.State = company.IsNonUSAddress ? company.InternationalState : company.State;
                    objAddress.Country = company.IsNonUSAddress ? company.Country : "USA";
                    objAddress.City = company.City;
                    objAddress.UpdateSource = "ShowCompanyController - Add";
                    objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);

                    objCompanyAddress.CompanyId = objCompany.Id;
                    objCompanyAddress.Address = objAddress;
                    objCompanyAddress.UpdateSource = "ShowCompanyController - Add";
                    objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);

                    ObjectService.Add<ShowCompany>(objCompany);
                    ObjectService.Add<ShowAddress>(objAddress);
                    ObjectService.Add<ShowCompanyAddress>(objCompanyAddress);
                    ObjectService.SaveChanges();

                }
                catch (Exception ex)
                {
                    messages.Add("Error: " + ex.Message);
                }
                return RedirectToAction("CompanyList");
            }
            else
            {
                return View("../Show/Company/AddCompany", company);
            }
        }

        public ActionResult Delete(int id)
        {
            ShowCompany company = ObjectService.GetAll<ShowCompany>().Where(item => item.Id == id).FirstOrDefault();
            if (company != null)
            {
                int addressCount = company.CompanyAddresses.Count();
                int employeeCount = company.Employees.Count();
                int companyAttendeeCount = company.Attendees.Count();
                for (int i = addressCount; i > 0; i--)
                {
                    ObjectService.Delete<ShowCompanyAddress>(company.CompanyAddresses.ElementAt(i - 1));
                }
                for (int i = employeeCount; i > 0; i--)
                {
                    ObjectService.Delete<ShowEmployee>(company.Employees.ElementAt(i - 1));
                }
                for (int i = companyAttendeeCount; i > 0; i--)
                {
                    ObjectService.Delete<ShowAttendee>(company.Attendees.ElementAt(i - 1));
                }
                ObjectService.Delete<ShowCompany>(company);
                ObjectService.SaveChanges();
            }
            return Redirect("CompanyList");

        }

        public ActionResult List(int id)
        {
            CompanyInformation companyInformation = new CompanyInformation();
            if (id != 0)
            {
                ShowCompany company = ObjectService.GetAll<ShowCompany>().Where(item => item.Id == id).FirstOrDefault();
                companyInformation.Name = company.Name;
                companyInformation.Id = company.Id;
                IQueryable<ShowCompanyAddress> companyAddress = ObjectService.GetAll<ShowCompanyAddress>().Where(item => item.CompanyId == id);
                companyInformation.CompanyAddress = companyAddress.ToList();
            }
            return View("../Show/Company/CompanyInformation", companyInformation);
        }

        [HttpGet]
        public ActionResult AddAddress(int CompanyID)
        {
            AddressModel Address = new AddressModel();
            Address.CompanyId = CompanyID;
            return View("../Show/Company/AddAddress", Address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddAddress(AddressModel Address)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShowAddress objAddress = new ShowAddress();
                    ShowCompanyAddress objCompanyAddress = new ShowCompanyAddress();

                    objAddress.Phone = Address.Phone;
                    objAddress.PhoneAreaCode = Address.PhoneAreaCode;
                    objAddress.FaxAreaCode = Address.FaxAreaCode;
                    objAddress.Fax = Address.Fax;
                    objAddress.Street1 = Address.Address1;
                    objAddress.Street2 = Address.Address2;
                    objAddress.Zip = Address.Zip;
                    objAddress.State = Address.IsNonUSAddress ? Address.InternationalState : Address.State;
                    objAddress.Country = Address.IsNonUSAddress ? Address.Country : "USA";
                    objAddress.City = Address.City;
                    objAddress.UpdateSource = "ShowCompanyController - AddAddress";
                    objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);

                    objCompanyAddress.CompanyId = Address.CompanyId;
                    objCompanyAddress.Address = objAddress;
                    objCompanyAddress.UpdateSource = "ShowCompanyController - Add";
                    objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);

                    ObjectService.Add<ShowAddress>(objAddress);
                    ObjectService.Add<ShowCompanyAddress>(objCompanyAddress);
                    ObjectService.SaveChanges();

                    return RedirectToAction("CompanyList");
                }
                else
                {
                    return View("../Show/Company/AddAddress", Address);
                }
            }
            catch (Exception ex)
            {
                messages.Add("Error: " + ex.Message);
            }
            return View("../Show/Company/AddCompany", Address);
        }

        [HttpGet]
        public ActionResult AddEmployee(int CompanyID)
        {
            var model = new AddressModel();
            return View("../Show/Company/AddEmployee", model);
        }

        [HttpPost]
        public ActionResult IsValidCompany(string name)
        {
            var company = new CompanyModel();
            IQueryable<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true);
            companyList = companyList.Where(item => item.Name != null
                 && item.Name.Contains(name));
            company.company = companyList.ToList();
            if (company.company.Any())
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet]
        public ActionResult GetCompanyList(int showId)
        {
            ShowCompaniesModel showCompanies = new ShowCompaniesModel();
            IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>(true).Where(attendee => attendee.ShowId == showId).ToList();
            foreach (ShowAttendee attendee in existingAttendees)
            {
                showCompanies.Show = attendee.Show;
                showCompanies.ShowAttendees.Add(attendee);
            }
            if (showCompanies.Show == null) showCompanies.Show = ObjectService.GetAll<ShowASI>(true).SingleOrDefault(show => show.Id == showId);
            IList<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true).ToList();
            foreach (ShowCompany company in companyList)
            {
                if (showCompanies.ShowAttendees.Where(item => item.CompanyId == company.Id).Count() == 0)
                {
                    ShowAttendee attendee = new ShowAttendee();
                    attendee.Company = company;
                    showCompanies.ShowAttendees.Add(attendee);
                }
            }
            return View("../Show/ShowCompaniesList", showCompanies);
        }

        [HttpGet]
        public ActionResult GetCompanyDetailsForShow(int companyId, int showId)
        {
            ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>(true).SingleOrDefault(sa => sa.ShowId == showId && sa.CompanyId == companyId);
            IList<ShowEmployee> employees = ObjectService.GetAll<ShowEmployee>(true).Where(e => e.CompanyId == companyId).ToList();
            ShowCompaniesModel showCompanies = new ShowCompaniesModel();
            if (attendee != null)
            {
                showCompanies.ShowAttendees.Add(attendee);
                showCompanies.Show = attendee.Show;
                IList<ShowEmployeeAttendee> attendees = ObjectService.GetAll<ShowEmployeeAttendee>(true).Where(sea => sea.AttendeeId == attendee.Id).ToList();
                foreach (ShowEmployee showEmployee in employees)
                {
                    EmployeeAttendance employeeAttendance = AddEmployeeAttandance(showEmployee, attendees.Where(a => a.EmployeeId == showEmployee.Id).Count() == 1);
                    showCompanies.ShowEmployees.Add(employeeAttendance);
                }
            }
            else
            {
                ShowCompany company = ObjectService.GetAll<ShowCompany>(true).SingleOrDefault(sc => sc.Id == companyId);
                ShowASI show = ObjectService.GetAll<ShowASI>(true).SingleOrDefault(s => s.Id == showId);
                ShowAttendee newAttendee = new ShowAttendee();
                newAttendee.Company = company;
                newAttendee.CompanyId = company.Id;
                newAttendee.ShowId = show.Id;
                showCompanies.ShowAttendees.Add(newAttendee);
                foreach (ShowEmployee showEmployee in employees)
                {
                    EmployeeAttendance employeeAttendance = AddEmployeeAttandance(showEmployee, false);
                    showCompanies.ShowEmployees.Add(employeeAttendance);
                }
                showCompanies.Show = show;
            }
            return View("../Show/AddCompanyAttendeesToShow", showCompanies);
        }

        private EmployeeAttendance AddEmployeeAttandance(ShowEmployee showEmployee, bool isAttending)
        {
            EmployeeAttendance employeeAttendance = new EmployeeAttendance();
            employeeAttendance.Employee = showEmployee;
            employeeAttendance.IsAttending = isAttending;
            return employeeAttendance;
        }
    }
}
