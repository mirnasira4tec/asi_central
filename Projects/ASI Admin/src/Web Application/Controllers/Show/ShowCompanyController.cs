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
            if (!string.IsNullOrEmpty(MemberType) && !string.IsNullOrEmpty(companyName))
            {
                companyList = companyList.Where(item => (item.Name != null
                 && item.Name.Contains(companyName) && item.MemberType != null
                 && item.MemberType.Contains(MemberType)));
            }

            else if (!string.IsNullOrEmpty(MemberType))
            {
                companyList = companyList.Where(item => item.MemberType != null
                 && item.MemberType.Contains(MemberType));
            }
            else if (!string.IsNullOrEmpty(companyName))
            {
                companyList = companyList.Where(item => item.Name != null
                 && item.Name.Contains(companyName));
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
            if (company.IsNonUSAddress)
            {
                if (ModelState.ContainsKey("State")) ModelState["State"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ShowCompany objCompany = new ShowCompany();
                    ShowAddress objAddress = new ShowAddress();
                    ShowCompanyAddress objCompanyAddress = new ShowCompanyAddress();
                    objCompany.Id = company.Id;
                    objCompany.Name = company.Name;
                    objCompany.WebUrl = company.Url;
                    objCompany.MemberType = company.MemberType;
                    objCompany.ASINumber = (company.MemberType == "Non-Member") ? null : company.ASINumber;
                    objCompany.UpdateSource = "ShowCompanyController - AddCompany";
                    objCompany = ShowHelper.CreateOrUpdateCompany(ObjectService, objCompany);

                    objAddress.Id = company.AddressId;
                    objAddress.Phone = company.Phone;
                    objAddress.PhoneAreaCode = company.PhoneAreaCode.Trim();
                    objAddress.FaxAreaCode = company.FaxAreaCode;
                    objAddress.Fax = company.Fax;
                    objAddress.Street1 = company.Address1;
                    objAddress.Street2 = company.Address2;
                    objAddress.Zip = company.Zip;
                    objAddress.State = company.IsNonUSAddress ? company.InternationalState : company.State;
                    objAddress.Country = company.IsNonUSAddress ? company.Country : "USA";
                    objAddress.City = company.City;
                    objAddress.UpdateSource = "ShowCompanyController - AddCompany";
                    objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);

                    ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().Where(item => item.CompanyId == company.Id && item.Address.Id == company.AddressId).FirstOrDefault();
                    if (companyAddress != null)
                    {
                        objCompanyAddress.Id = companyAddress.Id;
                        objCompanyAddress.CompanyId = companyAddress.CompanyId;
                        objCompanyAddress.Address = companyAddress.Address;
                        objCompanyAddress.UpdateSource = "ShowCompanyController - AddCompany";
                        objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);
                    }
                    else
                    {
                        objCompanyAddress.CompanyId = objCompany.Id;
                        objCompanyAddress.Address = objAddress;
                        objCompanyAddress.UpdateSource = "ShowCompanyController - AddAddress";
                        objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);
                    }
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

        [HttpGet]
        public ActionResult EditCompany(int id)
        {
            CompanyModel company = new CompanyModel();
            AddressModel address = new AddressModel();

            if (id != 0)
            {
                ShowCompany CompanyModel = ObjectService.GetAll<ShowCompany>().Where(item => item.Id == id).FirstOrDefault();
                if (company != null)
                {
                    company.Id = id;
                    company.Name = CompanyModel.Name;
                    company.MemberType = CompanyModel.MemberType;
                    if (CompanyModel.MemberType == "Non-Member")
                    {
                        company.ASINumber = null;
                    }
                    else
                    {
                        company.ASINumber = CompanyModel.ASINumber;
                    }
                    company.Url = CompanyModel.WebUrl;
                    ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().Where(item => item.CompanyId == id).FirstOrDefault();
                    if (companyAddress != null)
                    {
                        ShowAddress addressModel = ObjectService.GetAll<ShowAddress>().Where(item => item.Id == companyAddress.Address.Id).FirstOrDefault();
                        if (addressModel != null)
                        {
                            company.AddressId = companyAddress.Address.Id;
                            company.Phone = addressModel.Phone;
                            company.PhoneAreaCode = addressModel.PhoneAreaCode.Trim();
                            company.Fax = addressModel.Fax;
                            company.FaxAreaCode = addressModel.FaxAreaCode;
                            company.Address1 = addressModel.Street1;
                            company.Address2 = addressModel.Street2;
                            company.Zip = addressModel.Zip;
                            company.City = addressModel.City;
                            company.IsNonUSAddress = addressModel.Country != "USA";
                            if (company.IsNonUSAddress)
                            {
                                company.IsNonUSAddress = true;
                                company.InternationalState = addressModel.State;
                                company.State = addressModel.State;
                                company.Country = addressModel.Country;
                            }
                            else
                            {
                                company.State = addressModel.State;
                                company.Country = "USA";
                            }
                        }
                    }
                }
            }
            return View("../Show/Company/AddCompany", company);
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
                IQueryable<ShowEmployee> employeeList = ObjectService.GetAll<ShowEmployee>().Where(item => item.CompanyId == id);
                companyInformation.Employee = employeeList.OrderByDescending(form => form.CreateDate).ToList();
            }
            return View("../Show/Company/CompanyInformation", companyInformation);
        }

        [HttpGet]
        public ActionResult AddAddress(int companyId)
        {
            AddressModel Address = new AddressModel();
            Address.CompanyId = companyId;
            return View("../Show/Company/AddAddress", Address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddAddress(AddressModel Address)
        {
            try
            {
                if (Address.IsNonUSAddress)
                {
                    if (ModelState.ContainsKey("State")) ModelState["State"].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    ShowAddress objAddress = new ShowAddress();
                    ShowCompanyAddress objCompanyAddress = new ShowCompanyAddress();
                    objAddress.Id = Address.AdreessId;
                    objAddress.Phone = Address.Phone;
                    objAddress.PhoneAreaCode = Address.PhoneAreaCode.Trim();
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

                    ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().Where(item => item.CompanyId == Address.CompanyId && item.Address.Id == objAddress.Id).FirstOrDefault();
                    if (companyAddress != null)
                    {
                        objCompanyAddress.Id = companyAddress.Id;
                        objCompanyAddress.CompanyId = companyAddress.CompanyId;
                        objCompanyAddress.Address = companyAddress.Address;
                        objCompanyAddress.UpdateSource = "ShowCompanyController - AddAddress";
                        objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);
                    }
                    else
                    {
                        objCompanyAddress.CompanyId = Address.CompanyId;
                        objCompanyAddress.Address = objAddress;
                        objCompanyAddress.UpdateSource = "ShowCompanyController - AddAddress";
                        objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);
                    }
                    ObjectService.SaveChanges();
                    return RedirectToAction("List", "ShowCompany", new { id = Address.CompanyId });
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

        public ActionResult DeleteAddress(int id, int companyId)
        {
            ShowAddress address = ObjectService.GetAll<ShowAddress>().Where(item => item.Id == id).FirstOrDefault();
            IList<ShowCompanyAddress> companyAddress = ObjectService.GetAll<ShowCompanyAddress>().Where(item => item.Address.Id == id).ToList();
            int companyAddressCount = companyAddress.Count();
            if (companyAddressCount > 0)
            {
                for (int i = companyAddressCount; i > 0; i--)
                {
                    ObjectService.Delete<ShowCompanyAddress>(companyAddress.ElementAt(i - 1));
                }
            }
            ObjectService.Delete<ShowAddress>(address);
            ObjectService.SaveChanges();
            return RedirectToAction("List", new
            {
                ID = companyId
            });
        }

        [HttpGet]
        public ActionResult AddEmployee(int companyId)
        {
            var objEmployee = new CompanyInformation();
            objEmployee.CompanyId = companyId;
            return View("../Show/Company/AddEmployee", objEmployee);
        }

        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddEmployee(CompanyInformation employee)
        {
            try
            {
                if (!employee.HasAddress)
                {
                    if (ModelState.ContainsKey("Phone")) ModelState["Phone"].Errors.Clear();
                    if (ModelState.ContainsKey("PhoneAreaCode")) ModelState["PhoneAreaCode"].Errors.Clear();
                    if (ModelState.ContainsKey("Address1")) ModelState["Address1"].Errors.Clear();
                    if (ModelState.ContainsKey("Zip")) ModelState["Zip"].Errors.Clear();
                    if (ModelState.ContainsKey("State")) ModelState["State"].Errors.Clear();
                    if (ModelState.ContainsKey("Country")) ModelState["Country"].Errors.Clear();
                    if (ModelState.ContainsKey("City")) ModelState["City"].Errors.Clear();
                }
                if (employee.HasAddress && employee.IsNonUSAddress && ModelState.ContainsKey("State")) ModelState["State"].Errors.Clear();
                if (ModelState.IsValid)
                {
                    ShowAddress objAddress = null;
                    if (employee.HasAddress)
                    {
                        objAddress = new ShowAddress();
                        objAddress.Id = employee.AdreessId;
                        objAddress.Phone = employee.Phone;
                        objAddress.PhoneAreaCode = employee.PhoneAreaCode.Trim();
                        objAddress.FaxAreaCode = employee.FaxAreaCode;
                        objAddress.Fax = employee.Fax;
                        objAddress.Street1 = employee.Address1;
                        objAddress.Street2 = employee.Address2;
                        objAddress.Zip = employee.Zip;
                        objAddress.State = employee.IsNonUSAddress ? employee.InternationalState : employee.State;
                        objAddress.Country = employee.IsNonUSAddress ? employee.Country : "USA";
                        objAddress.City = employee.City;
                        objAddress.UpdateSource = "ShowCompanyController - Add";
                        objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);
                    }
                    ShowEmployee objEmployee = new ShowEmployee();
                    objEmployee.Id = employee.Id;
                    objEmployee.FirstName = employee.FirstName;
                    objEmployee.LastName = employee.LastName;
                    objEmployee.Email = employee.Email;
                    objEmployee.CompanyId = employee.CompanyId;
                    objEmployee.Address = objAddress;
                    objEmployee.UpdateSource = "ShowCompanyController - Add";
                    objEmployee = ShowHelper.CreateOrUpdateEmployee(ObjectService, objEmployee);
                    ObjectService.SaveChanges();
                    return RedirectToAction("List", "ShowCompany", new { id = employee.CompanyId });
                }
                else
                {
                    return View("../Show/Company/AddEmployee", employee);
                }
            }
            catch (Exception ex)
            {
                messages.Add("Error: " + ex.Message);
            }
            return View("../Show/Company/AddEmployee", employee);
        }

        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            CompanyInformation companyInfo = new CompanyInformation();
            if (id != 0)
            {
                ShowEmployee employeeModel = ObjectService.GetAll<ShowEmployee>().Where(item => item.Id == id).FirstOrDefault();
                if (companyInfo != null)
                {
                    companyInfo.EmployeeId = id;
                    companyInfo.AdreessId = employeeModel.AddressId.HasValue ? employeeModel.AddressId.Value : 0;
                    companyInfo.FirstName = employeeModel.FirstName;
                    companyInfo.LastName = employeeModel.LastName;
                    companyInfo.Email = employeeModel.Email;
                    companyInfo.CompanyId = employeeModel.CompanyId.HasValue ? employeeModel.CompanyId.Value : 0;
                    companyInfo.HasAddress = employeeModel.Address != null;
                    if (companyInfo.HasAddress)
                    {
                        companyInfo.IsNonUSAddress = employeeModel.Address.Country != "USA";
                        companyInfo.HasAddress = true;
                        companyInfo.Phone = employeeModel.Address.Phone;
                        companyInfo.PhoneAreaCode = employeeModel.Address.PhoneAreaCode.Trim();
                        companyInfo.FaxAreaCode = employeeModel.Address.FaxAreaCode;
                        companyInfo.Fax = employeeModel.Address.Fax;
                        companyInfo.Address1 = employeeModel.Address.Street1;
                        companyInfo.Address2 = employeeModel.Address.Street2;
                        companyInfo.Zip = employeeModel.Address.Zip;
                        companyInfo.City = employeeModel.Address.City;
                        if (companyInfo.IsNonUSAddress)
                        {
                            companyInfo.IsNonUSAddress = true;
                            companyInfo.InternationalState = employeeModel.Address.State;
                            companyInfo.Country = employeeModel.Address.Country;
                        }
                        else
                        {
                            companyInfo.State = employeeModel.Address.State;
                            companyInfo.Country = "USA";
                        }
                    }
                }
            }
            return View("../Show/Company/AddEmployee", companyInfo);
        }

        [HttpGet]
        public ActionResult EditAddress(int id, int companyId)
        {
            AddressModel address = new AddressModel();
            if (id != 0)
            {
                ShowAddress addressModel = ObjectService.GetAll<ShowAddress>().Where(item => item.Id == id).FirstOrDefault();
                if (address != null)
                {
                    address.AdreessId = addressModel.Id;
                    address.Phone = addressModel.Phone;
                    address.PhoneAreaCode = addressModel.PhoneAreaCode.Trim();
                    address.Fax = addressModel.Fax;
                    address.FaxAreaCode = addressModel.FaxAreaCode;
                    address.Address1 = addressModel.Street1;
                    address.Address2 = addressModel.Street2;
                    address.Zip = addressModel.Zip;
                    address.City = addressModel.City;
                    address.CompanyId = companyId;
                    address.IsNonUSAddress = addressModel.Country != "USA";
                    if (address.IsNonUSAddress)
                    {
                        address.IsNonUSAddress = true;
                        address.InternationalState = addressModel.State;
                        address.Country = addressModel.Country;
                    }
                    else
                    {
                        address.State = addressModel.State;
                        address.Country = "USA";
                    }
                }
            }
            return View("../Show/Company/AddAddress", address);
        }

        public ActionResult DeleteEmployee(int id)
        {
            ShowEmployee employee = ObjectService.GetAll<ShowEmployee>().Where(item => item.Id == id).FirstOrDefault();
            var companyId = employee.CompanyId;
            ShowAddress addresss = ObjectService.GetAll<ShowAddress>().Where(item => item.Id == employee.AddressId).FirstOrDefault();
            ShowEmployeeAttendee employeeAttendee = ObjectService.GetAll<ShowEmployeeAttendee>().Where(item => item.Employee.Id == employee.Id).FirstOrDefault();
            if (employeeAttendee != null)
            {
                ObjectService.Delete<ShowEmployeeAttendee>(employeeAttendee);
                if (addresss != null)
                {
                    ObjectService.Delete<ShowAddress>(addresss);
                    ObjectService.Delete<ShowEmployee>(employee);
                }
                else
                {
                    ObjectService.Delete<ShowEmployee>(employee);
                }
            }
            else
            {
                if (addresss != null)
                {
                    ObjectService.Delete<ShowAddress>(addresss);
                    ObjectService.Delete<ShowEmployee>(employee);
                }
                else
                {
                    ObjectService.Delete<ShowEmployee>(employee);
                }
            }
            ObjectService.SaveChanges();
            return RedirectToAction("List", new
            {
                ID = companyId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsValidEmail(string Email)
        {
            IList<ShowEmployee> employeeList = ObjectService.GetAll<ShowEmployee>().Where(item => item.Email != null && item.Email.Contains(Email)).ToList();
            if (employeeList.Any())
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult GetAttendeeCompany(int? showId, String companyTab, string companyName, string MemberType)
        {
            ShowCompaniesModel showCompanies = new ShowCompaniesModel();
            if (string.IsNullOrEmpty(companyTab)) companyTab = ShowCompaniesModel.TAB_COMPANYNAME;
            showCompanies.CompanyTab = companyTab;
            IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>().Where(attendee => (attendee.ShowId == showId && (attendee.IsAttending == true || attendee.IsExhibitDay == true))).ToList();
            if (existingAttendees.Any())
            {
                foreach (ShowAttendee attendee in existingAttendees)
                {
                    showCompanies.Show = attendee.Show;
                    showCompanies.ShowAttendees.Add(attendee);
                }
            }
            showCompanies.ShowId = showId.HasValue ? showId.Value : 0; 
            return View("../Show/ShowCompaniesList", showCompanies);
        }

        public ActionResult EmployeeList()
        {
            var employee = new CompanyInformation();
            IQueryable<ShowEmployee> employeeList = ObjectService.GetAll<ShowEmployee>(true);
            employee.Employee = employeeList.OrderByDescending(form => form.CreateDate).ToList();
            return View("../Show/Company/CompanyList", employee);
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

        [HttpGet]
        public virtual ActionResult AddCompaniesToShow(int? showId, String companyTab, string companyName, string MemberType)
        {
            ShowCompaniesModel showCompanies = new ShowCompaniesModel();
            IList<ShowCompany> list = null;
            if (string.IsNullOrEmpty(companyTab)) companyTab = ShowCompaniesModel.TAB_COMPANYNAME;
            showCompanies.CompanyTab = companyTab;
            IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => ((item.IsAttending == true || item.IsExhibitDay == true)  && item.ShowId == showId)).ToList();
            if (existingAttendees.Any())
            {
                IList<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true).ToList();
                list = companyList.Where(p => !existingAttendees.Any(p2 => p2.CompanyId == p.Id)).ToList();
            }
            else
            {
                list = ObjectService.GetAll<ShowCompany>(true).ToList();
            }
            if (!string.IsNullOrEmpty(MemberType) && !string.IsNullOrEmpty(companyName))
            {
                list = list.Where(item => (item.Name != null
                && item.Name.Contains(companyName) && item.MemberType != null
                && item.MemberType.Contains(MemberType))).ToList();
            }
            else if (!string.IsNullOrEmpty(MemberType))
            {
                list = list.Where(item => item.MemberType != null
                 && item.MemberType.Contains(MemberType)).ToList();
            }
            else if (!string.IsNullOrEmpty(companyName))
            {
                list = list.Where(item => item.Name != null
                 && item.Name.Contains(companyName)).ToList();
            }
            showCompanies.ShowCompanies = list;
            showCompanies.ShowId = showId.HasValue ? showId.Value : 0; 


            return View("../Show/AddCompaniesToShow", showCompanies);
        }


        public ActionResult DeleteAttendeeCompany(int id, int showId)
        {
            ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>().Where(item => item.Id == id).FirstOrDefault();
            if (attendee != null)
            {
                int employeeAttendeeCount = attendee.EmployeeAttendees.Count();

                for (int i = employeeAttendeeCount; i > 0; i--)
                {
                    ObjectService.Delete<ShowEmployeeAttendee>(attendee.EmployeeAttendees.ElementAt(i - 1));
                }
                ObjectService.Delete<ShowAttendee>(attendee);
                ObjectService.SaveChanges();
            }
            return RedirectToAction("GetAttendeeCompany", new
            {
                showId = showId
            });
        }
    }
}
