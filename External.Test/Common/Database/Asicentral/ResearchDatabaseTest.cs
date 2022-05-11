using asi.asicentral.database.mappings;
using asi.asicentral.model.asicentral;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.interfaces;
using asi.asicentral.services;
using StructureMap.Configuration.DSL;

namespace External.Test.Common.Database.Asicentral
{
    [TestFixture]
    public class ResearchDatabaseTest
    {
        [Test]
        public void GetResearchDataTest()
        {
            Registry registry = new EFRegistry();
            IContainer container = new Container(registry);
            using (var objectService = new ObjectService(container))
            {
                var import = objectService.GetAll<ResearchImport>().FirstOrDefault();
                Assert.IsTrue(import.Id == 1);
                var values = objectService.GetAll<ResearchData>().Take(5).ToList();
                values = objectService.GetAll<ResearchData>().Where(d => d.Year == "2020").ToList();
                Assert.IsTrue(values.Count == 5);

            }
         }
    }
}
