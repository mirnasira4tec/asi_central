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
            objAttendee.IsSponsor = Convert.ToBoolean(ds.Rows[rowId]["Sponsor"].ToString().Contains('X')) ? true : false;
            objAttendee.IsPresentation = Convert.ToBoolean(ds.Rows[rowId]["Presentation"].ToString().Contains('X')) ? true : false;
            objAttendee.IsRoundTable = Convert.ToBoolean(ds.Rows[rowId]["Roundtable"].ToString().Contains('X')) ? true : false;
            objAttendee.IsExhibitDay = Convert.ToBoolean(ds.Rows[rowId]["ExhibitOnly"].ToString().Contains('X')) ? true : false;
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
            if (file != null)
            {
                var show = new ShowModel();
                DataSet ds = new DataSet();
                string excelConnectionString = string.Empty;
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
                    int totalsheets = workBook.Worksheets.Count;
                    for (int sheetcount = 1; sheetcount <= totalsheets; sheetcount++)
                    {
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
                            if (worksheet.Name.Contains("ENGAGE EAST 2016"))
                            {
                                objShow = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("ENGAGE EAST"));
                            }
                            else if (worksheet.Name.Contains("ENGAGE WEST 2016"))
                            {
                                objShow = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("ENGAGE WEST"));
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
                                ShowCompany objCompany = ConvertDataAsShowCompany(excelDataTable, i);
                                ShowAddress objAddress = ConvertDataAsShowAddress(excelDataTable, objCompany.Id, i);
                                ShowCompanyAddress objCompanyAddress = ConvertDataAsShowCompanyAddress(excelDataTable, objCompany.Id, objAddress.Id, i);
                                ShowAttendee objShowAttendee = ConvertDataAsShowAttendee(excelDataTable, objShow.Id, objCompany.Id, i);
                                if (objCompany.MemberType == "Distributor")
                                {
                                    ShowEmployee objEmployee = ConvertDataAsShowEmployee(excelDataTable, objCompany.Id, i);
                                    ShowEmployeeAttendee objEmployeeAttendee = ConvertDataAsShowEmployeeAttendee(excelDataTable, objCompany.Id, objShowAttendee.Id, objEmployee.Id, i);
                                }
                                ObjectService.SaveChanges();

                            }
                            IList<ShowAttendee> deleteAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.IsExisting == false && item.ShowId == objShow.Id).ToList();
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
                                ObjectService.SaveChanges();
                            }

                            IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == objShow.Id).ToList();
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
                                    objexistingAttendee.IsExisting = false;
                                    objexistingAttendee = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, objexistingAttendee);
                                    ObjectService.SaveChanges();
                                }
                            }
                        }
                    }
                }
                return RedirectToAction("../Show/ShowList");
            }
            else
            {
                return RedirectToAction("../Show/ShowList");
            }
        }
    }
}

