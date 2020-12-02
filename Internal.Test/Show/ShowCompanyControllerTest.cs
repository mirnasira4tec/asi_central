using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.util.show;
using asi.asicentral.web.Controllers.Show;
using System.Web.Mvc;

namespace Internal.Test.Show
{
    [TestFixture]
    public class ShowCompanyControllerTest
    {
        private ShowAttendee CreateAttendee()
        {
            ShowAttendee attendee = new ShowAttendee()
            {
                Id = 3456,
                ShowId = 108,
                CompanyId = 2321,
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

        private ShowEmployeeAttendee CreateShowEmployeeAttendee()
        {
            ShowEmployeeAttendee empAttendee = new ShowEmployeeAttendee()
            {
                Id = 9753,
                AttendeeId = 1236,
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
                Id = 4565,
                Email = "wesptest@mail.com",
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
            var value = CreateFormPropertyValue(231, formInstance.Id, 1, "Property1", "One");
            var value1 = CreateFormPropertyValue(232, formInstance.Id, 2, "Property2", "Two");
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
            profileRequests.Id = 1232;
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
            profileSupplierData.Id = 5454;
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
            profileDistributorData.Id = 4234;
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

        [Test]
        public void DeleteAttendeeCompanyTest()
        {
           
            IList<ShowAttendee> attendees = new List<ShowAttendee>();
            var showAttendee = CreateAttendee();
            attendees.Add(showAttendee);

            showAttendee.EmployeeAttendees = new List<ShowEmployeeAttendee>();

            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<ShowAttendee>(false)).Returns(attendees.AsQueryable());
            mockObjectService.Setup(objectService => objectService.Delete<ShowAttendee>(showAttendee));
            ShowCompanyController controller = new ShowCompanyController();
            controller.ObjectService = mockObjectService.Object;

            RedirectToRouteResult actionResult = controller.DeleteAttendeeCompany(showAttendee.Id, showAttendee.ShowId.Value) as RedirectToRouteResult; // Unit Tested Function

            Assert.AreEqual(actionResult.RouteValues["action"], "GetAttendeeCompany");

            mockObjectService.Verify(objectService => objectService.Delete<ShowAttendee>(showAttendee), Times.Exactly(1));
            mockObjectService.Verify(objectService => objectService.SaveChanges(), Times.Exactly(1));
        }
    }
}
