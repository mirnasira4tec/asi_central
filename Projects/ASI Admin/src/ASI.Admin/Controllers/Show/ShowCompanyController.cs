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

        public ActionResult CompanyList(String companyTab, string Name, string MemberType, string asiNumber, int page = 1, int pageSize = 20)
        {
            var company = new CompanyModel();
            company.CurrentPageIndex = page;
            company.PageSize = pageSize;
            company.TabCompanyName = Name;
            company.TabMemberType = MemberType;
            var start = DateTime.Now;
            DateTime end;
            ILogService log = LogService.GetLog(this.GetType());
            log.Debug("CompanyList.cshtml - Start");
            var companyList = ObjectService.GetAll<ShowCompany>(true);
            if (string.IsNullOrEmpty(companyTab)) companyTab = CompanyModel.TAB_COMPANYNAME;
            if (!string.IsNullOrEmpty(MemberType))
            {
                companyList = companyList.Where(item => item.MemberType != null
                 && item.MemberType.ToLower().Contains(MemberType.ToLower()));
            }
            if (!string.IsNullOrEmpty(Name))
            {
                companyList = companyList.Where(item => item.Name != null
                 && item.Name.ToLower().Contains(Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(asiNumber))
            {
                companyList = companyList.Where(item => item.ASINumber != null
                 && item.ASINumber == asiNumber || item.SecondaryASINo != null
                 && item.SecondaryASINo == asiNumber);
            }
            company.TotalRecordCount = companyList.Count();
            companyList = companyList.OrderBy(form => form.Name);
            company.CompanyTab = companyTab;
            company.Name = Name;
            company.ASINumber = asiNumber;
            company.company = companyList.Skip((company.CurrentPageIndex - 1) * company.PageSize)
                                            .Take(company.PageSize).ToList();
            end = DateTime.Now;
            log.Debug("CompanyList.cshtml - End " + (end - start));
            return View("../Show/Company/CompanyList", company);

        }


        [HttpGet]
        public virtual ActionResult AddCompany()
        {
            var company = new CompanyModel();
            return View("../Show/Company/AddCompany", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddCompany(CompanyModel company)
        {
            if (company.IsNonUSAddress && ModelState.ContainsKey("State")) ModelState["State"].Errors.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    var objCompany = new ShowCompany();
                    var objAddress = new ShowAddress();
                    var objCompanyAddress = new ShowCompanyAddress();
                    objCompany.Id = company.Id;
                    objCompany.Name = company.Name;
                    objCompany.WebUrl = company.Url;
                    objCompany.MemberType = company.MemberType;
                    objCompany.ASINumber = (company.MemberType == "Non-Member") ? null : company.ASINumber;
                    objCompany.SecondaryASINo = company.SecondaryASINo;
                    objCompany.UpdateSource = "ShowCompanyController - AddCompany";
                    objCompany = ShowHelper.CreateOrUpdateCompany(ObjectService, objCompany);

                    objAddress.Id = company.AddressId;
                    objAddress.Phone = !String.IsNullOrEmpty(company.Phone) ? company.Phone.Trim() : null;
                    objAddress.PhoneAreaCode = !String.IsNullOrEmpty(company.PhoneAreaCode) ? company.PhoneAreaCode.Trim() : null;
                    objAddress.FaxAreaCode = !String.IsNullOrEmpty(company.FaxAreaCode) ? company.FaxAreaCode.Trim() : null;
                    objAddress.Fax = !String.IsNullOrEmpty(company.Fax) ? company.Fax.Trim() : null;
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
            var company = new CompanyModel();
            var address = new AddressModel();

            if (id != 0)
            {
                ShowCompany CompanyModel = ObjectService.GetAll<ShowCompany>().FirstOrDefault(item => item.Id == id);
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
                    company.SecondaryASINo = CompanyModel.SecondaryASINo;
                    company.Url = CompanyModel.WebUrl;
                    ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == id);
                    if (companyAddress != null)
                    {
                        ShowAddress addressModel = ObjectService.GetAll<ShowAddress>().FirstOrDefault(item => item.Id == companyAddress.Address.Id);
                        if (addressModel != null)
                        {
                            company.AddressId = companyAddress.Address.Id;
                            company.Phone = !String.IsNullOrEmpty(addressModel.Phone) ? addressModel.Phone.Trim() : null;
                            company.PhoneAreaCode = !String.IsNullOrEmpty(addressModel.PhoneAreaCode) ? addressModel.PhoneAreaCode.Trim() : null;
                            company.Fax = !String.IsNullOrEmpty(addressModel.Fax) ? addressModel.Fax.Trim() : null;
                            company.FaxAreaCode = !String.IsNullOrEmpty(addressModel.FaxAreaCode) ? addressModel.FaxAreaCode.Trim() : null;
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
            ShowCompany company = ObjectService.GetAll<ShowCompany>().FirstOrDefault(item => item.Id == id);
            if (company != null)
            {
                int addressCount = company.CompanyAddresses.Count();
                int employeeCount = company.Employees.Count();
                int companyAttendeeCount = company.Attendees.Count();

                for (int i = addressCount - 1; i >= 0; i--)
                {
                    ObjectService.Delete(company.CompanyAddresses.ElementAt(i));
                }
                for (int i = employeeCount - 1; i >= 0; i--)
                {
                    var employeeId = company.Employees.ElementAt(i).Id;
                    IList<ShowEmployeeAttendee> employeeAttendees = ObjectService.GetAll<ShowEmployeeAttendee>().Where(item => item.EmployeeId == employeeId).ToList();
                    if (employeeAttendees.Count() > 0)
                    {
                        for (int j = employeeAttendees.Count() - 1; j >= 0; j--)
                        {
                            ObjectService.Delete<ShowEmployeeAttendee>(employeeAttendees.ElementAt(j));
                        }
                    }
                    ObjectService.Delete(company.Employees.ElementAt(i));
                }
                for (int i = companyAttendeeCount - 1; i >= 0; i--)
                {
                    ObjectService.Delete(company.Attendees.ElementAt(i));
                }
                ObjectService.Delete<ShowCompany>(company);
                ObjectService.SaveChanges();
            }
            return Redirect("CompanyList");
        }

        public ActionResult List(int id)
        {
            var companyInformation = new CompanyInformation();
            if (id != 0)
            {
                ShowCompany company = ObjectService.GetAll<ShowCompany>().FirstOrDefault(item => item.Id == id);
                companyInformation.Name = company.Name;
                companyInformation.Id = company.Id;
                IQueryable<ShowCompanyAddress> companyAddress = ObjectService.GetAll<ShowCompanyAddress>().Where(item => item.CompanyId == id);
                companyInformation.CompanyAddress = companyAddress.OrderByDescending(form => form.CreateDate).ToList();
                IQueryable<ShowEmployee> employeeList = ObjectService.GetAll<ShowEmployee>().Where(item => item.CompanyId == id);
                companyInformation.Employee = employeeList.OrderByDescending(form => form.CreateDate).ToList();
            }
            return View("../Show/Company/CompanyInformation", companyInformation);
        }

        [HttpGet]
        public ActionResult AddAddress(int companyId)
        {
            var Address = new AddressModel();
            Address.CompanyId = companyId;
            return View("../Show/Company/AddAddress", Address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult AddAddress(AddressModel address)
        {
            try
            {
                if (address.IsNonUSAddress)
                {
                    if (ModelState.ContainsKey("State")) ModelState["State"].Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    var objAddress = new ShowAddress();
                    var objCompanyAddress = new ShowCompanyAddress();
                    objAddress.Id = address.AdreessId;
                    objAddress.Phone = !String.IsNullOrEmpty(address.Phone) ? address.Phone.Trim() : null;
                    objAddress.PhoneAreaCode = !String.IsNullOrEmpty(address.PhoneAreaCode) ? address.PhoneAreaCode.Trim() : null;
                    objAddress.FaxAreaCode = !String.IsNullOrEmpty(address.FaxAreaCode) ? address.FaxAreaCode.Trim() : null;
                    objAddress.Fax = !String.IsNullOrEmpty(address.Fax) ? address.Fax.Trim() : null;
                    objAddress.Street1 = address.Address1;
                    objAddress.Street2 = address.Address2;
                    objAddress.Zip = address.Zip;
                    objAddress.State = address.IsNonUSAddress ? address.InternationalState : address.State;
                    objAddress.Country = address.IsNonUSAddress ? address.Country : "USA";
                    objAddress.City = address.City;
                    objAddress.UpdateSource = "ShowCompanyController - AddAddress";
                    objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);

                    ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == address.CompanyId && item.Address.Id == objAddress.Id);
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
                        objCompanyAddress.CompanyId = address.CompanyId;
                        objCompanyAddress.Address = objAddress;
                        objCompanyAddress.UpdateSource = "ShowCompanyController - AddAddress";
                        objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);
                    }
                    ObjectService.SaveChanges();
                    return RedirectToAction("List", "ShowCompany", new { id = address.CompanyId });
                }
                else
                {
                    return View("../Show/Company/AddAddress", address);
                }
            }
            catch (Exception ex)
            {
                messages.Add("Error: " + ex.Message);
            }
            return View("../Show/Company/AddCompany", address);
        }

        public ActionResult DeleteAddress(int id, int companyId)
        {
            ShowAddress address = ObjectService.GetAll<ShowAddress>().FirstOrDefault(item => item.Id == id);
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
                        objAddress.Phone = !String.IsNullOrEmpty(employee.Phone) ? employee.Phone.Trim() : null;
                        objAddress.PhoneAreaCode = !String.IsNullOrEmpty(employee.PhoneAreaCode) ? employee.PhoneAreaCode.Trim() : null;
                        objAddress.FaxAreaCode = !String.IsNullOrEmpty(employee.FaxAreaCode) ? employee.FaxAreaCode.Trim() : null;
                        objAddress.Fax = !String.IsNullOrEmpty(employee.Fax) ? employee.Fax.Trim() : null;
                        objAddress.Street1 = employee.Address1;
                        objAddress.Street2 = employee.Address2;
                        objAddress.Zip = employee.Zip;
                        objAddress.State = employee.IsNonUSAddress ? employee.InternationalState : employee.State;
                        objAddress.Country = employee.IsNonUSAddress ? employee.Country : "USA";
                        objAddress.City = employee.City;
                        objAddress.UpdateSource = "ShowCompanyController - Add";
                        objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);
                    }
                    var objEmployee = new ShowEmployee();
                    objEmployee.Id = employee.Id;
                    objEmployee.FirstName = employee.FirstName;
                    objEmployee.LastName = employee.LastName;
                    objEmployee.Email = employee.Email;
                    objEmployee.LoginEmail = employee.LoginEmail;
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
            var companyInfo = new CompanyInformation();
            if (id != 0)
            {
                ShowEmployee employeeModel = ObjectService.GetAll<ShowEmployee>().FirstOrDefault(item => item.Id == id);
                if (companyInfo != null)
                {
                    companyInfo.EmployeeId = id;
                    companyInfo.AdreessId = employeeModel.AddressId.HasValue ? employeeModel.AddressId.Value : 0;
                    companyInfo.FirstName = employeeModel.FirstName;
                    companyInfo.LastName = employeeModel.LastName;
                    companyInfo.Email = employeeModel.Email;
                    companyInfo.LoginEmail = employeeModel.LoginEmail;
                    companyInfo.CompanyId = employeeModel.CompanyId.HasValue ? employeeModel.CompanyId.Value : 0;
                    companyInfo.HasAddress = employeeModel.Address != null;
                    if (companyInfo.HasAddress)
                    {
                        companyInfo.IsNonUSAddress = employeeModel.Address.Country != "USA";
                        companyInfo.HasAddress = true;
                        companyInfo.Phone = !String.IsNullOrEmpty(employeeModel.Address.Phone) ? employeeModel.Address.Phone.Trim() : null;
                        companyInfo.PhoneAreaCode = !String.IsNullOrEmpty(employeeModel.Address.PhoneAreaCode) ? employeeModel.Address.PhoneAreaCode.Trim() : null;
                        companyInfo.FaxAreaCode = !String.IsNullOrEmpty(employeeModel.Address.FaxAreaCode) ? employeeModel.Address.FaxAreaCode.Trim() : null;
                        companyInfo.Fax = !String.IsNullOrEmpty(employeeModel.Address.Fax) ? employeeModel.Address.Fax.Trim() : null;
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
            var address = new AddressModel();
            if (id != 0)
            {
                ShowAddress addressModel = ObjectService.GetAll<ShowAddress>().FirstOrDefault(item => item.Id == id);
                if (address != null)
                {
                    address.AdreessId = addressModel.Id;
                    address.Phone = !String.IsNullOrEmpty(addressModel.Phone) ? addressModel.Phone.Trim() : null;
                    address.PhoneAreaCode = !String.IsNullOrEmpty(addressModel.PhoneAreaCode) ? addressModel.PhoneAreaCode.Trim() : null;
                    address.Fax = !String.IsNullOrEmpty(addressModel.Fax) ? addressModel.Fax.Trim() : null;
                    address.FaxAreaCode = !String.IsNullOrEmpty(addressModel.FaxAreaCode) ? addressModel.FaxAreaCode.Trim() : null;
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
            ShowEmployee employee = ObjectService.GetAll<ShowEmployee>().FirstOrDefault(item => item.Id == id);
            var companyId = employee.CompanyId;
            ShowAddress addresss = ObjectService.GetAll<ShowAddress>().FirstOrDefault(item => item.Id == employee.AddressId);
            ShowEmployeeAttendee employeeAttendee = ObjectService.GetAll<ShowEmployeeAttendee>().FirstOrDefault(item => item.Employee.Id == employee.Id);
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


        public ActionResult IsValidEmail(string Email)
        {
            IList<ShowEmployee> employeeList = ObjectService.GetAll<ShowEmployee>().Where(item => item.Email != null && item.Email.ToLower().Equals(Email.ToLower())).ToList();
            if (employeeList.Any())
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public ActionResult IsValidCompany(string name)
        {
            var company = new CompanyModel();
            IQueryable<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true);
            companyList = companyList.Where(item => item.Name != null
                 && item.Name.ToLower().Equals(name.ToLower()));
            company.company = companyList.ToList();
            if (company.company.Any())
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }
        [HttpGet]
        public ActionResult GetAttendeeCompany(int? showId, String companyTab, string MemberType, int page = 1, int pageSize = 20)
        {
            var showCompanies = new ShowCompaniesModel();
            showCompanies.CurrentPageIndex = page;
            showCompanies.PageSize = pageSize;
            if (string.IsNullOrEmpty(companyTab)) companyTab = ShowCompaniesModel.TAB_COMPANYNAME;
            showCompanies.CompanyTab = companyTab;
            IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>().Where(attendee => attendee.ShowId == showId).OrderBy(form => form.Company.Name).ToList();
            showCompanies.TotalRecordCount = existingAttendees.Count();
            existingAttendees = existingAttendees.Skip((showCompanies.CurrentPageIndex - 1) * showCompanies.PageSize)
                                            .Take(showCompanies.PageSize).ToList();
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
            IList<ShowEmployee> employeeList = ObjectService.GetAll<ShowEmployee>().OrderByDescending(form => form.CreateDate).ToList(); ;
            return View("../Show/Company/CompanyList", employee);
        }

        [HttpGet]
        public ActionResult GetCompanyDetailsForShow(int companyId, int showId)
        {
            ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>(true).SingleOrDefault(sa => sa.ShowId == showId && sa.CompanyId == companyId);
            IList<ShowEmployee> employees = ObjectService.GetAll<ShowEmployee>(true).Where(e => e.CompanyId == companyId).ToList();
            var showCompanies = new ShowCompaniesModel();
            if (attendee != null)
            {
                showCompanies.ShowAttendees.Add(attendee);
                showCompanies.Show = attendee.Show;
                IList<ShowEmployeeAttendee> attendees = ObjectService.GetAll<ShowEmployeeAttendee>(true).Where(sea => sea.AttendeeId == attendee.Id).ToList();
                foreach (ShowEmployee showEmployee in employees)
                {
                    var attendeeInfo = attendees.Where(a => a.EmployeeId == showEmployee.Id);
                    EmployeeAttendance employeeAttendance = AddEmployeeAttandance(showEmployee, attendeeInfo.Count() == 1, attendeeInfo.FirstOrDefault() != null ? attendeeInfo.FirstOrDefault().PriorityOrder : null);
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
                    EmployeeAttendance employeeAttendance = AddEmployeeAttandance(showEmployee, false, null);
                    showCompanies.ShowEmployees.Add(employeeAttendance);
                }
                showCompanies.Show = show;
                ShowAttendee addAttendee = new ShowAttendee();
                addAttendee.CompanyId = companyId;
                addAttendee.ShowId = showId;
                addAttendee.UpdateDate = DateTime.UtcNow;
                addAttendee.UpdateSource = "ShowCompanyController - GetCompanyDetailsForShow";
                addAttendee = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, addAttendee);
                ObjectService.SaveChanges();
            }
            return View("../Show/AddCompanyAttendeesToShow", showCompanies);
        }

        private EmployeeAttendance AddEmployeeAttandance(ShowEmployee showEmployee, bool isAttending, int? priorityOrder)
        {
            var employeeAttendance = new EmployeeAttendance();
            employeeAttendance.Employee = showEmployee;
            employeeAttendance.IsAttending = isAttending;
            employeeAttendance.PriorityOrder = priorityOrder;
            return employeeAttendance;
        }

        [HttpGet]
        public virtual ActionResult AddCompaniesToShow(int? showId, String companyTab, string companyName, string MemberType, int page = 1, int pageSize = 20)
        {
            var showCompanies = new ShowCompaniesModel();
            IList<ShowCompany> list = null;
            showCompanies.CurrentPageIndex = page;
            showCompanies.PageSize = pageSize;
            showCompanies.TabCompanyName = companyName;
            showCompanies.TabMemberType = MemberType;
            if (string.IsNullOrEmpty(companyTab)) companyTab = ShowCompaniesModel.TAB_COMPANYNAME;
            showCompanies.CompanyTab = companyTab;
            IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == showId).ToList();
            if (existingAttendees.Any())
            {
                IList<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true).ToList();
                list = companyList.Where(p => !existingAttendees.Any(p2 => p2.CompanyId == p.Id)).OrderBy(form => form.Name).ToList();
            }
            else
            {
                list = ObjectService.GetAll<ShowCompany>(true).OrderBy(form => form.Name).ToList();
            }
            if (!string.IsNullOrEmpty(MemberType))
            {
                list = list.Where(item => item.MemberType != null
                 && item.MemberType.ToLower().Contains(MemberType.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(companyName))
            {
                list = list.Where(item => item.Name != null
                 && item.Name.ToLower().Contains(companyName.ToLower())).ToList();
            }
            showCompanies.TotalRecordCount = list.Count();
            showCompanies.ShowCompanies = list.Skip((showCompanies.CurrentPageIndex - 1) * showCompanies.PageSize)
                                            .Take(showCompanies.PageSize).ToList();
            showCompanies.ShowId = showId.HasValue ? showId.Value : 0;


            return View("../Show/AddCompaniesToShow", showCompanies);
        }


        public ActionResult DeleteAttendeeCompany(int id, int showId)
        {
            ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>().FirstOrDefault(item => item.Id == id);
            if (attendee != null)
            {
                int employeeAttendeeCount = attendee.EmployeeAttendees.Count();

                for (int i = employeeAttendeeCount; i > 0; i--)
                {
                    ObjectService.Delete(attendee.EmployeeAttendees.ElementAt(i - 1));
                }
                ObjectService.Delete<ShowAttendee>(attendee);
                ObjectService.SaveChanges();
            }
            return RedirectToAction("GetAttendeeCompany", new { showId = showId });
        }
    }
}
