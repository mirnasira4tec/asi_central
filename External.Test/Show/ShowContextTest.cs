using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.services;
using NUnit.Framework;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Test.Show
{
    [TestFixture]
    public class ShowContextTest
    {
        private ObjectService InitializeObjectService()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            return new ObjectService(container);
        }
        [Test]
        public void ShowAttendanceTest()
        {
            var objectService = InitializeObjectService();
            var profilePackages = objectService.GetAll<ProfilePackage>().ToList();
            Assert.IsTrue(profilePackages.Count > 0);

            var attendees = objectService.GetAll<ShowAttendee>().ToList();
            Assert.IsTrue(attendees.Count > 0);
        }

        [Test]
        public void CreateAndUpdateCompanyProfileTest()
        {
            var objectService = InitializeObjectService();
            var attendee = objectService.GetAll<ShowAttendee>().FirstOrDefault();

            //creating  show and attendee info
            var show = _createShow(objectService, "Lko Show");
            Assert.IsNotNull(show);
            //creating company
            var company = _createCompany(objectService, "Test Company", "39250", "supplier");
            Assert.IsNotNull(company);
            //creating ShowAttendee 
            var showAttendee = _createAttendee(objectService, show.Id, company.Id);

            //Get package from database
            var premiumPackage = objectService.GetAll<ProfilePackage>().FirstOrDefault(m => m.ProfilePackageName == "Premium");
            showAttendee.ProfilePackageId = premiumPackage.Id;

            #region creatingCompanyProfile
            //creating company profile from db.
            var companyProfile = _createCompanyProfile(company.Id, show.Id, objectService);
            Assert.IsNotNull(companyProfile);

            foreach (var option in premiumPackage.ProfilePackageOptions)
            {
                CompanyProfileData profileData = null;
                if (option.ProfileOption.ProfileOptionValues != null && option.ProfileOption.ProfileOptionValues.Count > 0)
                {
                    profileData = _createCompanyProfileData(companyProfile.Id, option.ProfileOption.Id, option.ProfileOption.ProfileOptionValues[0].Value + "Test Value", string.Empty, objectService);
                }
                else
                {
                    profileData = _createCompanyProfileData(companyProfile.Id, option.ProfileOption.Id, option.ProfileOption.ProfileOptionName + "Test Value", string.Empty, objectService);
                }
                Assert.IsNotNull(profileData);
            }

            //get created package from database
            var profile = objectService.GetAll<CompanyProfile>().FirstOrDefault(m => m.CompanyId == companyProfile.CompanyId && m.ShowId == companyProfile.ShowId);
            Assert.AreEqual(companyProfile.UserReference, profile.UserReference);
            Assert.AreEqual(companyProfile.CompanyProfileData.Count, profile.CompanyProfileData.Count);
            #endregion

            //Updating profile data
            foreach (var data in profile.CompanyProfileData)
            {
                data.UpdateValue = "Test UpdateValue";
            }
            objectService.SaveChanges();

            profile = objectService.GetAll<CompanyProfile>().FirstOrDefault(m => m.CompanyId == companyProfile.CompanyId && m.ShowId == companyProfile.ShowId);

            foreach (var data in profile.CompanyProfileData)
            {
                var companyData = companyProfile.CompanyProfileData.FirstOrDefault(m => m.ProfileOptionId == data.ProfileOptionId);
                Assert.AreEqual(data.OriginalValue, companyData.OriginalValue);
                Assert.AreEqual(data.UpdateValue, companyData.UpdateValue);
            }

            #region cleanUp
            for (int i = companyProfile.CompanyProfileData.Count; i > 0; i--)
            {
                objectService.Delete(companyProfile.CompanyProfileData[i - 1]);
            }
            objectService.Delete(companyProfile);
            objectService.SaveChanges();
            objectService.Delete(showAttendee);
            objectService.Delete(company);
            objectService.Delete(show);
            objectService.SaveChanges();
            #endregion
        }


        private CompanyProfile _createCompanyProfile(int companyId, int showId, IObjectService objectService)
        {
            var companyProfile = new CompanyProfile
            {
                UserReference = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                ShowId = showId,
                Status = "SUBMITTED",
                SubmitBy = "Test Case",
                CreateDateUTC = DateTime.UtcNow,
                UpdateDateUTC = DateTime.UtcNow,
                UpdateSource = "Test Case"
            };
            objectService.Add(companyProfile);
            objectService.SaveChanges();
            return companyProfile.Id > 0 ? companyProfile : null;
        }

        private CompanyProfileData _createCompanyProfileData(int companyProfileId, int profileOptionId, string originalValue, string updateValue, IObjectService objectService)
        {
            var companyProfileData = new CompanyProfileData
            {
                CompanyProfileId = companyProfileId,
                ProfileOptionId = profileOptionId,
                OriginalValue = originalValue,
                UpdateValue = updateValue,
                CreateDateUTC = DateTime.UtcNow,
                UpdateDateUTC = DateTime.UtcNow,
                UpdateSource = "Test Case",
            };
            objectService.Add(companyProfileData);
            objectService.SaveChanges();
            return companyProfileData.Id > 0 ? companyProfileData : null;
        }

        private ShowAttendee _createAttendee(IObjectService objectService, int showId, int companyId)
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

        private ShowCompany _createCompany(IObjectService objectService, string companyName, string asiNo, string memberType)
        {
            var objCompany = new ShowCompany();
            objCompany.Name = companyName;
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

        private ShowASI _createShow(IObjectService objectService, string showName)
        {
            var show = new ShowASI
            {
                Name = showName,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(3),
                ShowTypeId = 3,
                Address = "Gomti Nagar, Lucknow",
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
