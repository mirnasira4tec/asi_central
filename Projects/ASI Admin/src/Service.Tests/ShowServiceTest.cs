using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model.show;
using System.Collections.Generic;
using asi.asicentral.services;
using asi.asicentral.util.show;
using asi.asicentral.interfaces;
using StructureMap.Configuration.DSL;
using asi.asicentral.database.mappings;
using Moq;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ShowServiceTest
    {
        [TestMethod]
        public void ShowTypeTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectContext = new ObjectService(container))
            {
                ShowType objShowType = ShowHelper.CreateOrUpdateShowType(objectContext, new ShowType { Type = "East", UpdateSource = "ShowServiceTest - ShowTypeTest1" });
                objectContext.SaveChanges();
                Assert.IsNotNull(objShowType);
                Assert.AreNotEqual(objShowType.Id,0);
                Assert.AreEqual(objShowType.Type, "East");
                int id = objShowType.Id;
                objShowType.Type = "West";
                objShowType.UpdateSource = "ShowServiceTest - ShowTypeTest1";
                objShowType = ShowHelper.CreateOrUpdateShowType(objectContext, objShowType );
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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


        [TestMethod]
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



    }
}
