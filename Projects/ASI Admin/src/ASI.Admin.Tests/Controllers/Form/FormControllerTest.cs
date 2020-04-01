using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.store;
using asi.asicentral.web.Controllers.forms;
using asi.asicentral.web.Models.forms;
using Moq;
using NUnit.Framework;

namespace asi.asicentral.WebApplication.Tests.Controllers.Form
{
    [TestFixture]
    public class FormControllerTest
    {
        public FormModel CreateFormInstace()
        {
            List<FormInstance> instanceList = new List<FormInstance>();
            FormValue value = new FormValue()
            {
                Sequence = 1,
                Name = "Email",
                Value = "Test@asicentral.com",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Teste Case"
            };
            FormValue value1 = new FormValue()
            {
                Sequence = 2,
                Name = "Name",
                Value = "Test",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Teste Case"
            };
            FormType formType = new FormType()
            {
                Id = 12,
                Name = "Distributor Join Now Form",
                RequestType = "Distributor",
                Implementation = null,
                TermsAndConditions = "Distributor Membership",
                NotificationEmails = "arun.kumar@a4technology.com;manish.srivastava@a4technology.com",
                ContextId = null,
                IsASINumberFlag = false,
                IsObsolete = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Teste Case",
                //  FormInstances = new List<FormInstance>() { formInstance }
            };
            FormInstance formInstance = new FormInstance()
            {
                Id = 17,
                FormTypeId = 12,
                Email = "wesptest12456@mail.com",
                Salutation = "N.A.",
                Greetings = "N.A.",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Test Case",
                FormType = formType,
                Values = new List<FormValue>() { value, value1 }
            };
            instanceList.Add(formInstance);

            FormModel formModel = new FormModel();
            formModel.Form = formInstance;
            formModel.Command = "New";
            return formModel;
        }
        public ContextProduct CreateProduct(int id = 1)
        {
            ContextProduct product = new ContextProduct()
            {
                Name = "test product" + id.ToString(),
                Cost = 100,
                Id = id,
                Origin = "ASI"
            };
            return product;
        }

        [Test]
        public void PostSendFormTest()
        {
           
            FormModel model = CreateFormInstace();


            Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
            //  mockStoreService.Setup(service => service.GetAll<FormInstance>(false)).Returns(CreateFormInstace().AsQueryable());
            mockStoreService.Setup(objectService => objectService.Add<FormInstance>(It.IsAny<FormInstance>()))
                .Callback<FormInstance>((FormInstance) => model.Form = FormInstance);
            HttpContext.Current = MockContext();


            // create controller
            var controller = new FormsController();
            controller.StoreService = mockStoreService.Object;
            RedirectToRouteResult result = controller.PostSendForm(model) as RedirectToRouteResult;
            Assert.IsNotNull(model);
            Assert.AreEqual(result.RouteValues["controller"], "Forms");
        }
        static System.Web.HttpContext MockContext()
        {
            System.Web.HttpRequest request = new System.Web.HttpRequest("", "http://my.url.com", "");
            System.Web.HttpResponse response = new System.Web.HttpResponse(new StringWriter());
            System.Web.HttpContext context = new System.Web.HttpContext(request, response);
            //  context.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity("TestUser"), new string[0]);
            context.User = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.WindowsIdentity("TestUser"), new string[0]); 
            return context;
        }
        //public void FormDetailTest()
        //{
        //    Mock<IStoreService> mockStoreService = new Mock<IStoreService>();
        //    mockStoreService.Setup(service => service.GetAll<FormInstance>(false)).Returns(CreateFormInstace().AsQueryable());
        //}
    }
}
