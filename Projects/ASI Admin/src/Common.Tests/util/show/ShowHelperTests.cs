using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.util.show;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Common.Tests.util.show
{
    [TestFixture]
    public class ShowHelperTests
    {
        #region Private Methods
        private readonly Random _random = new Random();

        private ShowAttendee CreateAttendee(int showId, int companyId)
        {
            var attendee = new ShowAttendee()
            {
                Id = _random.Next(1000, 9999),
                ShowId = showId,
                CompanyId = companyId,
                IsSponsor = false,
                IsExhibitDay = false,
                IsPresentation = false,
                IsRoundTable = false,
                IsExisting = false,
                IsCatalog = false,
                BoothNumber = "xyz",
                HasTravelForm = false,
                DistShowLogos = null,
                EmployeeAttendees = null,
                ProfileRequests = null,
                TravelForms = null,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Test Case",
                IsNew = false
            };
            return attendee;
        }

        private ShowEmployee CreateShowEmployee()
        {
            var empAttendee = new ShowEmployee()
            {
                Id = _random.Next(1000, 9999),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "FasilitateTest.cs -  CreateShowEmployee"
            };
            return empAttendee;
        }

        private ShowEmployeeAttendee CreateShowEmployeeAttendee(int? attendeeid = null)
        {
            ShowEmployeeAttendee empAttendee = new ShowEmployeeAttendee()
            {
                Id = _random.Next(1000, 9999),
                AttendeeId = attendeeid.HasValue ? attendeeid.Value : 1236,
                EmployeeId = 8521,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "FasilitateTest.cs -  CreateShowEmployee",
                HasTravelForm = true,
                ProfileRequests = null,
                TravelForms = null,
                PriorityOrder = null,
            };
            return empAttendee;
        }

        private ShowFormInstance CreateTravelForm(int? attendeeId, int? showEmployeeAttendeeId)
        {
            ShowFormInstance formInstance = new ShowFormInstance()
            {
                TypeId = 1,
                Id = _random.Next(1000, 9999),
                Email = "wesptest" + _random.Next() + "@mail.com",
                RequestReference = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Test Case",
            };
            if (attendeeId.HasValue)
            {
                formInstance.AttendeeId = attendeeId.Value;
            }
            else if (showEmployeeAttendeeId.HasValue)
            {
                formInstance.EmployeeAttendeeId = showEmployeeAttendeeId.Value;
            }
            var value = CreateFormPropertyValue(231, formInstance.Id, 1, "Property1", "One" + _random.Next());
            var value1 = CreateFormPropertyValue(232, formInstance.Id, 2, "Property2", "Two" + _random.Next());
            formInstance.PropertyValues = new List<ShowFormPropertyValue>() { value, value1 };
            return formInstance;
        }

        private ShowFormPropertyValue CreateFormPropertyValue(int propertyValueId, int formInstanceId, int sequence, string name, string value)
        {
            ShowFormPropertyValue showFormPropertyValue = new ShowFormPropertyValue()
            {
                Id = propertyValueId,
                FormInstanceId = formInstanceId,
                Sequence = sequence,
                Name = name,
                Value = value,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Test Case"
            };
            return showFormPropertyValue;
        }

        private ShowProfileRequests CreateProfileRequest(int? attendeeId, int? employeeAttendeeId)
        {
            ShowProfileRequests profileRequests = new ShowProfileRequests();
            ShowProfileSupplierData profileSupplierData = null;
            ShowProfileDistributorData profileDistributorData = null;

            if (attendeeId.HasValue)
            {
                profileRequests.AttendeeId = attendeeId.Value;
            }
            else if (employeeAttendeeId.HasValue)
            {
                profileRequests.EmployeeAttendeeId = employeeAttendeeId.Value;
            }
            profileRequests.Id = _random.Next(1000, 9999);
            profileRequests.RequestedBy = "Test Case";
            profileRequests.ApprovedBy = "Test Case";
            profileRequests.RequestReference = new Guid().ToString();
            profileRequests.CreateDate = DateTime.Now;
            profileRequests.UpdateDate = DateTime.Now;
            profileRequests.UpdateSource = "FasilitateTest.cs - Create Profile Request";
            if (attendeeId.HasValue)
            {
                profileSupplierData = CreateSupplierData(profileRequests.Id);
                profileRequests.ProfileSupplierData = new List<ShowProfileSupplierData>() { profileSupplierData };
            }
            else if (employeeAttendeeId.HasValue)
            {
                profileDistributorData = CreateDistributorData(profileRequests.Id);
                profileRequests.ProfileDistributorData = new List<ShowProfileDistributorData>() { profileDistributorData };
            }
            return profileRequests;
        }

        private ShowProfileSupplierData CreateSupplierData(int profileRequestId)
        {
            ShowProfileSupplierData profileSupplierData = new ShowProfileSupplierData();
            profileSupplierData.Id = _random.Next(1000, 9999);
            profileSupplierData.ProfileRequestId = profileRequestId;
            profileSupplierData.Email = "supplierNoti@gmail.com";
            profileSupplierData.CompanyName = "Supplier Test Company";
            profileSupplierData.ASINumber = "39250";
            profileSupplierData.AttendeeImage = string.Empty;
            profileSupplierData.AttendeeName = "Test Attendee";
            profileSupplierData.AttendeeTitle = "Sales Person";
            profileSupplierData.AttendeeCommEmail = "suppAttend@gmail.com";
            profileSupplierData.AttendeeCellPhone = "1478523690";
            profileSupplierData.AttendeeWorkPhone = "3698521470";
            profileSupplierData.CorporateAddress = "CorPorate Addres";
            profileSupplierData.City = "WOODLAND PARK";
            profileSupplierData.State = "CO";
            profileSupplierData.Zip = "80863";
            profileSupplierData.CompanyWebsite = "www.attendeeShow.com";
            profileSupplierData.ProductSummary = "Summary";
            profileSupplierData.TrustFromDistributor = "Truested Distributor";
            profileSupplierData.SpecialServices = "Services are Special";
            profileSupplierData.LoyaltyPrograms = "Many programs for loyality";
            profileSupplierData.Samples = "Random";
            profileSupplierData.ProductSafety = "Products are Insured";
            profileSupplierData.FactAboutCompany = "Fortune 500 Company";
            profileSupplierData.IsUpdate = false;
            profileSupplierData.CreateDate = DateTime.Now;
            profileSupplierData.UpdateDate = DateTime.Now;
            profileSupplierData.UpdateSource = "FasilitateTest.cs - CreateSupplierData";
            return profileSupplierData;
        }

        private ShowProfileDistributorData CreateDistributorData(int profileRequestId)
        {
            ShowProfileDistributorData profileDistributorData = new ShowProfileDistributorData();
            profileDistributorData.Id = _random.Next(1000, 9999);
            profileDistributorData.ProfileRequestId = profileRequestId;
            profileDistributorData.Email = "wesp123@gmail.com";
            profileDistributorData.CompanyName = "Test WESP";
            profileDistributorData.ASINumber = "125724";
            profileDistributorData.AttendeeImage = string.Empty;
            profileDistributorData.AttendeeName = "Test Wesp";
            profileDistributorData.AttendeeTitle = "Sales Person";
            profileDistributorData.AttendeeCommEmail = "distAttendee@gmail.com";
            profileDistributorData.AttendeeCellPhone = "1478523690";
            profileDistributorData.AttendeeWorkPhone = "3698521470";
            profileDistributorData.AttendeeBiography = "Attendee Biography";
            profileDistributorData.Focus2018 = "Focus 2018";
            profileDistributorData.BussinessFrom = "Bussiness From";
            profileDistributorData.SalesByCustomer = "Sales By Customer";
            profileDistributorData.AnnualSalesVolume = "5000 unit";
            profileDistributorData.CatalogPercentage = "3";
            profileDistributorData.WebPercentage = "2";
            profileDistributorData.SpotPercentage = "5";
            profileDistributorData.DifferncFromOtherDistributor = "Have huge Client Base";
            profileDistributorData.HasSupplierNetwork = true;
            profileDistributorData.VendorContact = "5 years";
            profileDistributorData.PreviousBuyerEventAttendee = false;
            profileDistributorData.BuyingGroupsDetail = "Buying Groups Detail";
            profileDistributorData.FasilitateAttendedDetail = "Fasilitate Attended Detail";
            profileDistributorData.IsBuyingGroup = false;
            profileDistributorData.ShowSample = "sample";
            profileDistributorData.SalesAids = "500";
            profileDistributorData.SellingMode = "Home Delivery";
            profileDistributorData.SalesChallenge = "Services after Sales";
            profileDistributorData.IdealSupDescription = "Have good Products";
            profileDistributorData.SupImportanceRating = "4";
            profileDistributorData.Importancelist = "sup1, sup2";
            profileDistributorData.CorporateAddress = "address test street";
            profileDistributorData.CompanyDescription = "simple but best";
            profileDistributorData.CompanyAmtForProductSale = "50000";
            profileDistributorData.AcceptTerms = true;
            profileDistributorData.CorporateAddress = "CorPorate Addres";
            profileDistributorData.City = "WOODLAND PARK";
            profileDistributorData.State = "CO";
            profileDistributorData.Zip = "80863";
            profileDistributorData.IsUpdate = false;
            profileDistributorData.CreateDate = DateTime.Now;
            profileDistributorData.UpdateDate = DateTime.Now;
            profileDistributorData.UpdateSource = "FasilitateTest.cs - CreateSupplierData";
            return profileDistributorData;
        } 
        #endregion

        [Test]
        public void DeleteDistributorAttendee_WithTravelForm_ShouldSetEmpAttendeeIdToNull()
        {
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            var showEmpAttendee = CreateShowEmployeeAttendee(9753);
            var travelForm = CreateTravelForm(null, showEmpAttendee.Id);
            showEmpAttendee.TravelForms = new List<ShowFormInstance>() { travelForm };

            mockObjectService.Setup(objectService => objectService.Delete<ShowEmployeeAttendee>(showEmpAttendee));

            ShowHelper.DeleteShowEmployeeAttendee(mockObjectService.Object, showEmpAttendee, "ShowHelperTests - DeleteDistributorAttendeeWithTravelForm"); // Unit Tested Function
            Assert.IsNull(travelForm.EmployeeAttendeeId);
            mockObjectService.Verify(objectService => objectService.Delete<ShowEmployeeAttendee>(showEmpAttendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }

        [Test]
        public void DeleteShowAttendee_WithEmployeeAttendeeWithTravelform_ShouldSetEmpAttendeeIdToNull()
        {
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            // create Attendee with profile and travelform
            var attendee = CreateAttendee(108, 2321);
           
            // create Employee Attendee with travelform
            var employeeAttendee = CreateShowEmployeeAttendee(attendee.Id);
            var distProfileRequest = CreateProfileRequest(null, employeeAttendee.Id);
            employeeAttendee.ProfileRequests = new List<ShowProfileRequests>() { distProfileRequest };

            attendee.EmployeeAttendees = new List<ShowEmployeeAttendee>() { employeeAttendee };

            mockObjectService.Setup(objectService => objectService.Delete<ShowAttendee>(attendee));

            ShowHelper.DeleteShowAttendee(mockObjectService.Object, attendee, "ShowHelperTests - DeleteSupplierAttendeeWithTravelFormAndProfile"); // Unit Tested Function
            
            Assert.IsNull(distProfileRequest.EmployeeAttendeeId);

            mockObjectService.Verify(objectService => objectService.Delete<ShowAttendee>(attendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.Delete<ShowEmployeeAttendee>(employeeAttendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(2));
        }

        [Test]
        public void DeleteShowAttendee_WithEmployeeAttendeeWithDistProfile_ShouldSetAttendeeIdToNull()
        {
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            // create Attendee with profile and travelform
            var attendee = CreateAttendee(108, 2321);                        

            // create Employee Attendee with profile and travelform
            var employeeAttendee = CreateShowEmployeeAttendee(attendee.Id);
            var distTravelForm = CreateTravelForm(null, employeeAttendee.Id);
            employeeAttendee.TravelForms = new List<ShowFormInstance>() { distTravelForm };

            var distProfileRequest = CreateProfileRequest(null, employeeAttendee.Id);
            employeeAttendee.ProfileRequests = new List<ShowProfileRequests>() { distProfileRequest };

            attendee.EmployeeAttendees = new List<ShowEmployeeAttendee>() { employeeAttendee };

            mockObjectService.Setup(objectService => objectService.Delete<ShowAttendee>(attendee));

            ShowHelper.DeleteShowAttendee(mockObjectService.Object, attendee, "ShowHelperTests - DeleteSupplierAttendeeWithTravelFormAndProfile"); // Unit Tested Function
            Assert.IsNull(distTravelForm.EmployeeAttendeeId);
            Assert.IsNull(distProfileRequest.EmployeeAttendeeId);

            mockObjectService.Verify(objectService => objectService.Delete<ShowAttendee>(attendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.Delete<ShowEmployeeAttendee>(employeeAttendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(2));
        }

        [Test]
        public void DeleteShowAttendee_WithTravelform_ShouldSetAttendeeIdToNull()
        {
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            // create Attendee with profile and travelform
            var attendee = CreateAttendee(108, 2321);
            var suppTravelForm = CreateTravelForm(attendee.Id, null);
            attendee.TravelForms = new List<ShowFormInstance>() { suppTravelForm };
            var suppProfileRequest = CreateProfileRequest(attendee.Id, null);
            attendee.ProfileRequests = new List<ShowProfileRequests>() { suppProfileRequest };

            // create Employee Attendee with profile and travelform
            var employeeAttendee = CreateShowEmployeeAttendee(attendee.Id);
            var distTravelForm = CreateTravelForm(null, employeeAttendee.Id);
            employeeAttendee.TravelForms = new List<ShowFormInstance>() { distTravelForm };

            var distProfileRequest = CreateProfileRequest(null, employeeAttendee.Id);
            employeeAttendee.ProfileRequests = new List<ShowProfileRequests>() { distProfileRequest };

            attendee.EmployeeAttendees = new List<ShowEmployeeAttendee>() { employeeAttendee };

            mockObjectService.Setup(objectService => objectService.Delete<ShowAttendee>(attendee));

            ShowHelper.DeleteShowAttendee(mockObjectService.Object, attendee, "ShowHelperTests - DeleteSupplierAttendeeWithTravelFormAndProfile"); // Unit Tested Function
            Assert.IsNull(suppTravelForm.AttendeeId);
            Assert.IsNull(suppProfileRequest.AttendeeId);

            Assert.IsNull(distTravelForm.EmployeeAttendeeId);
            Assert.IsNull(distProfileRequest.EmployeeAttendeeId);

            mockObjectService.Verify(objectService => objectService.Delete<ShowAttendee>(attendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.Delete<ShowEmployeeAttendee>(employeeAttendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(2));
        }

        [Test]
        public void DeleteShowAttendee_WithSuppProfile_ShouldSetAttendeeIdToNull()
        {
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            // create Attendee with profile and travelform
            var attendee = CreateAttendee(108, 2321);
            var suppTravelForm = CreateTravelForm(attendee.Id, null);
            attendee.TravelForms = new List<ShowFormInstance>() { suppTravelForm };
            var suppProfileRequest = CreateProfileRequest(attendee.Id, null);
            attendee.ProfileRequests = new List<ShowProfileRequests>() { suppProfileRequest };

            // create Employee Attendee with profile and travelform
            var employeeAttendee = CreateShowEmployeeAttendee(attendee.Id);
            var distTravelForm = CreateTravelForm(null, employeeAttendee.Id);
            employeeAttendee.TravelForms = new List<ShowFormInstance>() { distTravelForm };

            var distProfileRequest = CreateProfileRequest(null, employeeAttendee.Id);
            employeeAttendee.ProfileRequests = new List<ShowProfileRequests>() { distProfileRequest };

            attendee.EmployeeAttendees = new List<ShowEmployeeAttendee>() { employeeAttendee };

            mockObjectService.Setup(objectService => objectService.Delete<ShowAttendee>(attendee));

            ShowHelper.DeleteShowAttendee(mockObjectService.Object, attendee, "ShowHelperTests - DeleteSupplierAttendeeWithTravelFormAndProfile"); // Unit Tested Function
            Assert.IsNull(suppTravelForm.AttendeeId);
            Assert.IsNull(suppProfileRequest.AttendeeId);

            Assert.IsNull(distTravelForm.EmployeeAttendeeId);
            Assert.IsNull(distProfileRequest.EmployeeAttendeeId);

            mockObjectService.Verify(objectService => objectService.Delete<ShowAttendee>(attendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.Delete<ShowEmployeeAttendee>(employeeAttendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(2));
        }
        
        public void AddOrDeleteShowEmployeeAttendance_WithAdd_ReturnsShowEmployeeAttendee()
        {
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            // create Attendee with profile and travelform
            var attendee = CreateAttendee(108, 2321);
            var showEmployee = CreateShowEmployee();

            var employeeAttendee = ShowHelper.AddOrDeleteShowEmployeeAttendance(mockObjectService.Object, attendee, showEmployee, true, "ShowHelperTests");
            Assert.IsNotNull(employeeAttendee);
            Assert.AreEqual(employeeAttendee.AttendeeId, attendee.Id);
            Assert.AreEqual(employeeAttendee.EmployeeId, showEmployee.Id);
        }

        public void AddOrDeleteShowEmployeeAttendance_WithDelete_ReturnsShowEmployeeAttendee()
        {
            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            // create Attendee with profile and travelform
            var attendee = CreateAttendee(108, 2321);
            var showEmployee = CreateShowEmployee();

            var employeeAttendee = ShowHelper.AddOrDeleteShowEmployeeAttendance(mockObjectService.Object, attendee, showEmployee, false, "ShowHelperTests"); // Unit Tested Function
            mockObjectService.Verify(objectService => objectService.Delete<ShowEmployeeAttendee>(employeeAttendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }
        
    }
}
