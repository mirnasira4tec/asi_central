using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using asi.asicentral.web.Models.asicentral;
using asi.asicentral.web.Helpers;
using ClosedXML.Excel;
using System.Linq.Dynamic;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;

namespace asi.asicentral.web.Controllers.asicentral
{
    public class RateSupplierController : Controller
    {
        public IObjectService ObjectService { get; set; }
        // GET: RateSupplier
        [HttpGet]
        public ActionResult RateSupplierImport()
        {
            var imports = new List<RateSupplierImport>();
            imports = ObjectService.GetAll<RateSupplierImport>(true)?.ToList();
            return View("~/Views/asicentral/ratesupplier/ratesupplierimports.cshtml", imports);
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file, int? importId)
        {
            if (file == null)
            {
                TempData["ErrorMessage"] = "Please select file to upload.";
                return RedirectToAction("RateSupplierImport", "RateSupplier");
            }

            LogService log = LogService.GetLog(this.GetType());
            log.Debug("RateSupplierImport - start process");
            var startdate = DateTime.Now;
            var isFileValid = false;
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);

                string fileExtension = Path.GetExtension(fileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    isFileValid = true;
                }
            }

            if (isFileValid)
            {
                using (XLWorkbook workBook = new XLWorkbook(file.InputStream))
                {
                    var sheets = workBook.Worksheets;
                    if (sheets != null && sheets.Count > 0)
                    {
                        var userName = ControllerContext.HttpContext.User.Identity.Name;
                        RateSupplierImport rateSupplierImport = null;
                        if (!importId.HasValue)
                        {
                            rateSupplierImport = new RateSupplierImport()
                            {
                                LastUpdatedBy = userName,
                                CreateDateUTC = DateTime.Now,
                                UpdateDateUTC = DateTime.Now,
                                UpdateSource = "RateSupplierController - Import",
                                NumberOfImports = 1,
                                IsActive = true,
                                RateSupplierForms = new List<RateSupplierForm>()
                            };
                        }
                        else
                        {
                            try
                            {
                                rateSupplierImport = ObjectService.GetAll<RateSupplierImport>("RateSupplierForms.RateSupplierFormDetails").Where(m => m.RateSupplierImportId == importId.Value).FirstOrDefault();
                                if (rateSupplierImport == null)
                                {
                                    log.Debug("No existing import found for importId : " + importId.Value);
                                    TempData["ErrorMessage"] = "No existing import found for importId : " + importId.Value;
                                    return RedirectToAction("RateSupplierImport", "RateSupplier");
                                }
                                var minFormId = 0;
                                var maxFormId = 0;
                                if (rateSupplierImport.RateSupplierForms != null && rateSupplierImport.RateSupplierForms.Count > 0)
                                {
                                    var forms = rateSupplierImport.RateSupplierForms;
                                    minFormId = forms.Min(m => m.RateSupplierFormId);
                                    maxFormId = forms.Max(m => m.RateSupplierFormId); ;
                                    DeleteSupplierFormAndDetails(rateSupplierImport.RateSupplierImportId, minFormId, maxFormId);
                                }
                                rateSupplierImport.UpdateDateUTC = DateTime.UtcNow;
                                rateSupplierImport.NumberOfImports += 1;
                                ObjectService.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                log.Debug("Exception while Deleting Previous data, exception message: " + ex.Message);
                                TempData["ErrorMessage"] = "Error occured while removing previous data -'" + ex.Message;
                                return RedirectToAction("RateSupplierImport", "RateSupplier");
                            }
                        }
                        foreach (var sheet in sheets)
                        {
                            var rowCount = 0;
                            try
                            {
                                Dictionary<int, string> headings = new Dictionary<int, string>();
                                var rateSupplierForms = new List<RateSupplierForm>();
                                bool isFirstRow = true;
                                int distASIIndex = 0;
                                int distNameIndex = 0;
                                //int distFaxIndex = 0;
                                int supplierCount = 0;
                                var suplierNameColumn = "suppname";
                                var suplierASINoColumn = "supp";
                                var suplierTransactionColumn = "tran";
                                var firstRow = sheet.Row(1);
                                int cellIndex = 1;
                                while (!firstRow.Cell(cellIndex).IsEmpty())
                                {
                                    headings.Add(cellIndex, firstRow.Cell(cellIndex).GetString());
                                    cellIndex++;
                                }
                                distASIIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "distasi").Key;
                                distNameIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "distname").Key;
                                //distFaxIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "distfax").Key;

                                var lastValue = headings.LastOrDefault().Value;
                                if (lastValue.ToLower().Contains(suplierNameColumn))
                                    supplierCount = Convert.ToInt32(lastValue.Substring(suplierNameColumn.Length));
                                else if (lastValue.ToLower().Contains(suplierASINoColumn))
                                    supplierCount = Convert.ToInt32(lastValue.Substring(suplierASINoColumn.Length));
                                else if (lastValue.ToLower().Contains(suplierTransactionColumn))
                                    supplierCount = Convert.ToInt32(lastValue.Substring(suplierTransactionColumn.Length));
                                foreach (IXLRow row in sheet.Rows())
                                {
                                    rowCount++;
                                    if (isFirstRow)
                                    {
                                        isFirstRow = false;
                                        continue;
                                    }
                                    var distAsi = row.Cell(distASIIndex).GetString();
                                    if (string.IsNullOrWhiteSpace(distAsi))
                                    {
                                        break;
                                    }
                                    var rateSupplierForm = new RateSupplierForm()
                                    {
                                        DistASINum = row.Cell(distASIIndex).GetString(),
                                        DistCompanyName = row.Cell(distNameIndex).GetString(),
                                        IsDirty = false,
                                        SubmitBy = string.Empty,
                                        SubmitSuccessful = false,
                                        CreateDateUTC = startdate,
                                        UpdateDateUTC = startdate,
                                        SubmitDateUTC = null,
                                        UpdateSource = "RateSupplierController - Import",
                                        SubmitName = string.Empty
                                    };
                                    rateSupplierForm.RateSupplierFormDetails = new List<RateSupplierFormDetail>();

                                    int tranIndex, suppIndex, suppNameIndex;
                                    for (int i = 1; i <= supplierCount; i++)
                                    {
                                        var rateSupplierFormDetail = new RateSupplierFormDetail()
                                        {
                                            CreateDateUTC = startdate,
                                            UpdateDateUTC = startdate,
                                            SubmitSuccessful = false,
                                            UpdateSource = "RateSupplierController - Import"
                                        };
                                        tranIndex = headings.FirstOrDefault(x => x.Value.ToLower() == suplierTransactionColumn + i).Key;
                                        suppIndex = headings.FirstOrDefault(x => x.Value.ToLower() == suplierASINoColumn + i).Key;
                                        suppNameIndex = headings.FirstOrDefault(x => x.Value.ToLower() == suplierNameColumn + i).Key;
                                        var tran = row.Cell(tranIndex).GetString();
                                        var supp = row.Cell(suppIndex).GetString();
                                        var suppName = row.Cell(suppNameIndex).GetString();

                                        if (string.IsNullOrWhiteSpace(tran) && string.IsNullOrWhiteSpace(supp) && string.IsNullOrWhiteSpace(suppName))
                                            break;
                                        var noOfTransImport = Convert.ToInt32(tran);

                                        rateSupplierFormDetail.NumOfTransImport = noOfTransImport < 100 ? noOfTransImport : 99;
                                        rateSupplierFormDetail.NumOfTransSubmit = 0;
                                        rateSupplierFormDetail.SupASINum = supp;
                                        rateSupplierFormDetail.SupCompanyName = suppName;
                                        rateSupplierForm.RateSupplierFormDetails.Add(rateSupplierFormDetail);
                                    }
                                    rateSupplierImport.RateSupplierForms.Add(rateSupplierForm);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Debug("Exception while importing the file, exception message: " + ex.Message);
                                TempData["ErrorMessage"] = "Please verify all cells having correct data in Sheet -'" + sheet.Name + "' at Row " + rowCount + "<br/>" + ex.Message;
                                return RedirectToAction("RateSupplierImport", "RateSupplier");
                            }
                        }
                        try
                        {
                            if (rateSupplierImport.RateSupplierImportId == 0)
                            {
                                ObjectService.Add(rateSupplierImport);
                            }
                            ObjectService.SaveChanges();

                            TempData["SuccessMessage"] = "Data imported successfully";
                        }
                        catch (Exception ex)
                        {
                            log.Debug("Exception while storing the imported data of Supplier Rating Excel file, exception message: " + ex.Message);
                            TempData["ErrorMessage"] = "Error occured while saving the data -'" + ex.Message;
                            return RedirectToAction("RateSupplierImport", "RateSupplier");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "File don't have any sheets.";
                        return RedirectToAction("RateSupplierImport", "RateSupplier");
                    }
                }
            }

            log.Debug("Index - end process - " + (DateTime.Now - startdate).TotalMilliseconds);
            return RedirectToAction("RateSupplierImport", "RateSupplier");
        }

        [HttpGet]
        public ActionResult RateSupplierDistributors(string distCompanyName, string asiNumber, int? importId, int page = 1, int pageSize = 20)
        {
            var importModel = new RateSupplierImportModel();
            importModel.CurrentPageIndex = page;
            importModel.PageSize = pageSize;

            if (importId.HasValue)
            {
                importModel.ImportId = importId.Value;
                var rateSupplierDistributorList = ObjectService.GetAll<RateSupplierForm>(true)?.Where(m => m.RateSupplierImportId == importId.Value);
                if (rateSupplierDistributorList != null && rateSupplierDistributorList.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(distCompanyName))
                    {
                        rateSupplierDistributorList = rateSupplierDistributorList.Where(item => item.DistCompanyName != null
                         && item.DistCompanyName.ToLower().Contains(distCompanyName.ToLower()));
                        importModel.DistCompanyName = distCompanyName;
                    }
                    if (!string.IsNullOrEmpty(asiNumber))
                    {
                        rateSupplierDistributorList = rateSupplierDistributorList.Where(item => item.DistASINum != null && item.DistASINum == asiNumber);
                        importModel.ASINumber = asiNumber;
                    }

                    importModel.Import.RateSupplierForms = rateSupplierDistributorList.OrderBy(m => m.DistCompanyName).Skip((importModel.CurrentPageIndex - 1) * importModel.PageSize)
                                            .Take(importModel.PageSize).ToList();
                    importModel.TotalRecordCount = rateSupplierDistributorList.Count();
                }

            }
            return View("~/Views/asicentral/ratesupplier/ratesupplierdistributors.cshtml", importModel);
        }

        [HttpGet]
        public ActionResult RateSupplierRatings(int? formId)
        {
            var details = new List<RateSupplierFormDetail>();
            if (formId.HasValue)
            {
                details = ObjectService.GetAll<RateSupplierFormDetail>(true)?.Where(m => m.RateSupplierFormId == formId.Value)?.ToList();
            }
            return View("~/Views/asicentral/ratesupplier/ratesupplierratings.cshtml", details);
        }

        public ActionResult RatingSummary(int? importId, int reportType = 1)
        {
            ViewBag.ImportId = 0;
            ViewBag.ReportType = reportType;
            if (importId.HasValue)
            {
                ViewBag.ImportId = importId;
            }
            return View("~/Views/asicentral/ratesupplier/ratingsummary.cshtml");
        }
        [HttpPost]
        public ActionResult FilterRatingSummary(int? importId, int reportType)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var search = Request.Form.GetValues("search[value]").FirstOrDefault();
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();

            // get ratings for the requested type
            IQueryable<RateDistributor> forms = null;
            IQueryable<RateSupplierFormDetail> details = null;
            var distributors = GetFilterQuery(importId, reportType, search, out forms, out details);

            // get sorted ratings
            int recordsTotal = 0;                
            IQueryable result = null;
            if (distributors != null && details != null)
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                recordsTotal = details.Count();
                var sortedRatings = _getSortedRatings(details, sortColumn);
                var ratings = sortedRatings.Skip(skip).Take(pageSize).ToList();
                var summaries = new List<RateSupplierSummaryModel>();
                foreach ( var rating in ratings) 
                {
                    var dist = distributors.FirstOrDefault(d => d.RateSupplierFormId == rating.RateSupplierFormId);
                    if( dist != null)
                    {
                        var supRatings = new RateSupplierSummaryModel
                        {
                            DistASINum = dist.DistASINum,
                            DistName = dist.DistCompanyName,
                            SupASINum = rating.SupASINum,
                            SupCompanyName = rating.SupCompanyName,
                            NumOfTransImport = rating.NumOfTransImport,
                            NumOfTransSubmit = rating.NumOfTransSubmit,
                            TransDifference = Math.Abs(rating.NumOfTransImport - rating.NumOfTransSubmit),
                            OverallRating = rating.OverallRating.HasValue ? rating.OverallRating.Value : 0,
                            ProdQualityRating = rating.ProdQualityRating.HasValue ? rating.ProdQualityRating.Value : 0,
                            CommunicationRating = rating.CommunicationRating.HasValue ? rating.CommunicationRating.Value : 0,
                            DeliveryRating = rating.DeliveryRating.HasValue ? rating.DeliveryRating.Value : 0,
                            ProbResolutionRating = rating.ProbResolutionRating.HasValue ? rating.ProbResolutionRating.Value : 0,
                            ImprintingRating = rating.ImprintingRating.HasValue ? rating.ImprintingRating.Value : 0,
                        };
                        summaries.Add(supRatings);
                    }
                }
                result = summaries.AsQueryable(); 
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = result == null ? Enumerable.Empty<RateSupplierSummaryModel>().AsQueryable() : result }, JsonRequestBehavior.AllowGet);
        }

        public void DeleteSupplierFormAndDetails(int importId, int minFormId, int maxFormId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
            var deleteDetils = @"delete from USR_RateSupplierFormDetail  where RateSupplierFormId between 
                               @minFormId and @maxformId";

            var deleteForm = "delete from USR_RateSupplierForm where RateSupplierImportId=@importId";


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(deleteDetils, connection))
                {
                    command.Parameters.AddWithValue("@importId", importId);
                    command.Parameters.AddWithValue("@minFormId", minFormId);
                    command.Parameters.AddWithValue("@maxformId", maxFormId);
                    var result = command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(deleteForm, connection))
                {
                    command.Parameters.AddWithValue("@importId", importId);
                    var result = command.ExecuteNonQuery();
                }
            }
        }

        public string TestDelete(int importId)
        {
            var import = ObjectService.GetAll<RateSupplierImport>("RateSupplierForms.RateSupplierFormDetails").Where(m => m.RateSupplierImportId == importId).FirstOrDefault();
            var minFormId = 0;
            var maxFormId = 0;
            if (import != null && import.RateSupplierForms != null && import.RateSupplierForms.Count > 0)
            {
                var forms = import.RateSupplierForms;
                minFormId = forms.Min(m => m.RateSupplierFormId);
                maxFormId = forms.Max(m => m.RateSupplierFormId); ;
                DeleteSupplierFormAndDetails(import.RateSupplierImportId, minFormId, maxFormId);
            }
            return "delete completed.";
        }

        public ActionResult DownloadRateSupplierSummary(int? importId, string asiNumber, int reportType = 1)
        {
            LogService log = LogService.GetLog(this.GetType());
            try
            {                
                IQueryable<RateDistributor> forms = null;
                IQueryable<RateSupplierFormDetail> details = null;
                var distributors = GetFilterQuery(importId, reportType, asiNumber,out forms,out details);
                var csvString = new StringBuilder();
                if (details != null)
                {
                    var ratings = details.ToList();                    
                    var columnNames = string.Empty;
                    columnNames = "Distributor ASI#,Distributor Name,Supplier ASI#,Supplier Name,Trans# in System,Trans# Submit,Diff. of Trans#,Overall,Prod Quality,Communication,Delivery,Prob Resolution,Imprinting";
                    csvString.AppendLine(columnNames);
                    if (forms != null)
                    {
                        foreach (var form in forms)
                        {
                            var line = string.Empty;
                            var formRatings = ratings.Where(q => q.RateSupplierFormId == form.RateSupplierFormId).ToList();
                            foreach (var rating in formRatings)
                            {
                                line = string.Join(",", form.DistASINum, form.DistCompanyName, rating.SupASINum, rating.NumOfTransImport, rating.NumOfTransSubmit, Math.Abs(rating.NumOfTransImport - rating.NumOfTransSubmit), rating.OverallRating, rating.ProdQualityRating, rating.CommunicationRating, rating.DeliveryRating, rating.ImprintingRating);
                                csvString.AppendLine(line);
                            }
                        }
                    }
                }
                var filename = "SupplierRating-" + ((ReportType)reportType).ToString() + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day + "-" + DateTime.Today.Year + ".csv";
                return File(new System.Text.UTF8Encoding().GetBytes(csvString.ToString()), "text/csv", filename);

            }
            catch (Exception ex)
            {
                log.Debug("Error in RateSupplierController -- DownloadRateSupplierSummary , exception message: " + ex.Message);
                TempData["ErrorMessage"] = "Error occured while downloading data -'" + ex.Message;
            }
            return null;
        }

        private IQueryable<RateSupplierForm> GetFilterQuery(int? importId, int reportType, string search, out IQueryable<RateDistributor> forms,out IQueryable<RateSupplierFormDetail> details)
        {
            IQueryable<RateSupplierForm> distributors = null;
            forms = null;
            details = null;
            if (importId.HasValue)
            {
                distributors = ObjectService.GetAll<RateSupplierForm>().Where(m => m.RateSupplierImportId == importId);
                switch(reportType)
                {
                    case (int)ReportType.NotRated: distributors = distributors.Where(m => !m.IsDirty && m.SubmitDateUTC == null);
                        break;
                    case (int)ReportType.Plus50Rated:
                        distributors = distributors.Where(m => m.RateSupplierFormDetails.Any(q => q.NumOfTransSubmit > 50 && q.SubmitSuccessful));
                        break;
                    case (int)ReportType.AllRated:
                        distributors = distributors.Where(m => m.RateSupplierFormDetails.Any(q => q.SubmitSuccessful));
                        break;
                    case (int)ReportType.UserInteracted:
                        distributors = distributors.Where(m => m.IsDirty && m.SubmitDateUTC == null && !m.SubmitSuccessful && m.RateSupplierFormDetails.Where(q => q.SubmitSuccessful).Count() < 1);
                        break;
                    case (int)ReportType.Saved:
                        distributors = distributors.Where(m => m.SubmitDateUTC != null && m.RateSupplierFormDetails.Where(q => q.SubmitSuccessful).Count() < 1);
                        break;
                }
                
                if (!string.IsNullOrWhiteSpace(search))
                {
                    distributors = distributors.Where(m => m.DistASINum.Contains((search)));
                }
                forms = distributors.Select(q => new RateDistributor { RateSupplierFormId = q.RateSupplierFormId, DistASINum = q.DistASINum, DistCompanyName = q.DistCompanyName });
                var formIds = forms.Select(q => q.RateSupplierFormId);
                details = ObjectService.GetAll<RateSupplierFormDetail>().Where(m => formIds.Contains(m.RateSupplierFormId));
                if (reportType == (int)ReportType.Plus50Rated)
                {
                    details = details.Where(m => m.NumOfTransSubmit >= 50 && m.NumOfTransSubmit < 100);
                }
                if (reportType == (int)ReportType.NotRated || reportType == (int)ReportType.UserInteracted || reportType == (int)ReportType.Saved)
                {
                    details = details.Where(m => !m.SubmitSuccessful);
                }
                else
                {
                    details = details.Where(m => m.SubmitSuccessful);
                }
            }
            return distributors;
        }

        private IQueryable<RateSupplierFormDetail> _getSortedRatings(IQueryable<RateSupplierFormDetail> allRatings, string sortBy)
        {
            var sortedRatings = allRatings;
            switch (sortBy)
            {
                case "DistASINum":
                    sortedRatings = allRatings.OrderBy(d => d.RateSupplierForm.DistASINum);
                    break;
                case "DistName":
                    sortedRatings = allRatings.OrderBy(d => d.RateSupplierForm.DistCompanyName);
                    break;
                case "SupASINum":
                    sortedRatings = allRatings.OrderBy(d => d.SupASINum);
                    break;
                case "SupCompanyName":
                    sortedRatings = allRatings.OrderBy(d => d.SupCompanyName);
                    break;
                case "NumOfTransImport":
                    sortedRatings = allRatings.OrderBy(d => d.NumOfTransImport);
                    break;
                case "NumOfTransSubmit":
                    sortedRatings = allRatings.OrderBy(d => d.NumOfTransSubmit);
                    break;
                case "OverallRating":
                    sortedRatings = allRatings.OrderBy(d => d.OverallRating);
                    break;
                case "ProdQualityRating":
                    sortedRatings = allRatings.OrderBy(d => d.ProdQualityRating);
                    break;
                case "CommunicationRating":
                    sortedRatings = allRatings.OrderBy(d => d.CommunicationRating);
                    break;
                case "DeliveryRating":
                    sortedRatings = allRatings.OrderBy(d => d.DeliveryRating);
                    break;
                case "ImprintingRating":
                    sortedRatings = allRatings.OrderBy(d => d.ImprintingRating);
                    break;
                case "ProbResolutionRating":
                    sortedRatings = allRatings.OrderBy(d => d.ProbResolutionRating);
                    break;
            };

            return sortedRatings;
        }
        private class RateDistributor
        {
            public int RateSupplierFormId { get; set; }
            public string DistASINum { get; set; }
            public string DistCompanyName { get; set; }
        }

    }
    public enum ReportType
    {
        Plus50Rated = 1,
        AllRated = 2,
        NotRated = 3,
        UserInteracted = 4,
        Saved = 5
    }
}