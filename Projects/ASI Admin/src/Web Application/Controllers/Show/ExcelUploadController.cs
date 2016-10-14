﻿using asi.asicentral.interfaces;
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
using System.Text.RegularExpressions;
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

        public ShowCompany UpdateShowCompanyData(DataTable ds, int rowId, int showId = 0)
        { 
            var show = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Id == showId);

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
                ObjectService.SaveChanges();  // need to have the new companyId later

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

                var showCompanyAddress = new ShowCompanyAddress() 
                { 
                    Address = address, 
                    Company = company, 
                    CompanyId = company.Id,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };
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

                    if( employee.Id == 0 || attendee.Id == 0 )
                        ObjectService.SaveChanges();
                
                    // update employeeAttendee
                    var employeeAttendee = attendee.EmployeeAttendees.FirstOrDefault( item => item.EmployeeId == employee.Id);
                    if (employeeAttendee == null)
                    {
                        employeeAttendee = new ShowEmployeeAttendee()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            AttendeeId = attendee.Id,
                            EmployeeId = employee.Id,
                            UpdateSource = "ExcelUploadcontroller-Index"
                        };

                        employeeAttendee.Employee = employee;
                        employeeAttendee.Attendee = attendee;

                        attendee.EmployeeAttendees.Add(employeeAttendee);
                    }
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
                log.Debug("Index - start Process the file");
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
                    file.SaveAs(currFilePath);
                    FileInfo fi = new FileInfo(currFilePath);
                    var workBook = new XLWorkbook(fi.FullName);
                    int totalsheets = workBook.Worksheets.Count;
                    var startLoop = DateTime.Now;
                    log.Debug("Index - Start main for loop for sheets");
                    for (int sheetcount = 1; sheetcount <= totalsheets; sheetcount++)
                    {
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
                            IList<ShowASI> objShows = null;
                            var objShow = new ShowASI();
                            string[] columnNameList = null;

                            foreach(var showName in _engageShows)
                            {
                                if (worksheet.Name.Contains(showName))
                                {
                                    objShow = ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains(showName)).OrderByDescending(s => s.StartDate).FirstOrDefault();
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
                            for (int i = 0; i < excelDataTable.Rows.Count; i++)
                            {
                                var memberType = excelDataTable.Rows[i]["MemberType"].ToString();
                                if (excelDataTable.Rows[i]["Company"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("Company cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                if (excelDataTable.Rows[i]["Zip Code"].ToString() == "" ) { ModelState.AddModelError("CustomError", string.Format("Zip Code cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                if (excelDataTable.Rows[i]["ASINO"].ToString() == "" ) { ModelState.AddModelError("CustomError", string.Format("ASI Number cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                if (string.IsNullOrEmpty(memberType)) { ModelState.AddModelError("CustomError", string.Format("MemberType cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                if (excelDataTable.Rows[i]["Address"].ToString() == "" ) { ModelState.AddModelError("CustomError", string.Format("Address cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                if (excelDataTable.Rows[i]["City"].ToString() == "" ) { ModelState.AddModelError("CustomError", string.Format("City cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                if (excelDataTable.Rows[i]["State"].ToString() == "" ) { ModelState.AddModelError("CustomError", string.Format("State Code cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                if (excelDataTable.Rows[i]["Country"].ToString() == "") { ModelState.AddModelError("CustomError", string.Format("Country Code cannot be empty in sheet {0} , row {1}", worksheet.Name, i)); }
                                
                                if ( memberType.Equals("Distributor", StringComparison.CurrentCultureIgnoreCase ))
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

                                UpdateShowCompanyData(excelDataTable, i, objShow.Id);
                            }
                            
                            ObjectService.SaveChanges();

                            var postAddingStart = DateTime.Now;
                            log.Debug("Index - start updating attendee data");
                            var showAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == objShow.Id).ToList();
                            var attendeesToBeDeleted = showAttendees.Where(attendee => attendee.IsExisting == false);
                            foreach (var attendee in attendeesToBeDeleted)
                            {
                                ObjectService.Delete<ShowAttendee>(attendee);
                            }
                            showAttendees.ForEach( a => a.IsExisting = false );
                            ObjectService.SaveChanges();
                            log.Debug("Index - end updating attendee data - " + (DateTime.Now - postAddingStart).TotalMilliseconds);

                        }
                        log.Debug("Index - end processing one sheet - " + (DateTime.Now - start).TotalMilliseconds);
                    }
                    log.Debug("Index - end main for loop for sheets - " + (DateTime.Now - startLoop).TotalMilliseconds);
                }

                log.Debug("Index - end process - " + (DateTime.Now - startdate).TotalMilliseconds);
                return RedirectToAction("../Show/ShowList");
            }
            else
            {
                return RedirectToAction("../Show/ShowList");
            }
        }
    }
}

