using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database.mappings;
using asi.asicentral.model;
using asi.asicentral.interfaces;
using System.Collections.Generic;
using asi.asicentral.services;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class StructureMapTest
    {
        [TestMethod]
        public void LoadingEFRepository()
        {
            //make sure we load the appropriate EF concrete class with the right context
            IContainer container = new Container(new EFRegistry());
            IRepository<Publication> publicationRepository = container.GetInstance<IRepository<Publication>>();
            Assert.IsNotNull(publicationRepository);
            IList<Publication> publications = publicationRepository.GetAll(true).ToList();
            Assert.IsTrue(publications.Count > 0);
            publicationRepository = container.GetInstance<IRepository<Publication>>();
            Assert.IsNotNull(publicationRepository);
        }
    }
}
