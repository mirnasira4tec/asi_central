using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.counselor;
using asi.asicentral.model.sgr;
using asi.asicentral.services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ObjectServiceTest
    {
        [TestMethod]
        public void TestAddProducts()
        {
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                Company company = objectService.GetAll<Company>().FirstOrDefault();
                Category category = objectService.GetAll<Category>().Where(cat => cat.Id == Category.CATEGORY_ALL).Single();
                for (int i = 0; i < 2; i++)
                {
                    Product product = new Product();
                    product.Name = "Test " + i;
                    product.ModelNumber = "AAA";
                    product.Categories.Add(category);
                    //this will create an error if we have multiple instances of the context class
                    objectService.Add<Product>(product);
                }                
            }
        }

        [TestMethod]
        public void MultipleContextTest()
        {
            //make sure we can retrieve data from 2 separate contexts using the one object service
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                int rows = objectService.GetAll<Company>().Count();
                Assert.IsTrue(rows > 0);
                rows = objectService.GetAll<CounselorCategory>().Count();
                Assert.IsTrue(rows > 0);
            }
        }
    }
}
