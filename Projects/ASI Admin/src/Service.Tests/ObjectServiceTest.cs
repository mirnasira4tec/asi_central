using asi.asicentral.database.mappings;
using asi.asicentral.interfaces;
using asi.asicentral.model.sgr;
using asi.asicentral.services;
using NUnit.Framework;
using System.Linq;

namespace asi.asicentral.Tests
{
    [TestFixture]
    public class ObjectServiceTest
    {
        [Test]
        public void TestAddProducts()
        {
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                objectService.GetAll<Company>().FirstOrDefault();
				Category category = objectService.GetAll<Category>().Single(cat => cat.Id == Category.CATEGORY_ALL);
                for (int i = 0; i < 2; i++)
                {
                    var product = new Product()
                    {
						Name = "Test " + i,
						ModelNumber = "AAA",	                    
                    };
                    product.Categories.Add(category);
                    //this will create an error if we have multiple instances of the context class
                    objectService.Add<Product>(product);
                }                
            }
        }

        [Test]
        public void MultipleContextTest()
        {
            //make sure we can retrieve data from 2 separate contexts using the one object service
            using (IObjectService objectService = new ObjectService(new Container(new EFRegistry())))
            {
                int rows = objectService.GetAll<Company>().Count();
                Assert.IsTrue(rows > 0);
            }
        }
    }
}
