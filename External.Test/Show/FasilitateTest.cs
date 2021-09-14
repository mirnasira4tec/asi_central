using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.database.mappings;
using NUnit.Framework;
using StructureMap.Configuration.DSL;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using asi.asicentral.model.show;
using asi.asicentral.util.show;
using asi.asicentral.model.store;

namespace External.Test.Show
{
    [TestFixture]
    public class FasilitateTest
    {
        Random rand = new Random();
        private ObjectService InitializeObjectService()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            return new ObjectService(container);
        }

        private ShowAttendee CreateAttendee(ObjectService objectService, int showId, int companyId)
        {
            ShowAttendee attendee = null;
            attendee = new ShowAttendee()
            {
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
            objectService.Add<ShowAttendee>(attendee);
            objectService.SaveChanges();
            return attendee.Id == 0 ? null : attendee;
        }

        private ShowEmployee CreateShowEmployee(ObjectService objectService, int companyId)
        {
            ShowEmployee employee = null;
            employee = new ShowEmployee()
            {
                FirstName = "Employee",
                MiddleName = "show",
                LastName = "Attendee",
                Email = "employee@attendee.com",
                LoginEmail = "employee" + rand.Next() + "@attendee.com",
                EPhoneAreaCode = "215",
                EPhone = "2323234",
                CompanyId = companyId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "FasilitateTest.cs - CreateShowEmployee"
            };
            objectService.Add<ShowEmployee>(employee);
            objectService.SaveChanges();
            return employee.Id == 0 ? null : employee;
        }

        private ShowEmployeeAttendee CreateShowEmployeeAttendee(ObjectService objectServices, int showAttendeeId, int showEmployeeId)
        {
            ShowEmployeeAttendee empAttendee = null;
            empAttendee = new ShowEmployeeAttendee()
            {
                AttendeeId = showAttendeeId,
                EmployeeId = showEmployeeId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "FasilitateTest.cs -  CreateShowEmployee",
                HasTravelForm = true,
                ProfileRequests = null,
                TravelForms = null,
                PriorityOrder = null,
            };
            objectServices.Add<ShowEmployeeAttendee>(empAttendee);
            objectServices.SaveChanges();
            return empAttendee.Id == 0 ? null : empAttendee;
        }

        private ShowFormInstance CreateTravelForm(ObjectService objectService, int? attendeeId, int? showEmployeeAttendeeId)
        {
            var formType = objectService.GetAll<ShowFormType>().Where(m => m.Name == "TravelForm").OrderByDescending(m => m.CreateDate).FirstOrDefault();//Travel Form
            ShowFormInstance formInstance = new ShowFormInstance()
            {
                TypeId = formType.Id,
                Email = "wesptest" + rand.Next() + "@mail.com",
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
            objectService.Add<ShowFormInstance>(formInstance);
            objectService.SaveChanges();
            var value = CreateFormPropertyValue(objectService, formInstance.Id, 1, "Property1", "One" + rand.Next());
            var value1 = CreateFormPropertyValue(objectService, formInstance.Id, 2, "Property2", "Two" + rand.Next());
            formInstance.PropertyValues = new List<ShowFormPropertyValue>() { value, value1 };
            return formInstance.Id == 0 ? null : formInstance;
        }

        private ShowFormPropertyValue CreateFormPropertyValue(ObjectService objectService, int formInstanceId, int sequence, string name, string value)
        {
            ShowFormPropertyValue showFormPropertyValue = new ShowFormPropertyValue()
            {
                FormInstanceId = formInstanceId,
                Sequence = sequence,
                Name = name,
                Value = value,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateSource = "Test Case"
            };
            objectService.Add<ShowFormPropertyValue>(showFormPropertyValue);
            objectService.SaveChanges();
            return showFormPropertyValue.Id == 0 ? null : showFormPropertyValue;
        }


        private ShowCompany CreateCompany(ObjectService objectService, string companyName, string asiNo, string memberType)
        {
            var objCompany = new ShowCompany();
            objCompany.Name = companyName + rand.Next();
            objCompany.WebUrl = "www.test.com";
            objCompany.MemberType = memberType;
            objCompany.ASINumber = asiNo;
            objCompany.SecondaryASINo = string.Empty;
            objCompany.UpdateSource = "FasilitateTest.cs - CreateCompany";
            objCompany.UpdateDate = DateTime.Now;
            objCompany.CreateDate = DateTime.Now;
            objectService.Add<ShowCompany>(objCompany);//adding company to Database
            objectService.SaveChanges();
            return objCompany.Id == 0 ? null : objCompany;
        }


        private ShowProfileRequests CreateProfileRequest(ObjectService objectService, int? attendeeId, int? employeeAttendeeId)
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
            profileRequests.RequestedBy = "Test Case";
            profileRequests.ApprovedBy = "Test Case";
            profileRequests.RequestReference = new Guid().ToString();
            profileRequests.CreateDate = DateTime.Now;
            profileRequests.UpdateDate = DateTime.Now;
            profileRequests.UpdateSource = "FasilitateTest.cs - Create Profile Request";
            objectService.Add<ShowProfileRequests>(profileRequests);
            objectService.SaveChanges();
            if (attendeeId.HasValue)
            {
                profileSupplierData = CreateSupplierData(objectService, profileRequests.Id, "39250", "supplierTest" + rand.Next() + "@asi.com", "SupplierTest Company", "supplierAttendee", "attendee" + rand.Next() + "asi.com");
                profileRequests.ProfileSupplierData = new List<ShowProfileSupplierData>() { profileSupplierData };
            }
            else if (employeeAttendeeId.HasValue)
            {
                profileDistributorData = CreateDistributorData(objectService, profileRequests.Id, "125724", "distributorTest" + rand.Next() + "@asi.com", "DistributorTest Company", "DistAttendee", "distattendee" + rand.Next() + "asi.com");
                profileRequests.ProfileDistributorData = new List<ShowProfileDistributorData>() { profileDistributorData };
            }
            return profileRequests.Id == 0 ? null : profileRequests;
        }

        private ShowProfileSupplierData CreateSupplierData(ObjectService objectService, int profileRequestId, string asiNo, string email, string companyName, string attendeeName, string attendeeEmail)
        {
            ShowProfileSupplierData profileSupplierData = new ShowProfileSupplierData();
            profileSupplierData.ProfileRequestId = profileRequestId;
            profileSupplierData.Email = email;
            profileSupplierData.CompanyName = companyName + rand.Next();
            profileSupplierData.ASINumber = asiNo;
            profileSupplierData.AttendeeImage = string.Empty;
            profileSupplierData.AttendeeName = attendeeName;
            profileSupplierData.AttendeeTitle = "Sales Person";
            profileSupplierData.AttendeeCommEmail = attendeeEmail;
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
            objectService.Add<ShowProfileSupplierData>(profileSupplierData);
            objectService.SaveChanges();
            return profileSupplierData.Id == 0 ? null : profileSupplierData;
        }

        private ShowProfileDistributorData CreateDistributorData(ObjectService objectService, int profileRequestId, string asiNo, string email, string companyName, string attendeeName, string attendeeEmail)
        {
            ShowProfileDistributorData profileDistributorData = new ShowProfileDistributorData();
            profileDistributorData.ProfileRequestId = profileRequestId;
            profileDistributorData.Email = email;
            profileDistributorData.CompanyName = companyName + rand.Next();
            profileDistributorData.ASINumber = asiNo;
            profileDistributorData.AttendeeImage = string.Empty;
            profileDistributorData.AttendeeName = attendeeName;
            profileDistributorData.AttendeeTitle = "Sales Person";
            profileDistributorData.AttendeeCommEmail = attendeeEmail;
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
            objectService.Add<ShowProfileDistributorData>(profileDistributorData);
            objectService.SaveChanges();
            return profileDistributorData.Id == 0 ? null : profileDistributorData;
        }

        [Test]
        [Ignore("Ignore a test")]
        public void DeleteDistributorAttendeeWithTravelForm()
        {
            using (var objectService = InitializeObjectService())
            {

                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Company", "125724", "distributor");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);

                var employee = CreateShowEmployee(objectService, company.Id);
                Assert.IsNotNull(employee);

                var showEmpAttendee = CreateShowEmployeeAttendee(objectService, attendee.Id, employee.Id);
                Assert.IsNotNull(showEmpAttendee);


                var formInstance = CreateTravelForm(objectService, null, showEmpAttendee.Id);
                Assert.IsNotNull(formInstance);

                showEmpAttendee.TravelForms = new List<ShowFormInstance>() { formInstance };

                ShowHelper.DeleteShowEmployeeAttendee(objectService, showEmpAttendee, "FistilitateTest.cs - DeleteDistributorAttendeeWithTravelForm"); // Unit Tested Function

                var employeeAttendee = objectService.GetAll<ShowEmployeeAttendee>().Where(m => m.EmployeeId == employee.Id && m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(employeeAttendee);

                var travelForm = objectService.GetAll<ShowFormInstance>().Where(m => m.EmployeeAttendeeId == showEmpAttendee.Id).FirstOrDefault();
                Assert.IsNull(travelForm);



                #region cleanUp
                for (int i = formInstance.PropertyValues.Count; i > 0; i--)
                {
                    objectService.Delete<ShowFormPropertyValue>(formInstance.PropertyValues[i - 1]);
                }
                objectService.Delete<ShowFormInstance>(formInstance);
                objectService.Delete<ShowEmployee>(employee);
                objectService.Delete<ShowAttendee>(attendee);
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion

            }

        }

        [Test]
        [Ignore("Ignore a test")]
        public void DeleteDistributorAttendeeWithTravelFormAndProfile()
        {
            using (var objectService = InitializeObjectService())
            {

                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Company", "125724", "distributor");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);

                var employee = CreateShowEmployee(objectService, company.Id);
                Assert.IsNotNull(employee);

                var showEmpAttendee = CreateShowEmployeeAttendee(objectService, attendee.Id, employee.Id);
                Assert.IsNotNull(showEmpAttendee);


                var formInstance = CreateTravelForm(objectService, null, showEmpAttendee.Id);
                Assert.IsNotNull(formInstance);

                showEmpAttendee.TravelForms = new List<ShowFormInstance>() { formInstance };

                var profileRequest = CreateProfileRequest(objectService, null, showEmpAttendee.Id);//profile Request
                Assert.IsNotNull(profileRequest);

                attendee.ProfileRequests = new List<ShowProfileRequests>() { profileRequest };

                ShowHelper.DeleteShowEmployeeAttendee(objectService, showEmpAttendee, "FistilitateTest.cs - DeleteDistributorAttendeeWithTravelForm"); // Unit Tested Function

                var employeeAttendee = objectService.GetAll<ShowEmployeeAttendee>().Where(m => m.EmployeeId == employee.Id && m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(employeeAttendee);

                var travelForm = objectService.GetAll<ShowFormInstance>().Where(m => m.EmployeeAttendeeId == showEmpAttendee.Id).FirstOrDefault();
                Assert.IsNull(travelForm);

                var request = objectService.GetAll<ShowProfileRequests>().Where(m => m.EmployeeAttendeeId == showEmpAttendee.Id).FirstOrDefault();
                Assert.IsNull(request);

                #region cleanUp
                for (int i = profileRequest.ProfileDistributorData.Count; i > 0; i--)
                {
                    objectService.Delete<ShowProfileDistributorData>(profileRequest.ProfileDistributorData[i - 1]);
                }
                objectService.Delete<ShowProfileRequests>(profileRequest);
                for (int i = formInstance.PropertyValues.Count; i > 0; i--)
                {
                    objectService.Delete<ShowFormPropertyValue>(formInstance.PropertyValues[i - 1]);
                }
                objectService.Delete<ShowFormInstance>(formInstance);
                objectService.Delete<ShowEmployee>(employee);
                objectService.Delete<ShowAttendee>(attendee);
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion

            }

        }
        [Test]
        [Ignore("Ignore a test")]
        public void DeleteDistributorAttendeeWithProfile()
        {
            using (var objectService = InitializeObjectService())
            {

                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Company", "125724", "distributor");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);

                var employee = CreateShowEmployee(objectService, company.Id);
                Assert.IsNotNull(employee);

                var showEmpAttendee = CreateShowEmployeeAttendee(objectService, attendee.Id, employee.Id);
                Assert.IsNotNull(showEmpAttendee);


                var profileRequest = CreateProfileRequest(objectService, null, showEmpAttendee.Id);//profile Request
                Assert.IsNotNull(profileRequest);

                attendee.ProfileRequests = new List<ShowProfileRequests>() { profileRequest };

                ShowHelper.DeleteShowEmployeeAttendee(objectService, showEmpAttendee, "FistilitateTest.cs - DeleteDistributorAttendeeWithTravelForm"); // Unit Tested Function

                var employeeAttendee = objectService.GetAll<ShowEmployeeAttendee>().Where(m => m.EmployeeId == employee.Id && m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(employeeAttendee);


                var request = objectService.GetAll<ShowProfileRequests>().Where(m => m.EmployeeAttendeeId == showEmpAttendee.Id).FirstOrDefault();
                Assert.IsNull(request);

                #region cleanUp
                for (int i = profileRequest.ProfileDistributorData.Count; i > 0; i--)
                {
                    objectService.Delete<ShowProfileDistributorData>(profileRequest.ProfileDistributorData[i - 1]);
                }
                objectService.Delete<ShowProfileRequests>(profileRequest);
                objectService.Delete<ShowEmployee>(employee);
                objectService.Delete<ShowAttendee>(attendee);
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion
            }
        }

        [Test]
        [Ignore("Ignore a test")]
        public void DeleteDistributorAttendeeWithOutTravelFormAndProfile()
        {
            using (var objectService = InitializeObjectService())
            {

                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Company", "125724", "distributor");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);

                var employee = CreateShowEmployee(objectService, company.Id);
                Assert.IsNotNull(employee);

                var showEmpAttendee = CreateShowEmployeeAttendee(objectService, attendee.Id, employee.Id);
                Assert.IsNotNull(showEmpAttendee);


                ShowHelper.DeleteShowEmployeeAttendee(objectService, showEmpAttendee, "FistilitateTest.cs - DeleteDistributorAttendeeWithTravelForm"); // Unit Tested Function

                var employeeAttendee = objectService.GetAll<ShowEmployeeAttendee>().Where(m => m.EmployeeId == employee.Id && m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(employeeAttendee);


                var request = objectService.GetAll<ShowProfileRequests>().Where(m => m.EmployeeAttendeeId == showEmpAttendee.Id).FirstOrDefault();
                Assert.IsNull(request);

                #region cleanUp
                objectService.Delete<ShowEmployee>(employee);
                objectService.Delete<ShowAttendee>(attendee);
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion
            }
        }

        [Test]
        [Ignore("Ignore a test")]
        public void DeleteSupplierAttendeeWithTravelFormAndProfile()
        {
            using (var objectService = InitializeObjectService())
            {
                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Supplier Company", "39250", "supplier");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);


                var formInstance = CreateTravelForm(objectService, attendee.Id, null);
                Assert.IsNotNull(formInstance);

                attendee.TravelForms = new List<ShowFormInstance>() { formInstance };

                var profileRequest = CreateProfileRequest(objectService, attendee.Id, null);//profile Request
                Assert.IsNotNull(profileRequest);

                attendee.ProfileRequests = new List<ShowProfileRequests>() { profileRequest };

                ShowHelper.DeleteShowAttendee(objectService, attendee, "FistilitateTest.cs - DeleteSupplierAttendeeWithTravelFormAndProfile"); // Unit Tested Function

                var showAttendee = objectService.GetAll<ShowAttendee>().Where(m => m.CompanyId == company.Id && m.ShowId == show.Id).FirstOrDefault();
                Assert.IsNull(showAttendee);

                var travelForm = objectService.GetAll<ShowFormInstance>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(travelForm);

                var request = objectService.GetAll<ShowProfileRequests>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(request);


                #region cleanUp
                for (int i = profileRequest.ProfileSupplierData.Count; i > 0; i--)
                {
                    objectService.Delete<ShowProfileSupplierData>(profileRequest.ProfileSupplierData[i - 1]);
                }
                objectService.Delete<ShowProfileRequests>(profileRequest);
                for (int i = formInstance.PropertyValues.Count; i > 0; i--)
                {
                    objectService.Delete<ShowFormPropertyValue>(formInstance.PropertyValues[i - 1]);
                }
                objectService.Delete<ShowFormInstance>(formInstance);
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion
            }
        }

        [Test]
        [Ignore("Ignore a test")]
        public void DeleteSupplierAttendeeWithTravelForm()
        {
            using (var objectService = InitializeObjectService())
            {
                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Supplier Company", "39250", "supplier");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);


                var formInstance = CreateTravelForm(objectService, attendee.Id, null);
                Assert.IsNotNull(formInstance);

                attendee.TravelForms = new List<ShowFormInstance>() { formInstance };


                ShowHelper.DeleteShowAttendee(objectService, attendee, "FistilitateTest.cs - DeleteSupplierAttendeeWithTravelForm"); // Unit Tested Function

                var showAttendee = objectService.GetAll<ShowAttendee>().Where(m => m.CompanyId == company.Id && m.ShowId == show.Id).FirstOrDefault();
                Assert.IsNull(showAttendee);

                var travelForm = objectService.GetAll<ShowFormInstance>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(travelForm);

                var request = objectService.GetAll<ShowProfileRequests>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(request);


                #region cleanUp
                for (int i = formInstance.PropertyValues.Count; i > 0; i--)
                {
                    objectService.Delete<ShowFormPropertyValue>(formInstance.PropertyValues[i - 1]);
                }
                objectService.Delete<ShowFormInstance>(formInstance);
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion
            }
        }

        [Test]
        [Ignore("Ignore a test")]
        public void DeleteSupplierAttendeeWithProfile()
        {
            using (var objectService = InitializeObjectService())
            {
                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Supplier Company", "39250", "supplier");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);


                var profileRequest = CreateProfileRequest(objectService, attendee.Id, null);//profile Request
                Assert.IsNotNull(profileRequest);

                attendee.ProfileRequests = new List<ShowProfileRequests>() { profileRequest };


                ShowHelper.DeleteShowAttendee(objectService, attendee, "FistilitateTest.cs - DeleteSupplierAttendeeWithProfile"); // Unit Tested Function

                var showAttendee = objectService.GetAll<ShowAttendee>().Where(m => m.CompanyId == company.Id && m.ShowId == show.Id).FirstOrDefault();
                Assert.IsNull(showAttendee);

                var travelForm = objectService.GetAll<ShowFormInstance>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(travelForm);

                var request = objectService.GetAll<ShowProfileRequests>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(request);


                #region cleanUp
                for (int i = profileRequest.ProfileSupplierData.Count; i > 0; i--)
                {
                    objectService.Delete<ShowProfileSupplierData>(profileRequest.ProfileSupplierData[i - 1]);
                }
                objectService.Delete<ShowProfileRequests>(profileRequest);
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion
            }
        }
        [Test]
        [Ignore("Ignore a test")]
        public void DeleteSupplierAttendeeWithoutTravelFormAndProfile()
        {
            using (var objectService = InitializeObjectService())
            {
                var show = objectService.GetAll<ShowASI>().Where(s => s.ShowTypeId == 5).OrderByDescending(m => m.CreateDate).FirstOrDefault();//5 is fasilitate showTypeId
                Assert.IsNotNull(show);

                var company = CreateCompany(objectService, "Test Supplier Company", "39250", "supplier");
                Assert.IsNotNull(company);

                var attendee = CreateAttendee(objectService, show.Id, company.Id);
                Assert.IsNotNull(attendee);

                ShowHelper.DeleteShowAttendee(objectService, attendee, "FistilitateTest.cs - DeleteSupplierAttendeeWithoutTravelFormAndProfile"); // Unit Tested Function

                var showAttendee = objectService.GetAll<ShowAttendee>().Where(m => m.CompanyId == company.Id && m.ShowId == show.Id).FirstOrDefault();
                Assert.IsNull(showAttendee);

                var travelForm = objectService.GetAll<ShowFormInstance>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(travelForm);

                var request = objectService.GetAll<ShowProfileRequests>().Where(m => m.AttendeeId == attendee.Id).FirstOrDefault();
                Assert.IsNull(request);

                #region cleanUp
                objectService.Delete<ShowCompany>(company);
                objectService.SaveChanges();
                #endregion
            }
        }
    }
}
