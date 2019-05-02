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
            var sheets = UploadHelper.GetExcelSheets(file);

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
                    rateSupplierImport.RateSupplierForms = new List<RateSupplierForm>();
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

        public ActionResult RatingSummary(int? importId, int reportType=1)
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
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var distributors = ObjectService.GetAll<RateSupplierForm>().Where(m => m.RateSupplierImportId == importId);
            if (reportType == (int)ReportType.NotRated)
            {
                distributors = distributors.Where(m => !m.SubmitSuccessful);
            }
            else if (reportType == (int)ReportType.Plus50Rated)
            {
                distributors = distributors.Where(m => m.SubmitSuccessful);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                distributors = distributors.Where(m => m.DistASINum.Contains((search)));
            }
            var forms = distributors.Select(q => new RateDistributor { RateSupplierFormId = q.RateSupplierFormId, DistASINum = q.DistASINum, DistCompanyName = q.DistCompanyName });
            var formIds = forms.Select(q => q.RateSupplierFormId);
            var details = ObjectService.GetAll<RateSupplierFormDetail>().Where(m => formIds.Contains(m.RateSupplierFormId));
            if (reportType == (int)ReportType.Plus50Rated)
            {
                details = details.Where(m=> m.NumOfTransSubmit >= 50 && m.NumOfTransSubmit < 100);
            }
            if (reportType == (int)ReportType.NotRated)
            {
                details = details.Where(m => !m.SubmitSuccessful);
            }
            else
            {
                details = details.Where(m => m.SubmitSuccessful);
            }
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            recordsTotal = (from dist in distributors
                            join rating in details
                         on dist.RateSupplierFormId equals rating.RateSupplierFormId
                            select 1).Count();

            var summaries = (from dist in forms
                             join rating in details
                             on dist.RateSupplierFormId equals rating.RateSupplierFormId
                             select new RateSupplierSummaryModel
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
                             }).OrderBy(sortColumn + " " + sortColumnDir).Skip(skip).Take(pageSize);
            IQueryable result = null;
            result = summaries.AsQueryable();
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
    }
}