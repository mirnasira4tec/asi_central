using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.model.show.form;
using asi.asicentral.oauth;
using asi.asicentral.services;
using NUnit.Framework;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace External.Test.Common.database
{
    [TestFixture]
    public class UmbracoShowContextTest
    {
        private ObjectService _initializeObjectService()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            return new ObjectService(container);
        }

        [Test]
        public void CreateSHWFormInstance()
        {
            var objectService = _initializeObjectService();

            //creating show
            var show = _createShow(objectService, "Test SHW_Form");
            Assert.IsNotNull(show);

            // creating Showcase products forms
            var formType = objectService.GetAll<SHW_FormType>().FirstOrDefault(f => f.Name == "Showcase Products");
            if ( formType != null)
            {
                var showcaseForm = new SHW_FormInstance
                {
                    FormTypeId = formType.Id,
                    RequestReference = Guid.NewGuid().ToString(),
                    Status = "Created",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "Unit Test"                   
                };
                showcaseForm.PropertyValues = new List<SHW_FormPropertyValue>();
                objectService.Add(showcaseForm);
                objectService.SaveChanges();

                // add form questions
                var questions = objectService.GetAll<SHW_FormQuestion>()
                                             .Where(t => t.FormTypeId == formType.Id)
                                             .ToList();
                foreach(var question in questions )
                {
                    var formPropertyValue = new SHW_FormPropertyValue
                    {
                        FormInstanceId = showcaseForm.Id,
                        FormQuestionId = question.Id,
                        OrigValue = $"Value for Question: {question.Description}",
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        UpdateSource = "Unit Test"
                    };

                    showcaseForm.PropertyValues.Add(formPropertyValue);
                }

                objectService.SaveChanges();

                #region need to cleanUp all the objects created
                //objectService.Delete(showcaseForm);
                //objectService.SaveChanges();
                //objectService.Delete(show);
                //objectService.SaveChanges();
                #endregion
            }
        }

        private ShowASI _createShow(IObjectService objectService, string showName)
        {
            var show = new ShowASI
            {
                Name = showName,
                StartDate = DateTime.UtcNow.AddDays(30),
                EndDate = DateTime.UtcNow.AddDays(3),
                ShowTypeId = 4,
                Address = "4800 Streer Rd, Trevose",
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                UpdateSource = "Initial"
            };
            objectService.Add(show);
            objectService.SaveChanges();
            return show.Id != 0 ? show : null;
        }
    }
}