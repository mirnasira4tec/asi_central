using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model.product;
using System.Collections.Generic;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void ContextCrud()
        {
            Guid newId;
            string name = "Yann MemberShip";
            using (var objectContext = new ProductContext())
            {
                IList<Context> contextList = objectContext.Contexts.ToList();
                Assert.IsTrue(contextList.Count >= 0);
                Context context = new Context()
                {
                    ContextId = Guid.NewGuid(),
                    Name = name,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "Test Case",
                };
                objectContext.Contexts.Add(context);
                objectContext.SaveChanges();
                newId = context.ContextId;
            }
            using (var objectContext = new ProductContext())
            {
                Context context = objectContext.Contexts.Where(ctxt => ctxt.ContextId == newId).SingleOrDefault();
                Assert.IsNotNull(context);
                Assert.AreEqual(name, context.Name);
                objectContext.Contexts.Remove(context);
                objectContext.SaveChanges();
            }
            using (var objectContext = new ProductContext())
            {
                Context context = objectContext.Contexts.Where(ctxt => ctxt.ContextId == newId).SingleOrDefault();
                Assert.IsNull(context);
            }
        }
    }
}
