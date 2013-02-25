using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model.store;
using System.Collections.Generic;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ProductTest
    {
        [TestInitialize]
        public void PopulateDemoData()
        {
            //Delete the current data
            using (var objectContext = new ProductContext())
            {
                IList<ContextProductSequence> ProdSequenceList = objectContext.ProductSequences.ToList();
                foreach (ContextProductSequence prodSequence in ProdSequenceList)
                {
                    objectContext.ProductSequences.Remove(prodSequence);
                }
                IList<ContextFeatureProduct> featureProductList = objectContext.FeatureProducts.ToList();
                foreach (ContextFeatureProduct featureProduct in featureProductList)
                {
                    objectContext.FeatureProducts.Remove(featureProduct);
                }
                IList<ContextFeature> featureList = objectContext.Features.ToList();
                foreach (ContextFeature feature in featureList)
                {
                    objectContext.Features.Remove(feature);
                }
                IList<ContextProduct> productList = objectContext.Products.ToList();
                foreach (ContextProduct product in productList)
                {
                    objectContext.Products.Remove(product);
                }
                IList<Context> contextList = objectContext.Contexts.ToList();
                foreach (Context context in contextList)
                {
                    objectContext.Contexts.Remove(context);
                }
                objectContext.SaveChanges();
            }
            //add the membership data
            Context distributorMembership = null;
            using (var objectContext = new ProductContext())
            {
                distributorMembership = new Context()
                {
                    ContextId = Guid.NewGuid(),
                    Name = "Distributor Membership",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Contexts.Add(distributorMembership);
                ContextProduct product = new ContextProduct()
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Membership",
                    Cost = 29.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",                    
                };
                objectContext.Products.Add(product);
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    ContextProductSequenceId = Guid.NewGuid(),
                    Product = product,
                    Qualifier = "",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Basic",
                    Cost = 139.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    ContextProductSequenceId = Guid.NewGuid(),
                    Product = product,
                    Qualifier = "Good",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Standard",
                    Cost = 199.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    ContextProductSequenceId = Guid.NewGuid(),
                    Product = product,
                    Qualifier = "Better",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Executive",
                    Cost = 219.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    ContextProductSequenceId = Guid.NewGuid(),
                    Product = product,
                    Qualifier = "Best Value!",
                    Sequence = 4,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });

                ContextFeature feature = new ContextFeature()
                {
                    ContextFeatureId = Guid.NewGuid(),
                    Name = "ASI Number",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",                    
                };
                distributorMembership.Features.Add(feature);

                feature.AssociatedProducts.Add(new ContextFeatureProduct()
                {
                    ContextFeatureProductId = Guid.NewGuid(),
                    Label = "",
                    Product = objectContext.Products.Local.Where(prod => prod.Name == "Basic").FirstOrDefault(),
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                feature.AssociatedProducts.Add(new ContextFeatureProduct()
                {
                    ContextFeatureProductId = Guid.NewGuid(),
                    Label = "1",
                    Product = objectContext.Products.Local.Where(prod => prod.Name == "Standard").FirstOrDefault(),
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                feature.AssociatedProducts.Add(new ContextFeatureProduct()
                {
                    ContextFeatureProductId = Guid.NewGuid(),
                    Label = "2",
                    Product = objectContext.Products.Local.Where(prod => prod.Name == "Executive").FirstOrDefault(),
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                feature = new ContextFeature()
                {
                    ContextFeatureId = Guid.NewGuid(),
                    Name = "Free admission to ASI shows",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    ContextFeatureId = Guid.NewGuid(),
                    Name = "Free industry education sessions",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    ContextFeatureId = Guid.NewGuid(),
                    Name = "ESP Users",
                    Sequence = 4,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                ContextFeature subFeature = new ContextFeature();
                subFeature = new ContextFeature()
                {
                    ContextFeatureId = Guid.NewGuid(),
                    Name = "Access to all ASI suppliers",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                subFeature = new ContextFeature();
                subFeature = new ContextFeature()
                {
                    ContextFeatureId = Guid.NewGuid(),
                    Name = "Presentations",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                objectContext.SaveChanges();
            }
        }

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

        [TestMethod]
        public void RetrieveProductStructure()
        {
            using (var objectContext = new ProductContext())
            {
                Context distributorMembership = objectContext.Contexts.Include("Features.AssociatedProducts.Product").Where(ctxt => ctxt.Name == "Distributor Membership").SingleOrDefault();
                Assert.IsNotNull(distributorMembership);
                IList<ContextFeature> features = distributorMembership.Features.OrderBy(ctxFeature => ctxFeature.Sequence).ToList();
                Assert.IsTrue(features.Count > 0);
                ContextFeature feature = features.Where(feat => feat.Name == "ESP Users").SingleOrDefault();
                Assert.IsNotNull(feature);
                Assert.IsNotNull(feature.ChildFeatures);
                Assert.IsTrue(feature.ChildFeatures.Count > 0);
                feature = features.Where(feat => feat.Name == "ASI Number").SingleOrDefault();
                Assert.IsNotNull(feature);
                Assert.IsNotNull(feature.AssociatedProducts);
                foreach (ContextFeatureProduct featProd in feature.AssociatedProducts)
                {
                    Assert.IsNotNull(featProd.Product);
                }
                Assert.IsNotNull(distributorMembership.Products);
                Assert.IsTrue(distributorMembership.Products.Count > 0);
                foreach (ContextProductSequence prodSequence in distributorMembership.Products)
                {
                    Assert.IsNotNull(prodSequence.Product);
                }
            }
        }
    }
}
