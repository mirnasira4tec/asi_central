using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.services;
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
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace asi.asicentral.web.Controllers.Show
{
    public class ExcelUploadController : Controller
    {
        private readonly static List<string> _engageShows = new List<string>() { "ENGAGE EAST", "ENGAGE WEST"};
        private readonly static List<string> _singleShows = new List<string>() {"Orlando Show", "Dallas Show", "Long Beach Show", "New York Show", "Chicago Show"};
        private readonly static List<string> _weekShows = new List<string>() { "WEEK 1-", "WEEK 2-", "WEEK 3-", "WEEK 4-", "WEEK 5-", "WEEK 6-",
                                                                   "WEEK 7-", "WEEK 8-", "WEEK 9-", "WEEK 10-", "WEEK 11-", "WEEK 12-",};
        
        public IObjectService ObjectService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region need to be removed

        //public ShowAttendee ConvertDataAsShowAttendee(DataTable ds, int objShowId, int objCompanyId, int rowId)
        //{
        //    var objAttendee = new ShowAttendee();
        //    ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>().FirstOrDefault(item => item.ShowId == objShowId && item.CompanyId == objCompanyId);
        //    if (attendee != null)
        //    {
        //        objAttendee.Id = attendee.Id;
        //    }
        //    objAttendee.CompanyId = objCompanyId;
        //    objAttendee.ShowId = objShowId;
        //    if (ds.Columns.Contains("Sponsor"))
        //    {
        //        objAttendee.IsSponsor = Convert.ToBoolean(ds.Rows[rowId]["Sponsor"].ToString().Contains('X')) ? true : false;
        //    }
        //    if (ds.Columns.Contains("Presentation"))
        //    {
        //        objAttendee.IsPresentation = Convert.ToBoolean(ds.Rows[rowId]["Presentation"].ToString().Contains('X')) ? true : false;
        //    }
        //    if (ds.Columns.Contains("Roundtable"))
        //    {
        //        objAttendee.IsRoundTable = Convert.ToBoolean(ds.Rows[rowId]["Roundtable"].ToString().Contains('X')) ? true : false;
        //    }
        //    if (ds.Columns.Contains("ExhibitOnly"))
        //    {
        //        objAttendee.IsExhibitDay = Convert.ToBoolean(ds.Rows[rowId]["ExhibitOnly"].ToString().Contains('X')) ? true : false;
        //    }
        //    if (ds.Columns.Contains("IsCatalog"))
        //    {
        //        objAttendee.IsCatalog = Convert.ToBoolean(ds.Rows[rowId]["IsCatalog"].ToString().Contains('X')) ? true : false;
        //    }
        //    if (ds.Columns.Contains("BoothNumber"))
        //    {
        //        objAttendee.BoothNumber = ds.Rows[rowId]["BoothNumber"].ToString();
        //    }
        //    objAttendee.IsExisting = true;
        //    objAttendee.UpdateSource = "ExcelUploadcontroller-Index";
        //    objAttendee = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, objAttendee);
        //    return objAttendee;
        //}

        //public ShowEmployeeAttendee ConvertDataAsShowEmployeeAttendee(DataTable ds, int objCompanyId, int objAttendeeId, int objEmployeeId, int rowId)
        //{
        //    var objEmployeeAttendee = new ShowEmployeeAttendee();
        //    ShowAttendee objAttendee = ObjectService.GetAll<ShowAttendee>().FirstOrDefault(item => item.Id == objAttendeeId);
        //    ShowEmployee objEmployee = ObjectService.GetAll<ShowEmployee>().FirstOrDefault(item => item.Id == objEmployeeId);
        //    ShowEmployeeAttendee employeeAttendee = ObjectService.GetAll<ShowEmployeeAttendee>().FirstOrDefault(item => item.AttendeeId == objAttendeeId && item.EmployeeId == objEmployeeId);
        //    if (employeeAttendee != null)
        //    {
        //        objEmployeeAttendee.Id = employeeAttendee.Id;
        //    }
        //    objEmployeeAttendee.Employee = objEmployee;
        //    objEmployeeAttendee.AttendeeId = objAttendeeId;
        //    objEmployeeAttendee.UpdateSource = "ExcelUploadcontroller-Index";
        //    objEmployeeAttendee = ShowHelper.CreateOrUpdateEmployeeAttendee(ObjectService, objEmployeeAttendee);
        //    return objEmployeeAttendee;

        //}

        //public ShowEmployee ConvertDataAsShowEmployee(DataTable ds, int objCompanyId, int rowId)
        //{
        //    var objEmployee = new ShowEmployee();
        //    var firstName = ds.Rows[rowId]["FirstName"].ToString();
        //    var lastName = ds.Rows[rowId]["LastName"].ToString();
        //    ShowEmployee employee = ObjectService.GetAll<ShowEmployee>().FirstOrDefault(item => (item.FirstName == firstName && item.LastName == lastName && item.CompanyId == objCompanyId));
        //    if (employee != null)
        //    {
        //        objEmployee.Id = employee.Id;
        //    }
        //    objEmployee.CompanyId = objCompanyId;
        //    objEmployee.FirstName = ds.Rows[rowId]["FirstName"].ToString();
        //    objEmployee.LastName = ds.Rows[rowId]["LastName"].ToString();
        //    objEmployee.UpdateSource = "ExcelUploadcontroller-Index";
        //    objEmployee = ShowHelper.CreateOrUpdateEmployee(ObjectService, objEmployee);
        //    return objEmployee;
        //}

        //public ShowAddress ConvertDataAsShowAddress(DataTable ds, int objCompanyId, int rowId)
        //{
        //    var objAddress = new ShowAddress();
        //    ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == objCompanyId);
        //    if (companyAddress != null)
        //    {
        //        ShowAddress address = ObjectService.GetAll<ShowAddress>().FirstOrDefault(item => item.Id == companyAddress.Address.Id);
        //        if (address != null)
        //        {
        //            objAddress.Id = address.Id;
        //        }
        //    }
        //    objAddress.Street1 = ds.Rows[rowId]["Address"].ToString();
        //    objAddress.City = ds.Rows[rowId]["City"].ToString();
        //    objAddress.State = ds.Rows[rowId]["State"].ToString();
        //    objAddress.Zip = ds.Rows[rowId]["Zip Code"].ToString();
        //    objAddress.Country = ds.Rows[rowId]["Country"].ToString();
        //    objAddress.UpdateSource = "ExcelUploadcontroller-Index";
        //    objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);
        //    return objAddress;
        //}

        //public ShowCompanyAddress ConvertDataAsShowCompanyAddress(DataTable ds, int objCompanyId, int objAddressId, int rowId)
        //{
        //    var objCompanyAddress = new ShowCompanyAddress();
        //    ShowAddress objAddress = ConvertDataAsShowAddress(ds, objCompanyId, rowId);
        //    ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == objCompanyId);
        //    ShowCompanyAddress showCompanyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == objCompanyId && item.Address.Id == objAddressId);
        //    if (companyAddress != null)
        //    {
        //        objCompanyAddress.Id = showCompanyAddress.Id;
        //    }
        //    objCompanyAddress.CompanyId = objCompanyId;
        //    objCompanyAddress.Address = objAddress;
        //    objCompanyAddress.UpdateSource = "ExcelUploadcontroller-Index";
        //    objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);
        //    return showCompanyAddress;
        //}
        #endregion
        public ShowCompany UpdateShowCompanyData(DataTable ds, int rowId, int showId = 0)
        { 
            var asinumber = ds.Rows[rowId]["ASINO"].ToString();
            var name = ds.Rows[rowId]["Company"].ToString();
            var memberType = ds.Rows[rowId]["MemberType"].ToString();
            var company = ObjectService.GetAll<ShowCompany>().FirstOrDefault(item => (item.ASINumber == asinumber || (item.Name == name && item.MemberType == memberType)));
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
            company.MemberType = memberType;
            company.UpdateSource = "ExcelUploadcontroller-Index";
            company.UpdateDate = DateTime.UtcNow;  
         
            if( company.Id == 0 )
                ObjectService.SaveChanges();  // need to get the new companyId later

            #region update company Address
            ShowAddress address = null;
            if (company.CompanyAddresses != null && company.CompanyAddresses.Any())
            {
                address = company.CompanyAddresses.FirstOrDefault().Address;
            }

            if( address == null)
            {
                address = new ShowAddress()
                {
                    CreateDate = DateTime.UtcNow,
                };
                //ObjectService.Add<ShowAddress>(address);

                var showCompanyAddress = new ShowCompanyAddress() { Address = address, Company = company, CompanyId = company.Id };
                company.CompanyAddresses.Add(showCompanyAddress);
                //ObjectService.Add<ShowCompanyAddress>(showCompanyAddress);
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
            var attendee = company.Attendees != null ? company.Attendees.FirstOrDefault(a => a.ShowId == showId ) : null;
            if( attendee == null )
            {
                attendee = new ShowAttendee() { CreateDate = DateTime.UtcNow };
                company.Attendees.Add(attendee);
                //ObjectService.Add<ShowAttendee>(attendee);
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
            #region update distributor data
            if (company.MemberType == "Distributor")
            {
                // update showEmployee
                var firstName = ds.Rows[rowId]["FirstName"].ToString();
                var lastName = ds.Rows[rowId]["LastName"].ToString();
                if (string.IsNullOrEmpty(firstName) ) { ModelState.AddModelError("CustomError", "First Name cannot be empty in " + rowId + " rows."); }
                if (string.IsNullOrEmpty(lastName) ) { ModelState.AddModelError("CustomError", "Last Name cannot be empty in " + rowId + " rows."); }

                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    var employee = company.Employees.FirstOrDefault(item => (item.FirstName == firstName && item.LastName == lastName));
                    if (employee == null)
                    {
                        employee = new ShowEmployee()
                        {
                            CompanyId = company.Id,
                            FirstName = firstName,
                            LastName = lastName,
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            UpdateSource = "ExcelUploadcontroller-Index"
                        };
                        company.Employees.Add(employee);
                    }

                    ObjectService.SaveChanges();
                
                    // update employeeAttendee
                    var employeeAttendee = ObjectService.GetAll<ShowEmployeeAttendee>().FirstOrDefault(item => item.AttendeeId == attendee.Id && item.EmployeeId == employee.Id);
                    if (employeeAttendee == null)
                    {
                        employeeAttendee = new ShowEmployeeAttendee()
                        {
                            CreateDate = DateTime.UtcNow,
                            AttendeeId = attendee.Id,
                            EmployeeId = employee.Id
                        };
                        ObjectService.Add<ShowEmployeeAttendee>(employeeAttendee);
                    }
                    employeeAttendee.UpdateDate = DateTime.UtcNow;
                    employeeAttendee.Employee = employee;
                    employeeAttendee.Attendee = attendee;
                    employeeAttendee.UpdateSource = "ExcelUploadcontroller-Index";
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
                log.Debug("Index - Process the file");
                var start = DateTime.Now;
                var show = new ShowModel();
                DataSet ds = new DataSet();
                var objErrors = new ErrorModel();
                string excelConnectionString = string.Empty;
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
                    log.Debug("Index - save the file");
                    file.SaveAs(currFilePath);
                    log.Debug("Index - end save the file - " + (DateTime.Now - start));
                    log.Debug("Index - read the file");
                    FileInfo fi = new FileInfo(currFilePath);
                    var workBook = new XLWorkbook(fi.FullName);
                    log.Debug("Index - read the file - " + (DateTime.Now - start));
                    int totalsheets = workBook.Worksheets.Count;
                    int weekCount = 0;
                    log.Debug("Index - Start main for loop for sheets");
                    for (int sheetcount = 1; sheetcount <= totalsheets; sheetcount++)
                    {
                        var worksheet = workBook.Worksheet(sheetcount);
                        var firstRowUsed = worksheet.FirstRowUsed();
                        if (firstRowUsed != null)
                        {
                            var categoryRow = firstRowUsed.RowUsed();
                            int coCategoryId = 1;
                            Dictionary<int, string> keyValues = new Dictionary<int, string>();
                            log.Debug("Index - start looping for adding each cell");
                            for (int cell = 1; cell <= categoryRow.CellCount(); cell++)
                            {
                                keyValues.Add(cell, categoryRow.Cell(cell).GetString());
                            }
                            log.Debug("Index - end looping for adding each cell - " + (DateTime.Now - start));
                            categoryRow = categoryRow.RowBelow();
                            IList<ShowASI> objShows = null;
                            var objShow = new ShowASI();
                            string[] columnNameList = null;
                            log.Debug("Index - start checking for each sheets name");

                            foreach(var showName in _engageShows)
                            {
                                if (worksheet.Name.Contains(showName))
                                {
                                    objShow = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains(showName) && item.EndDate.Year == DateTime.Now.Year);
                                    columnNameList = new string[] { "ASINO", "Company", "Sponsor", "Presentation", "Roundtable", "ExhibitOnly", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName" };
                                    break;
                                }
                            }
                            if( columnNameList == null )
                            {
                                foreach (var showName in _singleShows)
                                {
                                    if (worksheet.Name.Contains(showName))
                                    {
                                        objShow = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains(showName));
                                        columnNameList = new string[] { "ASINO", "Company", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName", "BoothNumber" };
                                        break;
                                    }
                                }
                            }
                            if( columnNameList == null )
                            {
                                foreach (var showName in _weekShows)
                                {
                                    if (worksheet.Name.Contains(showName))
                                    {
                                        objShows = ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains(showName.Replace("-", " -")) && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList();
                                        columnNameList = new string[] { "ASINO", "Company", "IsCatalog", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName" };
                                        break;
                                    }
                                }
                            }

                            log.Debug("Index - end checking for each sheets name - " + (DateTime.Now - start));
                            if (objShows != null)
                            {
                                objShow = objShows[weekCount];
                                if( weekCount +1 == objShows.Count())
                                    weekCount = -1;
                            }
                            log.Debug("Index - start looping for checking column name null");
                            var containsAll = columnNameList.Where(x => keyValues.Values.Any(d => d.Contains(x))).ToList();
                            if (containsAll.Count() != columnNameList.Count())
                            {
                                if (!keyValues.ContainsValue(columnNameList.ToString()))
                                {
                                    ModelState.AddModelError("CustomError", "Please add column in spreadsheet");
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
                            }
                            log.Debug("Index - end looping for checking column name null - " + (DateTime.Now - start));
                            var parent = new List<IDictionary<string, object>>();
                            log.Debug("Index - start looping for adding each cell in dictionary");
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
                            log.Debug("Index - end looping for adding each cell in dictionary - " + (DateTime.Now - start));
                            log.Debug("Index - start converting dictionary data to data table");
                            DataTable excelDataTable = ToDictionary(parent);
                            log.Debug("Index - end converting dictionary data to data table - " + (DateTime.Now - start));

                            log.Debug("Index - start adding each row in database");
                            for (int i = 0; i < excelDataTable.Rows.Count; i++)
                            {
                                if (excelDataTable.Rows[i]["Company"].ToString() == "") { ModelState.AddModelError("CustomError", "Company cannot be empty in " + i + " rows."); }
                                if (excelDataTable.Rows[i]["Zip Code"].ToString() == "" ) { ModelState.AddModelError("CustomError", "Zip Code cannot be empty in " + i + " rows.");  }
                                if (excelDataTable.Rows[i]["ASINO"].ToString() == "" ) { ModelState.AddModelError("CustomError", "ASI Number cannot be empty in " + i + " rows."); }
                                if (excelDataTable.Rows[i]["MemberType"].ToString() == "" ) { ModelState.AddModelError("CustomError", "MemberType cannot be empty in " + i + " rows."); }
                                if (excelDataTable.Rows[i]["Address"].ToString() == "" ) { ModelState.AddModelError("CustomError", "Address cannot be empty in " + i + " rows."); }
                                if (excelDataTable.Rows[i]["City"].ToString() == "" ) { ModelState.AddModelError("CustomError", "City cannot be empty in " + i + " rows.");  }
                                if (excelDataTable.Rows[i]["State"].ToString() == "" ) { ModelState.AddModelError("CustomError", "State Code cannot be empty in " + i + " rows."); }
                                if (excelDataTable.Rows[i]["Country"].ToString() == "") { ModelState.AddModelError("CustomError", "Country Code cannot be empty in " + i + " rows."); }

                                log.Debug("Index - start adding company row in database");
                                UpdateShowCompanyData(excelDataTable, i, objShow.Id);
                                //log.Debug("Index - end adding company row in database - " + (DateTime.Now - start));
                                //log.Debug("Index - start adding address row in database");
                                //ShowAddress objAddress = ConvertDataAsShowAddress(excelDataTable, objCompany.Id, i);
                                //ShowCompanyAddress objCompanyAddress = ConvertDataAsShowCompanyAddress(excelDataTable, objCompany.Id, objAddress.Id, i);
                                //log.Debug("Index - end adding address row in database - " + (DateTime.Now - start));
                                //log.Debug("Index - start adding Attendee row in database");
                                //ShowAttendee objShowAttendee = ConvertDataAsShowAttendee(excelDataTable, objShow.Id, objCompany.Id, i);
                                //log.Debug("Index - end adding Attendee row in database - " + (DateTime.Now - start));
                                //if (objCompany.MemberType == "Distributor")
                                //{
                                //    if (excelDataTable.Rows[i]["FirstName"].ToString() == "" && isFNamePresent == false) { ModelState.AddModelError("CustomError", "First Name cannot be empty in " + i + " rows."); isFNamePresent = true; }
                                //    if (excelDataTable.Rows[i]["LastName"].ToString() == "" && isLNamePresent == false) { ModelState.AddModelError("CustomError", "Last Name cannot be empty in " + i + " rows."); isLNamePresent = true; }
                                //    log.Debug("Index - start adding Employee attendee row in database");
                                //    ShowEmployee objEmployee = ConvertDataAsShowEmployee(excelDataTable, objCompany.Id, i);
                                //    ShowEmployeeAttendee objEmployeeAttendee = ConvertDataAsShowEmployeeAttendee(excelDataTable, objCompany.Id, objShowAttendee.Id, objEmployee.Id, i);
                                //    log.Debug("Index - end adding Employee attendee row in database - " + (DateTime.Now - start));
                                //}
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
                            
                            ObjectService.SaveChanges();
                            log.Debug("Index - end adding each row in database - " + (DateTime.Now - start).TotalMilliseconds);

                            start = DateTime.Now;
                            log.Debug("Index - start updating attendee data");
                            var showAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == objShow.Id).ToList();
                            var attendeesToBeDeleted = showAttendees.Where(attendee => attendee.IsExisting == false);
                            foreach (var attendee in attendeesToBeDeleted)
                            {
                                ObjectService.Delete<ShowAttendee>(attendee);
                            }
                            showAttendees.ForEach( a => a.IsExisting = false );
                            ObjectService.SaveChanges();
                            log.Debug("Index - end updating attendee data - " + (DateTime.Now - start).TotalMilliseconds);

                        }
                        weekCount++;
                    }
                    log.Debug("Index - end main for loop for sheets - " + (DateTime.Now - start));
                }
               log.Debug("Index - end process - " + (DateTime.Now - startdate));
                return RedirectToAction("../Show/ShowList");
            }
            else
            {
                return RedirectToAction("../Show/ShowList");
            }
        }
    }
}

