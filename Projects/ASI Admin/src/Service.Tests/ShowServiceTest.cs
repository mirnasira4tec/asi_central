using System;
using System.Linq;
using asi.asicentral.database;
using asi.asicentral.model.show;
using System.Collections.Generic;
using asi.asicentral.services;
using asi.asicentral.util.show;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using asi.asicentral.database.mappings;
using Moq;
using System.Data.OleDb;
using System.Data;
using asi.asicentral.WebApplication;
using asi.asicentral.web.Controllers.Show;
using System.Web;
using System.IO;
using ClosedXML.Excel;
using System.Dynamic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class ShowServiceTest
    {
        [Test]
        public void ShowTypeTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowType objShowType = ShowHelper.CreateOrUpdateShowType(objectContext, new ShowType { Type = "East", UpdateSource = "ShowServiceTest - ShowTypeTest1" });
                objectContext.SaveChanges();
                Assert.IsNotNull(objShowType);
                Assert.AreNotEqual(objShowType.Id, 0);
                Assert.AreEqual(objShowType.Type, "East");
                int id = objShowType.Id;
                objShowType.Type = "West";
                objShowType.UpdateSource = "ShowServiceTest - ShowTypeTest1";
                objShowType = ShowHelper.CreateOrUpdateShowType(objectContext, objShowType);
                objectContext.SaveChanges();
                Assert.IsNotNull(objShowType);
                Assert.AreEqual(objShowType.Id, id);
                Assert.AreEqual(objShowType.Type, "West");
                objectContext.Delete<ShowType>(objShowType);
                objectContext.SaveChanges();
                objShowType = objectContext.GetAll<ShowType>().SingleOrDefault(ctxt => ctxt.Id == objShowType.Id);
                Assert.IsNull(objShowType);
            }
        }

        [Test]
        public void ShowTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowType objShowType = ShowHelper.CreateOrUpdateShowType(objectContext, new ShowType { Type = "East", UpdateSource = "ShowServiceTest - ShowTypeTest1" });
                objectContext.Add<ShowType>(objShowType);

                ShowASI objShow = ShowHelper.CreateOrUpdateShow(objectContext, new ShowASI
                {
                    Name = "Orlando",
                    Address = "Test",
                    ShowTypeId = objShowType.Id,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow,
                    UpdateSource = "ShowServiceTest - ShowTest"
                });
                objectContext.Add<ShowASI>(objShow);
                objectContext.SaveChanges();
                Assert.IsNotNull(objShow);
                Assert.AreNotEqual(objShow.Id, 0);
                Assert.AreEqual(objShow.Name, "Orlando");
                int id = objShow.Id;
                objShow.Name = "Chicago";
                objShow.UpdateSource = "ShowServiceTest - ShowTest";
                objShow = ShowHelper.CreateOrUpdateShow(objectContext, objShow);
                objectContext.SaveChanges();
                Assert.IsNotNull(objShow);
                Assert.AreEqual(objShow.Id, id);
                Assert.AreEqual(objShow.Name, "Chicago");
                objectContext.Delete<ShowASI>(objShow);
                objectContext.Delete<ShowType>(objShowType);
                objectContext.SaveChanges();
                objShow = objectContext.GetAll<ShowASI>().SingleOrDefault(ctxt => ctxt.Id == objShow.Id);
                Assert.IsNull(objShow);
            }

        }

        [Test]
        public void AddressTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowAddress objAddress = ShowHelper.CreateOrUpdateAddress(objectContext, new ShowAddress
                {
                    PhoneAreaCode = "1234",
                    Phone = "11231231234",
                    FaxAreaCode = "12345",
                    Fax = "11231231234",
                    Street1 = "Street 1",
                    Street2 = "Street 2",
                    Zip = "11111",
                    State = "State",
                    Country = "Country",
                    City = "City",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - AddressTest"
                });
                objectContext.SaveChanges();
                Assert.IsNotNull(objAddress);
                Assert.AreNotEqual(objAddress.Id, 0);
                int id = objAddress.Id;
                objAddress.PhoneAreaCode = "569328";
                objAddress.UpdateSource = "ShowServiceTest - ShowTest";
                objAddress = ShowHelper.CreateOrUpdateAddress(objectContext, objAddress);
                objectContext.SaveChanges();
                Assert.IsNotNull(objAddress);
                Assert.AreEqual(objAddress.Id, id);
                Assert.AreEqual(objAddress.PhoneAreaCode, "569328");
                objectContext.Delete<ShowAddress>(objAddress);
                objectContext.SaveChanges();
                objAddress = objectContext.GetAll<ShowAddress>().SingleOrDefault(ctxt => ctxt.Id == objAddress.Id);
                Assert.IsNull(objAddress);
            }

        }

        [Test]
        public void DistLogoTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowDistShowLogo objDistShowLogo = ShowHelper.CreateOrUpdateDistShowLogo(objectContext, new ShowDistShowLogo
                {
                    AttendeeId = 1764,
                    LogoImageUrl = "/fasilitate/logos/companyLogo/Orlando Show/banner_2017events.png",
                    UpdateSource = "ShowServiceTest - DistLogoTest",
                    UpdateDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                });
                objectContext.Add<ShowDistShowLogo>(objDistShowLogo);
                objectContext.SaveChanges();
                Assert.IsNotNull(objDistShowLogo);
            }
        }

        [Test]
        public void EmployeeTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowAddress objAddress = ShowHelper.CreateOrUpdateAddress(objectContext, new ShowAddress
                {
                    PhoneAreaCode = "1234",
                    Phone = "11231231234",
                    FaxAreaCode = "12345",
                    Fax = "11231231234",
                    Street1 = "Street 1",
                    Street2 = "Street 2",
                    Zip = "11111",
                    State = "State",
                    Country = "Country",
                    City = "City",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - AddressTest"
                });
                objectContext.Add<ShowAddress>(objAddress);
                ShowCompany objCompany = ShowHelper.CreateOrUpdateCompany(objectContext, new ShowCompany
                {
                    Name = "ComapnyName",
                    WebUrl = "www.company.com",
                    MemberType = "Distributor",
                    ASINumber = "32456",
                    LogoUrl = "/logo/logo.jpg",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - EmployeeTest"
                });
                objectContext.Add<ShowCompany>(objCompany);
                ShowEmployee objEmployee = ShowHelper.CreateOrUpdateEmployee(objectContext, new ShowEmployee
                {
                    FirstName = "FirstName",
                    MiddleName = "MiddleName",
                    LastName = "LastName",
                    Email = "email@email.com",
                    CompanyId = objCompany.Id,
                    Address = objAddress,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - EmployeeTest"
                });
                objectContext.SaveChanges();
                Assert.IsNotNull(objEmployee);
                Assert.AreNotEqual(objEmployee.Id, 0);
                int id = objEmployee.Id;
                objEmployee.FirstName = "FirstNameChange";
                objEmployee.UpdateSource = "ShowServiceTest - ShowTest";
                objEmployee = ShowHelper.CreateOrUpdateEmployee(objectContext, objEmployee);
                objectContext.SaveChanges();
                Assert.IsNotNull(objEmployee);
                Assert.AreEqual(objEmployee.Id, id);
                Assert.AreEqual(objEmployee.FirstName, "FirstNameChange");
                objectContext.Delete<ShowEmployee>(objEmployee);
                objectContext.Delete<ShowCompany>(objCompany);
                objectContext.Delete<ShowAddress>(objAddress);
                objectContext.SaveChanges();
                objEmployee = objectContext.GetAll<ShowEmployee>().SingleOrDefault(ctxt => ctxt.Id == objEmployee.Id);
                Assert.IsNull(objEmployee);
            }

        }

        [Test]
        public void ShowCompanyTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowCompany objCompany = ShowHelper.CreateOrUpdateCompany(objectContext, new ShowCompany
                {
                    Name = "ComapnyName",
                    WebUrl = "www.company.com",
                    MemberType = "Supplier",
                    ASINumber = "32456",
                    LogoUrl = "/logo/logo.jpg",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - ShowCompanyTest"
                });
                objectContext.Add<ShowCompany>(objCompany);
                ShowAddress objAddress = ShowHelper.CreateOrUpdateAddress(objectContext, new ShowAddress
                {
                    PhoneAreaCode = "1234",
                    Phone = "11231231234",
                    FaxAreaCode = "12345",
                    Fax = "11231231234",
                    Street1 = "Street 1",
                    Street2 = "Street 2",
                    Zip = "11111",
                    State = "State",
                    Country = "Country",
                    City = "City",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - AddressTest"
                });
                objectContext.Add<ShowAddress>(objAddress);
                ShowAddress objAddress1 = ShowHelper.CreateOrUpdateAddress(objectContext, new ShowAddress
                {
                    PhoneAreaCode = "1234",
                    Phone = "11231231234",
                    FaxAreaCode = "12345",
                    Fax = "11231231234",
                    Street1 = "Street 1",
                    Street2 = "Street 2",
                    Zip = "11111",
                    State = "State",
                    Country = "Country",
                    City = "City",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - AddressTest"
                });
                objectContext.Add<ShowAddress>(objAddress1);

                ShowCompanyAddress objCompanyAddress = ShowHelper.CreateOrUpdateCompanyAddress(objectContext, new ShowCompanyAddress
                {
                    CompanyId = objCompany.Id,
                    Address = objAddress,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - AddressTest"
                });
                objectContext.Add<ShowCompanyAddress>(objCompanyAddress);
                objectContext.SaveChanges();
                Assert.IsNotNull(objCompany);
                Assert.AreNotEqual(objCompany.Id, 0);
                Assert.IsNotNull(objCompanyAddress);
                Assert.AreNotEqual(objCompanyAddress.Company.Id, 0);
                Assert.AreNotEqual(objCompanyAddress.Address.Id, 0);
                Assert.IsNotNull(objCompanyAddress.Company.Id);
                Assert.IsNotNull(objCompanyAddress.Address);
                int id = objCompany.Id;
                objCompany.Name = "NameChange";
                objCompany.UpdateSource = "ShowServiceTest - ShowCompanyTest";
                objCompany = ShowHelper.CreateOrUpdateCompany(objectContext, objCompany);
                objectContext.SaveChanges();
                Assert.IsNotNull(objCompany);
                Assert.AreEqual(objCompany.Id, id);
                Assert.AreEqual(objCompany.Name, "NameChange");
                objectContext.Delete<ShowAddress>(objAddress);
                objectContext.Delete<ShowCompany>(objCompany);
                objectContext.Delete<ShowCompanyAddress>(objCompanyAddress);
                objectContext.SaveChanges();
                objCompany = objectContext.GetAll<ShowCompany>().SingleOrDefault(ctxt => ctxt.Id == objCompany.Id);
                Assert.IsNull(objCompany);
            }
        }

        [Test]
        public void Test()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowType objShowType = ShowHelper.CreateOrUpdateShowType(objectContext, new ShowType { Type = "East", UpdateSource = "ShowServiceTest - ShowTypeTest1" });
                objectContext.Add<ShowType>(objShowType);
                ShowAddress objAddress = ShowHelper.CreateOrUpdateAddress(objectContext, new ShowAddress
                {
                    PhoneAreaCode = "1234",
                    Phone = "11231231234",
                    FaxAreaCode = "12345",
                    Fax = "11231231234",
                    Street1 = "Street 1",
                    Street2 = "Street 2",
                    Zip = "11111",
                    State = "State",
                    Country = "Country",
                    City = "City",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - AddressTest"
                });
                objectContext.Add<ShowAddress>(objAddress);
                ShowASI objShow = ShowHelper.CreateOrUpdateShow(objectContext, new ShowASI
                {
                    Name = "Orlando",
                    Address = "test",
                    ShowTypeId = objShowType.Id,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow,
                    UpdateSource = "ShowServiceTest - ShowTest"
                });
                objectContext.Add<ShowASI>(objShow);

                objectContext.SaveChanges();
                Assert.IsNotNull(objShow);
                Assert.AreNotEqual(objShow.Id, 0);
                Assert.AreEqual(objShow.Name, "Orlando");
                int id = objShow.Id;
                objShow.Name = "Chicago";
                objShow.UpdateSource = "ShowServiceTest - ShowTest";
                objShow = ShowHelper.CreateOrUpdateShow(objectContext, objShow);
                objectContext.SaveChanges();
                Assert.IsNotNull(objShow);
                Assert.AreEqual(objShow.Id, id);
                Assert.AreEqual(objShow.Name, "Chicago");
                objectContext.Delete<ShowASI>(objShow);
                objectContext.Delete<ShowAddress>(objAddress);
                objectContext.Delete<ShowType>(objShowType);
                objectContext.SaveChanges();
                objShow = objectContext.GetAll<ShowASI>().SingleOrDefault(ctxt => ctxt.Id == objShow.Id);
                Assert.IsNull(objShow);
            }

        }
        [Test]
        public void ExcelUploadTest()
        {
            IList<ShowASI> shows = new List<ShowASI>();
            shows.Add(CreateShowData(4, "WEEK 1 - SOUTHEAST- Raleigh"));
            shows.Add(CreateShowData(5, "WEEK 1 - SOUTHEAST - Charlotte"));
            shows.Add(CreateShowData(6, "WEEK 1 - SOUTHEAST - Atlanta"));
            shows.Add(CreateShowData(7, "WEEK 1 - SOUTHEAST - Nashville"));

            IList<ShowCompany> supplierCompanies = new List<ShowCompany>();
            supplierCompanies.Add(CreateCompanyData(216, "12310", "Action Illustrated"));

            IList<ShowAddress> supplierCompanyAddresses = new List<ShowAddress>();
            supplierCompanyAddresses.Add(CreateAddressData(370, "N/A", "N/A", "N/A", "N/A", "N/A"));

            IList<ShowAttendee> attendees = new List<ShowAttendee>();
            attendees.Add(CreateAttendeeData(264, 216, 4, false, false, false, false, false));

            Mock<IObjectService> mockObjectService = new Mock<IObjectService>();
            mockObjectService.Setup(objectService => objectService.GetAll<ShowASI>(false)).Returns(shows.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<ShowCompany>(false)).Returns(supplierCompanies.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<ShowAddress>(false)).Returns(supplierCompanyAddresses.AsQueryable());
            mockObjectService.Setup(objectService => objectService.GetAll<ShowAttendee>(false)).Returns(attendees.AsQueryable());

            ExcelUploadController objExcel = new ExcelUploadController();
            objExcel.ObjectService = mockObjectService.Object;

            var dataTable = GetDataTable();
            Assert.IsNotNull(dataTable);

            var supplierCompany = objExcel.UpdateShowCompanyData(dataTable, 0);
            Assert.AreEqual(supplierCompanies.ElementAt(0).ASINumber, supplierCompany.ASINumber);
            Assert.AreEqual(supplierCompanies.ElementAt(0).Name, supplierCompany.Name);
            Assert.AreEqual(supplierCompanies.ElementAt(0).LogoUrl, supplierCompany.LogoUrl);

            var supplierCompanyAddress = objExcel.UpdateShowCompanyData(dataTable, 0);
            Assert.AreEqual(supplierCompanyAddresses.ElementAt(0).Street1, supplierCompanyAddress.CompanyAddresses[0].Address.Street1);
            Assert.AreEqual(supplierCompanyAddresses.ElementAt(0).City, supplierCompanyAddress.CompanyAddresses[0].Address.City);
            Assert.AreEqual(supplierCompanyAddresses.ElementAt(0).Zip, supplierCompanyAddress.CompanyAddresses[0].Address.Zip);

            var attendee = objExcel.UpdateShowCompanyData(dataTable, 0, 4);
            Assert.AreEqual(attendees.ElementAt(0).CompanyId, attendee.Attendees[1].CompanyId);
            Assert.AreEqual(attendees.ElementAt(0).ShowId, attendee.Attendees[1].ShowId);
        }

        [Test]
        public void GetProfileUpdateRequest()
        {
            using (var context = new Umbraco_ShowContext())
            {
                var request = context.ProfileRequests.OrderByDescending(x => x.Id).FirstOrDefault();
                Assert.IsNotNull(request);
            }
        }

        [Test]
        public void SupplierProfileRequest()
        {
            var request = RequestForAttendee(101);
            RequestProfile(request.Id);
            DeleteRequest(request.Id);
        }

        [Test]
        public void DistributorProfileRequest()
        {
            var request = RequestForEmployeeAttendee(5184);
            RequestDistributorProfile(request.Id);
            DeleteDistributorRequest(request.Id);
        }

        private ShowProfileRequests RequestForAttendee(int attnedeeId)
        {
            using (var context = new Umbraco_ShowContext())
            {
                //retrieve update field
                var fields = context.ProfileOptionalDataLabel.Where(s => !s.IsObsolete.HasValue && s.IsSupplier == true).ToList();
                Assert.IsNotNull(fields);
                String guid = Guid.NewGuid().ToString();
                var profileRequests = context.ProfileRequests.FirstOrDefault(x => x.AttendeeId == attnedeeId && x.Status == (int)ProfileRequestStatus.Pending);
                if (profileRequests == null)
                {
                    profileRequests = new ShowProfileRequests()
                    {
                        AttendeeId = attnedeeId,
                        RequestedBy = "rprajapati_unit",
                        RequestReference = guid,
                        Status = ProfileRequestStatus.Pending,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        UpdateSource = "Initial Unit Tests"
                    };
                    context.ProfileRequests.Add(profileRequests);
                    context.SaveChanges();
                    Assert.IsNotNull(profileRequests);
                }
                return profileRequests;
            }
        }

        private ShowProfileRequests RequestForEmployeeAttendee(int employeeAttendeeId)
        {
            using (var context = new Umbraco_ShowContext())
            {
                var profileRequests = context.ProfileRequests.FirstOrDefault(x => x.EmployeeAttendeeId == employeeAttendeeId && x.Status == (int)ProfileRequestStatus.Pending);
                var fields = context.ProfileOptionalDataLabel.Where(s => s.IsObsolete.HasValue && s.IsSupplier == true).ToList();
                if (profileRequests == null)
                {
                    profileRequests = new ShowProfileRequests()
                    {
                        EmployeeAttendeeId = employeeAttendeeId,
                        RequestedBy = "rprajapati_unit",
                        Status = ProfileRequestStatus.Pending,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        UpdateSource = "Initial Unit Tests"
                    };
                    context.ProfileRequests.Add(profileRequests);
                    context.SaveChanges();
                    Assert.IsNotNull(profileRequests);
                }
                return profileRequests;
            }
        }

        private void RequestProfile(int profileRequestsId)
        {
            using (var context = new Umbraco_ShowContext())
            {
                var profileRequiredData = context.ProfileSupplierData.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.IsUpdate == false);
                if (profileRequiredData == null)
                {
                    profileRequiredData = new ShowProfileSupplierData()
                    {
                        ProfileRequestId = profileRequestsId,
                        Email = "reena.prajapati@a4technology.com",
                        CompanyName = "A4 Tech",
                        ASINumber = "1234",
                        AttendeeName = "test Name",
                        AttendeeTitle = "test title",
                        AttendeeCommEmail = "test@test.com",
                        AttendeeCellPhone = "1234567892",
                        AttendeeWorkPhone = "9874563215",
                        CorporateAddress = "test Address",
                        City = "test City",
                        State = "test State",
                        Zip = "zip",
                        CompanyWebsite = "test.com",
                        ProductSummary = "test Product Summary",
                        TrustFromDistributor = "test Trust From Distributor",
                        SpecialServices = "test Special Services",
                        LoyaltyPrograms = "test Loyalty Programs",
                        Samples = "samples",
                        ProductSafety = "test Product Safety",
                        FactAboutCompany = "test Fact About Company",
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        UpdateSource = "Initial Unit Tests"
                    };

                    context.ProfileSupplierData.Add(profileRequiredData);
                    context.SaveChanges();
                    Assert.IsNotNull(profileRequiredData);
                }
                else
                {
                    profileRequiredData = context.ProfileSupplierData.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.IsUpdate == true);
                    if (profileRequiredData == null)
                    {
                        profileRequiredData = new ShowProfileSupplierData()
                        {
                            ProfileRequestId = profileRequestsId,
                            Email = "reena.prajapati1@a4technology.com",
                            CompanyName = "A4 Tech1",
                            ASINumber = "1234",
                            AttendeeName = "test Name",
                            AttendeeTitle = "test title",
                            AttendeeCommEmail = "test@test.com",
                            AttendeeCellPhone = "1234567892",
                            AttendeeWorkPhone = "9874563215",
                            CorporateAddress = "test Address",
                            City = "test City",
                            State = "test State",
                            Zip = "zip",
                            CompanyWebsite = "test.com",
                            ProductSummary = "test Product Summary",
                            TrustFromDistributor = "test Trust From Distributor",
                            SpecialServices = "test Special Services",
                            LoyaltyPrograms = "test Loyalty Programs",
                            Samples = "samples",
                            ProductSafety = "test Product Safety",
                            FactAboutCompany = "test Fact About Company",
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            UpdateSource = "Initial Unit Tests",
                            IsUpdate = true
                        };

                        context.ProfileSupplierData.Add(profileRequiredData);
                        context.SaveChanges();
                        Assert.IsNotNull(profileRequiredData);
                    }
                    else
                    {
                        profileRequiredData.CompanyWebsite = "test1.com";
                        context.SaveChanges();
                    }
                }
                var profileRequestOptionalDetails = context.ProfileOptionalDetails.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.ProfileOptionalDataLabelId == 1);
                if (profileRequestOptionalDetails == null)
                {
                    profileRequestOptionalDetails = new ShowProfileOptionalDetails()
                    {
                        ProfileRequestId = profileRequestsId,
                        ProfileOptionalDataLabelId = 1,
                        UpdateValue = "updateValue",
                        OrigValue = "origiValue",
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        UpdateSource = "Unit Test"
                    };

                    context.ProfileOptionalDetails.Add(profileRequestOptionalDetails);
                    context.SaveChanges();
                }
                else
                {
                    profileRequestOptionalDetails.UpdateValue = "updateValue1";
                    context.SaveChanges();
                }
                Assert.IsNotNull(profileRequestOptionalDetails);
            }
        }


        private void RequestDistributorProfile(int profileRequestsId)
        {
            using (var context = new Umbraco_ShowContext())
            {
                var profileRequiredData = context.ProfileDistributorData.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.IsUpdate == false);
                if (profileRequiredData == null)
                {
                    profileRequiredData = new ShowProfileDistributorData()
                    {

                        ProfileRequestId = profileRequestsId,
                        Email = "arun.kumar@a4technology.com",
                        CompanyName = "A4Technology",
                        ASINumber = "123452",
                        AttendeeName = "Arun Verma",
                        AttendeeTitle = "Sales Person",
                        AttendeeCommEmail = "arun.kumar@a4technology.com",
                        AttendeeCellPhone = "1223434545",
                        AttendeeWorkPhone = "1223434545",
                        AttendeeBiography = "From India",
                        Focus2018 = "Focus",
                        BussinessFrom = "Test Company",
                        SalesByCustomer = "Test Sales",
                        AnnualSalesVolume = "1000",
                        CatalogPercentage = 65.0m,
                        WebPercentage = 89.0m,
                        SpotPercentage = 67.0m,
                        DifferncFromOtherDistributor = "Test Other Distributor",
                        HasSupplierNetwork = true,
                        VendorContact = "Test Vendor Contanct",
                        PreviousBuyerEventAttendee = false,
                        BuyingGroupsDetail = "Test Buyer Group",
                        PreviousFasilitateAttendee = true,
                        FasilitateAttendedDetail = "New Jersey- 2015",
                        IsBuyingGroup = true,
                        ShowSample = "Generic",
                        SalesAids = "Test Sales",
                        SellingMode = "Online",
                        SalesChallenge = "Test Challenge",
                        IdealSupDescription = "Supplier Descriptions",
                        SupImportanceRating = "Supplier Importance Rating",
                        Importancelist = "Test List",
                        CorporateAddress = "Address",
                        City = "Test City",
                        State = "Test State",
                        Zip = "40218",
                        CompanyDescription = "Company Description",
                        CompanyAmtForProductSale = 5000,
                        AcceptTerms = true,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        UpdateSource = "Admin",
                        IsUpdate = true,
                        AttendeeImage = "test image path"
                    };
                    context.ProfileDistributorData.Add(profileRequiredData);
                    context.SaveChanges();
                    Assert.IsNotNull(profileRequiredData);
                }
                else
                {
                    profileRequiredData = context.ProfileDistributorData.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.IsUpdate == true);
                    if (profileRequiredData == null)
                    {
                        profileRequiredData = new ShowProfileDistributorData()
                        {

                            ProfileRequestId = profileRequestsId,
                            Email = "arun.kumar@a4technology.com",
                            CompanyName = "A4Technology",
                            ASINumber = "123452",
                            AttendeeName = "Arun Verma",
                            AttendeeTitle = "Sales Person",
                            AttendeeCommEmail = "arun.kumar@a4technology.com",
                            AttendeeCellPhone = "1223434545",
                            AttendeeWorkPhone = "1223434545",
                            AttendeeBiography = "From India",
                            Focus2018 = "Focus",
                            BussinessFrom = "Test Company",
                            SalesByCustomer = "Test Sales",
                            AnnualSalesVolume = "1000",
                            CatalogPercentage = 65.0m,
                            WebPercentage = 89.0m,
                            SpotPercentage = 67.0m,
                            DifferncFromOtherDistributor = "Test Other Distributor",
                            HasSupplierNetwork = true,
                            VendorContact = "Test Vendor Contanct",
                            PreviousBuyerEventAttendee = false,
                            BuyingGroupsDetail = "Test Buyer Group",
                            PreviousFasilitateAttendee = true,
                            FasilitateAttendedDetail = "New Jersey- 2015",
                            IsBuyingGroup = true,
                            ShowSample = "Generic",
                            SalesAids = "Test Sales",
                            SellingMode = "Online",
                            SalesChallenge = "Test Challenge",
                            IdealSupDescription = "Supplier Descriptions",
                            SupImportanceRating = "Supplier Importance Rating",
                            Importancelist = "Test List",
                            CorporateAddress = "Address",
                            City = "Test City",
                            State = "Test State",
                            Zip = "40218",
                            CompanyDescription = "Company Description",
                            CompanyAmtForProductSale = 5000,
                            AcceptTerms = true,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                            UpdateSource = "Admin",
                            IsUpdate = true,
                            AttendeeImage = "test image path"
                        };
                        context.ProfileDistributorData.Add(profileRequiredData);
                        context.SaveChanges();
                        Assert.IsNotNull(profileRequiredData);
                    }
                    else
                    {
                        context.SaveChanges();
                    }
                }

                var distributorOptionLableList = context.ProfileOptionalDataLabel.Where(m => m.IsDistributor == true).ToList();
                if (distributorOptionLableList != null && distributorOptionLableList.Count > 0)
                {
                    foreach (var distributorOptionalLable in distributorOptionLableList)
                    {
                        var profileRequestOptionalDetails = context.ProfileOptionalDetails
                                            .FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.ProfileOptionalDataLabelId == distributorOptionalLable.Id);
                        if (profileRequestOptionalDetails == null)
                        {
                            profileRequestOptionalDetails = new ShowProfileOptionalDetails()
                            {
                                ProfileRequestId = profileRequestsId,
                                ProfileOptionalDataLabelId = distributorOptionalLable.Id,
                                UpdateValue = "updateValue",
                                OrigValue = "origiValue",
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                UpdateSource = "Unit Test"
                            };

                            context.ProfileOptionalDetails.Add(profileRequestOptionalDetails);
                            context.SaveChanges();
                        }
                        else
                        {
                            profileRequestOptionalDetails.UpdateValue = "updateValue" + distributorOptionalLable.Id;
                            context.SaveChanges();
                        }
                        Assert.IsNotNull(profileRequestOptionalDetails); 
                    }
                }
               
            }
        }

        private void DeleteRequest(int profileRequestsId)
        {
            using (var context = new Umbraco_ShowContext())
            {
                var profileRequestOptionalDetails = context.ProfileOptionalDetails.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.ProfileOptionalDataLabelId == 1);
                context.ProfileOptionalDetails.Remove(profileRequestOptionalDetails);
                var profileRequiredData = context.ProfileSupplierData.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId);
                context.ProfileSupplierData.Remove(profileRequiredData);
                var profileRequests = context.ProfileRequests.FirstOrDefault(x => x.Id == profileRequestsId);
                context.ProfileRequests.Remove(profileRequests);
                context.SaveChanges();
            }
        }

        private void DeleteDistributorRequest(int profileRequestsId)
        {
            using (var context = new Umbraco_ShowContext())
            {
                 var distributorOptionLableList = context.ProfileOptionalDataLabel.Where(m => m.IsDistributor == true).ToList();
                 if (distributorOptionLableList != null && distributorOptionLableList.Count > 0)
                 {
                     foreach (var distributorOptionalLable in distributorOptionLableList)
                     {
                         var profileRequestOptionalDetails = context.ProfileOptionalDetails.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId && x.ProfileOptionalDataLabelId == distributorOptionalLable.Id);
                         context.ProfileOptionalDetails.Remove(profileRequestOptionalDetails);
                     }
                 }
                var profileRequiredData = context.ProfileDistributorData.FirstOrDefault(x => x.ProfileRequestId == profileRequestsId);
                context.ProfileDistributorData.Remove(profileRequiredData);
                var profileRequests = context.ProfileRequests.FirstOrDefault(x => x.Id == profileRequestsId);
                context.ProfileRequests.Remove(profileRequests);
                context.SaveChanges();
            }
        }

        private ShowAttendee CreateAttendee()
        {
            using (var context = new Umbraco_ShowContext())
            {
                var newAttendee = new ShowAttendee()
                {
                    CompanyId = 1,
                    ShowId = 1,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "Initial Unit Tests"
                };
                context.Attendee.Add(newAttendee);
                context.SaveChanges();
                Assert.IsNotNull(newAttendee);
                return newAttendee;
            }
        }

        [Test]
        public void ProfileUpdateRequestTest()
        {
            using (var context = new Umbraco_ShowContext())
            {
                var attendee = CreateAttendee();
                if (attendee != null)
                {
                    var request = RequestForAttendee(attendee.Id);
                    RequestProfile(request.Id);
                    context.Attendee.Attach(attendee);
                    context.Attendee.Remove(attendee);
                    var profileRequests = context.ProfileRequests.FirstOrDefault(x => x.AttendeeId == attendee.Id);
                    if (profileRequests != null)
                    {
                        Assert.AreEqual(profileRequests.AttendeeId, attendee.Id);
                        profileRequests.AttendeeId = null;
                    }
                    context.SaveChanges();
                    Assert.AreNotEqual(profileRequests.AttendeeId, attendee.Id);
                    DeleteRequest(profileRequests.Id);
                }
            }

        }

        public DataTable GetDataTable()
        {
            ExcelUploadController objExcel = new ExcelUploadController();
            DataTable dt = null;
            string tempPath = Path.GetTempPath();
            string currFilePath = tempPath + "Roadshow Import.xlsx";
            FileInfo fi = new FileInfo(currFilePath);
            var workBook = new XLWorkbook(fi.FullName);
            int totalsheets = workBook.Worksheets.Count;
            var worksheet = workBook.Worksheet(1);
            string[] columnNameList = null;
            var objShow = new ShowASI();
            var firstRowUsed = worksheet.FirstRowUsed();
            if (firstRowUsed != null)
            {
                var categoryRow = firstRowUsed.RowUsed();
                int coCategoryId = 1;
                Dictionary<int, string> keyValues = new Dictionary<int, string>();
                for (int cell = 1; cell <= categoryRow.CellCount(); cell++)
                {
                    keyValues.Add(cell, categoryRow.Cell(cell).GetString());
                }
                categoryRow = categoryRow.RowBelow();
                Registry registry = new EFRegistry();
                IContainer container = new Container(registry);
                using (var objectContext = new ObjectService(container))
                {
                    var matchShow = Regex.Match(worksheet.Name, @"^\s*WEEK\s+\d+\s*-\s*", RegexOptions.IgnoreCase);
                    if (matchShow.Success)
                    {
                        var weekNum = matchShow.Value.Trim();
                        var address = worksheet.Name.Substring(matchShow.Value.Length);
                        objShow = objectContext.GetAll<ShowASI>().Where(item => item.Name.Contains(weekNum.Replace("-", " -")) && item.Address.Contains(address.Trim()))
                                                                 .OrderByDescending(s => s.StartDate).FirstOrDefault();
                        columnNameList = new string[] { "ASINO", "Company", "IsCatalog", "Address", "City", "State", "Zip Code", "Country", "MemberType", "FirstName", "LastName" };
                    }
                }
                var containsAll = columnNameList.Where(x => keyValues.Values.Any(d => d.Contains(x))).ToList();
                if (containsAll.Count() == columnNameList.Count())
                {
                    Assert.AreEqual(containsAll.Count(), columnNameList.Count());
                }
                else
                {
                    Assert.AreNotEqual(containsAll.Count(), columnNameList.Count());
                }
                var parent = new List<IDictionary<string, object>>();
                while (!categoryRow.Cell(coCategoryId).IsEmpty())
                {
                    int count = 1;
                    var pc = new ExpandoObject();
                    while (count <= categoryRow.CellCount())
                    {
                        var data = categoryRow.Cell(count).Value;
                        ((IDictionary<string, object>)pc).Add(keyValues[count], data);
                        count++;
                    }

                    categoryRow = categoryRow.RowBelow();
                    parent.Add((IDictionary<string, object>)pc);
                }
                dt = objExcel.ToDictionary(parent);
            }
            return dt;
        }

        private DataSet GetDataset(int t, string conn)
        {
            DataSet ds = new DataSet();
            OleDbConnection excelConnection = new OleDbConnection(conn);
            excelConnection.Open();
            DataTable dt = new DataTable();
            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            String[] excelSheets = new String[dt.Rows.Count];

            foreach (DataRow row in dt.Rows)
            {
                excelSheets[t] = row["TABLE_NAME"].ToString();
                OleDbConnection excelConnection1 = new OleDbConnection(conn);
                string query = string.Format("Select * from [{0}]", excelSheets[t]);
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                {
                    dataAdapter.Fill(ds);
                }
            }
            return ds;
        }

        private ShowASI CreateShowData(int id, string name)
        {
            ShowASI objShow = new ShowASI()
            {
                Id = id,
                Name = name
            };
            return objShow;
        }

        private ShowCompany CreateCompanyData(int companyId, string asiNumber, string name, string logoUrl = null)
        {
            ShowCompany objCompany = new ShowCompany()
            {
                Id = companyId,
                ASINumber = asiNumber,
                Name = name,
                LogoUrl = logoUrl
            };
            return objCompany;
        }

        private ShowAddress CreateAddressData(int AddressId, string street1, string city, string state, string zip, string country)
        {
            ShowAddress objCompanyAddress = new ShowAddress()
            {
                Id = AddressId,
                Street1 = street1,
                City = city,
                State = state,
                Zip = zip,
                Country = country
            };
            return objCompanyAddress;
        }

        private ShowAttendee CreateAttendeeData(int attendeeId, int CompanyId, int showId, bool isSponsor, bool isPresentation, bool isRoundTable, bool isExhibitDay, bool isExisting)
        {
            ShowAttendee objShowAttendee = new ShowAttendee()
            {
                Id = attendeeId,
                CompanyId = CompanyId,
                ShowId = showId,
                IsSponsor = isSponsor,
                IsPresentation = isPresentation,
                IsRoundTable = isRoundTable,
                IsExhibitDay = isExhibitDay,
                IsExisting = isExisting
            };
            return objShowAttendee;
        }

        private ShowEmployee CreateEmployeeData(int employeeId, int CompanyId, string firstName, string lastName)
        {
            ShowEmployee objShowEmployee = new ShowEmployee()
            {
                Id = employeeId,
                CompanyId = CompanyId,
                FirstName = firstName,
                LastName = lastName
            };
            return objShowEmployee;
        }

        private ShowEmployeeAttendee CreateEmployeeAttendeeData(int employeeAttendeeId, int AttendeeId, int EmployeeId)
        {
            ShowEmployeeAttendee objShowEmployeeAttendee = new ShowEmployeeAttendee()
            {
                Id = employeeAttendeeId,
                AttendeeId = AttendeeId,
                EmployeeId = EmployeeId
            };
            return objShowEmployeeAttendee;
        }
    }
}
