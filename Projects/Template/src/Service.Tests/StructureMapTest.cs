using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using asi.asicentral.database.mappings;
using asi.asicentral.model;
using asi.asicentral.services.interfaces;
using System.Collections.Generic;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class StructureMapTest
    {
        [TestMethod]
        public void LoadingEFRepository()
        {
            //make sure we load the appropriate EF concrete class with the right context
            var container = new Container(new EFRegistry());
            IRepository<Publication> publicationRepository = container.GetInstance<IRepository<Publication>>();
            Assert.IsNotNull(publicationRepository);
            IList<Publication> publications = publicationRepository.GetAll(true).ToList();
            Assert.IsTrue(publications.Count > 0);
            publicationRepository = container.GetInstance<IRepository<Publication>>("asi.asicentral.model.Publication");
            Assert.IsNotNull(publicationRepository);
        }
    }
}
