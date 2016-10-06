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
        public IObjectService ObjectService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ShowAttendee ConvertDataAsShowAttendee(DataTable ds, int objShowId, int objCompanyId, int rowId)
        {
            var objAttendee = new ShowAttendee();
            ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>().FirstOrDefault(item => item.ShowId == objShowId && item.CompanyId == objCompanyId);
            if (attendee != null)
            {
                objAttendee.Id = attendee.Id;
            }
            objAttendee.CompanyId = objCompanyId;
            objAttendee.ShowId = objShowId;
            if (ds.Columns.Contains("Sponsor"))
            {
                objAttendee.IsSponsor = Convert.ToBoolean(ds.Rows[rowId]["Sponsor"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("Presentation"))
            {
                objAttendee.IsPresentation = Convert.ToBoolean(ds.Rows[rowId]["Presentation"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("Roundtable"))
            {
                objAttendee.IsRoundTable = Convert.ToBoolean(ds.Rows[rowId]["Roundtable"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("ExhibitOnly"))
            {
                objAttendee.IsExhibitDay = Convert.ToBoolean(ds.Rows[rowId]["ExhibitOnly"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("IsCatalog"))
            {
                objAttendee.IsCatalog = Convert.ToBoolean(ds.Rows[rowId]["IsCatalog"].ToString().Contains('X')) ? true : false;
            }
            if (ds.Columns.Contains("BoothNumber"))
            {
                objAttendee.BoothNumber = ds.Rows[rowId]["BoothNumber"].ToString();
            }
            objAttendee.IsExisting = true;
            objAttendee.UpdateSource = "ExcelUploadcontroller-Index";
            objAttendee = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, objAttendee);
            return objAttendee;
        }

        public ShowEmployeeAttendee ConvertDataAsShowEmployeeAttendee(DataTable ds, int objCompanyId, int objAttendeeId, int objEmployeeId, int rowId)
        {
            var objEmployeeAttendee = new ShowEmployeeAttendee();
            ShowAttendee objAttendee = ObjectService.GetAll<ShowAttendee>().FirstOrDefault(item => item.Id == objAttendeeId);
            ShowEmployee objEmployee = ObjectService.GetAll<ShowEmployee>().FirstOrDefault(item => item.Id == objEmployeeId);
            ShowEmployeeAttendee employeeAttendee = ObjectService.GetAll<ShowEmployeeAttendee>().FirstOrDefault(item => item.AttendeeId == objAttendeeId && item.EmployeeId == objEmployeeId);
            if (employeeAttendee != null)
            {
                objEmployeeAttendee.Id = employeeAttendee.Id;
            }
            objEmployeeAttendee.Employee = objEmployee;
            objEmployeeAttendee.AttendeeId = objAttendeeId;
            objEmployeeAttendee.UpdateSource = "ExcelUploadcontroller-Index";
            objEmployeeAttendee = ShowHelper.CreateOrUpdateEmployeeAttendee(ObjectService, objEmployeeAttendee);
            return objEmployeeAttendee;

        }

        public ShowEmployee ConvertDataAsShowEmployee(DataTable ds, int objCompanyId, int rowId)
        {
            var objEmployee = new ShowEmployee();
            var firstName = ds.Rows[rowId]["FirstName"].ToString();
            var lastName = ds.Rows[rowId]["LastName"].ToString();
            ShowEmployee employee = ObjectService.GetAll<ShowEmployee>().FirstOrDefault(item => (item.FirstName == firstName && item.LastName == lastName && item.CompanyId == objCompanyId));
            if (employee != null)
            {
                objEmployee.Id = employee.Id;
            }
            objEmployee.CompanyId = objCompanyId;
            objEmployee.FirstName = ds.Rows[rowId]["FirstName"].ToString();
            objEmployee.LastName = ds.Rows[rowId]["LastName"].ToString();
            objEmployee.UpdateSource = "ExcelUploadcontroller-Index";
            objEmployee = ShowHelper.CreateOrUpdateEmployee(ObjectService, objEmployee);
            return objEmployee;
        }

        public ShowAddress ConvertDataAsShowAddress(DataTable ds, int objCompanyId, int rowId)
        {
            var objAddress = new ShowAddress();
            ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == objCompanyId);
            if (companyAddress != null)
            {
                ShowAddress address = ObjectService.GetAll<ShowAddress>().FirstOrDefault(item => item.Id == companyAddress.Address.Id);
                if (address != null)
                {
                    objAddress.Id = address.Id;
                }
            }
            objAddress.Street1 = ds.Rows[rowId]["Address"].ToString();
            objAddress.City = ds.Rows[rowId]["City"].ToString();
            objAddress.State = ds.Rows[rowId]["State"].ToString();
            objAddress.Zip = ds.Rows[rowId]["Zip Code"].ToString();
            objAddress.Country = ds.Rows[rowId]["Country"].ToString();
            objAddress.UpdateSource = "ExcelUploadcontroller-Index";
            objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);
            return objAddress;
        }

        public ShowCompanyAddress ConvertDataAsShowCompanyAddress(DataTable ds, int objCompanyId, int objAddressId, int rowId)
        {
            var objCompanyAddress = new ShowCompanyAddress();
            ShowAddress objAddress = ConvertDataAsShowAddress(ds, objCompanyId, rowId);
            ShowCompanyAddress companyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == objCompanyId);
            ShowCompanyAddress showCompanyAddress = ObjectService.GetAll<ShowCompanyAddress>().FirstOrDefault(item => item.CompanyId == objCompanyId && item.Address.Id == objAddressId);
            if (companyAddress != null)
            {
                objCompanyAddress.Id = showCompanyAddress.Id;
            }
            objCompanyAddress.CompanyId = objCompanyId;
            objCompanyAddress.Address = objAddress;
            objCompanyAddress.UpdateSource = "ExcelUploadcontroller-Index";
            objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(ObjectService, objCompanyAddress);
            return showCompanyAddress;
        }

        public ShowCompany ConvertDataAsShowCompany(DataTable ds, int rowId)
        {
            var objCompany = new ShowCompany();
            var asinumber = ds.Rows[rowId]["ASINO"].ToString();
            var name = ds.Rows[rowId]["Company"].ToString();
            var memberType = ds.Rows[rowId]["MemberType"].ToString();
            ShowCompany company = ObjectService.GetAll<ShowCompany>().FirstOrDefault(item => (item.ASINumber == asinumber || (item.Name == name && item.MemberType == memberType)));
            if (company != null)
            {
                objCompany.Id = company.Id;
            }
            objCompany.Name = name;
            objCompany.ASINumber = asinumber;
            objCompany.MemberType = memberType;
            objCompany.UpdateSource = "ExcelUploadcontroller-Index";
            objCompany = ShowHelper.CreateOrUpdateCompany(ObjectService, objCompany);



            return objCompany;
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
                var start = DateTime.Now;
                var end = DateTime.Now;
                var show = new ShowModel();
                DataSet ds = new DataSet();
                var objErrors = new ErrorModel();
                string excelConnectionString = string.Empty;
                log.Debug("Index - get the file name");
                var fileName = Path.GetFileName(file.FileName);

                string tempPath = Path.GetTempPath();
                string currFilePath = tempPath + fileName;
                string fileExtension = Path.GetExtension(Request.Files["file"].FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    if (System.IO.File.Exists(currFilePath))
                    {
                        System.IO.File.Delete(currFilePath);
                    }
                    file.SaveAs(currFilePath);
                    FileInfo fi = new FileInfo(currFilePath);
                    var workBook = new XLWorkbook(fi.FullName);
                    log.Debug("Index - end get the file name - " + (DateTime.Now - start).TotalMilliseconds);
                    int totalsheets = workBook.Worksheets.Count;
                    int weekCount = 0;
                    log.Debug("Index - Start main for loop for sheets");
                    for (int sheetcount = 1; sheetcount <= totalsheets; sheetcount++)
                    {
                        using (var worksheet = workBook.Worksheet(sheetcount))
                        {
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
                                end = DateTime.Now;
                                log.Debug("Index - end looping for adding each cell - " + (end - start));
                                categoryRow = categoryRow.RowBelow();
                                #region checking for each sheets name
                                IList<ShowASI> objShows = null;
                                var objShow = new ShowASI();
                                string[] columnNameList = null;
                                log.Debug("Index - start checking for each sheets name");
                                Dictionary<string, IList<ShowASI>> dictionaryRoadshow = new Dictionary<string, IList<ShowASI>>();
                                dictionaryRoadshow.Add("WEEK 1-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 1 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 2-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 2 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 3-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 3 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 4-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 4 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 5-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 5 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 6-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 6 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 7-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 7 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 8-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 8 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 9-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 9 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 10-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 10 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 11-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 11 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                dictionaryRoadshow.Add("WEEK 12-", ObjectService.GetAll<ShowASI>().Where(item => item.Name.Contains("WEEK 12 -") && item.EndDate.Year == DateTime.Now.Year).OrderBy(item => item.EndDate).ToList());
                                foreach (KeyValuePair<string, IList<ShowASI>> pair in dictionaryRoadshow)
                                {
                                    if (worksheet.Name.Contains(pair.Key))
                                    {
                                        objShows = pair.Value;
                                        columnNameList = new string[] { "ASINO", "Company", "Sponsor", "Presentation", "Roundtable", "ExhibitOnly", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName" };
                                    }

                                }
                                Dictionary<string, ShowASI> dictionaryShowEngage = new Dictionary<string, ShowASI>();
                                dictionaryShowEngage.Add("ENGAGE EAST", ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("ENGAGE EAST") && item.EndDate.Year == DateTime.Now.Year));
                                dictionaryShowEngage.Add("ENGAGE WEST", ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("ENGAGE WEST") && item.EndDate.Year == DateTime.Now.Year));
                                dictionaryShowEngage.Add("Orlando Show", ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("Orlando Show")));
                                dictionaryShowEngage.Add("Dallas Show", ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("Dallas Show")));
                                dictionaryShowEngage.Add("Long Beach Show", ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("Long Beach Show")));
                                dictionaryShowEngage.Add("New York Show", ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("New York Show")));
                                dictionaryShowEngage.Add("Chicago Show", ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("Chicago Show")));
                                foreach (KeyValuePair<string, ShowASI> pair in dictionaryShowEngage)
                                {
                                    if (worksheet.Name.Contains(pair.Key))
                                    {
                                        objShow = pair.Value;
                                        if (pair.Key == "Engage")
                                            columnNameList = new string[] { "ASINO", "Company", "Sponsor", "Presentation", "Roundtable", "ExhibitOnly", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName" };
                                        else if (pair.Key == "Show")
                                            columnNameList = new string[] { "ASINO", "Company", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName", "BoothNumber" };
                                    }

                                }
                                log.Debug("Index - end checking for each sheets name - " + (DateTime.Now - start));
                                #endregion
                                if (objShows != null)
                                {
                                    objShow = objShows[weekCount];
                                    if (weekCount + 1 == objShows.Count())
                                        weekCount = -1;
                                }
                                #region checking column name is null or not 
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
                                #endregion
                                #region adding each cell in dictionary
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
                                #endregion
                                log.Debug("Index - start converting dictionary data to data table");
                                DataTable excelDataTable = ToDictionary(parent);
                                log.Debug("Index - end converting dictionary data to data table - " + (DateTime.Now - start));
                                #region adding each row in database
                                bool isZipPresent, isCompanyPresent, isAsiNoPresent, isMemberTypePresent, isAddressPresent, isCityPresent, isStatePresent, isCountryPresent, isFNamePresent, isLNamePresent;
                                isZipPresent = isCompanyPresent = isAsiNoPresent = isMemberTypePresent = isAddressPresent = isCityPresent = isStatePresent = isCountryPresent = isFNamePresent = isLNamePresent = false;
                                log.Debug("Index - start adding each row in database");
                                for (int i = 0; i < excelDataTable.Rows.Count; i++)
                                {
                                    if (excelDataTable.Rows[i]["Company"].ToString() == "" && isCompanyPresent == false) { ModelState.AddModelError("CustomError", "Company cannot be empty in " + i + " rows."); isCompanyPresent = true; }
                                    if (excelDataTable.Rows[i]["Zip Code"].ToString() == "" && isZipPresent == false) { ModelState.AddModelError("CustomError", "Zip Code cannot be empty in " + i + " rows."); isZipPresent = true; }
                                    if (excelDataTable.Rows[i]["ASINO"].ToString() == "" && isAsiNoPresent == false) { ModelState.AddModelError("CustomError", "ASI Number cannot be empty in " + i + " rows."); isAsiNoPresent = true; }
                                    if (excelDataTable.Rows[i]["MemberType"].ToString() == "" && isMemberTypePresent == false) { ModelState.AddModelError("CustomError", "MemberType cannot be empty in " + i + " rows."); isMemberTypePresent = true; }
                                    if (excelDataTable.Rows[i]["Address"].ToString() == "" && isAddressPresent == false) { ModelState.AddModelError("CustomError", "Address cannot be empty in " + i + " rows."); isAddressPresent = true; }
                                    if (excelDataTable.Rows[i]["City"].ToString() == "" && isCityPresent == false) { ModelState.AddModelError("CustomError", "City cannot be empty in " + i + " rows."); isCityPresent = true; }
                                    if (excelDataTable.Rows[i]["State"].ToString() == "" && isStatePresent == false) { ModelState.AddModelError("CustomError", "State Code cannot be empty in " + i + " rows."); isStatePresent = true; }
                                    if (excelDataTable.Rows[i]["Country"].ToString() == "" && isCountryPresent == false) { ModelState.AddModelError("CustomError", "Country Code cannot be empty in " + i + " rows."); isCountryPresent = true; }

                                    log.Debug("Index - start adding company row in database");
                                    ShowCompany objCompany = ConvertDataAsShowCompany(excelDataTable, i);
                                    log.Debug("Index - end adding company row in database - " + (DateTime.Now - start));
                                    log.Debug("Index - start adding address row in database");
                                    ShowAddress objAddress = ConvertDataAsShowAddress(excelDataTable, objCompany.Id, i);
                                    ShowCompanyAddress objCompanyAddress = ConvertDataAsShowCompanyAddress(excelDataTable, objCompany.Id, objAddress.Id, i);
                                    log.Debug("Index - end adding address row in database - " + (DateTime.Now - start));
                                    log.Debug("Index - start adding Attendee row in database");
                                    ShowAttendee objShowAttendee = ConvertDataAsShowAttendee(excelDataTable, objShow.Id, objCompany.Id, i);
                                    log.Debug("Index - end adding Attendee row in database - " + (DateTime.Now - start));
                                    if (objCompany.MemberType == "Distributor")
                                    {
                                        if (excelDataTable.Rows[i]["FirstName"].ToString() == "" && isFNamePresent == false) { ModelState.AddModelError("CustomError", "First Name cannot be empty in " + i + " rows."); isFNamePresent = true; }
                                        if (excelDataTable.Rows[i]["LastName"].ToString() == "" && isLNamePresent == false) { ModelState.AddModelError("CustomError", "Last Name cannot be empty in " + i + " rows."); isLNamePresent = true; }
                                        log.Debug("Index - start adding Employee attendee row in database");
                                        ShowEmployee objEmployee = ConvertDataAsShowEmployee(excelDataTable, objCompany.Id, i);
                                        ShowEmployeeAttendee objEmployeeAttendee = ConvertDataAsShowEmployeeAttendee(excelDataTable, objCompany.Id, objShowAttendee.Id, objEmployee.Id, i);
                                        log.Debug("Index - end adding Employee attendee row in database - " + (DateTime.Now - start));
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
                                ObjectService.SaveChanges();
                                log.Debug("Index - end adding each row in database - " + (DateTime.Now - start));
                                #endregion

                                # region delete the data from attendee table for isExisitng = false
                                log.Debug("Index - start getting attendee data for delete");
                                IList<ShowAttendee> deleteAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.IsExisting == false && item.ShowId == objShow.Id).ToList();
                                log.Debug("Index - end getting attendee data for delete - " + (DateTime.Now - start));
                                log.Debug("Index - start looping for deleting Attendee data");
                                if (deleteAttendees != null)
                                {
                                    foreach (var deleteAttendee in deleteAttendees)
                                    {
                                        ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>().FirstOrDefault(item => item.Id == deleteAttendee.Id);
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
                                    }
                                }
                                log.Debug("Index - end looping for deleting Attendee data - " + (DateTime.Now - start));
                                #endregion

                                #region  setting IsExisting = false for all data 
                                log.Debug("Index - start getting existing attendee list");
                                IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == objShow.Id).ToList();
                                log.Debug("Index - end getting existing attendee list - " + (DateTime.Now - start));
                                log.Debug("Index - start looping for setting isExisting false");
                                if (existingAttendees != null)
                                {
                                    foreach (var existingAttendee in existingAttendees)
                                    {
                                        var objexistingAttendee = new ShowAttendee();
                                        objexistingAttendee.Id = existingAttendee.Id;
                                        objexistingAttendee.CompanyId = existingAttendee.CompanyId;
                                        objexistingAttendee.ShowId = existingAttendee.ShowId;
                                        objexistingAttendee.IsSponsor = existingAttendee.IsSponsor;
                                        objexistingAttendee.IsRoundTable = existingAttendee.IsRoundTable;
                                        objexistingAttendee.IsExhibitDay = existingAttendee.IsExhibitDay;
                                        objexistingAttendee.IsPresentation = existingAttendee.IsPresentation;
                                        objexistingAttendee.IsCatalog = existingAttendee.IsCatalog;
                                        objexistingAttendee.BoothNumber = existingAttendee.BoothNumber;
                                        objexistingAttendee.IsExisting = false;
                                        objexistingAttendee = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, objexistingAttendee);
                                        ObjectService.SaveChanges();
                                    }
                                }
                                log.Debug("Index - end looping for setting isExisting false - " + (DateTime.Now - start));
                                #endregion
                            }
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

