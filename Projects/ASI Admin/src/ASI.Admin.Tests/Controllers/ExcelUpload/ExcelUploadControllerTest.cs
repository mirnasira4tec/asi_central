using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.web.Controllers.Show;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace asi.asicentral.WebApplication.Tests.Controllers.ExcelUpload
{
    [TestFixture]
    public class ExcelUploadControllerTest
    {
        private readonly Random _random = new Random();
        private const string mobileAppId = "a-008";


        private ShowEmployeeAttendee _createEmployeeAttendee(int showId, int companyId, string employeeEmail)
        {
            //create Attendee
            var attendee = new ShowAttendee()
            {
                Id = _random.Next(1000, 9999),
                ShowId = showId,
                CompanyId = companyId,
                BoothNumber = "xyz",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "ExcelploadControllerTest.cs -_createEmployeeAttendee",
            };

            //create Show Employee Attendee 
            var empAttendee = new ShowEmployeeAttendee
            {
                Id = _random.Next(1000, 9999),
                AttendeeId = (int)(attendee.Id != 0 ? attendee?.Id : 1236),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "ExcelploadControllerTest.cs -_createEmployeeAttendee",
                MobileAppID = null
            };

            //create Show Employee
            var employee = new ShowEmployee()
            {
                Id = _random.Next(1000, 9999),
                Email = employeeEmail,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "ExcelploadControllerTest.cs -  CreateShowEmployee",
            };

            empAttendee.Attendee = attendee;
            empAttendee.Employee = employee;
            return empAttendee;
        }

        [Test]
        public void UpdateMpbileAppIds_InSingleEmployeeAttendee_ShouldReturnEmployeeEmailListWithoutMobileAppId()
        {
            //arrange
            var mobileAppIdEmails = new Dictionary<string, string>();

            // create EmployeeAttendee List
            var employeeAttendee = new List<ShowEmployeeAttendee>();
            employeeAttendee.Add(_createEmployeeAttendee(241, 40, _random.Next(1000, 9999) + "@abc.com"));
            employeeAttendee.Add(_createEmployeeAttendee(241, 40, _random.Next(1000, 9999) + "@xyz.com"));
            mobileAppIdEmails.Add(employeeAttendee.Select(e => e.Employee.Email).FirstOrDefault(), mobileAppId);

            var objExcel = new ExcelUploadController();
            var mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<ShowEmployeeAttendee>(false)).Returns(employeeAttendee.AsQueryable());

            //act
            var emailList = objExcel.UpdateMobileAppIds(mockObjectService.Object, 241, mobileAppIdEmails);

            //assert
            Assert.AreEqual(emailList.Count, 1);
            Assert.AreEqual(employeeAttendee.Select(i => i.MobileAppID).FirstOrDefault(), mobileAppId);
        }

        [Test]
        public void UpdateMpbileAppIds_InAllEmployeeAttendee_ShouldReturnNoEmployeeEmailList()
        {
            //arrange
            var mobileAppIdEmails = new Dictionary<string, string>();

            // create EmployeeAttendee List 
            var employeeAttendee = new List<ShowEmployeeAttendee>();
            employeeAttendee.Add(_createEmployeeAttendee(241, 40, _random.Next(1000, 9999) + "@abc.com"));
            employeeAttendee.Add(_createEmployeeAttendee(241, 40, _random.Next(1000, 9999) + "@xyz.com"));
            mobileAppIdEmails = employeeAttendee.ToDictionary(x => x.Employee.Email, x => mobileAppId);

            var objExcel = new ExcelUploadController();
            var mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<ShowEmployeeAttendee>(false)).Returns(employeeAttendee.AsQueryable());

            //act
            var emailList = objExcel.UpdateMobileAppIds(mockObjectService.Object, 241, mobileAppIdEmails);

            //assert
            Assert.AreEqual(emailList.Count, 0);
            Assert.IsTrue(employeeAttendee.All(i => i.MobileAppID.Contains(mobileAppId)));
        }
    }
}
