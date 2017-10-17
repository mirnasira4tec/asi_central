using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.oauth;
using asi.asicentral.services;
using asi.asicentral.services.PersonifyProxy;
using asi.asicentral.util.show;
using asi.asicentral.web.models.show;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;


namespace asi.asicentral.web.Controllers.Show
{
    public class ExcelUploadController : Controller
    {
        public IObjectService ObjectService { get; set; }
        PersonifyService personifyService = new PersonifyService();

        public ActionResult Index()
        {
            return View();
        }

        public ShowCompany UpdateShowCompanyData(DataTable ds, int rowId, int showId = 0, bool fasiliateFlag = false, List<ShowEmployeeAttendee> employeeAttendees = null)
        {
            ShowCompany company = null;
            var asinumber = ds.Rows[rowId]["ASINO"].ToString().Trim();
            var name = ds.Rows[rowId]["Company"].ToString().Trim();
            var memberType = ds.Rows[rowId]["MemberType"].ToString().Trim();
            string secondaryASINo = string.Empty;
            if (ds.Columns.Contains("Secondary ASINO"))
            {
                secondaryASINo = ds.Rows[rowId]["Secondary ASINO"].ToString().Trim();
            }
            if (fasiliateFlag == true)
            {
                var companies = ObjectService.GetAll<ShowCompany>().Where(item => (item.ASINumber == asinumber)).ToList();
                var specialCharsPattern = @"[\s,\./\\&\?;=]";
                var compName = Regex.Replace(name, specialCharsPattern, "");
                company = companies.FirstOrDefault(item => Regex.Replace(item.Name, specialCharsPattern, "").Equals(compName, StringComparison.CurrentCultureIgnoreCase) &&
                                                           item.MemberType.Equals(memberType, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                company = ObjectService.GetAll<ShowCompany>().FirstOrDefault(item => (item.ASINumber == asinumber || (item.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && item.MemberType.Equals(memberType, StringComparison.CurrentCultureIgnoreCase))));
            }
            if (company == null)
            {
                company = new ShowCompany()
                {
                    CreateDate = DateTime.UtcNow,
                };
                ObjectService.Add<ShowCompany>(company);
            }

            company.Name = name;
            company.ASINumber = asinumber;
            company.SecondaryASINo = secondaryASINo;
            company.MemberType = memberType;
            company.UpdateSource = "ExcelUploadcontroller-Index";
            company.UpdateDate = DateTime.UtcNow;

            if (company.Id == 0)
                ObjectService.SaveChanges();  // need to have the new companyId later

            #region update company Address
            ShowAddress address = null;
            if (company.CompanyAddresses != null && company.CompanyAddresses.Any())
            {
                address = company.CompanyAddresses.FirstOrDefault().Address;
            }

            if (address == null)
            {
                address = new ShowAddress()
                {
                    CreateDate = DateTime.UtcNow,
                };

                var showCompanyAddress = new ShowCompanyAddress()
                {
                    Address = address,
                    Company = company,
                    CompanyId = company.Id,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };
                company.CompanyAddresses.Add(showCompanyAddress);
            }

            address.Street1 = ds.Rows[rowId]["Address"].ToString();
            address.City = ds.Rows[rowId]["City"].ToString();
            address.State = ds.Rows[rowId]["State"].ToString();
            address.Zip = ds.Rows[rowId]["Zip Code"].ToString();
            address.Country = ds.Rows[rowId]["Country"].ToString();
            address.UpdateSource = "ExcelUploadcontroller-Index";
            address.UpdateDate = DateTime.UtcNow;
            #endregion

            #region update attendees
            var attendee = company.Attendees != null ? company.Attendees.FirstOrDefault(a => a.ShowId == showId) : null;
            if (attendee == null)
            {
                attendee = new ShowAttendee() { CreateDate = DateTime.UtcNow };
                company.Attendees.Add(attendee);
            }

            attendee.CompanyId = company.Id;
            attendee.ShowId = showId;
            if (ds.Columns.Contains("Sponsor"))
            {
                attendee.IsSponsor = Convert.ToBoolean(ds.Rows[rowId]["Sponsor"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("Presentation"))
            {
                attendee.IsPresentation = Convert.ToBoolean(ds.Rows[rowId]["Presentation"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("Roundtable"))
            {
                attendee.IsRoundTable = Convert.ToBoolean(ds.Rows[rowId]["Roundtable"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("ExhibitOnly"))
            {
                attendee.IsExhibitDay = Convert.ToBoolean(ds.Rows[rowId]["ExhibitOnly"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("IsCatalog"))
            {
                attendee.IsCatalog = Convert.ToBoolean(ds.Rows[rowId]["IsCatalog"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("BoothNumber"))
            {
                attendee.BoothNumber = ds.Rows[rowId]["BoothNumber"].ToString();
            }
            attendee.IsExisting = true;
            attendee.UpdateSource = "ExcelUploadcontroller-Index";
            attendee.UpdateDate = DateTime.UtcNow;

            #endregion
            #region update employee data for distributors or fasilitate
            if (company.MemberType == "Distributor" || fasiliateFlag)
            {
                // update showEmployee
                var firstName = ds.Rows[rowId]["FirstName"].ToString().Trim();
                var lastName = ds.Rows[rowId]["LastName"].ToString().Trim();
                string phone = string.Empty;
                string email = string.Empty;
                string loginEmail = string.Empty;
                if (ds.Columns.Contains("Phone"))
                {
                    phone = ds.Rows[rowId]["Phone"].ToString().Trim();
                }
                if (ds.Columns.Contains("Email Address"))
                {
                    email = ds.Rows[rowId]["Email Address"].ToString().Trim();
                }
                if (ds.Columns.Contains("Login Email"))
                {
                    loginEmail = ds.Rows[rowId]["Login Email"].ToString().Trim();
                }
                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    ShowEmployee employee = null;
                    if (!string.IsNullOrEmpty(email))
                    {
                        employee = company.Employees.FirstOrDefault(item => !string.IsNullOrEmpty(item.Email) && item.Email.Trim().Equals(email, StringComparison.CurrentCultureIgnoreCase));
                    }
                    if (employee == null)
                    {
                        employee = company.Employees.FirstOrDefault(item => (item.FirstName.Trim().Equals(firstName, StringComparison.CurrentCultureIgnoreCase) &&
                                                                             item.LastName.Trim().Equals(lastName, StringComparison.CurrentCultureIgnoreCase)));
                    }

                    if (employee == null)
                    {
                        employee = new ShowEmployee()
                        {
                            CreateDate = DateTime.UtcNow,
                        };
                        company.Employees.Add(employee);
                    }
                    employee.CompanyId = company.Id;
                    employee.FirstName = firstName;
                    employee.LastName = lastName;
                    employee.EPhone = phone;
                    employee.Email = email;
                    employee.LoginEmail = loginEmail;
                    employee.UpdateDate = DateTime.UtcNow;
                    employee.UpdateSource = "ExcelUploadcontroller-Index";

                    if (fasiliateFlag)
                    {
                        var street1 = ds.Rows[rowId]["Shipping Address 1"].ToString();
                        var city = ds.Rows[rowId]["Shipping City"].ToString();
                        var zip = ds.Rows[rowId]["Shipping Zip Code"].ToString();
                        var state = ds.Rows[rowId]["Shipping State"].ToString();
                        var country = ds.Rows[rowId]["Shipping Country"].ToString();
                        if (!string.IsNullOrEmpty(street1) && !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(zip) && !string.IsNullOrEmpty(state))
                        {
                            employee.Address = employee.Address ?? new ShowAddress() { CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow };
                            employee.Address.Street1 = street1;
                            employee.Address.Street2 = ds.Rows[rowId]["Shipping Address 2"].ToString();
                            employee.Address.City = city;
                            employee.Address.Zip = zip;
                            employee.Address.State = state;
                            employee.Address.Country = string.IsNullOrEmpty(country) ? "United States" : country;
                            employee.Address.UpdateSource = "ExcelUploadcontroller-Index";
                        }
                    }

                    if (employee.Id == 0 || attendee.Id == 0)
                        ObjectService.SaveChanges();

                    // update employeeAttendee
                    var employeeAttendee = attendee.EmployeeAttendees.FirstOrDefault(item => item.EmployeeId == employee.Id);
                    if (employeeAttendee == null)
                    {
                        employeeAttendee = new ShowEmployeeAttendee()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            AttendeeId = attendee.Id,
                            EmployeeId = employee.Id,
                            UpdateSource = "ExcelUploadcontroller-Index",
                        };

                        employeeAttendee.Employee = employee;
                        employeeAttendee.Attendee = attendee;

                        attendee.EmployeeAttendees.Add(employeeAttendee);
                    }
                    if (ds.Columns.Contains("HasTravelForm"))
                    {
                        employeeAttendee.HasTravelForm = Convert.ToBoolean(ds.Rows[rowId]["HasTravelForm"].ToString() == "Yes") ? true : false;
                    }
                    if (employeeAttendees != null)
                        employeeAttendees.Add(employeeAttendee);
                }
            }
            #endregion update distributor data

            return company;
        }

        public DataTable ToDictionary(List<IDictionary<string, object>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0)
                return result;

            result.Columns.AddRange(
                list.First().Select(r => new DataColumn(r.Key)).ToArray()
            );

            list.ForEach(r => result.Rows.Add(r.Select(c => c.Value).Cast<object>().ToArray()));
            return result;
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            LogService log = LogService.GetLog(this.GetType());
            var startdate = DateTime.Now;
            log.Debug("Index - start process");
            if (file != null)
            {
                try
                {
                    log.Debug("Index - start Process the file");
                    var start = DateTime.Now;
                    var objErrors = new ErrorModel();
                    var fileName = Path.GetFileName(file.FileName);
                    string tempPath = Path.GetTempPath();
                    string currFilePath = tempPath + fileName;
                    string fileExtension = Path.GetExtension(Request.Files["file"].FileName);
                    log.Debug("Index - end process the file - " + (DateTime.Now - start).TotalMilliseconds);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        if (System.IO.File.Exists(currFilePath))
                        {
                            log.Debug("Index - Delete file if exists");
                            System.IO.File.Delete(currFilePath);
                            log.Debug("Index - end Delete file if exists - " + (DateTime.Now - start));
                        }
                        file.SaveAs(currFilePath);
                        FileInfo fi = new FileInfo(currFilePath);
                        var workBook = new XLWorkbook(fi.FullName);
                        int totalsheets = workBook.Worksheets.Count;
                        var startLoop = DateTime.Now;
                        log.Debug("Index - Start main for loop for sheets");
                        for (int sheetcount = 1; sheetcount <= totalsheets; sheetcount++)
                        {
                            bool fasiliateFlag = false;
                            log.Debug("Index - start processing one sheet");
                            start = DateTime.Now;
                            var worksheet = workBook.Worksheet(sheetcount);
                            var firstRowUsed = worksheet.FirstRowUsed();
                            if (firstRowUsed != null)
                            {
                                var categoryRow = firstRowUsed.RowUsed();
                                int coCategoryId = 1;
                                Dictionary<int, string> keyValues = new Dictionary<int, string>();
                                for (int cell = 1; cell <= categoryRow.CellCount(); cell++)
                                {
                                    keyValues.Add(cell, categoryRow.Cell(cell).GetString());
                                }
                                categoryRow = categoryRow.RowBelow();
                                var objShow = new ShowASI();
                                string[] columnNameList = null;
                                if (columnNameList == null)
                                {
                                    objShow = ObjectService.GetAll<ShowASI>().Where(item => item.Name.Trim() == worksheet.Name.Trim())
                                                                               .OrderByDescending(s => s.StartDate).FirstOrDefault();
                                    if (objShow != null && (objShow.ShowTypeId == 1 || objShow.ShowTypeId == 2))
                                    {
                                        columnNameList = new string[] { "ASINO", "Company", "Sponsor", "Presentation", "Roundtable", "ExhibitOnly", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName" };
                                    }
                                    if (objShow != null && objShow.ShowTypeId == 4)
                                    {
                                        columnNameList = new string[] { "ASINO", "Company", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName", "BoothNumber" };
                                    }
                                    if (objShow != null && objShow.ShowTypeId == 5)
                                    {
                                        fasiliateFlag = true;
                                        columnNameList = new string[] { "ASINO", "MemberType", "Company", "FirstName", "LastName", "Address", "City", "State", "Zip Code", "Country", "Shipping Address 1", "Shipping Address 2", "Shipping City", "Shipping State", "Shipping Zip Code", "Shipping Country", "Phone", "Email Address" };
                                    }
                                    var matchShow = Regex.Match(worksheet.Name, @"^\s*WEEK\s+\d+\s*-\s*", RegexOptions.IgnoreCase);
                                    if (matchShow.Success)
                                    {
                                        var weekNum = matchShow.Value.Trim();
                                        var address = worksheet.Name.Substring(matchShow.Value.Length);
                                        objShow = ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains(weekNum.Replace("-", " -")) && item.Address.Contains(address.Trim()))
                                                                                 .OrderByDescending(s => s.StartDate).FirstOrDefault();
                                        columnNameList = new string[] { "ASINO", "Company", "IsCatalog", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName" };
                                    }
                                }
                                if (objShow == null)
                                {
                                    ModelState.AddModelError("CustomError", string.Format("Show {0} doesn't exist.", worksheet.Name));
                                }
                                else
                                {
                                    var containsAll = columnNameList.Where(x => keyValues.Values.Any(d => d.ToLower() == x.ToLower())).ToList();
                                    if (containsAll.Count() != columnNameList.Count())
                                    {
                                        var problematicCols = columnNameList.Where(x => keyValues.Values.FirstOrDefault(d => d.ToLower() == x.ToLower()) == null).ToList();
                                        if (problematicCols != null && problematicCols.Any())
                                        {
                                            ModelState.AddModelError("CustomError", string.Format("Columns '{0}' doesn't exist in spreadsheet {1}.", string.Join(",", problematicCols), worksheet.Name));
                                        }
                                    }
                                }

                                if (!ModelState.IsValid)
                                {
                                    objErrors.Error = string.Join(",",
                                        ModelState.Values.Where(E => E.Errors.Count > 0)
                                        .SelectMany(E => E.Errors)
                                        .Select(E => E.ErrorMessage)
                                        .ToArray());

                                    return View("../Show/ViewError", objErrors);
                                }

                                var parent = new List<IDictionary<string, object>>();
                                while (!categoryRow.Cell(coCategoryId).IsEmpty())
                                {
                                    int count = 1;
                                    var pc = new ExpandoObject();
                                    while (count <= categoryRow.CellCount())
                                    {
                                        var data = categoryRow.Cell(count).Value;
                                        ((IDictionary<string, object>)pc).Add(keyValues[count], data);
                                        count++;
                                    }

                                    categoryRow = categoryRow.RowBelow();
                                    parent.Add((IDictionary<string, object>)pc);
                                }
                                DataTable excelDataTable = ToDictionary(parent);
                                // all employeeAttendees for this event
                                var employeeAttendees = new List<ShowEmployeeAttendee>();
                                for (int i = 0; i < excelDataTable.Rows.Count; i++)
                                {
                                    var memberType = excelDataTable.Rows[i]["MemberType"].ToString();
                                    var excelRow = i + 2;
                                    if (excelDataTable.Rows[i]["Company"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("Company cannot be empty in sheet {0} , row {1}", worksheet.Name, excelRow)); }
                                    if (excelDataTable.Rows[i]["Zip Code"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("Zip Code cannot be empty in sheet {0} , row {1}", worksheet.Name, excelRow)); }
                                    if (excelDataTable.Rows[i]["ASINO"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("ASI Number cannot be empty in sheet {0} , row {1}", worksheet.Name, excelRow)); }
                                    if (string.IsNullOrEmpty(memberType)) { ModelState.AddModelError("CustomError", string.Format("MemberType cannot be empty in sheet {0} , row {1}", worksheet.Name, excelRow)); }
                                    if (excelDataTable.Rows[i]["Address"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("Address cannot be empty in sheet {0} , row {1}", worksheet.Name, excelRow)); }
                                    if (excelDataTable.Rows[i]["City"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("City cannot be empty in sheet {0} , row {1}", worksheet.Name, excelRow)); }
                                    if (excelDataTable.Rows[i]["State"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("State Code cannot be empty in sheet {0} , row {1}", worksheet.Name, excelRow)); }
                                    if (excelDataTable.Rows[i]["Country"].ToString() == "") excelDataTable.Rows[i]["Country"] = "United State";

                                    if (memberType.Equals("Distributor", StringComparison.CurrentCultureIgnoreCase) || fasiliateFlag)
                                    {
                                        if (string.IsNullOrEmpty(excelDataTable.Rows[i]["FirstName"].ToString())) { ModelState.AddModelError("CustomError", string.Format("Distributor First Name cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                        if (string.IsNullOrEmpty(excelDataTable.Rows[i]["LastName"].ToString())) { ModelState.AddModelError("CustomError", string.Format("Distributor Last Name cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                    }

                                    if (!ModelState.IsValid)
                                    {
                                        objErrors.Error = string.Join(",",
                                            ModelState.Values.Where(E => E.Errors.Count > 0)
                                            .SelectMany(E => E.Errors)
                                            .Select(E => E.ErrorMessage)
                                            .ToArray());

                                        return View("../Show/ViewError", objErrors);
                                    }

                                    UpdateShowCompanyData(excelDataTable, i, objShow.Id, fasiliateFlag, employeeAttendees);
                                }

                                ObjectService.SaveChanges();

                                // delete attendees in DB not in the excel sheet for the show
                                var postAddingStart = DateTime.Now;
                                log.Debug("Index - start updating attendee data");
                                var showAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == objShow.Id).ToList();
                                var attendeesToBeDeleted = showAttendees.Where(attendee => attendee.IsExisting == false).ToList();
                                if (attendeesToBeDeleted.Count > 0)
                                {
                                    for (var i = attendeesToBeDeleted.Count() - 1; i >= 0; i--)
                                    {
                                        var attendee = attendeesToBeDeleted[i];
                                        if (attendee.EmployeeAttendees != null && attendee.EmployeeAttendees.Any())
                                        {
                                            for (var j = attendee.EmployeeAttendees.Count() - 1; j >= 0; j--)
                                            {
                                                if (attendee.EmployeeAttendees[j].ProfileRequests.Count() > 0)
                                                {
                                                    attendee.EmployeeAttendees[j].ProfileRequests.ForEach(a => a.EmployeeAttendeeId = null);
                                                }
                                                ObjectService.Delete(attendee.EmployeeAttendees[j]);
                                            }
                                        }

                                        if (attendee.DistShowLogos != null && attendee.DistShowLogos.Any())
                                        {
                                            for (var j = attendee.DistShowLogos.Count() - 1; j >= 0; j--)
                                            {
                                                ObjectService.Delete(attendee.DistShowLogos[j]);
                                            }
                                        }
                                        if (attendee.ProfileRequests != null && attendee.ProfileRequests.Any())
                                        {
                                            attendee.ProfileRequests.ForEach(a => a.AttendeeId = null);
                                        }
                                        ObjectService.Delete<ShowAttendee>(attendee);
                                    }
                                    log.Debug(string.Format("{0} company attendees have been deleted for '{1}' after uploading", attendeesToBeDeleted.Count, objShow.Name));
                                }
                                showAttendees.ForEach(a => a.IsExisting = false);
                                ObjectService.SaveChanges();
                                // delete any employee attendees not in the sheet
                                var attendeeIds = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == objShow.Id).Select(a => a.Id).ToList();
                                var attendees = ObjectService.GetAll<ShowEmployeeAttendee>().Where(e => attendeeIds.Contains(e.AttendeeId)).ToList();
                                var countDel = 0;
                                for (var k = attendees.Count - 1; k >= 0; k--)
                                {
                                    if (employeeAttendees.FirstOrDefault(a => a.EmployeeId == attendees[k].EmployeeId) == null)
                                    { // delete employee from attendee list only, not from database
                                        countDel++;
                                        if (attendees[k].ProfileRequests.Count() > 0)
                                        {
                                            attendees[k].ProfileRequests.ForEach(a => a.EmployeeAttendeeId = null);
                                        }
                                        ObjectService.Delete(attendees[k]);
                                    }
                                }

                                if (countDel > 0)
                                {
                                    ObjectService.SaveChanges();
                                    log.Debug(string.Format("{0} employee attendees have been deleted for '{1}' after uploading", countDel, objShow.Name));
                                }

                                log.Debug("Index - end updating attendee data - " + (DateTime.Now - postAddingStart).TotalMilliseconds);

                            }
                            log.Debug("Index - end processing one sheet - " + (DateTime.Now - start).TotalMilliseconds);
                        }
                        log.Debug("Index - end main for loop for sheets - " + (DateTime.Now - startLoop).TotalMilliseconds);
                    }

                    log.Debug("Index - end process - " + (DateTime.Now - startdate).TotalMilliseconds);
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while importing the file, exception message: " + ex.Message);
                    ModelState.AddModelError("CustomError", ex.Message);
                    return View("../Show/ViewError", new ErrorModel() { Error = ex.Message });
                }
                return RedirectToAction("../Show/ShowList");
            }

            return RedirectToAction("../Show/ShowList");
        }
        [HttpGet]
        public ActionResult MigrateCompanies()
        {
            return View("~/Views/Show/ExcelUpload/MigrateCompanies.cshtml");
        }

        [HttpPost]
        public ActionResult MigrateCompanies(List<HttpPostedFileBase> files)
        {
            DataTable companiesDt = new DataTable();
            DataTable userDt = new DataTable();
            List<asi.asicentral.model.CompanyInformation> compnayInformationList = new List<asi.asicentral.model.CompanyInformation>();
            if (files.Count > 0 && files[0] != null && files[1] != null)
            {
                companiesDt = ExcelToDataTable(files[0]);
                userDt = ExcelToDataTable(files[1]);
            }
            if (companiesDt.Rows.Count > 0)
            {
                foreach (DataRow crow in companiesDt.Rows)
                {
                    compnayInformationList.Add(CreateCompany(crow));
                }
            }
            if (compnayInformationList.Count > 0)
            {
                foreach (var companyInfo in compnayInformationList)
                {
                    DataRow[] filterUsers = userDt.Select("asi=" + companyInfo.ASINumber);
                    foreach (var userInfo in filterUsers)
                    {
                        CreateUser(companyInfo, userInfo);
                    }
                }
            }
            return View();
        }

        //converts Excel sheet to datatable
        public DataTable ExcelToDataTable(HttpPostedFileBase userFile)
        {
            DataTable dt = new DataTable();
            LogService log = LogService.GetLog(this.GetType());
            if (userFile != null)
            {
                try
                {
                    log.Debug("Index - start Process the file");
                    var start = DateTime.Now;
                    var objErrors = new ErrorModel();
                    var fileName = Path.GetFileName(userFile.FileName);
                    string tempPath = Path.GetTempPath();
                    string currFilePath = tempPath + fileName;
                    string fileExtension = Path.GetExtension(userFile.FileName);
                    log.Debug("Index - end process the file - " + (DateTime.Now - start).TotalMilliseconds);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        if (System.IO.File.Exists(currFilePath))
                        {
                            log.Debug("Index - Delete file if exists");
                            System.IO.File.Delete(currFilePath);
                            log.Debug("Index - end Delete file if exists - " + (DateTime.Now - start));
                        }
                        userFile.SaveAs(currFilePath);
                        FileInfo fi = new FileInfo(currFilePath);
                        var workBook = new XLWorkbook(fi.FullName);
                        IXLWorksheet workSheet = workBook.Worksheet(1);

                        //Create a new DataTable.

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in workSheet.Rows())
                        {
                            //Use the first row to add columns to DataTable.
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                //Add rows to DataTable.
                                dt.Rows.Add();
                                int i = 0;
                                foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    i++;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while importing the file, exception message: " + ex.Message);
                }
            }
            return dt;
        }

        //creates company in personify
        private asi.asicentral.model.CompanyInformation CreateCompany(DataRow companyInforow)
        {

            var asiNo = Convert.ToString(companyInforow["ACCOUNT"]);
            var phone = Convert.ToString(companyInforow["PHONE"]) != string.Empty ? Convert.ToString(companyInforow["PHONE"]) : string.Empty;
            var phoneNo = string.Empty;
            var areaCode = string.Empty;
            if (phone != string.Empty)
            {
                List<string> phoneNumber = SeprateAreaCodeFromPhonNo(phone);
                if (phoneNumber.Count() > 0)
                {
                    phoneNo = phoneNumber[1];
                    areaCode = phoneNumber[0];
                }
            }
            asi.asicentral.model.CompanyInformation companyInfo = null;
            var companyInformation = new asi.asicentral.model.CompanyInformation
                 {
                     Name = Convert.ToString(companyInforow["COMPANY"]),
                     Phone = phoneNo + areaCode,
                     Street1 = Convert.ToString(companyInforow["ADDRESS1"]),
                     Street2 = Convert.ToString(companyInforow["ADDRESS2"]),
                     City = Convert.ToString(companyInforow["CITY"]),
                     Zip = Convert.ToString(companyInforow["ZIP"]),
                     State = Convert.ToString(companyInforow["STATE"]),
                     Country = "USA",
                     MemberType = "ASICENTRAL",
                     MemberTypeNumber = 0,
                     ASINumber = asiNo
                 };

            //create equivalent store objects
            var company = new asi.asicentral.model.store.StoreCompany
              {
                  Name = companyInformation.Name,
                  Phone = companyInformation.Phone,
                  ASINumber = asiNo,
              };
            var address = new asi.asicentral.model.store.StoreAddress
               {
                   Street1 = companyInformation.Street1,
                   Street2 = companyInformation.Street2,
                   City = companyInformation.City,
                   State = companyInformation.State,
                   Country = companyInformation.Country,
                   Zip = companyInformation.Zip
               };
            company.Addresses.Add(new asi.asicentral.model.store.StoreCompanyAddress
            {
                Address = address,
                IsBilling = true,
                IsShipping = true,
            });

            company.MemberType = companyInformation.MemberType;
            companyInfo = personifyService.GetCompanyInfoByAsiNumber(asiNo);

            if (companyInfo == null)
            {
                companyInfo = PersonifyClient.CreateCompany(company, companyInformation.MemberType, null);
                PersonifyClient.AddCustomerAddresses(company, companyInfo.MasterCustomerId, companyInfo.SubCustomerId, null);
                PersonifyClient.AddPhoneNumber(companyInformation.Phone, companyInformation.Country, companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
            }
            else
            {
                company.ExternalReference = companyInfo.MasterCustomerId + ";" + companyInfo.SubCustomerId;
                asi.asicentral.model.store.StoreAddress companyAddress = company.GetCompanyAddress();
                string countryCode = "USA";
                PersonifyClient.AddPhoneNumber(company.Phone, countryCode, companyInfo.MasterCustomerId, companyInfo.SubCustomerId);
                PersonifyClient.AddCompanyEmail(company, companyInfo);
                PersonifyClient.AddCustomerAddresses(company, companyInfo.MasterCustomerId, companyInformation.SubCustomerId, null);
            }
            return companyInfo;
        }

        //creates user
        private void CreateUser(asi.asicentral.model.CompanyInformation companyInfo, DataRow UserInfo)
        {
            LogService _log = LogService.GetLog(this.GetType());
            asi.asicentral.model.User user = new asi.asicentral.model.User();
            string ssoId = string.Empty;
            if (Convert.ToString(UserInfo["Active"])=="yes")
            {
                user.Email = string.Format(Convert.ToString(UserInfo["Email"]));
                var name = Convert.ToString(UserInfo["Name"]).Split(new[] { ' ' }, 2);
                user.FirstName = name[0];
                user.LastName = name[1];
                //Title
                user.Title = "";
                //Company
                user.CompanyName = companyInfo.Name;
                user.CompanyId = companyInfo.CompanyId;
                user.UserName = user.Email;
                //ASI Number
                user.StatusCode = StatusCode.ACTV.ToString(); ;
                user.AsiNumber = companyInfo.ASINumber;
                user.MemberType_CD = companyInfo.MemberType;
               // user.PhoneAreaCode = companyInfo.;
                user.Phone = companyInfo.Phone;
                user.Street1 = Convert.ToString(UserInfo["address"]);
                user.Street2 = "";
                user.City = Convert.ToString(UserInfo["city"]);
                user.CountryCode = "USA";
                user.Country = "USA";
                user.State = Convert.ToString(UserInfo["state"]);
                user.Zip = Convert.ToString(UserInfo["zip"]);
                user.Password = Convert.ToString(UserInfo["Password"]);

                var userInformation = ASIOAuthClient.GetUserByEmail(user.Email);
                if (userInformation == null)
                {
                    ssoId = ASIOAuthClient.CreateUserWithOutCompany(user);
                }
              
            }
        }

        private List<string> SeprateAreaCodeFromPhonNo(string phoneNo)
        {
            ILogService log = LogService.GetLog(this.GetType());
            List<string> phoneWithArea = new List<string>();
            var charsToRemove = new string[] { "(", ")", " ", "-" };
            try
            {
                foreach (var c in charsToRemove)
                {
                    phoneNo = phoneNo.Replace(c, string.Empty);
                }
                string areaCode = phoneNo.Substring(0, 3);
                string phone = phoneNo.Substring(phoneNo.Length - 7);
                phoneWithArea = new List<string>();
                phoneWithArea.Add(areaCode);
                phoneWithArea.Add(phone);
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return phoneWithArea;
        }
    }
}
