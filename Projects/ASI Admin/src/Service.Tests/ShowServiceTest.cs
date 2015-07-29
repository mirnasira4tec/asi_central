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
                ShowAddress objAddress = ShowHelper.CreateOrUpdateAddress(objectContext, new ShowAddress
                {
                    PhoneAreaCode = "1234",
                    Phone = "11231231234",
                    Cell = "9029123456",
                    FaxAreaCode = "12345",
                    Fax = "11231231234",
                    Title = "Title",
                    Street1 = "Street 1",
                    Street2 = "Street 2",
                    Zip = "11111",
                    State = "State",
                    CountryCode = "CountryCode",
                    Country = "Country",
                    City = "City",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    UpdateSource = "ShowServiceTest - AddressTest"
                });
                objectContext.Add<ShowAddress>(objAddress);
                Show objShow = ShowHelper.CreateOrUpdateShow(objectContext, new Show
                {
                    Name = "Orlando",
                    Address = objAddress,
                    ShowType = objShowType,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow,
                    UpdateSource = "ShowServiceTest - ShowTest"
                });
                objectContext.Add<Show>(objShow);
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
                objectContext.Delete<Show>(objShow);
                objectContext.Delete<ShowAddress>(objAddress);
                objectContext.Delete<ShowType>(objShowType);
                objectContext.SaveChanges();
                objShow = objectContext.GetAll<Show>().SingleOrDefault(ctxt => ctxt.Id == objShow.Id);
                Assert.IsNull(objShow);
            }

        }
    }
}
