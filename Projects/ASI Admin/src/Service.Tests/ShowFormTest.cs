using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.services;
using NUnit.Framework;
using StructureMap.Configuration.DSL;
using System;
using System.Linq;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class ShowFormTest
    {
        [Test]
        public void CreateShowFormTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                string formName = "TravelForm";
                var formType = objectContext.GetAll<ShowFormType>(true).FirstOrDefault(t => t.Name.ToLower() == formName.ToLower());
                Assert.NotNull(formType);

                var showFormInstance = new ShowFormInstance()
                {
                    EmployeeAttendeeId = 7866,
                    RequestReference = "ae36936f-f9a7-4bb1-8622-4de80cd714a4",
                    Email = "test@asicentral.com",
                    Identity = "test",
                    SenderIP = "::1",
                    SubmitSuccessful = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "TestCase"
                };
                showFormInstance.TypeId = formType.TypeId;
                objectContext.Add(showFormInstance);
                objectContext.SaveChanges();
                Assert.AreNotEqual(showFormInstance.InstanceId, 0);

                var showFormPropertyValue = new ShowFormPropertyValue()
                {
                    Name = "Name",
                    Value = "Value",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "TestCase"
                };
                showFormPropertyValue.FormInstanceId = showFormInstance.InstanceId;
                objectContext.Add(showFormPropertyValue);
                objectContext.SaveChanges();
                Assert.AreNotEqual(showFormPropertyValue.PropertyValueId, 0);
            }
        }

        [Test]
        public void GetEmpAttendeeTravelForm()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                var empAttendeeId = 7866;
                var employeeAttendee = objectContext.GetAll<ShowEmployeeAttendee>(true).FirstOrDefault(t => t.Id == empAttendeeId);
                //Test to check Mapping From Employee Attendee to TravelForm
                var instanceFromEmpAttendee = employeeAttendee.TravelForms.FirstOrDefault();
                Assert.IsNotNull(instanceFromEmpAttendee);
                Assert.Greater(instanceFromEmpAttendee.InstanceId, 0); // 7
                //Test to check Mapping From TravelForm to Employee Attendee 
                var instanceFromDB = objectContext.GetAll<ShowFormInstance>(true).FirstOrDefault(t => t.InstanceId == instanceFromEmpAttendee.InstanceId);
                Assert.IsNotNull(instanceFromDB);
                Assert.AreEqual(instanceFromDB.InstanceId, instanceFromEmpAttendee.InstanceId);
                Assert.AreEqual(instanceFromDB.EmployeeAttendeeId, empAttendeeId);

                // Test to check Profile Request Mapping 
                var profileFromEmpAttendee = employeeAttendee.ProfileRequests.FirstOrDefault();
                Assert.IsNotNull(profileFromEmpAttendee);
                var profileRequestId = profileFromEmpAttendee.Id;// 638
                var profile = objectContext.GetAll<ShowProfileRequests>(true).FirstOrDefault(t => t.Id == profileRequestId);
                Assert.IsNotNull(profile);
                Assert.AreEqual(profileFromEmpAttendee.Id, profileRequestId);
            }
        }
    }
}
