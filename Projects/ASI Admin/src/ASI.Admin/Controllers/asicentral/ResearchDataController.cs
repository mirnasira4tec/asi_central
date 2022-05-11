using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using asi.asicentral.web.Models.asicentral;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace asi.asicentral.web.Controllers.asicentral
{
    public class ResearchDataController : Controller
    {
        private ILogService _log = LogService.GetLog(typeof(ResearchDataController));
        private static string _delimiter = "|";
        //private static List<string> _columnNames = new List<string>()
        //{
        //    "Visual", "Year", "Product", "Country", "Region", "State", "Industry", "Gender", "Generation", "Topic"
        //};

        public IObjectService ObjectService { get; set; }

        // GET: ResearchData
        public ActionResult Index()
        {
            var imports = ObjectService.GetAll<ResearchImport>(true)?.ToList();
            var model = new ResearchDataUploadModel()
            {
                UploadedImports = imports,
                ResearchNames = new List<SelectListItem>() { new SelectListItem() { Text = "Select Research" } }
            };

            foreach( var import in imports )
            {
                model.ResearchNames.Add(new SelectListItem() { Text = import.Name, Value = import.Name });
            }

            model.ResearchNames.Add(new SelectListItem() { Text = "Other", Value = "Other" });

            return View("~/Views/asicentral/researchData/index.cshtml", model);
        }

        public ActionResult Details(int importId)
        {
            ResearchImport model = null;
            if(importId > 0)
            {
                model = ObjectService.GetAll<ResearchImport>().FirstOrDefault(i => i.Id == importId);
            }
            return View("~/Views/asicentral/researchData/details.cshtml", model);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string researchName, int? importId)
        {
            if (file == null || string.IsNullOrWhiteSpace(researchName))
            {
                TempData["ErrorMessage"] = "Please select file and Research Name to upload.";
                return new RedirectResult("/ResearchData/Index");
            }

            var isFileValid = false;
            if (file != null)
            {
                var fileExtension = Path.GetExtension(Path.GetFileName(file.FileName));
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    isFileValid = true;
                }
            }

            if (!isFileValid)
            {
                TempData["ErrorMessage"] = "Please select a .xls or .xlsx file to upload.";
                return new RedirectResult("/ResearchData/Index");
            }

            var startTime = DateTime.Now;
            _log.Debug($"Research Upload - start process - {startTime}");
            using (var workBook = new XLWorkbook(file.InputStream))
            {
                var sheets = workBook.Worksheets;
                if (sheets != null && sheets.Count > 0)
                {
                    var userName = ControllerContext.HttpContext.User.Identity.Name;
                    foreach (var sheet in sheets)
                    {
                        var researchImport = new ResearchImport()
                        {
                            Name = researchName,
                            LastUpdatedBy = string.IsNullOrEmpty(userName) ? "Admin" : userName,
                            ResearchDataList = new List<ResearchData>()
                        };
                        _retrieveResearchData(sheet, researchImport);
                        _saveUploadedData(researchImport);
                    }
                }

                _log.Debug("Research Upload - end process - " + (DateTime.Now - startTime).TotalMilliseconds);
                return new RedirectResult("/ResearchData/Index");
            }
        }

        private void _retrieveResearchData(IXLWorksheet sheet, ResearchImport researchImport)
        {
             var rowCount = 1;
            try
            {
                #region Reading Contacts From Excel
                Dictionary<int, string> headings = new Dictionary<int, string>();
                var firstRow = sheet.Row(1);
                int cellIndex = 1;
                while (!firstRow.Cell(cellIndex).IsEmpty())
                {
                    headings.Add(cellIndex, firstRow.Cell(cellIndex).GetString().Trim());
                    cellIndex++;
                }

                var imageColIndex = headings.FirstOrDefault(x => x.Value == "Visual").Key;
                var yearColIndex = headings.FirstOrDefault(x => x.Value == "Year").Key;
                var productColIndex = headings.FirstOrDefault(x => x.Value == "Product").Key;
                var countryColIndex = headings.FirstOrDefault(x => x.Value == "Country").Key;
                var regionColIndex = headings.FirstOrDefault(x => x.Value == "Region").Key;
                var stateColIndex = headings.FirstOrDefault(x => x.Value == "State").Key;
                var industryColIndex = headings.FirstOrDefault(x => x.Value == "Industry").Key;
                var genderColIndex = headings.FirstOrDefault(x => x.Value == "Gender").Key;
                var generationColIndex = headings.FirstOrDefault(x => x.Value == "Generation").Key;
                var topicColIndex = headings.FirstOrDefault(x => x.Value == "Topic").Key;
                foreach (IXLRow row in sheet.Rows().Skip(1))
                {
                    if (!row.IsEmpty())
                    {
                        rowCount++;
                        var year = row.Cell(yearColIndex).GetString();
                        var researchData = new ResearchData()
                        {
                            Year = year,
                            ImageUrl = _parseResearchData(row.Cell(imageColIndex).GetString(), true, year),
                            Product = _parseResearchData(row.Cell(productColIndex).GetString()),
                            Country = _parseResearchData(row.Cell(countryColIndex).GetString()),
                            Region = _parseResearchData(row.Cell(regionColIndex).GetString()),
                            State = _parseResearchData(row.Cell(stateColIndex).GetString()),
                            Industry = _parseResearchData(row.Cell(industryColIndex).GetString()),
                            Gender = _parseResearchData(row.Cell(genderColIndex).GetString()),
                            Generation = _parseResearchData(row.Cell(generationColIndex).GetString()),
                            Topic = _parseResearchData(row.Cell(topicColIndex).GetString()),
                            CreateDateUTC = DateTime.UtcNow,
                            UpdateDateUTC = DateTime.UtcNow,
                            UpdateSource = "ResearchData - Upload",
                        };
                        researchImport.ResearchDataList.Add(researchData);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _log.Debug("Exception while importing the file, exception message: " + ex.Message);
                TempData["ErrorMessage"] = "Please verify all cells having correct data in Sheet -'" + sheet.Name + "' at Row " + rowCount + "<br/>" + ex.Message;
            }
        }

        private string _parseResearchData(string data, bool isImage = false, string year = "")
        {
            if( isImage && !string.IsNullOrEmpty(year))
            {  // need to process image
                var match = System.Text.RegularExpressions.Regex.Match(data, @"[Pp]+age\s*(\d+)\s*$");
                if(match.Success )
                {
                    data = string.Format("https://media.asicentral.com/Resources/AdImpressionStudy/{0}/Ad_impressions_{0}-{1}.jpg", year, match.Groups[1].Value);
                }
            }
            else if( !string.IsNullOrEmpty(data) && data.Contains(","))
            {
                data = Regex.Replace(data, @"\s*,\s*", _delimiter);
                data = $"{_delimiter}{data.Trim()}{_delimiter}";
            }
            else
            {
                data = data.Trim();
            }

            return data;
        }

        private void _saveUploadedData(ResearchImport researchImport)
        {
            try
            {
                // delete existing research data for the same name
                var dbImport = ObjectService.GetAll<ResearchImport>()
                                                  .FirstOrDefault(i => i.Name == researchImport.Name);
                if( dbImport != null)
                {
                    // update existing research data
                    var researchDbData = dbImport.ResearchDataList;
                    var updateDate = DateTime.Now;
                    var dbDataCount = researchDbData.Count;
                    var newDataCount = researchImport.ResearchDataList.Count;
                    for (var i = 0; i < newDataCount; i++)
                    {
                        var newData = researchImport.ResearchDataList[i];
                        if( i < dbDataCount )
                        {
                            var dbData = researchDbData[i];
                            dbData.ImageUrl = newData.ImageUrl;
                            dbData.Year = newData.Year;
                            dbData.Region = newData.Region;
                            dbData.Country = newData.Country;
                            dbData.State = newData.State;
                            dbData.Product = newData.Product;
                            dbData.Industry = newData.Industry;
                            dbData.Gender = newData.Gender;
                            dbData.Generation = newData.Generation;
                            dbData.Topic = newData.Topic;
                            dbData.UpdateDateUTC = updateDate;
                        }
                        else
                        {
                            newData.CreateDateUTC = updateDate;
                            newData.UpdateDateUTC = updateDate;
                            newData.UpdateSource = "ResearchDataController";
                            researchDbData.Add(newData);
                        }
                    }

                    // delete extra old records if any
                    if (dbDataCount > newDataCount)
                    {
                        for (var i = dbDataCount - 1; i > newDataCount - 1; i--)
                        {
                            ObjectService.Delete(researchDbData[i]);
                        }
                    }

                    dbImport.UpdateDateUTC = updateDate;
                    dbImport.LastUpdatedBy = researchImport.LastUpdatedBy;
                    ObjectService.SaveChanges();
                }
                else
                {
                    dbImport = new ResearchImport() 
                        { 
                            Name = researchImport.Name,
                            ResearchDataList = new List<ResearchData>(),
                            CreateDateUTC = DateTime.Now,
                            UpdateDateUTC = DateTime.Now,
                            LastUpdatedBy = researchImport.LastUpdatedBy,
                            UpdateSource = "ResearchDataController"
                        };

                    ObjectService.Add(dbImport);
                    ObjectService.SaveChanges();
                    // add new research Data
                    foreach(var data in researchImport.ResearchDataList)
                    {
                        data.CreateDateUTC = DateTime.Now;
                        data.UpdateDateUTC = DateTime.Now;
                        data.UpdateSource = "ResearchDataController";
                        dbImport.ResearchDataList.Add(data);
                    }
                }

                ObjectService.SaveChanges();
                TempData["SuccessMessage"] = "Data imported successfully";
            }
            catch (Exception ex)
            {
                _log.Debug("Exception while saving the imported data, exception message: " + ex.Message);
                TempData["ErrorMessage"] = "Error occured while saving the data -'" + ex.Message;
            }
        }
    }
}

