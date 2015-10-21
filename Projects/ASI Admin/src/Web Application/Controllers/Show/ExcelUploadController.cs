using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.services;
using asi.asicentral.util.show;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
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

        public string stringConn()
        {
            string excelConnectionString = string.Empty;
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);

                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {

                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                }
            }
            return excelConnectionString;
        }

        public ShowAttendee ConvertDataAsShowAttendee(DataSet ds, int objShowId, int objCompanyId, int rowId)
        {
            var objAttendee = new ShowAttendee();
            ShowAttendee attendee = ObjectService.GetAll<ShowAttendee>().FirstOrDefault(item => item.ShowId == objShowId && item.CompanyId == objCompanyId);
            if (attendee != null)
            {
                objAttendee.Id = attendee.Id;
            }
            objAttendee.CompanyId = objCompanyId;
            objAttendee.ShowId = objShowId;
            objAttendee.IsSponsor = Convert.ToBoolean(ds.Tables[0].Rows[rowId]["Sponsor"].ToString().Contains('X')) ? true : false;
            objAttendee.IsPresentation = Convert.ToBoolean(ds.Tables[0].Rows[rowId]["Presentation"].ToString().Contains('X')) ? true : false;
            objAttendee.IsRoundTable = Convert.ToBoolean(ds.Tables[0].Rows[rowId]["Roundtable"].ToString().Contains('X')) ? true : false;
            objAttendee.IsExhibitDay = Convert.ToBoolean(ds.Tables[0].Rows[rowId]["Exhibit Only"].ToString().Contains('X')) ? true : false;
            objAttendee.IsExisting = true;
            objAttendee.UpdateSource = "ExcelUploadcontroller-Index";
            objAttendee = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, objAttendee);

            return objAttendee;
        }

        public ShowASI ConvertDataAsShow(DataSet ds)
        {
            var objShow = new ShowASI();
            if (ds.Tables[0].Columns.Contains("ENGAGE EAST 2016"))
            {
                objShow = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("ENGAGE EAST"));
            }
            else if (ds.Tables[0].Columns.Contains("ENGAGE WEST 2016"))
            {
                objShow = ObjectService.GetAll<ShowASI>().FirstOrDefault(item => item.Name.Contains("ENGAGE WEST"));
            }
            return objShow;
        }

        public ShowAddress ConvertDataAsShowAddress(DataSet ds, int objCompanyId, int rowId)
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
            objAddress.Street1 = ds.Tables[0].Rows[rowId]["Address"].ToString();
            objAddress.City = ds.Tables[0].Rows[rowId]["City"].ToString();
            objAddress.State = ds.Tables[0].Rows[rowId]["State"].ToString();
            objAddress.Zip = ds.Tables[0].Rows[rowId]["Zip Code"].ToString();
            objAddress.Country = ds.Tables[0].Rows[rowId]["Country"].ToString();
            objAddress.UpdateSource = "ExcelUploadcontroller-Index";
            objAddress = ShowHelper.CreateOrUpdateAddress(ObjectService, objAddress);
            return objAddress;
        }

        public ShowCompanyAddress ConvertDataAsShowCompanyAddress(DataSet ds, int objCompanyId, int objAddressId, int rowId)
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

        public ShowCompany ConvertDataAsShowCompany(DataSet ds, int rowId)
        {
            var objCompany = new ShowCompany();
            var name = string.Empty;
            ShowASI objShow = ConvertDataAsShow(ds);
            if (ds.Tables[0].Columns.Contains("ENGAGE EAST 2016"))
            {
                name = ds.Tables[0].Rows[rowId]["ENGAGE EAST 2016"].ToString();
            }
            else if (ds.Tables[0].Columns.Contains("ENGAGE WEST 2016"))
            {
                name = ds.Tables[0].Rows[rowId]["ENGAGE WEST 2016"].ToString();
            }

            //IList<ShowAttendee> attendeelist = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == objShow.Id).ToList();

            var asinumber = ds.Tables[0].Rows[rowId]["ASINO"].ToString();


            ShowCompany company = ObjectService.GetAll<ShowCompany>().FirstOrDefault(item => (item.ASINumber == asinumber || item.Name == name));
            if (company != null)
            {
                objCompany.Id = company.Id;
            }
            objCompany.Name = name;
            objCompany.ASINumber = asinumber;
            objCompany.UpdateSource = "ExcelUploadcontroller-Index";
            objCompany = ShowHelper.CreateOrUpdateCompany(ObjectService, objCompany);

            return objCompany;
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            string conn = stringConn();
            //Create Connection to Excel work book and add oledb namespace
            OleDbConnection excelConnection = new OleDbConnection(conn);
            excelConnection.Open();
            DataTable dt = new DataTable();

            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }

            String[] excelSheets = new String[dt.Rows.Count];
            int t = 0;
            //excel data saves in temp file here.
            foreach (DataRow row in dt.Rows)
            {
                excelSheets[t] = row["TABLE_NAME"].ToString();
                OleDbConnection excelConnection1 = new OleDbConnection(conn);
                string query = string.Format("Select * from [{0}]", excelSheets[t]);
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                {
                    dataAdapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ShowASI objShow = ConvertDataAsShow(ds);
                        ShowCompany objCompany = ConvertDataAsShowCompany(ds, i);
                        ShowAddress objAddress = ConvertDataAsShowAddress(ds, objCompany.Id, i);
                        ShowCompanyAddress objCompanyAddress = ConvertDataAsShowCompanyAddress(ds, objCompany.Id, objAddress.Id, i);
                        ShowAttendee objShowAttendee = ConvertDataAsShowAttendee(ds, objShow.Id, objCompany.Id, i);
                        ObjectService.SaveChanges();
                    }
                     var obj =  ConvertDataAsShow(ds);

                    IList<ShowAttendee> deleteAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.IsExisting == false && item.ShowId == obj.Id).ToList();
                    if (deleteAttendees != null)
                    {
                        foreach (var deleteAttendee in deleteAttendees)
                        {
                            ObjectService.Delete<ShowAttendee>(deleteAttendee);
                        }
                        ObjectService.SaveChanges();
                    }

                    IList<ShowAttendee> existingAttendees = ObjectService.GetAll<ShowAttendee>().Where(item => item.ShowId == obj.Id).ToList();
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
                            objexistingAttendee.IsExhibitDay = false;
                            objexistingAttendee = ShowHelper.CreateOrUpdateShowAttendee(ObjectService, objexistingAttendee);
                            ObjectService.SaveChanges();
                        }
                    }
                    ds.Reset();
                }
                t++;
            }
            return RedirectToAction("../Show/ShowList");
        }
    }
}

