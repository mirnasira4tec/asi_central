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
                IList<ContextFeatureProduct> featureProductList = objectContext.FeatureProducts.ToList();
                foreach (ContextFeatureProduct featureProduct in featureProductList)
                {
                    objectContext.FeatureProducts.Remove(featureProduct);
                }
                IList<ContextProductSequence> ProdSequenceList = objectContext.ProductSequences.ToList();
                foreach (ContextProductSequence prodSequence in ProdSequenceList)
                {
                    objectContext.ProductSequences.Remove(prodSequence);
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

            #region add the membership data

            Context distributorMembership = null;
            using (var objectContext = new ProductContext())
            {
                distributorMembership = new Context()
                {
                    Name = "Distributor no Processing Fee",
                    Type = "Distributor Membership",
                    Active = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Contexts.Add(distributorMembership);
                objectContext.SaveChanges();
                ContextProduct product = new ContextProduct()
                {
                    Name = "Membership",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "",
                    Sequence = 1,
                    Cost = 29.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    Name = "Basic",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                objectContext.SaveChanges();
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "Good",
                    Cost = 139.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    Name = "Standard",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "Better",
                    Cost = 199.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    Name = "Executive",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                distributorMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "Best Value!",
                    Cost = 219.99m,
                    Currency = "USD",
                    Frequency = "mo",
                    Sequence = 4,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                objectContext.SaveChanges();
                ContextFeature feature = new ContextFeature()
                {
                    Name = "<dfn>ASI Number</dfn>",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Free admission to <dfn>ASI Shows</dfn>",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Free industry <dfn>education</dfn> sessions",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "<dfn>ESP</dfn> Users",
                    Sequence = 4,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                ContextFeature subFeature = null;
                subFeature = new ContextFeature()
                {
                    Name = "Access to all ASI suppliers",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                subFeature = new ContextFeature()
                {
                    Name = "Presentations",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                subFeature = new ContextFeature()
                {
                    Name = "Order Management",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                subFeature = new ContextFeature()
                {
                    Name = "Mobile",
                    Sequence = 4,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                feature = new ContextFeature()
                {
                    Name = "<dfn>ESP Websites</dfn> Users",
                    Sequence = 5,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                subFeature = new ContextFeature()
                {
                    Name = "Ecommerce Site",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                subFeature = new ContextFeature()
                {
                    Name = "100's of designs",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                subFeature = new ContextFeature()
                {
                    Name = "Ready-to-use content",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                feature.ChildFeatures.Add(subFeature);
                feature = new ContextFeature()
                {
                    Name = "<dfn>CRM</dfn> Per Users",
                    Sequence = 6,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                distributorMembership.Features.Add(feature);
                objectContext.SaveChanges();
                int i = 0;
                foreach (ContextFeature featr in distributorMembership.Features.OrderBy(feat => feat.Sequence))
                {
                    if (i < 3)
                    {
                        foreach (ContextProductSequence prod in distributorMembership.Products.OrderBy(prodct => prodct.Sequence))
                        {
                            featr.AssociatedProducts.Add(new ContextFeatureProduct()
                            {
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow,
                                Label = "",
                                ProductId = prod.Product.ProductId,
                                Product = prod.Product,
                                UpdateSource = "ProductTest.PopulateDemoData",
                            });
                        }
                        objectContext.SaveChanges();
                    }
                    if (i == 3)
                    {
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "1",
                            Product = objectContext.Products.Where(prod => prod.Name == "Basic").SingleOrDefault(),
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "1",
                            Product = objectContext.Products.Where(prod => prod.Name == "Standard").SingleOrDefault(),
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "2",
                            Product = objectContext.Products.Where(prod => prod.Name == "Executive").SingleOrDefault(),
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        objectContext.SaveChanges();
                        foreach (ContextFeature subFeatr in featr.ChildFeatures)
                        {
                            foreach (ContextProductSequence prod in distributorMembership.Products.OrderBy(prodct => prodct.Sequence))
                            {
                                if (prod.Sequence > 1)
                                {
                                    subFeatr.AssociatedProducts.Add(new ContextFeatureProduct()
                                    {
                                        CreateDate = DateTime.UtcNow,
                                        UpdateDate = DateTime.UtcNow,
                                        Label = "",
                                        Product = prod.Product,
                                        ProductId = prod.Product.ProductId,
                                        UpdateSource = "ProductTest.PopulateDemoData",
                                    });
                                }
                            }
                            objectContext.SaveChanges();
                        }
                    }
                    if (i == 4)
                    {
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "1",
                            Product = objectContext.Products.Where(prod => prod.Name == "Standard").SingleOrDefault(),
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "1",
                            Product = objectContext.Products.Where(prod => prod.Name == "Executive").SingleOrDefault(),
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        objectContext.SaveChanges();
                        foreach (ContextFeature subFeatr in featr.ChildFeatures)
                        {
                            foreach (ContextProductSequence prod in distributorMembership.Products.OrderBy(prodct => prodct.Sequence))
                            {
                                if (prod.Sequence > 2)
                                {
                                    subFeatr.AssociatedProducts.Add(new ContextFeatureProduct()
                                    {
                                        CreateDate = DateTime.UtcNow,
                                        UpdateDate = DateTime.UtcNow,
                                        Label = "",
                                        Product = prod.Product,
                                        ProductId = prod.Product.ProductId,
                                        UpdateSource = "ProductTest.PopulateDemoData",
                                    });
                                }
                            }
                            objectContext.SaveChanges();
                        }
                    }
                    if (i == 5)
                    {
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "$35",
                            Product = objectContext.Products.Where(prod => prod.Name == "Membership").SingleOrDefault(),
                            ProductId = objectContext.Products.Where(prod => prod.Name == "Membership").SingleOrDefault().ProductId,
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "$35",
                            Product = objectContext.Products.Where(prod => prod.Name == "Basic").SingleOrDefault(),
                            ProductId = objectContext.Products.Where(prod => prod.Name == "Basic").SingleOrDefault().ProductId,
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "$30",
                            Product = objectContext.Products.Where(prod => prod.Name == "Standard").SingleOrDefault(),
                            ProductId = objectContext.Products.Where(prod => prod.Name == "Standard").SingleOrDefault().ProductId,
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "$25",
                            Product = objectContext.Products.Where(prod => prod.Name == "Executive").SingleOrDefault(),
                            ProductId = objectContext.Products.Where(prod => prod.Name == "Executive").SingleOrDefault().ProductId,
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        objectContext.SaveChanges();
                    }
                    i++;
                }
            }

            #endregion add the membership data

            #region Add the supplier data

            Context supplierMembership = null;
            using (var objectContext = new ProductContext())
            {
                supplierMembership = new Context()
                {
                    Type = "Supplier Membership",
                    Name = "Supplier no processing fees",
                    Active = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Contexts.Add(supplierMembership);
                objectContext.SaveChanges();
                ContextProduct product = new ContextProduct()
                {
                    Name = "Standard",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                supplierMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "",
                    Cost = 199m,
                    Currency = "USD",
                    Frequency = "mo",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    Name = "Sales Pro",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                supplierMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "",
                    Cost = 299m,
                    Currency = "USD",
                    Frequency = "mo",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    Name = "Show",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                supplierMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "",
                    Cost = 399m,
                    Currency = "USD",
                    Frequency = "mo",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                product = new ContextProduct()
                {
                    Name = "Advantage Show",
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                objectContext.Products.Add(product);
                //add product to the context
                supplierMembership.Products.Add(new ContextProductSequence()
                {
                    Product = product,
                    Qualifier = "",
                    Cost = 599m,
                    Currency = "USD",
                    Frequency = "mo",
                    Sequence = 4,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                });
                ContextFeature feature = new ContextFeature()
                {
                    Name = "<dfn>ASI Number</dfn>",
                    Sequence = 1,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Inclusion in the <dfn>ESP Database</dfn>",
                    Sequence = 2,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "<dfn>ESP Website</dfn> homepage banner ad",
                    Sequence = 3,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Inclusion in <dfn>LogoMall</dfn>",
                    Sequence = 4,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Placement in <dfn>Supplier Showcase</dfn>",
                    Sequence = 5,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "$300 of <dfn>ASI avertising</dfn> credit",
                    Sequence = 6,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "<dfn>Supplier Global Resource</dfn> subscription",
                    Sequence = 7,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Free training sessions",
                    Sequence = 8,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Booth space at 1 of out 5 <dfn>ASI Shows</dfn>",
                    Sequence = 9,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "1/4 page <dfn>Advantages ad placement</dfn>",
                    Sequence = 10,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "<dfn>EmailExpress</dfn> blast three times a year",
                    Sequence = 11,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                feature = new ContextFeature()
                {
                    Name = "Additional $200 ASI advertising credit",
                    Sequence = 12,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateSource = "ProductTest.PopulateDemoData",
                };
                supplierMembership.Features.Add(feature);
                objectContext.SaveChanges();
                int i = 0;
                foreach (ContextFeature featr in supplierMembership.Features.OrderBy(feat => feat.Sequence))
                {
                    if (i < 8)
                    {
                        foreach (ContextProductSequence prod in supplierMembership.Products.OrderBy(prodct => prodct.Sequence))
                        {
                            featr.AssociatedProducts.Add(new ContextFeatureProduct()
                            {
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow,
                                Label = "",
                                ProductId = prod.Product.ProductId,
                                Product = prod.Product,
                                UpdateSource = "ProductTest.PopulateDemoData",
                            });
                        }
                        objectContext.SaveChanges();
                    }
                    else if (i == 8)
                    {
                        foreach (ContextProductSequence prod in supplierMembership.Products.OrderBy(prodct => prodct.Sequence))
                        {
                            if (prod.Product.Name != "Standard")
                            {
                                featr.AssociatedProducts.Add(new ContextFeatureProduct()
                                {
                                    CreateDate = DateTime.UtcNow,
                                    UpdateDate = DateTime.UtcNow,
                                    Label = "",
                                    ProductId = prod.Product.ProductId,
                                    Product = prod.Product,
                                    UpdateSource = "ProductTest.PopulateDemoData",
                                });
                            }
                        }
                        objectContext.SaveChanges();
                    }
                    else if (i == 9)
                    {
                        foreach (ContextProductSequence prod in supplierMembership.Products.OrderBy(prodct => prodct.Sequence))
                        {
                            if (prod.Product.Name != "Standard" && prod.Product.Name != "Sales Pro")
                            {
                                featr.AssociatedProducts.Add(new ContextFeatureProduct()
                                {
                                    CreateDate = DateTime.UtcNow,
                                    UpdateDate = DateTime.UtcNow,
                                    Label = "",
                                    ProductId = prod.Product.ProductId,
                                    Product = prod.Product,
                                    UpdateSource = "ProductTest.PopulateDemoData",
                                });
                            }
                        }
                        objectContext.SaveChanges();
                    }
                    else if (i > 9)
                    {
                        ContextProductSequence prod = supplierMembership.Products.OrderBy(prodct => prodct.Sequence).Last();
                        featr.AssociatedProducts.Add(new ContextFeatureProduct()
                        {
                            CreateDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            Label = "",
                            ProductId = prod.Product.ProductId,
                            Product = prod.Product,
                            UpdateSource = "ProductTest.PopulateDemoData",
                        });
                        objectContext.SaveChanges();
                    }
                    i++;
                }

            }
            #endregion Add the supplier data
        }

        [TestMethod]
        public void ContextCrud()
        {
            int newId;
            string name = "Yann MemberShip";
            using (var objectContext = new ProductContext())
            {
                IList<Context> contextList = objectContext.Contexts.ToList();
                Assert.IsTrue(contextList.Count >= 0);
                Context context = new Context()
                {
                    Name = name,
                    Type = name,
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
                Context distributorMembership = objectContext.Contexts
                    .Include("Features.AssociatedProducts.Product")
                    .Where(ctxt => ctxt.Name == "Distributor no Processing Fee").SingleOrDefault();
                Assert.IsNotNull(distributorMembership);
                IList<ContextFeature> features = distributorMembership.Features.OrderBy(ctxFeature => ctxFeature.Sequence).ToList();
                Assert.IsTrue(features.Count > 0);
                ContextFeature feature = features.Where(feat => feat.Name == "<dfn>ESP</dfn> Users").SingleOrDefault();
                Assert.IsNotNull(feature);
                Assert.IsNotNull(feature.ChildFeatures);
                Assert.IsTrue(feature.ChildFeatures.Count > 0);
                Assert.IsTrue(feature.ChildFeatures.ElementAt(0).AssociatedProducts.Count > 0);
                Assert.IsTrue(feature.ChildFeatures.ElementAt(0).AssociatedProducts.ElementAt(0).ProductId > 0);
                feature = features.Where(feat => feat.Name == "<dfn>ASI Number</dfn>").SingleOrDefault();
                Assert.IsNotNull(feature);
                Assert.IsNotNull(feature.AssociatedProducts);
                foreach (ContextFeatureProduct featProd in feature.AssociatedProducts)
                {
                    Assert.IsTrue(featProd.ProductId > 0);
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
