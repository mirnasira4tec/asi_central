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
            int ShowTypeId;
            string Type = "East";
            using (var objectContext = new ShowContext())
            {
                int recordCount = objectContext.ShowType.Count();
                Assert.IsTrue(recordCount >= 0);
                ShowType context = new ShowType()
                {
                    Type = Type,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "Test Case",
                };
                objectContext.ShowType.Add(context);
                objectContext.SaveChanges();
                ShowTypeId = context.Id;
            }
            using (var objectContext = new ShowContext())
            {
                ShowType context = objectContext.ShowType.Where(ctxt => ctxt.Id == ShowTypeId).SingleOrDefault();
                Assert.IsNotNull(context);
                Assert.AreEqual(Type, context.Type);
                objectContext.ShowType.Remove(context);
                objectContext.SaveChanges();
            }
            using (var objectContext = new ShowContext())
            {
                ShowType context = objectContext.ShowType.Where(ctxt => ctxt.Id == ShowTypeId).SingleOrDefault();
                Assert.IsNull(context);
            }
        
            
        }

        [TestMethod]
        public void ShowTypeTest1()
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

    }
}
