using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using asi.asicentral.util;
using asi.asicentral.web.Helpers;
using asi.asicentral.web.Models.asicentral;
using ClosedXML.Excel;
namespace asi.asicentral.web.Controllers.asicentral
{
    public class CatalogController : Controller
    {
        public IObjectService ObjectService { get; set; }
        public IEmailService EmailService { get; set; }
        public ITemplateService TemplateService { get; set; }
        public ActionResult Index()
        {
            return View("~/Views/asicentral/Catalog/Index.cshtml");
        }

        #region Excel Import
        public ActionResult CatalogContactImport()
        {
            CatalogContactImportModel importModel = new CatalogContactImportModel();
            var imports = ObjectService.GetAll<CatalogContactImport>()?.OrderByDescending(m => m.CreateDateUTC);
            var industries = imports.Select(m => m.IndustryName).Distinct();
            importModel.Industries = new List<SelectListItem>();
            importModel.Industries.Add(new SelectListItem { Value = "", Text = "--Select--" });
            if (industries != null && industries.Count() > 0)
            {
                foreach (var inds in industries)
                {
                    if (!string.IsNullOrWhiteSpace(inds))
                    {
                        importModel.Industries.Add(new SelectListItem { Value = inds, Text = inds });
                    }
                }
            }
            importModel.Industries.Add(new SelectListItem { Value = "other", Text = "Other" });
            importModel.Imports = imports.ToList();
            importModel.contactSalesInfo = new Dictionary<int, KeyValuePair<bool, int>>();
            foreach (var import in importModel.Imports)
            {
                var reservedContact = ObjectService.GetAll<CatalogContactSaleDetail>()
                                         .Where(
                                            m => m.CatalogContacts.CatalogContactImport.CatalogContactImportId == import.CatalogContactImportId
                                         && !m.CatalogContactSale.IsCancelled).Count();
                var isInSales = false;
                int saleApproveStatus = 0; //no sale exists
                if (reservedContact > 0)
                {
                    isInSales = true;
                }
                var pendingCount = ObjectService.GetAll<CatalogContactSaleDetail>()
                                         .Where(m => m.CatalogContacts.CatalogContactImport.CatalogContactImportId == import.CatalogContactImportId
                                                 && !m.CatalogContactSale.IsCancelled)
                                         .SelectMany(m => m.CatalogContacts.CatalogContactSaleDetails.Where(c => !c.CatalogContactSale.IsApproved)).Count();
                if (pendingCount > 0)
                {
                    saleApproveStatus = 2;//sale Unapproved;
                }
                else
                {
                    saleApproveStatus = 1;// sale Approved
                }
                importModel.contactSalesInfo.Add(import.CatalogContactImportId, new KeyValuePair<bool, int>(isInSales, saleApproveStatus));
            }
            return View("~/Views/asicentral/Catalog/CatalogContactImport.cshtml", importModel);
        }

        [HttpPost]
        public ActionResult CatalogContactImport(HttpPostedFileBase file, string Industry, int? importId)
        {
            if (file == null || string.IsNullOrWhiteSpace(Industry))
            {
                TempData["ErrorMessage"] = "Please select file and Industry to upload.";
                return RedirectToAction("CatalogContactImport", "Catalog");
            }
            LogService log = LogService.GetLog(this.GetType());
            log.Debug("CatalogContactsImport - start process");
            var startdate = DateTime.Now;
            IQueryable<CatalogContactSaleDetail> contactInSale = null;
            var sheets = UploadHelper.GetExcelSheets(file);
            IXLWorksheet sheet = null;
            var catalogName = string.Empty;
            if (sheets != null && sheets.Count > 0)
            {
                var userName = ControllerContext.HttpContext.User.Identity.Name;
                sheet = sheets.First();
                catalogName = sheet.Name;
                CatalogContactImport catalogContactImport = null;
                var rowCount = 0;
                var isFirstRow = true;
                var isOtherIdustry = false;
                try
                {
                    #region Add or Update Import
                    if (!importId.HasValue)
                    {
                        DisableImportByIndustry(Industry);
                        catalogContactImport = new CatalogContactImport()
                        {
                            CatalogName = catalogName,
                            ImportedBy = userName,
                            IsActive = true,
                            IndustryName = Industry,
                            CreateDateUTC = DateTime.UtcNow,
                            UpdateDateUTC = DateTime.UtcNow,
                            UpdateSource = "CatalogController - Import",
                            CatalogContacts = new List<CatalogContact>()
                        };
                    }
                    else
                    {
                        catalogContactImport = ObjectService.GetAll<CatalogContactImport>().Where(m => m.CatalogContactImportId == importId.Value).FirstOrDefault();
                        if (catalogContactImport == null)
                        {
                            log.Debug("No existing import found for importId : " + importId.Value);
                            TempData["ErrorMessage"] = "No existing import found for importId : " + importId.Value;
                            return RedirectToAction("CatalogContactImport", "Catalog");
                        }

                        return CatalogContactUpdate(catalogContactImport, sheet); 
                    }
                    #endregion

                    #region Reading Contacts From Excel
                    Dictionary<int, string> headings = new Dictionary<int, string>();
                    var firstRow = sheet.Row(1);
                    int cellIndex = 1;
                    while (!firstRow.Cell(cellIndex).IsEmpty())
                    {
                        headings.Add(cellIndex, firstRow.Cell(cellIndex).GetString());
                        cellIndex++;
                    }

                    var industryColIndex = 0;
                    var stateColIndex = 0;
                    var countyColIndex = 0;
                    var contactColIndex = 0;

                    industryColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "industry").Key;
                    stateColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "state").Key;
                    countyColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "county").Key;
                    contactColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "leads").Key;
                    foreach (IXLRow row in sheet.Rows())
                    {
                        if (!row.IsEmpty())
                        {
                            rowCount++;
                            if (isFirstRow)
                            {
                                isFirstRow = false;
                                continue;
                            }
                            var industry = row.Cell(industryColIndex).GetString();
                            if (industry != Industry)
                            {
                                isOtherIdustry = true;
                                continue;
                            }
                            var catalogContact = new CatalogContact()
                            {
                                State = row.Cell(stateColIndex).GetString(),
                                County = row.Cell(countyColIndex).GetString(),
                                Percentage = 0.0M,
                                OriginalContacts = Convert.ToInt32(row.Cell(contactColIndex).GetString()),
                                RemainingContacts = Convert.ToInt32(row.Cell(contactColIndex).GetString()),
                                CreateDateUTC = DateTime.UtcNow,
                                UpdateDateUTC = DateTime.UtcNow,
                                UpdateSource = "CatalogController - Import",
                            };
                            catalogContactImport.CatalogContacts.Add(catalogContact);
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while importing the file, exception message: " + ex.Message);
                    TempData["ErrorMessage"] = "Please verify all cells having correct data in Sheet -'" + sheet.Name + "' at Row " + rowCount + "<br/>" + ex.Message;
                    return RedirectToAction("CatalogContactImport", "Catalog");
                }
                #region Saving Data In Database
                try
                {
                    if (catalogContactImport.CatalogContactImportId == 0)
                    {
                        ObjectService.Add(catalogContactImport);
                    }
                    ObjectService.SaveChanges();
                    if (isOtherIdustry)
                    {
                        TempData["SuccessMessage"] = $"Data imported partialy, this excel contains records other than {Industry} industry, those records are skipped.";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Data imported successfully";
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while storing the imported data of Supplier Rating Excel file, exception message: " + ex.Message);
                    TempData["ErrorMessage"] = "Error occured while saving the data -'" + ex.Message;
                    return RedirectToAction("CatalogContactImport", "Catalog");
                }
                #endregion
            }
            log.Debug("Index - end process - " + (DateTime.Now - startdate).TotalMilliseconds);
            return RedirectToAction("CatalogContactImport", "Catalog");
        }

        #endregion Excel Import

        #region Add/Edit/approve catalog contacts
        public ActionResult CatalogContacts(int? importId, string county = "", string state = "", string industry = "", int page = 1, int pageSize = 20)
        {
            CatalogContactModel contacttModel = null;
            if (importId.HasValue)
            {
                contacttModel = new CatalogContactModel();
                contacttModel.Page = page;
                contacttModel.ResultsPerPage = pageSize;
                contacttModel.Industry = industry;
                //====Search and sort Parameters=====
                contacttModel.q = new Dictionary<string, string>();
                contacttModel.q.Add("importId", importId.Value.ToString());
                contacttModel.q.Add("state", state);
                contacttModel.q.Add("county", county);
                contacttModel.q.Add("industry", industry);

                var import = ObjectService.GetAll<CatalogContactImport>(true)?.Where(c => c.CatalogContactImportId == importId.Value).FirstOrDefault();
                if (import != null)
                {
                    contacttModel.ImportId = import.CatalogContactImportId;
                    contacttModel.CatalogName = import.CatalogName;
                    if (import.CatalogContacts != null && import.CatalogContacts.Count() > 0)
                    {
                        if (TempData["States"] == null)
                        {
                            TempData["States"] = asi.asicentral.util.HtmlHelper.GetStates();
                        }
                       
                        var apiStates = (IList<SelectListItem>)TempData.Peek("States");
                        var dbStates = import.CatalogContacts.Select(m => m.State)?.Distinct();
                        if (dbStates != null && dbStates.Count() > 0)
                        {
                            foreach (var st in dbStates)
                            {
                                var apiState = apiStates.Where(m => m.Value == st).FirstOrDefault();
                                if (apiState != null)
                                    contacttModel.States.Add(new SelectListItem { Text = apiState.Text, Value = apiState.Value });
                            }
                            contacttModel.States= contacttModel.States.OrderBy(m => m.Text).ToList();
                        }
                        contacttModel.States.Insert(0,new SelectListItem { Value = "", Text = "--Select--" });
                        var counties = import.CatalogContacts.Where(s => s.State == state).Select(c => c.County)?.Distinct();
                        if (counties != null && counties.Count() > 0)
                        {
                            foreach (var ct in counties)
                            {
                                contacttModel.Counties.Add(new SelectListItem { Value = ct, Text = ct });
                            }
                            contacttModel.Counties= contacttModel.Counties.OrderBy(m => m.Text).ToList();
                        }
                        contacttModel.Counties.Insert(0,new SelectListItem { Value = "", Text = "--Select--" });

                        var contactList = import.CatalogContacts.Where(c => (c.CatalogContactImportId == importId.Value)
                                            && (c.County == county || string.IsNullOrEmpty(county))
                                            && (c.State == state || string.IsNullOrWhiteSpace(state))
                                            );

                        contacttModel.Contacts = contactList.OrderBy(m => m.CatalogContactId).Skip((contacttModel.Page - 1) * contacttModel.ResultsPerPage)
                                               .Take(contacttModel.ResultsPerPage)
                                               .Select(m => new CatalogContact()
                                               {
                                                   State = contacttModel.States.Where(s => s.Value == m.State).FirstOrDefault().Text,
                                                   County = m.County,
                                                   OriginalContacts = m.OriginalContacts,
                                                   RemainingContacts = m.RemainingContacts,
                                                   UpdateDateUTC = m.UpdateDateUTC,
                                                   CreateDateUTC = m.CreateDateUTC,
                                                   CatalogContactId = m.CatalogContactId,
                                                   Note = m.Note
                                               })
                                               .ToList();
                        contacttModel.ResultsTotal = contactList.Count();
                    }
                }
            }
            return View("~/Views/asicentral/Catalog/CatalogContacts.cshtml", contacttModel);
        }

        public ActionResult CatalogSales(int? importId = 0,
            int? contactId = 0, int page = 1,
            int pageSize = 20, string state = "",
            string county = "", string industry = "",
            string salesRep = "", string email = "", string reservedBy = "",
            string toDate = "", string fromDate = "",
            bool pendingApprovals = false,
            string asiNo = "", string orderTitle = "", string orderType = "asc")
        {
            CatalogContactsSalesModel salesModel = null;
            salesModel = new CatalogContactsSalesModel();
            salesModel.Page = page;
            salesModel.ResultsPerPage = pageSize;
            salesModel.ImportId = importId.Value;
            salesModel.ContactId = contactId.Value;

            //====Search and sort Parameters=====
            salesModel.q = new Dictionary<string, string>();
            salesModel.q.Add("importId", importId.Value.ToString());
            salesModel.q.Add("state", state);
            salesModel.q.Add("county", county);
            salesModel.q.Add("industry", industry);
            salesModel.q.Add("contactId", contactId.Value.ToString());
            salesModel.q.Add("pendingApprovals", pendingApprovals.ToString().ToLower());
            salesModel.q.Add("orderTitle", orderTitle);
            var sales = new List<CatalogSales>();
            var imports = new List<CatalogContactImport>();
            var salesDetails = new List<CatalogContactSaleDetail>();
            if (importId == 0)
            {
                salesDetails = ObjectService.GetAll<CatalogContactSaleDetail>("CatalogContactSale", true)?.Where(c => c.CatalogContacts.CatalogContactImport.IsActive && !c.CatalogContactSale.IsCancelled).ToList();
            }
            else
            {
                salesDetails = ObjectService.GetAll<CatalogContactSaleDetail>("CatalogContactSale", true)?.Where(c => c.CatalogContacts.CatalogContactImport.CatalogContactImportId == importId
                               && (c.CatalogContacts.CatalogContactId == contactId || contactId == 0)
                               && !c.CatalogContactSale.IsCancelled).ToList();
            }
            if (salesDetails != null && salesDetails.Count() > 0)
            {
                foreach (var detail in salesDetails)
                {
                    CatalogSales catalogSales = new CatalogSales();
                    var cSales = detail.CatalogContactSale;
                    catalogSales.CatalogContactSaleId = cSales.CatalogContactSaleId;
                    catalogSales.CompanyName = cSales.CompanyName;
                    catalogSales.ASINumber = cSales.ASINumber;
                    catalogSales.Email = cSales.Email;
                    catalogSales.Industry = detail.CatalogContacts.CatalogContactImport.IndustryName;
                    catalogSales.UpdateDateUTC = cSales.UpdateDateUTC;
                    catalogSales.CreateDateUTC = cSales.CreateDateUTC;
                    catalogSales.ASIRep = cSales.ASIRep;
                    catalogSales.IsApproved = cSales.IsApproved;
                    catalogSales.PendingContact = detail.ContactsRequested;
                    catalogSales.ApprovedContact = detail.ContactsApproved != null ? detail.ContactsApproved.Value : 0;
                    catalogSales.State = detail.CatalogContacts.State;
                    catalogSales.County = detail.CatalogContacts.County;
                    catalogSales.PendingSales = detail.ContactsRequested;
                    sales.Add(catalogSales);
                }
            }
            var otherOptionSales = ObjectService.GetAll<CatalogContactSale>()
                .Where(m => (m.OtherOptions != null && m.OtherOptions != "") && !m.CatalogContactSaleDetails.Any() && !m.IsCancelled).ToList();
            if (otherOptionSales != null && otherOptionSales.Count() > 0)
            {
                foreach (var otherSale in otherOptionSales)
                {
                    CatalogSales catalogSales = new CatalogSales();
                    catalogSales.CatalogContactSaleId = otherSale.CatalogContactSaleId;
                    catalogSales.CompanyName = otherSale.CompanyName;
                    catalogSales.ASINumber = otherSale.ASINumber;
                    catalogSales.Email = otherSale.Email;
                    catalogSales.Industry = "";
                    catalogSales.UpdateDateUTC = otherSale.UpdateDateUTC;
                    catalogSales.CreateDateUTC = otherSale.CreateDateUTC;
                    catalogSales.ASIRep = otherSale.ASIRep;
                    catalogSales.IsApproved = otherSale.IsApproved;
                    catalogSales.OtherOption = otherSale.OtherOptions;
                    sales.Add(catalogSales);
                }
            }
            // salesModel.Industries 
            var industries = sales.Select(m => m.Industry).Distinct();
            if (industries != null && industries.Count() > 0)
            {
                salesModel.Industries = new List<SelectListItem>();
                salesModel.Industries.Add(new SelectListItem { Value = "", Text = "--Select--" });
                foreach (var inds in industries)
                {
                    if (!string.IsNullOrWhiteSpace(inds))
                    {
                        salesModel.Industries.Add(new SelectListItem { Value = inds, Text = inds });
                    }
                }
            }
            sales = sales.OrderByDescending(o => o.CatalogContactSaleId).Where(m => (m.ASIRep.ToLower().Contains(salesRep.ToLower()) || salesRep == "")
                                         && (m.CompanyName.ToLower().Contains(reservedBy.ToLower()) || reservedBy == "")
                                          && (m.Email.ToLower().Contains(email.ToLower()) || email == "")
                                           && (m.ASINumber.ToLower().Contains(asiNo.ToLower()) || asiNo == "")
                                             && (m.Industry.ToLower().Contains(industry.ToLower()) || industry == "")
                                      )?.ToList();
            if (toDate != "" || fromDate != "")
            {
                if (toDate != "" && fromDate != "")
                {
                    sales = sales.Where(m => (Convert.ToDateTime(fromDate) <= Convert.ToDateTime(m.UpdateDateUTC) && Convert.ToDateTime(m.UpdateDateUTC) <= Convert.ToDateTime(toDate)))?.ToList();
                }
                else if (toDate != null)
                {
                    sales = sales.Where(m => Convert.ToDateTime(m.UpdateDateUTC) == Convert.ToDateTime(toDate))?.ToList();
                }
                else if (fromDate != "")
                {
                    sales = sales.Where(m => Convert.ToDateTime(m.UpdateDateUTC) == Convert.ToDateTime(fromDate))?.ToList();
                }
            }
            if (pendingApprovals == true)
            {
                sales = sales.Where(m => m.IsApproved == false)?.ToList();
            }
            else
            {
                sales = sales.Where(m => m.Industry != null && m.Industry != "")?.ToList();
            }
            sales = sales.GroupBy(m => m.ASINumber)
             .Select(s => new CatalogSales
             {
                 CompanyName = s.FirstOrDefault().CompanyName,
                 ASINumber = s.FirstOrDefault().ASINumber,
                 Email = s.FirstOrDefault().Email,
                 ASIRep = s.FirstOrDefault().ASIRep,
                 CreateDateUTC = s.FirstOrDefault().CreateDateUTC,
                 UpdateDateUTC = s.FirstOrDefault().UpdateDateUTC,
                 IsApproved = s.All(m => m.IsApproved),
                 ApprovedContact = s.Sum(m => m.ApprovedContact),
                 PendingContact = s.Sum(m => m.PendingContact),
                 OtherOption = s.FirstOrDefault().OtherOption,
                 Industry = string.Join("<br/>", s.GroupBy(i => i.Industry).Select(m => m.FirstOrDefault().Industry)),
                 salesDetails = s.GroupBy(i => i.County).Select(m => new CatalogSalesDetails
                 {
                     SaleId = m.FirstOrDefault().CatalogContactSaleId,
                     State = m.FirstOrDefault().State,
                     County = m.FirstOrDefault().County,
                     PendingSales = m.Sum(pd => pd.PendingSales),
                     IsPending = !m.All(a => a.IsApproved)
                 }).ToList(),
             }).ToList();

            salesModel.Sales = sales.OrderBy(m => m.CatalogContactSaleId).Skip((salesModel.Page - 1) * salesModel.ResultsPerPage)
                                            .Take(salesModel.ResultsPerPage).ToList();
            salesModel.ResultsTotal = sales.Count();
            if (salesModel?.Sales != null)
            {
                #region sorting
                if (orderType == "desc")
                {
                    if (orderTitle == "pendingSales")
                    {
                        salesModel.Sales = salesModel.Sales.OrderByDescending(m => m.PendingContact - m.ApprovedContact).ToList();
                    }
                    else if (orderTitle == "allSales")
                    {
                        salesModel.Sales = salesModel.Sales.OrderByDescending(m => m.PendingContact).ToList();
                    }
                    else if (orderTitle == "salesRep")
                    {
                        salesModel.Sales = salesModel.Sales.OrderByDescending(m => m.ASIRep).ToList();
                    }
                    salesModel.q.Add("orderType", "asc");
                }
                else
                {
                    if (orderTitle == "pendingSales")
                    {
                        salesModel.Sales = salesModel.Sales.OrderBy(m => m.PendingContact - m.ApprovedContact).ToList();
                    }
                    else if (orderTitle == "allSales")
                    {
                        salesModel.Sales = salesModel.Sales.OrderBy(m => m.PendingContact).ToList();
                    }
                    else if (orderTitle == "salesRep")
                    {
                        salesModel.Sales = salesModel.Sales.OrderBy(m => m.ASIRep).ToList();
                    }
                    salesModel.q.Add("orderType", "desc");
                }
                #endregion
            }
            return View("~/Views/asicentral/Catalog/CatalogSales.cshtml", salesModel);
        }

        public ActionResult PendingApprovalList()
        {
            return View("~/Views/asicentral/Catalog/PendingApprovalList.cshtml");
        }

        public ActionResult ApproveContact(int? saleId, string asiNumber = "")
        {
            CatalogContactSalesDetailsModel salesDetailsModel = new CatalogContactSalesDetailsModel();
            var saleDetails = ObjectService.GetAll<CatalogContactSaleDetail>("CatalogContactSale.CatalogArtWorks", true);
            saleDetails = saleDetails.Where(m => m.CatalogContacts.CatalogContactImport.IsActive)?
                                             .OrderByDescending(m => m.CreateDateUTC);
            if (!string.IsNullOrWhiteSpace(asiNumber))
            {
                saleDetails = saleDetails.Where(s => s.CatalogContactSale.ASINumber == asiNumber && !s.CatalogContactSale.IsCancelled);
                salesDetailsModel.CatalogRequested = null;
            }
            if (saleId.HasValue)
            {
                CatalogContactSale contactRequested = null;
                contactRequested = saleDetails.Where(m => m.CatalogContactSaleId == saleId).Select(sd => sd.CatalogContactSale).FirstOrDefault();
                if (contactRequested == null)
                {
                    var sale = ObjectService.GetAll<CatalogContactSale>().Where(m => m.CatalogContactSaleId == saleId.Value).FirstOrDefault();
                    if (sale != null)
                    {
                        contactRequested = sale;
                        asiNumber = sale.ASINumber;
                    }
                }
                else
                {
                    asiNumber = contactRequested.ASINumber;
                }
                saleDetails = saleDetails.Where(m => m.CatalogContactSale.CatalogContactSaleId != saleId.Value && !m.CatalogContactSale.IsCancelled);
                salesDetailsModel.CatalogRequested = contactRequested;
            }
            if (saleDetails != null && saleDetails.Count() > 0)
            {
                salesDetailsModel.CatalogContactSaleDetails = saleDetails.ToList();
            }
            var sales = ObjectService.GetAll<CatalogContactSale>().Where(m => m.ASINumber == asiNumber && !m.IsCancelled)?.ToList();
            if (sales != null && sales.Count > 0)
            {
                salesDetailsModel.CatalogContactSale = sales.OrderByDescending(m => m.CreateDateUTC).ToList();
            }
            return View("~/Views/asicentral/Catalog/ApproveContact.cshtml", salesDetailsModel);
        }
        [HttpPost]
        public ActionResult ApproveContact(List<CatalogContactSaleDetail> unApprovedContact, string asiNo, int saleId = 0, 
            bool isSalesForm = false, string OtherOptions = "", bool requestMoreInfo=false)
        {
            LogService log = LogService.GetLog(this.GetType());

            if (unApprovedContact != null && unApprovedContact.Count() > 0)
            {
                try
                {
                    var saleDetailsList = new List<CatalogContactSaleDetail>();
                    foreach (var detail in unApprovedContact)
                    {
                        if (detail.ContactsRequested != 0 && detail.ContactsRequested <= detail.CatalogContacts.RemainingContacts)
                        {
                            CatalogContactSaleDetail savedDetails = new CatalogContactSaleDetail();
                            savedDetails = ObjectService.GetAll<CatalogContactSaleDetail>("CatalogContactSale").Where(m => m.CatalogContactSaleDetailId == detail.CatalogContactSaleDetailId).FirstOrDefault();
                            savedDetails.ContactsApproved = detail.ContactsRequested;
                            savedDetails.UpdateDateUTC = DateTime.UtcNow;
                            savedDetails.UpdateSource = "CatalogController - Approved";
                            savedDetails.CatalogContactSale.IsApproved = true;
                            savedDetails.CatalogContactSale.ApprovedDate = DateTime.UtcNow;
                            savedDetails.CatalogContactSale.ApprovedBy = User.Identity.Name;
                            savedDetails.CatalogContactSale.UpdateSource = "CatalogController - Approved";
                            savedDetails.CatalogContactSale.OtherOptions = OtherOptions;
                            savedDetails.CatalogContactSale.RequestMoreInfo = requestMoreInfo;
                            savedDetails.CatalogContacts.RemainingContacts = savedDetails.CatalogContacts.RemainingContacts - detail.ContactsRequested;
                            savedDetails.CatalogContacts.UpdateSource = "CatalogController - Approved";
                            savedDetails.CatalogContacts.UpdateDateUTC = DateTime.UtcNow;
                            saleDetailsList.Add(savedDetails);
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Not enough catalog bussinesses available.";
                            if (saleId > 0)
                            {

                                return RedirectToAction("ApproveContact", "Catalog", new { saleId });
                            }
                            else
                            {
                                return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = asiNo });
                            }
                        }
                    }
                    ObjectService.SaveChanges();
                    TempData["ShowEmail"] = "true";
                    SendEmail(saleDetailsList);
                    TempData["SuccessMessage"] = "Contact Approved successfully";
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while Approving the contacts, exception message: " + ex.Message);
                    TempData["ErrorMessage"] = "Exception while Approving the contacts";
                    if (saleId > 0)
                    {
                        return RedirectToAction("ApproveContact", "Catalog", new { saleId });
                    }
                    else
                    {
                        return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = asiNo });
                    }
                }
            }
            if (saleId > 0)
            {
                return RedirectToAction("ApproveContact", "Catalog", new { saleId });
            }
            else
            {
                return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = asiNo });
            }
        }

        [HttpGet]
        public ActionResult EditContact(int contactId, int contacts)
        {
            ViewData["contactId"] = contactId;
            ViewData["contacts"] = contacts;
            return PartialView("~/Views/asicentral/Catalog/EditContactForm.cshtml");
        }

        [HttpPost]
        public ActionResult SaveEditContact(int? contactId, int contacts = 0)
        {
            LogService log = LogService.GetLog(this.GetType());
            CatalogContact catalogContact = null;
            try
            {
                if (contactId.HasValue)
                {
                    catalogContact = ObjectService.GetAll<CatalogContact>().Where(m => m.CatalogContactId == contactId.Value).FirstOrDefault();
                    if (catalogContact != null)
                    {
                        catalogContact.RemainingContacts += contacts - catalogContact.OriginalContacts;
                        catalogContact.OriginalContacts = contacts;
                    }
                }
                ObjectService.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Debug("Exception while Updating the contacts, exception message: " + ex.Message);
            }
            return RedirectToAction("CatalogContacts", new { importId = catalogContact.CatalogContactImportId, state = catalogContact.State, industry = catalogContact.CatalogContactImport.IndustryName });
        }

        public ActionResult EditApprovedOrder(List<CatalogContactSaleDetail> currentSales, string isSaleIdView)
        {
            LogService log = LogService.GetLog(this.GetType());
            var asiNo = string.Empty;
            var saleId = 0;
            bool isSaleParitalySaved = false;
            var msg = string.Empty;
            if (currentSales != null && currentSales.Count() > 0)
            {
                try
                {
                    var saleDetailsList = new List<CatalogContactSaleDetail>();
                    foreach (var detail in currentSales)
                    {
                        var savedDetails = ObjectService.GetAll<CatalogContactSaleDetail>("CatalogContactSale").Where(m => m.CatalogContactSaleDetailId == detail.CatalogContactSaleDetailId).FirstOrDefault();
                        var maxContactAllowed = detail.CatalogContacts.RemainingContacts + savedDetails.ContactsRequested;
                        if (detail.ContactsRequested >= 1 && detail.ContactsRequested <= maxContactAllowed)
                        {
                            savedDetails.ContactsRequested = detail.ContactsRequested;
                            savedDetails.UpdateDateUTC = DateTime.UtcNow;
                            savedDetails.UpdateSource = "CatalogController - EditOrder";
                            savedDetails.CatalogContactSale.UpdateDateUTC = DateTime.UtcNow;
                            savedDetails.CatalogContactSale.UpdateSource = "CatalogController - EditOrder";
                            savedDetails.CatalogContacts.RemainingContacts = savedDetails.CatalogContacts.RemainingContacts + (savedDetails.ContactsApproved.Value - detail.ContactsRequested);
                            savedDetails.ContactsApproved = detail.ContactsRequested;
                            savedDetails.CatalogContacts.UpdateSource = "CatalogController - EditOrder";
                            savedDetails.CatalogContacts.UpdateDateUTC = DateTime.UtcNow;
                            saleDetailsList.Add(savedDetails);
                        }
                        else
                        {
                            isSaleParitalySaved = true;
                            msg += $"Not enough catalog bussinesses available for {savedDetails.CatalogContacts.CatalogContactImport.IndustryName} {savedDetails.CatalogContacts.State} {savedDetails.CatalogContacts.County}\n";
                        }
                    }
                    ObjectService.SaveChanges();
                    var currentSale = currentSales.FirstOrDefault().CatalogContactSale;
                    asiNo = currentSale.ASINumber;
                    saleId = currentSale.CatalogContactSaleId;
                    SendEmail(saleDetailsList);
                    if (isSaleParitalySaved)
                    {
                        TempData["SuccessMessage"] = $"{msg}, Order Saved Partially.";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Contact Save successfully";
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while Approving the contacts, exception message: " + ex.Message);
                    TempData["ErrorMessage"] = "Exception while Approving the contacts";
                }
            }
            if (isSaleIdView == "True")
            {
                return RedirectToAction("ApproveContact", "Catalog", new { saleId });
            }
            else
            {
                return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = asiNo });
            }
        }

        [HttpPost]
        public ActionResult AddMoreCatalog(int? saleId, int? catalogContactId, string IndustryName, int? approveContacts, string asiNo)
        {
            try
            {
                CatalogContactSaleDetail catalogSalesDetails = new CatalogContactSaleDetail()
                {
                    CatalogContactSaleId = saleId.Value,
                    CatalogContactId = catalogContactId.Value,
                    ContactsApproved = approveContacts.Value,
                    ContactsRequested = approveContacts.Value,
                    CreateDateUTC = DateTime.UtcNow,
                    UpdateDateUTC = DateTime.UtcNow,
                    UpdateSource = "Admin Tool"
                };
                ObjectService.Add(catalogSalesDetails);
                ObjectService.SaveChanges();
            }
            catch (Exception ex)
            {
                LogService log = LogService.GetLog(this.GetType());
                log.Debug("Exception while Adding the contacts, exception message: " + ex.Message);
                TempData["ErrorMessage"] = "Exception while Adding the contacts";
                return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = asiNo });
            }
            return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = asiNo });
        }

        [HttpGet]
        public PartialViewResult AddMoreCatalog()
        {
            CatalogContactsSalesModel salesModel = new CatalogContactsSalesModel();
            var imports =ObjectService.GetAll<CatalogContactImport>(true)?.OrderBy(m => !m.IsActive);
            var industries = imports.Where(m => m.IsActive).Select(m => m.IndustryName).Distinct();
            if (industries != null && industries.Count() > 0)
            {
                salesModel.Industries = new List<SelectListItem>();
                salesModel.Industries.Add(new SelectListItem { Value = "", Text = "--Select--" });
                foreach (var inds in industries)
                {
                    if (!string.IsNullOrWhiteSpace(inds))
                    {
                        salesModel.Industries.Add(new SelectListItem { Value = inds, Text = inds });
                    }
                }
            }
            salesModel.States.Add(new SelectListItem { Value = "", Text = "--Select--" });
            salesModel.Counties.Add(new SelectListItem { Value = "", Text = "--Select--" });
            return PartialView("~/Views/asicentral/Catalog/AddCatalog.cshtml", salesModel);
        }

        [HttpGet]
        public ActionResult AddEditNotes(int contactId, string note)
        {
            ViewData["contactId"] = contactId;
            ViewData["note"] = note;
            return PartialView("~/Views/asicentral/Catalog/EditContactNotesForm.cshtml");
        }
        [HttpPost]
        public ActionResult SaveContactNote(int? contactId, string note)
        {
            LogService log = LogService.GetLog(this.GetType());
            CatalogContact catalogContact = null;
            try
            {
                if (contactId.HasValue)
                {
                    catalogContact = ObjectService.GetAll<CatalogContact>().Where(m => m.CatalogContactId == contactId.Value).FirstOrDefault();
                    if (catalogContact != null)
                    {
                        catalogContact.Note = note;
                    }
                }
                ObjectService.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Debug("Exception while adding note, exception message: " + ex.Message);
            }
            return RedirectToAction("CatalogContacts", new { importId = catalogContact.CatalogContactImportId, state = catalogContact.State, industry = catalogContact.CatalogContactImport.IndustryName });
        }

        #endregion Add/Edit/approve catalog contacts

        #region delete 
        public ActionResult CancelOrder(int? saleId, string isSaleIdView)
        {
            LogService log = LogService.GetLog(this.GetType());
            var IsOrdCancelled = false;
            CatalogContactSale contactSale = null;
            try
            {
                if (saleId.HasValue)
                {
                    contactSale = ObjectService.GetAll<CatalogContactSale>().Where(m => m.CatalogContactSaleId == saleId.Value && !m.IsCancelled).FirstOrDefault();
                }
                if (contactSale != null)
                {
                    contactSale.IsCancelled = true;
                    contactSale.CancelledBy = User.Identity.Name;
                    contactSale.CancelledUTCDate = DateTime.UtcNow;
                    ObjectService.SaveChanges();
                    IsOrdCancelled = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception while Canceling the contacts, exception message: " + ex.Message);
            }
            if (IsOrdCancelled)
            {
                TempData["SuccessMessage"] = "Order cancelled sucessfully.";
                return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = contactSale.ASINumber });
            }
            else
            {
                TempData["ErrorMessage"] = "Exception while Canceling the Order";
                if (isSaleIdView == "True")
                {
                    return RedirectToAction("ApproveContact", "Catalog", new { saleId });
                }
                else
                {
                    return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = contactSale.ASINumber });
                }
            }
        }

        public ActionResult RemoveOrder(int? saleId, string isSaleIdView)
        {
            LogService log = LogService.GetLog(this.GetType());
            var IsOrdRemoved = false;
            CatalogContactSale contactSale = null;
            try
            {
                if (saleId.HasValue)
                {
                    contactSale = ObjectService.GetAll<CatalogContactSale>().Where(m => m.CatalogContactSaleId == saleId.Value && !m.IsCancelled).FirstOrDefault();
                }
                if (contactSale != null)
                {
                    if (contactSale.CatalogContactSaleDetails != null && contactSale.CatalogContactSaleDetails.Count > 0)
                    {
                        foreach (var details in contactSale.CatalogContactSaleDetails)
                        {
                            var catalogContact = ObjectService.GetAll<CatalogContact>().Where(m => m.CatalogContactId == details.CatalogContactId).FirstOrDefault();
                            if (catalogContact != null)
                            {
                                catalogContact.RemainingContacts += details.ContactsApproved.HasValue ? details.ContactsApproved.Value : 0;
                            }
                        }
                    }
                    contactSale.IsCancelled = true;
                    contactSale.CancelledBy = User.Identity.Name;
                    contactSale.CancelledUTCDate = DateTime.UtcNow;
                    ObjectService.SaveChanges();
                    IsOrdRemoved = true;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception while Removing the contacts, exception message: " + ex.Message);
            }
            if (IsOrdRemoved)
            {
                TempData["SuccessMessage"] = "Order Removed sucessfully.";
                return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = contactSale.ASINumber });
            }
            else
            {
                TempData["ErrorMessage"] = "Exception while Removing the Order";
                if (isSaleIdView == "True")
                {
                    return RedirectToAction("ApproveContact", "Catalog", new { saleId });
                }
                else
                {
                    return RedirectToAction("ApproveContact", "Catalog", new { asiNumber = contactSale.ASINumber });
                }
            }
        }

        public ActionResult RemoveSalesDetail(int? detailId, string isSaleIdView)
        {
            LogService log = LogService.GetLog(this.GetType());
            var asiNumber = string.Empty;
            var saleId = 0;
            if (detailId.HasValue)
            {
                try
                {
                    var savedDetails = ObjectService.GetAll<CatalogContactSaleDetail>("CatalogContactSale").Where(m => m.CatalogContactSaleDetailId == detailId.Value).FirstOrDefault();
                    if (savedDetails != null)
                    {
                        var catalogContact = ObjectService.GetAll<CatalogContact>().Where(m => m.CatalogContactId == savedDetails.CatalogContactId).FirstOrDefault();
                        var saleIsApproved = false;
                        if (savedDetails.CatalogContactSale != null)
                        {
                            saleIsApproved = savedDetails.CatalogContactSale.IsApproved;
                            asiNumber = savedDetails.CatalogContactSale.ASINumber;
                            saleId = savedDetails.CatalogContactSale.CatalogContactSaleId;
                        }
                        if (catalogContact != null)
                        {
                            if (saleIsApproved)
                            {
                                catalogContact.RemainingContacts += savedDetails.ContactsApproved.Value;
                            }
                            ObjectService.Delete(savedDetails);
                        }
                    }
                    ObjectService.SaveChanges();
                    TempData["SuccessMessage"] = "Contact Deleted successfully";
                }
                catch (Exception ex)
                {
                    log.Debug("Exception while deleting the contacts, exception message: " + ex.Message);
                    TempData["ErrorMessage"] = "Exception while deleting the contacts.";
                }
            }
            if (isSaleIdView == "True")
            {
                return RedirectToAction("ApproveContact", "Catalog", new { saleId });
            }
            else
            {
                return RedirectToAction("ApproveContact", "Catalog", new { asiNumber });
            }

        }

        #endregion delete 

        #region email
        public void SendEmail(List<CatalogContactSaleDetail> saleDetails)
        {
            if (saleDetails != null && saleDetails.Count > 0)
            {
                string emailBody = TemplateService.Render("asi.asicentral.web.Views.Emails.CatalogApprovalEmail.cshtml", saleDetails);
                MailMessage mail = new MailMessage();
                var toMails = saleDetails[0].CatalogContactSale.ASIRep;
                if (!string.IsNullOrEmpty(toMails))
                {
                    var repEmails = toMails.Split(';');
                    foreach (var email in repEmails)
                    {
                        mail.To.Add(new MailAddress(email));
                    }
                    mail.Subject = "Catalog county reservation order approved";
                    mail.Body = emailBody;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    EmailService.SendMail(mail);
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendCustomerEmail(string toEmail, string mailMessage, string asiNumber, string fromEmail, string subject)
        {
            string emailBody = mailMessage;
            MailMessage mail = new MailMessage();
            if (!string.IsNullOrWhiteSpace(toEmail))
            {
                var to = toEmail.Split(';');
                foreach (var email in to)
                {
                    mail.To.Add(new MailAddress(email));
                }
            }
            mail.From = new MailAddress(fromEmail);
            mail.Subject = subject;
            mail.Body = emailBody;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            try
            {
                EmailService.SendMail(mail);
                TempData["SuccessMessage"] = "Email sent to customer.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Exception while sending customer email.";
            }
            return RedirectToAction("ApproveContact", "Catalog", new { asiNumber });
        }

        public string GetCustomerEmailBody(int? saleId)
        {
            var customerEmailBody = string.Empty;
            if (saleId.HasValue)
            {
                var saleDetails = ObjectService.GetAll<CatalogContactSaleDetail>("CatalogContactSale").Where(m => m.CatalogContactSale.CatalogContactSaleId == saleId.Value).ToList();
                customerEmailBody = TemplateService.Render("asi.asicentral.web.Views.Emails.CatalogApprovalEmail.cshtml", saleDetails);
            }
            return customerEmailBody;
        }

        #endregion email

        #region retrieve data from database
        public ActionResult GetCoutiesByState(int? importId, string state)
        {
            List<SelectListItem> countyList = new List<SelectListItem>();
            var import = new List<CatalogContactImport>();
            if (importId.HasValue)
            {
                import = ObjectService.GetAll<CatalogContactImport>().Where(m => m.CatalogContactImportId == importId.Value).ToList();
            }
            else
            {
                import = ObjectService.GetAll<CatalogContactImport>().ToList();
            }
            if (import != null)
            {
                var counties = import.SelectMany(c => c.CatalogContacts.Where(m => m.State == state).Select(s => s.County)).Distinct();
                countyList.Add(new SelectListItem { Value = "", Text = "--Select--" });
                foreach (var ct in counties)
                {
                    countyList.Add(new SelectListItem { Value = ct, Text = ct });
                }
            }
            return Json(countyList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetValidCounties(string state = "", string industry = "")
        {
            CatalogContact catalogContact = new CatalogContact();
            List<SelectListItem> countyList = new List<SelectListItem>();
            List<CatalogCountyInfo> catalogCountyInfos = new List<CatalogCountyInfo>();
            var import = new CatalogContactImport();
            industry = industry.ToLower();
            import = ObjectService.GetAll<CatalogContactImport>().Where(m => m.IsActive && m.IndustryName.ToLower() == industry).FirstOrDefault();
            List<string> counties = null;
            if (import != null)
            {
                counties = import.CatalogContacts.Where(m => m.State == state)?.Select(s => s.County)?.Distinct().ToList();
                foreach (var ct in counties)
                {
                    state = state.ToLower();
                    catalogContact = ObjectService.GetAll<CatalogContact>().Where(m => m.State.ToLower() == state
                    && m.County.ToLower() == ct.ToLower()
                    && m.CatalogContactImport.CatalogContactImportId == import.CatalogContactImportId).FirstOrDefault();
                    if (catalogContact != null && catalogContact.RemainingContacts > 0)
                    {
                        countyList.Add(new SelectListItem { Value = ct, Text = ct, });
                        catalogCountyInfos.Add(new CatalogCountyInfo { RemaingContacts = catalogContact.RemainingContacts, CatalogContactId = catalogContact.CatalogContactId, County = ct, });
                    }
                }
            }
            return Json(catalogCountyInfos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStateByIndustryName(string industry)
        {
            var states = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "--Select--" } };
            if (!string.IsNullOrEmpty(industry))
            {
                var import = ObjectService.GetAll<CatalogContactImport>(true)?.Where(m => m.IndustryName.ToLower() == industry.ToLower());
                var industries = import.Where(m => m.IsActive).Select(m => m.IndustryName).Distinct();
                if (TempData["States"] == null)
                {
                    TempData["States"] = asi.asicentral.util.HtmlHelper.GetStates();
                }
                var apiStates = (IList<SelectListItem>)TempData.Peek("States");
                var dbStates = import.SelectMany(m => m.CatalogContacts.Select(s => s.State)).Distinct();

                if (dbStates != null && dbStates.Count() > 0)
                {
                    foreach (var st in dbStates)
                    {
                        var apiState = apiStates.Where(m => m.Value == st).FirstOrDefault();
                        if (apiState != null)
                            states.Add(new SelectListItem { Text = apiState.Text, Value = apiState.Value });
                    }
                }
            }
            return Json(states.OrderBy(m => m.Value), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult CatalogSubmissionHistory(string asiNumber)
        {
            var catalogSales = ObjectService.GetAll<CatalogContactSale>()?.OrderByDescending(m => m.CreateDateUTC).ToList();
            return View("~/Views/asicentral/Catalog/CatalogSubmissionHistory.cshtml", catalogSales);
        }

        #endregion retrieve data from database

        #region download
        public ActionResult DownloadCatalogInventry(int importId, string state)
        {
            LogService log = LogService.GetLog(this.GetType());
            try
            {
                var industry = string.Empty;
                var catalogContacts = ObjectService.GetAll<CatalogContact>(true)?.Where(c => c.CatalogContactImportId == importId && (c.State == state || state == string.Empty)).ToList();
                var csvString = new StringBuilder();
                var columnNames = string.Empty;
                columnNames = "Industry,State,County,Original Contacts, Remaining Contacts";
                csvString.AppendLine(columnNames);
                if (catalogContacts != null && catalogContacts.Count > 0)
                {
                    industry = catalogContacts.FirstOrDefault().CatalogContactImport.IndustryName;
                    var line = string.Empty;
                    foreach (var item in catalogContacts)
                    {
                        line = string.Join(",", industry, item.State, item.County,item.OriginalContacts, item.RemainingContacts);
                        csvString.AppendLine(line);
                    }
                }
                var filename = "CatalogInventry-" + industry + (string.IsNullOrWhiteSpace(state) ? string.Empty : "-" + state) + ".csv";
                return File(new System.Text.UTF8Encoding().GetBytes(csvString.ToString()), "text/csv", filename);
            }
            catch (Exception ex)
            {
                log.Debug("Error in CatalogController -- DownloadRemainingContacts , exception message: " + ex.Message);
                TempData["ErrorMessage"] = "Error occured while downloading data -'" + ex.Message;
            }
            return null;
        }
        
        public ActionResult DownloadCatalogSales(string industry, string asiNo)
        {
            LogService log = LogService.GetLog(this.GetType());
            try
            {
                var catalogContactImport = ObjectService.GetAll<CatalogContactImport>(true)?.Where(c => c.IndustryName.ToLower() == industry.ToLower() && c.IsActive).FirstOrDefault();
                if (catalogContactImport != null)
                {
                    var catalogContacts = ObjectService.GetAll<CatalogContactSaleDetail>(true)?.Where(c => !c.CatalogContactSale.IsCancelled && c.CatalogContacts.CatalogContactImportId == catalogContactImport.CatalogContactImportId && (c.CatalogContactSale.ASINumber == asiNo || asiNo == string.Empty)).ToList();
                    var csvString = new StringBuilder();
                    var columnNames = string.Empty;
                    columnNames = "ASINo,Industry,State,County,Contacts Requested,Contacts Approved";
                    csvString.AppendLine(columnNames);
                    if (catalogContacts != null && catalogContacts.Count > 0)
                    {
                        var line = string.Empty;
                        foreach (var item in catalogContacts)
                        {
                            line = string.Join(",", item.CatalogContactSale.ASINumber, industry, item.CatalogContacts.State, item.CatalogContacts.County, item.ContactsRequested, item.ContactsApproved);
                            csvString.AppendLine(line);
                        }
                    }
                    var filename = "CatalogSales-" + industry + (string.IsNullOrWhiteSpace(asiNo) ? string.Empty : "-" + asiNo) + ".csv";
                    return File(new System.Text.UTF8Encoding().GetBytes(csvString.ToString()), "text/csv", filename);
                }
            }
            catch (Exception ex)
            {
                log.Debug("Error in CatalogController -- DownloadAllSubmissions , exception message: " + ex.Message);
                TempData["ErrorMessage"] = "Error occured while downloading data -'" + ex.Message;
            }
            return null;
          }

        #endregion download

        #region Private helping functions
        private ActionResult CatalogContactUpdate(CatalogContactImport import, IXLWorksheet sheet)
        {
            if (import == null)
            {
                TempData["ErrorMessage"] = "No existing import found";
                return RedirectToAction("CatalogContactImport", "Catalog");
            }

            var industryName = import.IndustryName;
            LogService log = LogService.GetLog(this.GetType());
            log.Debug("CatalogContactsImport - start update process");
            var startTime = DateTime.Now;
            var rowCount = 0;
            var isFirstRow = true;
            var isOtherIdustry = false;

            Dictionary<int, string> headings = new Dictionary<int, string>();
            var firstRow = sheet.Row(1);
            int cellIndex = 1;
            while (!firstRow.Cell(cellIndex).IsEmpty())
            {
                headings.Add(cellIndex, firstRow.Cell(cellIndex).GetString());
                cellIndex++;
            }

            var industryColIndex = 0;
            var stateColIndex = 0;
            var countyColIndex = 0;
            var contactColIndex = 0;

            industryColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "industry").Key;
            stateColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "state").Key;
            countyColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "county").Key;
            contactColIndex = headings.FirstOrDefault(x => x.Value.ToLower() == "leads").Key;
            log.Debug("CatalogContactsImportUpdate - start Delete process");
            var startDeleteTime = DateTime.Now;
            // remove all contacts not in sales
            for (int i = import.CatalogContacts.Count - 1; i >= 0; i--)
            {
                var contact = import.CatalogContacts.ElementAt(i);
                if (contact.CatalogContactSaleDetails == null || contact.CatalogContactSaleDetails.Count < 1)
                {
                    import.CatalogContacts.Remove(contact);
                    ObjectService.Delete(contact);
                }
            }
            log.Debug("end Delete process - " + (DateTime.Now - startDeleteTime).TotalMilliseconds);
            // keep track of all contacts in approved/pening sales
            var saleContacts = import.CatalogContacts.ToList();

            log.Debug("CatalogContactsImportUpdate - start Excel read process");
            var startExcelTime = DateTime.Now;
            // update contacts from new import sheet
            foreach (IXLRow row in sheet.Rows())
            {
                if (!row.IsEmpty())
                {
                    rowCount++;
                    if (isFirstRow)
                    {
                        isFirstRow = false;
                        continue;
                    }
                    var industry = row.Cell(industryColIndex).GetString();
                    if (industry != industryName)
                    {
                        isOtherIdustry = true;
                        continue;
                    }

                    var state = row.Cell(stateColIndex).GetString();
                    var county = row.Cell(countyColIndex).GetString();
                    var newContactCount = Convert.ToInt32(row.Cell(contactColIndex).GetString());

                    CatalogContact catalogContact = null;
                    // check if the contact in pending/approved sale                
                    var curSaleContact = saleContacts.FirstOrDefault(c => c.State.Equals(state, StringComparison.CurrentCultureIgnoreCase) &&
                                                                      c.County.Equals(county, StringComparison.CurrentCultureIgnoreCase));
                    if (curSaleContact != null)
                    {// get catalogContact from the list
                        catalogContact = import.CatalogContacts.FirstOrDefault(c => c.CatalogContactId == curSaleContact.CatalogContactId);
                    }

                    if (catalogContact != null)
                    { // update existing record
                        var reservedContact = catalogContact.OriginalContacts - catalogContact.RemainingContacts;
                        catalogContact.RemainingContacts = newContactCount;
                        catalogContact.Percentage = 0.0M;
                        catalogContact.OriginalContacts = newContactCount + reservedContact;
                        catalogContact.UpdateDateUTC = DateTime.UtcNow;
                        catalogContact.UpdateSource = "CatalogController - Import";

                        // remove this contact from saleContacts list
                        saleContacts.Remove(curSaleContact);
                    }
                    else
                    {  // create new record 
                        catalogContact = new CatalogContact()
                        {
                            CatalogContactImportId = import.CatalogContactImportId,
                            State = state,
                            County = county,
                            Percentage = 0.0M,
                            OriginalContacts = newContactCount,
                            RemainingContacts = newContactCount,
                            CreateDateUTC = DateTime.UtcNow,
                            UpdateDateUTC = DateTime.UtcNow,
                            UpdateSource = "CatalogController - Import",
                        };
                        import.CatalogContacts.Add(catalogContact);
                    }
                }
            }
            log.Debug("end Excel read process - " + (DateTime.Now - startExcelTime).TotalMilliseconds);

            log.Debug("CatalogContactsImportUpdate - start update remaining contact process");
            var startUpdateContactTime = DateTime.Now;
            // for each remaing record in saleContact, not in new import, update remaing contact to 0
            foreach (var saleContact in saleContacts)
            {
                var catalogContact = import.CatalogContacts.FirstOrDefault(c => c.CatalogContactId == saleContact.CatalogContactId);
                if (catalogContact != null)
                {
                    catalogContact.OriginalContacts = catalogContact.OriginalContacts - catalogContact.RemainingContacts;
                    catalogContact.RemainingContacts = 0;
                }
            }
            log.Debug("end update remaining contact process - " + (DateTime.Now - startUpdateContactTime).TotalMilliseconds);

            #region Saving Data In Database
            try
            {
                ObjectService.SaveChanges();
                if (isOtherIdustry)
                {
                    TempData["SuccessMessage"] = $"Data imported partialy, this excel contains records other than {industryName} industry, those records are skipped.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Data updated successfully";
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception while updateing, exception message: " + ex.Message);
                TempData["ErrorMessage"] = "Error occured while updating the data -'" + ex.Message;

            }
            #endregion
            log.Debug("end Update process - " + (DateTime.Now - startTime).TotalMilliseconds);
            return RedirectToAction("CatalogContactImport", "Catalog");
        }

        private void DisableImportByIndustry(string industry)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
            var deleteContacts = @"update USR_CatalogContactImport set IsActive = @isActive where IndustryName = @industryName";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(deleteContacts, connection))
                {
                    command.Parameters.AddWithValue("@isActive", false);
                    command.Parameters.AddWithValue("@industryName", industry);
                    var result = command.ExecuteNonQuery();
                }
            }
        }
        #endregion Private helping functions
    }
}