using asi.asicentral.model.show;
using asi.asicentral.services;
using asi.asicentral.util.show;
using asi.asicentral.web.Models.Show;
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
        public ActionResult Company()
        {
            return View();
        }
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
        public virtual ActionResult Add()
        {
            CompanyModel company = new CompanyModel();
            return View("../Show/Company/CompanyEdit", company);
        }

        [HttpPost]
        [ValidateInput(true)]
        public virtual ActionResult Add(CompanyModel company)
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
                return View("../Show/Company/CompanyEdit", company);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CompanyModel updateCompany = new CompanyModel();
            if (id != 0)
            {
                ShowCompany company = ObjectService.GetAll<ShowCompany>().Where(item => item.Id == id).FirstOrDefault();
                updateCompany.Name = company.Name;
            }
            return View("../Show/Company/CompanyEdit", updateCompany);
        }

        public ActionResult Delete(int id)
        {
            ShowCompany company = ObjectService.GetAll<ShowCompany>().Where(item => item.Id == id).FirstOrDefault();
            if (company != null)
            {
                ObjectService.Delete<ShowCompany>(company);
                ObjectService.SaveChanges();
            }
            IList<ShowCompany> companyList = ObjectService.GetAll<ShowCompany>(true).ToList();
            return View("../Show/Company/CompanyList", companyList);

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
        [ValidateInput(true)]
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
            return View("../Show/Company/CompanyEdit", Address);
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

    }
}
