using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.services;
using asi.asicentral.util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace asi.asicentral.web.Controllers.asicentral
{
    public class ProposalToolController : Controller
    {
        public IObjectService ObjectService { get; set; }
        private static readonly ILogService log = LogService.GetLog(MethodBase.GetCurrentMethod().DeclaringType);

        #region constants
        public const string LOGOTAG = "<<LOGO>>";
        public const string CLIENTNAMETAG = "<<CLIENTNAME>>";
        public const string EDIMAGETAG = "<<EDIMAGE>>";
        public const string AMIMAGETAG = "<<AMIMAGE>>";
        public const string EDNAMETAG = "<<EDName>>";
        public const string AMNAMETAG = "<<AMName>>";
        public const string EDTITLETAG = "<<EDTITLE>>";
        public const string AMTITLETAG = "<<AMTITLE>>";
        public const string EDFIRSTNAMETAG = "<<EDFirstName>>";
        public const string AMFIRSTNAMETAG = "<<AMFirstName>>";
        public const string CURRENTSERVICETABLETAG = "<<CURRENTSERVICESTABLES>>";
        public string PROPOSEDSERVICETABLETAG = "<<PROPOSEDSERVICESTABLES>>";
        public const string EDSIGNTAG = "<<SIGN>>";
        public const string DATETAG = "<<DATE>>";
        public readonly string MEDIASERVERURL = ConfigurationManager.AppSettings["MediaServerURL"];
        public readonly string MEDIAPATH = ConfigurationManager.AppSettings["MediaPath"];
        public const string FILEMEDIAPATH = "resources\\proposalTool\\";
        public const string FILEUPLOADPATH = "ASICentral\\resources\\proposalTool\\";
        public const string TEMPLATEFILENAME = "ProposalTemplate.docx";
        public const string REPPHOTODIRECTORY = "RepPhotos";
        public const string LOGODIRECTORY = "Logo";
        public const string REPSIGNDIRECTORY = "RepSign";
        public const string WORDDOCSDIRECTORY = "WordDocs";
        
        public const int CLIENTNAMEQUESTIONID = 1;
        public const int UPLOADLOGOQUESTIONID = 2;
        public const int CLIENTTYPEQUESTIONID = 3;
        public const int EXECUTIVEDIRECTORQUESTIONID = 4;
        public const int ACCOUNTMANAGERQUESTIONID = 5;
        public const int PLANDETAILSQUESTIONID = 6;
        public const int CURRENTSERVICESQUESTIONID = 8;
        public const int CURRENTSERVICESNAMEQUESTIONID = 9;
        public const int CURRENTSERVICESPRICEQUESTIONID = 10;
        public const int PROPOSEDSERVICESQUESTIONID = 11;
        public const int PROPOSEDSERVICESNAMEQUESTIONID = 12;
        public const int PROPOSEDSERVICESPIRCEQUESTIONID = 13;
        public const int SOURCEFILEQUESTIONID = 14;

        #endregion

        // GET: ProposalTool
        public ActionResult Index()
        {
            var proposalFormInstance = ObjectService.GetAll<AsicentralFormInstance>()?
                                        .Where(f => f.FormType.Name == "Proposal Form")?.OrderByDescending(f => f.CreateDate).ToList();
            return View("~/views/asicentral/proposaltool/proposals.cshtml", proposalFormInstance);
        }

        [HttpGet]
        public ActionResult ProposalForm(int? instanceId)
        {
            var formInstance = new AsicentralFormInstance();
            if (instanceId.HasValue && instanceId.Value > 0)
            {
                formInstance = ObjectService.GetAll<AsicentralFormInstance>("FormType;DataValues.Question").Where(f => f.Id == instanceId.Value).FirstOrDefault();
            }
            else
            {
                formInstance.FormType = ObjectService.GetAll<AsicentralFormType>().Where(f => f.Name == "Proposal Form").FirstOrDefault();
                formInstance.DataValues = new List<FormDataValue>();
                foreach (var question in formInstance.FormType.FormQuestions)
                {
                    var formPropertyValue = new FormDataValue();
                    formPropertyValue.Question = question;
                    formInstance.DataValues.Add(formPropertyValue);
                }
            }
            _loadForm(formInstance);
            return View("~/views/asicentral/proposaltool/proposalform.cshtml", formInstance);
        }
        [HttpPost]
        public ActionResult ProposalForm(AsicentralFormInstance formInstance, HttpPostedFileBase logo)
        {
            AsicentralFormInstance proposalForm = null;
            try
            {
                if (formInstance.Id > 0)
                {
                    proposalForm = ObjectService.GetAll<AsicentralFormInstance>("FormType;DataValues.Question").Where(f => f.Id == formInstance.Id).FirstOrDefault();
                }
                else
                {
                    proposalForm = new AsicentralFormInstance()
                    {
                        CCProfileId = 0,
                        TypeId = formInstance.FormType.Id,
                        Reference = Guid.NewGuid().ToString(),
                        Email = string.Empty,
                        CreateDate = DateTime.Now,
                    };
                    proposalForm.DataValues = new List<FormDataValue>();
                }
                proposalForm.CompanyConstituentId = string.Empty;
                proposalForm.UpdateDate = DateTime.Now;
                proposalForm.UpdateSource = "ProposalToolController";
                if (!string.IsNullOrEmpty(Request.UserHostAddress))
                {
                    if (Request.Url.Authority.Contains("localhost"))
                    {
                        proposalForm.IPAddress = "127.0.0.1";
                    }
                    else
                    {
                        proposalForm.IPAddress = Request.UserHostAddress;
                    }
                }
                var clientName = string.Empty;

                if (formInstance.DataValues != null && formInstance.DataValues.Count > 0)
                {
                    for (int i = 0; i < formInstance.DataValues.Count; i++)
                    {
                        var formValue = formInstance.DataValues[i];
                        FormDataValue value = null;
                        if (formValue.Id > 0)
                        {
                            value = proposalForm.DataValues.FirstOrDefault(m => m.QuestionId == formValue.QuestionId);
                        }
                        else
                        {
                            value = new FormDataValue();
                            value.CreateDateUTC = DateTime.Now;
                        }
                        value.InstanceId = proposalForm.Id;
                        value.QuestionId = formValue.QuestionId;
                        if (formValue.QuestionId == CLIENTNAMEQUESTIONID)
                        {
                            clientName = formValue.Value;
                        }

                        value.Value = formValue.Value;
                        value.UpdateDateUTC = DateTime.Now;
                        value.UpdateSource = "ProposalToolController";
                        if (formValue.Id == 0)
                        {
                            proposalForm.DataValues.Add(value);
                        }
                    }
                }
                var uploadLogoOption = proposalForm.DataValues.Where(n => n.QuestionId == UPLOADLOGOQUESTIONID).FirstOrDefault();
                if (uploadLogoOption != null && logo != null && !string.IsNullOrWhiteSpace(logo.FileName))
                {
                    uploadLogoOption.Value = _fileUpload(logo, clientName);
                }
                var currentServicesValue = proposalForm.DataValues.Where(d => d.QuestionId == CURRENTSERVICESQUESTIONID).FirstOrDefault()?.Value;
                var proposedServicesValue = proposalForm.DataValues.Where(d => d.QuestionId == PROPOSEDSERVICESQUESTIONID).FirstOrDefault()?.Value;

                var isCurrentServicesSelected = !string.IsNullOrWhiteSpace(currentServicesValue) && currentServicesValue == "true";
                var isProposedServicesSelected = !string.IsNullOrWhiteSpace(proposedServicesValue) && proposedServicesValue == "true";
                if (!isCurrentServicesSelected)
                {
                    var serviceQuestionObj = proposalForm.DataValues.Where(n => n.QuestionId == CURRENTSERVICESNAMEQUESTIONID).FirstOrDefault();
                    if (serviceQuestionObj != null)
                    {
                        serviceQuestionObj.Value = string.Empty;
                    }
                    var servicePriceQuestonObj = proposalForm.DataValues.Where(n => n.QuestionId == CURRENTSERVICESPRICEQUESTIONID).FirstOrDefault();
                    if (servicePriceQuestonObj != null)
                    {
                        servicePriceQuestonObj.Value = string.Empty;
                    }
                }
                if (!isProposedServicesSelected)
                {
                    var serviceQuestionObj = proposalForm.DataValues.Where(n => n.QuestionId == PROPOSEDSERVICESNAMEQUESTIONID).FirstOrDefault();
                    {
                        serviceQuestionObj.Value = string.Empty;
                    }
                    var servicePriceQuestonObj = proposalForm.DataValues.Where(n => n.QuestionId == PROPOSEDSERVICESPIRCEQUESTIONID).FirstOrDefault();
                    if (servicePriceQuestonObj != null)
                    {
                        servicePriceQuestonObj.Value = string.Empty;
                    }
                }
                if (formInstance.Id == 0)
                {
                    ObjectService.Add(proposalForm);
                }
                var proposalDoc = _generateProposal(proposalForm);
                if (proposalDoc != null)
                {
                    var fileName = _saveProposal(proposalDoc, clientName);
                    TempData["fileName"] = fileName;
                    var servicePriceQuestonObj = proposalForm.DataValues.Where(n => n.QuestionId == SOURCEFILEQUESTIONID).FirstOrDefault();
                    if (servicePriceQuestonObj != null)
                    {
                        servicePriceQuestonObj.Value = fileName;
                    }
                }
                else
                {
                    TempData["fileName"] = "error";
                }
                ObjectService.SaveChanges();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                log.Error("ProposalToolController-ProposalForm" + ex.Message);
                TempData["fileName"] = "error";
            }
            return ProposalForm(proposalForm.Id);
        }

        public void _loadForm(AsicentralFormInstance formInstance)
        {
            var propertyValuesWithFollowUp = formInstance?.DataValues.Where(q => q.Question != null && !string.IsNullOrWhiteSpace(q.Question.FollowingUpQuestions)).ToList();
            foreach (var value in propertyValuesWithFollowUp)
            {
                var display = "display:none";
                if (!string.IsNullOrWhiteSpace(value.Value) && value.Question.InputType == "checkbox" && value.Value == "true")
                {
                    display = "display:block";
                }
                if (!string.IsNullOrWhiteSpace(value.Value) && value.Question.InputType != "checkbox")
                {
                    display = "display:block";
                }
                var questions = value.Question.FollowingUpQuestions.TrimEnd(',');
                var followUpQuestionIds = questions.Split(',').Select(int.Parse).ToList();
                foreach (var questionId in followUpQuestionIds)
                {
                    var followUpQuestion = formInstance.FormType.FormQuestions.Where(q => q.Id == questionId).FirstOrDefault();
                    if (followUpQuestion != null)
                    {
                        followUpQuestion.CssStyle = display;
                    }
                }
            }
        }

        private DocX _generateProposal(AsicentralFormInstance formInstance)
        {
            DocX proposalDoc = null;
            if (formInstance != null)
            {
                var temptatePath = $"{FILEUPLOADPATH}{WORDDOCSDIRECTORY}\\template";
                proposalDoc = DocX.Load(Path.Combine(MEDIAPATH, temptatePath, TEMPLATEFILENAME));
                RemovingReplacingTextInTemplate(formInstance, proposalDoc, ObjectService);

                #region add ed and am image
                _addImage(proposalDoc, formInstance, LOGODIRECTORY, LOGOTAG);
                _addImage(proposalDoc, formInstance, REPPHOTODIRECTORY, EDIMAGETAG);
                _addImage(proposalDoc, formInstance, REPPHOTODIRECTORY, AMIMAGETAG);
                _addImage(proposalDoc, formInstance, REPSIGNDIRECTORY, EDSIGNTAG, true);
                #endregion
            }
            return proposalDoc;
        }

        private string _saveProposal(DocX proposalDoc, string clientName)
        {
            var year = DateTime.Now.Year;
            var uploadDocPath = $"{FILEUPLOADPATH}{WORDDOCSDIRECTORY}\\{year}";
            var fileName = Utility.RemoveSpecialChars($"{clientName}_{DateTime.Now}") + ".docx";
            var uploadPath = Path.Combine(MEDIAPATH, uploadDocPath, fileName);
            log.Debug($"Proposal word doc path: {uploadPath}");
            proposalDoc.SaveAs(uploadPath);
            var dbMediaPath = $"{FILEMEDIAPATH}{WORDDOCSDIRECTORY}\\{year}";
            return Path.Combine(MEDIASERVERURL, dbMediaPath, fileName);
        }

        private void _addServiceTable(string serviceTag, DocX proposalDoc, AsicentralFormInstance formInstance, int serviceQuestionId)
        {
            var serviceParagraphs = proposalDoc.Paragraphs.Where(p => p.Text.Contains(serviceTag)).FirstOrDefault();
            var servicesValue = formInstance.DataValues.Where(d => d.QuestionId == serviceQuestionId).FirstOrDefault()?.Value;
            var services = servicesValue?.Split(',');
            var headerText = string.Empty;
            if (serviceQuestionId == CURRENTSERVICESNAMEQUESTIONID)
            {
                headerText = "Current Package";
            }
            else
            {
                headerText = "Proposed Services";
            }
            var servicesCount = services != null ? services.Length : 0;
            var serviceTable = proposalDoc.AddTable(servicesCount + 9, 1);
            serviceTable.Design = TableDesign.DarkListAccent2;
            serviceTable.Alignment = Alignment.right;
            serviceTable.Rows[0].Cells[0].FillColor = Color.FromName("red");
            serviceTable.Rows[0].Height = 25;
            serviceTable.Rows[0].Cells[0].Width = 220;
            serviceTable.Rows[0].Cells[0].Paragraphs[0].Append(headerText);
            serviceTable.Rows[0].Cells[0].Paragraphs[0].Color(Color.FromName("white"));
            serviceTable.Rows[0].Cells[0].Paragraphs[0].Bold(true);
            serviceTable.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[0].Cells[0].Paragraphs[0].LineSpacingBefore = 5;

            serviceTable.Rows[1].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[1].Cells[0].Paragraphs[0].Append("");
            serviceTable.Rows[1].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[1].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            serviceTable.Rows[2].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[2].Cells[0].Paragraphs[0].Append("- ASI Membership");
            serviceTable.Rows[2].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[2].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            var cellCount = 2;
            for (int i = 0; i < servicesCount; i++)
            {
                cellCount += 1;
                serviceTable.Rows[cellCount].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
                serviceTable.Rows[cellCount].Cells[0].Paragraphs[0].Append("- " + services[i]);
                serviceTable.Rows[cellCount].Cells[0].Paragraphs[0].Alignment = Alignment.center;
                serviceTable.Rows[cellCount].Cells[0].Paragraphs[0].Color(Color.FromName("black"));
            }

            serviceTable.Rows[cellCount + 1].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[cellCount + 1].Cells[0].Paragraphs[0].Append("- Unlimited Training and Support");
            serviceTable.Rows[cellCount + 1].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[cellCount + 1].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            serviceTable.Rows[cellCount + 2].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[cellCount + 2].Cells[0].Paragraphs[0].Append("- Access to ASI Shows and Other Events");
            serviceTable.Rows[cellCount + 2].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[cellCount + 2].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            var servicePrice = string.Empty;
            if (serviceQuestionId == CURRENTSERVICESNAMEQUESTIONID)
            {
                servicePrice = formInstance.DataValues.Where(d => d.QuestionId == CURRENTSERVICESPRICEQUESTIONID).FirstOrDefault()?.Value;
            }
            else
            {
                servicePrice = formInstance.DataValues.Where(d => d.QuestionId == PROPOSEDSERVICESPIRCEQUESTIONID).FirstOrDefault()?.Value;
            }
            serviceTable.Rows[cellCount + 3].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[cellCount + 3].Cells[0].Paragraphs[0].Append("");
            serviceTable.Rows[cellCount + 3].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[cellCount + 3].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            serviceTable.Rows[cellCount + 4].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[cellCount + 4].Cells[0].Paragraphs[0].Append("");
            serviceTable.Rows[cellCount + 4].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[cellCount + 4].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            serviceTable.Rows[cellCount + 5].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[cellCount + 5].Cells[0].Paragraphs[0].Append($"${servicePrice.Replace("$", "")}");
            serviceTable.Rows[cellCount + 5].Cells[0].Paragraphs[0].Bold(true);
            serviceTable.Rows[cellCount + 5].Cells[0].Paragraphs[0].Italic(true);
            serviceTable.Rows[cellCount + 5].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[cellCount + 5].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            serviceTable.Rows[cellCount + 6].Cells[0].FillColor = Color.FromArgb(236, 236, 236);
            serviceTable.Rows[cellCount + 6].Cells[0].Paragraphs[0].Append("");
            serviceTable.Rows[cellCount + 6].Cells[0].Paragraphs[0].Alignment = Alignment.center;
            serviceTable.Rows[cellCount + 6].Cells[0].Paragraphs[0].Color(Color.FromName("black"));

            if (serviceParagraphs != null)
            {
                serviceParagraphs.InsertTableAfterSelf(serviceTable);
                serviceParagraphs.ReplaceText((string)serviceTag, "");
            }
        }

        private void _addImage(DocX proposalDoc, AsicentralFormInstance formInstance, string imageDirectory, string tagName, bool isSignImage = false)
        {
            log.Debug($"Image Directory: {imageDirectory}, tag Name: {tagName}, isSignImage: {isSignImage} ");
            AsicentralFormQuestionOption option = null;
            var serverPath = Path.Combine(MEDIAPATH, FILEUPLOADPATH, imageDirectory);
            var imgHeight = 0;
            var imgWidth = 0;
            var file = string.Empty;
            if (tagName == LOGOTAG)
            {
                serverPath += $"\\{DateTime.Now.Year}";
                file = formInstance.DataValues.Where(d => d.QuestionId == UPLOADLOGOQUESTIONID).FirstOrDefault()?.Value;
            }
            else
            {
                if (tagName == EDIMAGETAG || isSignImage)
                {
                    var ednameDataValueObj = formInstance.DataValues.Where(d => d.QuestionId == EXECUTIVEDIRECTORQUESTIONID).FirstOrDefault();
                    AsicentralFormQuestionOption edOptions = new AsicentralFormQuestionOption();
                    if (ednameDataValueObj != null)
                    {
                        var selectOptionId = Convert.ToInt32(ednameDataValueObj.Value);
                        option = ObjectService.GetAll<AsicentralFormQuestionOption>().Where(q => q.Id == selectOptionId).FirstOrDefault();
                    }
                }
                else if (tagName == AMIMAGETAG)
                {
                    var amnameDataValueObj = formInstance.DataValues.Where(d => d.QuestionId == ACCOUNTMANAGERQUESTIONID).FirstOrDefault();
                    AsicentralFormQuestionOption amOptions = new AsicentralFormQuestionOption();
                    if (amnameDataValueObj != null)
                    {
                        var selectOptionId = Convert.ToInt32(amnameDataValueObj.Value);
                        option = ObjectService.GetAll<AsicentralFormQuestionOption>().Where(q => q.Id == selectOptionId).FirstOrDefault();
                    }
                }
                if (isSignImage)
                {
                    imgHeight = 30;
                    imgWidth = 125;
                    file = !string.IsNullOrWhiteSpace(option?.AdditionalData) ? option.AdditionalData.Split(';')[1] : string.Empty;
                }
                else
                {
                    imgHeight = 150;
                    imgWidth = 100;
                    file = !string.IsNullOrWhiteSpace(option?.AdditionalData) ? option.AdditionalData.Split(';')[0] : string.Empty;
                }

            }
            if (!string.IsNullOrWhiteSpace(file))
            {
                var fileName = Path.GetFileName(file);
                var filePath = Path.Combine(serverPath, fileName);
                log.Debug($"Image path: {filePath}");
                var image = proposalDoc.AddImage(filePath);

                var imgParaGraph = proposalDoc.Paragraphs.Where(x => x.Text.Contains(tagName)).FirstOrDefault();
                if (imgParaGraph != null)
                {
                    if (tagName == LOGOTAG)
                    {
                        imgParaGraph.InsertPicture(image.CreatePicture(), 0);
                    }
                    else
                    {
                        imgParaGraph.InsertPicture(image.CreatePicture(imgHeight, imgWidth), 0);
                    }
                    imgParaGraph.ReplaceText(tagName, "");
                }
            }

        }

        private string _fileUpload(HttpPostedFileBase file, string clientName)
        {
            var filePath = string.Empty;
            var extention = Path.GetExtension(file.FileName);
            var uploadAttendeeImagePath = $"{FILEUPLOADPATH}Logo\\{DateTime.Now.Year}";
            var dbMediaPath = $"{FILEMEDIAPATH}Logo\\{DateTime.Now.Year}";
            var absoluteLogoPath = Path.Combine(MEDIAPATH, uploadAttendeeImagePath);
            log.Debug($"Logo file path: {uploadAttendeeImagePath}");
            if (Directory.Exists(absoluteLogoPath))
            {
                if (!string.IsNullOrWhiteSpace(clientName))
                {
                    clientName = Utility.RemoveSpecialChars(clientName);
                    var fileName = Utility.RemoveSpecialChars($"{clientName}_{DateTime.Now}") + extention;
                    var imagePath = Path.Combine(absoluteLogoPath, fileName);
                    var image = _resizeImage(file);
                    image.Save(imagePath);
                    filePath = Path.Combine(dbMediaPath, fileName);
                }
            }
            return filePath;
        }

        private Bitmap _resizeImage(HttpPostedFileBase file)
        {
            var image = System.Drawing.Image.FromStream(file.InputStream, true, true);
            var maxHeight = 80;
            var ratio = (double)maxHeight / image.Height;
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var g = Graphics.FromImage(newImage))
            {
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        public void RemovingReplacingTextInTemplate(AsicentralFormInstance formInstance, DocX proposalDoc, IObjectService objectService)
        {
            #region removing Plans From template
            var planDetailsQuestionObj = formInstance.DataValues.Where(d => d.QuestionId == PLANDETAILSQUESTIONID).FirstOrDefault();
            var planDetailsValue = string.Empty;
            if (planDetailsQuestionObj != null)
            {
                planDetailsValue = planDetailsQuestionObj.Value;
                var planDetailsOptions = objectService.GetAll<AsicentralFormQuestionOption>().Where(q => q.FormQuestionId == planDetailsQuestionObj.QuestionId);
                if (!string.IsNullOrWhiteSpace(planDetailsValue))
                {
                    var arrDetails = planDetailsValue.Split(',');
                    foreach (var planOption in planDetailsOptions)
                    {
                        var tagName = $"<<{planOption.Name}>>";
                        var planParagraphs = proposalDoc.Paragraphs.Where(p => p.Text.Contains(tagName));
                        var isDeatialSelected = arrDetails.Any(m => string.Compare(m, planOption.Name, StringComparison.OrdinalIgnoreCase) == 0);
                        if (!isDeatialSelected)
                        {
                            foreach (var planParagraph in planParagraphs)
                            {
                                planParagraph.Remove(false);
                            }
                        }
                        else
                        {
                            foreach (var planParagraph in planParagraphs)
                            {
                                planParagraph.ReplaceText(tagName, "");
                            }
                        }
                    }
                }
            }
            #endregion

            #region removing Paragraph by clientType
            var ClientTypeObj = formInstance.DataValues.Where(d => d.QuestionId == CLIENTTYPEQUESTIONID).FirstOrDefault();
            if (ClientTypeObj != null)
            {
                var clientType = ClientTypeObj.Value;
                var clientTypeOptions = objectService.GetAll<AsicentralFormQuestionOption>().Where(q => q.FormQuestionId == ClientTypeObj.QuestionId);
                if (!string.IsNullOrWhiteSpace(clientType))
                {
                    foreach (var option in clientTypeOptions)
                    {
                        var tagName = $"<<{option.Name}>>";
                        var paragraphs = proposalDoc.Paragraphs.Where(p => p.Text.Contains(tagName));
                        if (string.Compare(option.Name, clientType, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            foreach (var clientTypeParaGraph in paragraphs)
                            {
                                clientTypeParaGraph.ReplaceText(tagName, "");
                            }
                        }
                        else
                        {
                            foreach (var clientTypeParaGraph in paragraphs)
                            {
                                clientTypeParaGraph.Remove(false);
                            }
                        }
                    }
                }
                #endregion

                #region Adding ServicesTable
                var currentServicesValue = formInstance.DataValues.Where(d => d.QuestionId == CURRENTSERVICESQUESTIONID).FirstOrDefault()?.Value;
                var proposedServicesValue = formInstance.DataValues.Where(d => d.QuestionId == PROPOSEDSERVICESQUESTIONID).FirstOrDefault()?.Value;

                var isCurrentServicesSelected = !string.IsNullOrWhiteSpace(currentServicesValue) && currentServicesValue == "true";
                var isProposedServicesSelected = !string.IsNullOrWhiteSpace(proposedServicesValue) && proposedServicesValue == "true";

                if (isCurrentServicesSelected)
                {
                    _addServiceTable(CURRENTSERVICETABLETAG, proposalDoc, formInstance, CURRENTSERVICESNAMEQUESTIONID);
                }
                else
                {
                    var serviceParagraphs = proposalDoc.Paragraphs.Where(p => p.Text.Contains(PROPOSEDSERVICETABLETAG)).FirstOrDefault();
                    if (serviceParagraphs != null)
                    {
                        serviceParagraphs.ReplaceText((string)PROPOSEDSERVICETABLETAG, "");
                    }
                    PROPOSEDSERVICETABLETAG = CURRENTSERVICETABLETAG;
                }
                if (isProposedServicesSelected)
                {
                    _addServiceTable(PROPOSEDSERVICETABLETAG, proposalDoc, formInstance, PROPOSEDSERVICESNAMEQUESTIONID);
                }
                var propPosedparagraphs = proposalDoc.Paragraphs.Where(p => p.Text.Contains(PROPOSEDSERVICETABLETAG));
                foreach (var para in propPosedparagraphs)
                {
                    para.Remove(false);
                }
                var currentparagraphs = proposalDoc.Paragraphs.Where(p => p.Text.Contains(CURRENTSERVICETABLETAG));
                foreach (var para in currentparagraphs)
                {
                    para.Remove(false);
                }
                #endregion

                #region replacing Cleint/ED/AM names
                var clientName = formInstance.DataValues.Where(d => d.QuestionId == CLIENTNAMEQUESTIONID).FirstOrDefault().Value;
                var edName = string.Empty;
                var edImage = string.Empty;
                var amName = string.Empty;
                var amImage = string.Empty;
                var edTitle = string.Empty;
                var amTitle = string.Empty;
                var ednameDataValueObj = formInstance.DataValues.Where(d => d.QuestionId == EXECUTIVEDIRECTORQUESTIONID).FirstOrDefault();
                var amnameDataValueObj = formInstance.DataValues.Where(d => d.QuestionId == ACCOUNTMANAGERQUESTIONID).FirstOrDefault();
                AsicentralFormQuestionOption edOptions = null;
                if (ednameDataValueObj != null)
                {
                    var selectOptionId = Convert.ToInt32(ednameDataValueObj.Value);
                    edOptions = objectService.GetAll<AsicentralFormQuestionOption>().Where(q => q.Id == selectOptionId).FirstOrDefault();
                    edName = edOptions.Name;
                    edImage = edOptions.AdditionalData;
                    edTitle = edOptions.Description.Substring(edOptions.Description.IndexOf(',') + 1).Trim();
                }
                AsicentralFormQuestionOption amOptions = null;
                if (amnameDataValueObj != null)
                {
                    var selectOptionId = Convert.ToInt32(amnameDataValueObj.Value);
                    amOptions = objectService.GetAll<AsicentralFormQuestionOption>().Where(q => q.Id == selectOptionId).FirstOrDefault();
                    amName = amOptions.Name;
                    amImage = amOptions.AdditionalData;
                    amTitle = amOptions.Description.Substring(amOptions.Description.IndexOf(',') + 1).Trim();
                }
                var edFirstName = !string.IsNullOrWhiteSpace(edName) ? edName.Split(' ')[0] : string.Empty;
                var amFirstName = !string.IsNullOrWhiteSpace(amName) ? amName.Split(' ')[0] : string.Empty;
                proposalDoc.ReplaceText(CLIENTNAMETAG, clientName, false, RegexOptions.IgnoreCase);
                proposalDoc.ReplaceText(EDNAMETAG, edName, false, RegexOptions.IgnoreCase);
                proposalDoc.ReplaceText(AMNAMETAG, amName, false, RegexOptions.IgnoreCase);
                proposalDoc.ReplaceText(EDFIRSTNAMETAG, edFirstName, false, RegexOptions.IgnoreCase);
                proposalDoc.ReplaceText(AMFIRSTNAMETAG, amFirstName, false, RegexOptions.IgnoreCase);
                proposalDoc.ReplaceText(EDTITLETAG, edTitle, false, RegexOptions.IgnoreCase);
                proposalDoc.ReplaceText(AMTITLETAG, amTitle, false, RegexOptions.IgnoreCase);
                proposalDoc.ReplaceText(DATETAG, DateTime.Now.ToString("MM/dd/yyyy"), false, RegexOptions.IgnoreCase);
                #endregion
            }
        }
    }
}
