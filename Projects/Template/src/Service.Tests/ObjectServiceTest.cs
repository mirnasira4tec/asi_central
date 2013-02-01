using asi.asicentral.database.mappings;
using asi.asicentral.model;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ObjectServiceTest
    {
        [TestMethod]
        public void Publication()
        {
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                var publications = objectService.GetAll<Publication>(true).ToList();
                int count = publications.Count;
                Assert.IsTrue(count > 0);
                //add a new one and then remove it
                Publication publication = new Publication()
                {
                    Name = DateTime.Now.ToShortDateString(),
                    PublicationId = count + 1,
                };
                objectService.Add<Publication>(publication);
                objectService.SaveChanges();
                Assert.IsTrue(objectService.GetAll<Publication>(true).ToList().Count == count + 1);
                objectService.Delete<Publication>(publication);
                objectService.SaveChanges();
                Assert.IsTrue(objectService.GetAll<Publication>(true).ToList().Count == count);
            }
        }

        [TestMethod]
        public void Delete()
        {
            int count = 0;
            Publication publication = new Publication { Name = "DeleteTest" };
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                var publications = objectService.GetAll<Publication>(true).ToList();
                count = publications.Count;
                publication.PublicationId = count + 1;
                objectService.Add<Publication>(publication);
                objectService.SaveChanges();
            }
            //create a new object service which would not hold the state
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                objectService.Delete<Publication>(publication);
                objectService.SaveChanges();
            }
            //create a new object service which would not hold the state
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                int newCount = objectService.GetAll<Publication>(true).Count();
                Assert.AreEqual(count, newCount);
            }
        }
    }
}
