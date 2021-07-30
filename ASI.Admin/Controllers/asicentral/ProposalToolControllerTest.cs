using asi.asicentral.interfaces;
using asi.asicentral.model.asicentral;
using asi.asicentral.web.Controllers.asicentral;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace ASI.Admin.Controllers.asicentral
{
    [TestFixture]
    public class ProposalToolControllerTest
    {
        private readonly string temptatePath = "Controllers\\asicentral\\Data\\";
        private readonly string dir = Path.GetDirectoryName(typeof(ProposalToolControllerTest).Assembly.Location);
        [Test]
        public void RemovingReplacingTextInTemplate()
        {
            var formInstance = _createFormInstance();
            var questionOptionList = new List<AsicentralFormQuestionOption>()
            {
                _createQuestionOption(1,4,"Michael D’Ottaviano, Executive Director, Corporate Accounts","Michael D’Ottaviano", 1,"michael.jpg;michael_sign.jpg"),
                _createQuestionOption(2,4,"Joan Miracle, Executive Director, Corporate Accounts","Joan Miracle",2,"joan.jpg;joan_sign.jpg"),
                _createQuestionOption(3,5,"Jillian DiBella, Account Manager, Corporate Accounts","Jillian DiBella",4,"jillian.jpg"),
                _createQuestionOption(4,5,"Melissa Hall Senior, Account Manager, Corporate Accounts","Melissa Hall",3,"hall.jpg"),
                _createQuestionOption(5,5,"Ann Gergal, Senior Account Manager, Corporate Accounts, Corporate Accounts","Ann Gergal",5,"ann.jpg"),
                _createQuestionOption(6,3,"New Client","New Client",1,null),
                _createQuestionOption(7,3,"Current Client","Current Client",2,null),
                _createQuestionOption(8,6,"ESP Platform","ESP Platform",1,null),
                _createQuestionOption(9,6,"Company Stores","Company Stores",2,null),
                _createQuestionOption(10,6,"ESP Websites","ESP Websites",3,null),
                _createQuestionOption(11,6,"Catalogs","Catalogs",4,null),
            };
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<AsicentralFormQuestionOption>(false)).Returns(questionOptionList.AsQueryable());
            ProposalToolController controller = new ProposalToolController();

            var path = Path.Combine(dir, temptatePath, "ProposalTemplate.docx");
            var proposalDoc = DocX.Load(path);
            controller.RemovingReplacingTextInTemplate(formInstance, proposalDoc, mockObjectService.Object);
            var clientNameTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.CLIENTNAMETAG)).Count();
            Assert.Zero(clientNameTagCount);
            
            var dateTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.DATETAG)).Count();
            Assert.Zero(dateTagCount);

            var edFirstNameTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.EDFIRSTNAMETAG)).Count();
            Assert.Zero(edFirstNameTagCount);

            var edNameTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.EDNAMETAG)).Count();
            Assert.Zero(edNameTagCount);

            var amFirstNameTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.AMFIRSTNAMETAG)).Count();
            Assert.Zero(amFirstNameTagCount);

            var amNameTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.AMNAMETAG)).Count();
            Assert.Zero(amNameTagCount);

            var edTitleTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.EDTITLETAG)).Count();
            Assert.Zero(edTitleTagCount);

            var amTitleTagCount = proposalDoc.Paragraphs.Where(p => p.Text.Contains(ProposalToolController.AMTITLETAG)).Count();
            Assert.Zero(amTitleTagCount);

            var clientName = formInstance.DataValues.Where(q => q.QuestionId == ProposalToolController.CLIENTNAMEQUESTIONID).FirstOrDefault().Value;
            var clientNameExists = proposalDoc.Paragraphs.Where(p => p.Text.Contains(clientName)).Count();
            Assert.Greater(clientNameExists, 0, "clientName replaced in the Document");

            var currentServicePrice = formInstance.DataValues.Where(q => q.QuestionId == ProposalToolController.CURRENTSERVICESPRICEQUESTIONID).FirstOrDefault().Value;
            var currentServicePriceExists = proposalDoc.Paragraphs.Where(p => p.Text.Contains(currentServicePrice)).Count();
            Assert.Greater(currentServicePriceExists, 0, "currentServiceName replaced in the Document");

            var proposedServicePrice = formInstance.DataValues.Where(q => q.QuestionId == ProposalToolController.PROPOSEDSERVICESPIRCEQUESTIONID).FirstOrDefault().Value;
            var proposedServicePriceExists = proposalDoc.Paragraphs.Where(p => p.Text.Contains(proposedServicePrice)).Count();
            Assert.Greater(proposedServicePriceExists, 0, "currentServiceName replaced in the Document");
        }

        #region private methods
        private AsicentralFormInstance _createFormInstance()
        {
            var formInstance = new AsicentralFormInstance()
            {
                Id = 1,
                Reference = Guid.NewGuid().ToString(),
                TypeId = 18,
                Email = "proposalTool@testCase.com",
                IPAddress = "127.0.0.1",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "ProposalToolControllerTest",
            };
            var formValue1 = _createFormValue(1, formInstance.Id, "TestClient Name", 1, null);
            var formValue2 = _createFormValue(2, formInstance.Id, "TestLogo.png", 2, null);
            var formValue3 = _createFormValue(3, formInstance.Id, "New Client", 3, null);
            var formValue4 = _createFormValue(3, formInstance.Id, "1", 4, null);
            var formValue5 = _createFormValue(3, formInstance.Id, "5", 5, null);
            var formValue6 = _createFormValue(3, formInstance.Id, "ESP Platform,Company Stores", 6, null);
            var formValue7 = _createFormValue(3, formInstance.Id, null, 7, null);
            var formValue8 = _createFormValue(3, formInstance.Id, "true", 8, null);
            var formValue9 = _createFormValue(3, formInstance.Id, "current Service1,current Service2,current Service3", 9, null);
            var formValue10 = _createFormValue(3, formInstance.Id, "5000", 10, null);
            var formValue11 = _createFormValue(3, formInstance.Id, "true", 11, null);
            var formValue12 = _createFormValue(3, formInstance.Id, "Proposed Service1,Proposed Service2,Proposed Service3,Proposed Service4", 12, null);
            var formValue13 = _createFormValue(3, formInstance.Id, "4000", 13, null);
            formInstance.DataValues = new List<FormDataValue>()
            {
                formValue1,
                formValue2,
                formValue3,
                formValue4,
                formValue5,
                formValue6,
                formValue7,
                formValue8,
                formValue9,
                formValue10,
                formValue11,
                formValue12,
                formValue13,
            };
            return formInstance;
        }

        private FormDataValue _createFormValue(int id, int instanceId, string value, int questionId, string updateValue)
        {
            return new FormDataValue()
            {
                Id = id,
                QuestionId = questionId,
                Value = value,
                UpdateValue = updateValue,
                InstanceId = instanceId,
                CreateDateUTC = DateTime.Now,
                UpdateDateUTC = DateTime.Now,
                UpdateSource = "ProposalToolControllerTest"
            };
        }

        private AsicentralFormQuestionOption _createQuestionOption(int id, int questionId, string description, string name, int sequence, string additionalData)
        {
            return new AsicentralFormQuestionOption()
            {
                Id = id,
                FormQuestionId = questionId,
                Sequence = sequence,
                Description = description,
                Name = name,
                AdditionalData = additionalData,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "ProposalToolControllerTest",
            };
        }
        #endregion
    }
}
