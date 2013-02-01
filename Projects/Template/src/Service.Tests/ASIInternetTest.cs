using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model;
using System.Collections.Generic;
using asi.asicentral.interfaces;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ASIInternetTest
    {
        public const string connectionString = "name=ASIInternetContext";
        [TestMethod]
        public void PublicationTest()
        {
            int count = 0;
            Publication publication = null;
            //basic crud operations for Publication
            using (var context = new ASIInternetContext())
            {
                count = context.Publications.Count();
                //make sure we have some
                Assert.IsTrue(count > 0);
                Publication pub = context.Publications.First();
                Assert.IsNotNull(pub);
                Assert.IsTrue(pub.Issues.Count > 0);
                //add a new one and then remove it
                publication = new Publication()
                {
                    Name = DateTime.Now.ToShortDateString(),
                    PublicationId = count + 1,
                    Description = "Description",
                    StartDate = DateTime.UtcNow,
                    IsPublic = true,
                };
                context.Publications.Add(publication);
                context.SaveChanges();
                Assert.IsTrue(context.Publications.Count() == count + 1);
            }
            //clear the cach and make sure we can get all properties
            using (var context = new ASIInternetContext())
            {
                publication = context.Publications.Where(pub => pub.PublicationId == publication.PublicationId).FirstOrDefault();
                Assert.IsNotNull(publication);
                Assert.IsNotNull(publication.Name);
                Assert.IsNotNull(publication.Description);
                Assert.IsTrue(publication.IsPublic);
                Assert.IsTrue(DateTime.UtcNow.AddDays(-1).CompareTo(publication.StartDate) < 0); //make sure value is not the min value
                Assert.IsNull(publication.EndDate);
                context.Publications.Remove(publication);
                context.SaveChanges();
                Assert.IsTrue(context.Publications.Count() == count);
            }
        }

        [TestMethod]
        public void PublicationIssues()
        {
            using (var context = new ASIInternetContext())
            {
                int count = context.PublicationIssues.Count();
                Assert.IsTrue(count > 0);
                PublicationIssue issue = context.PublicationIssues.First();
                Assert.IsNotNull(issue);
                Assert.IsTrue(issue.Publications.Count > 0);
                PublicationIssue newIssue = new PublicationIssue()
                {
                    PublicationIssueId = count + 1,
                    Name = DateTime.Now.ToShortDateString(),
                };
                context.PublicationIssues.Add(newIssue);
                context.SaveChanges();
                Assert.IsTrue(context.PublicationIssues.Count() == count + 1);
                context.PublicationIssues.Remove(newIssue);
                context.SaveChanges();
                Assert.IsTrue(context.PublicationIssues.Count() == count);
            }
        }

        [TestMethod]
        public void PublicationIssueAssociation()
        {
            using (var context = new ASIInternetContext())
            {
                //create a publication
                Publication pub = new Publication()
                {
                    PublicationId = context.Publications.Count() + 1,
                    Name = DateTime.Now.ToShortDateString(),
                };
                PublicationIssue issue = new PublicationIssue()
                {
                    PublicationIssueId = context.PublicationIssues.Count() + 1,
                    Name = DateTime.Now.ToShortDateString(),
                };
                pub.Issues.Add(issue);
                context.Publications.Add(pub);
                context.SaveChanges();
                //remove the issue - check the relationship
                context.PublicationIssues.Remove(issue);
                context.SaveChanges();
                Assert.IsTrue(pub.Issues.Count() == 0);
                context.Publications.Remove(pub);
                context.SaveChanges();
            }
        }

        [TestMethod()]
        public void Repository()
        {

            using (IRepository<Publication> publicationRepository = new EFRepository<Publication>(new ASIInternetContext()))
            {
                IList<Publication> publications = publicationRepository.GetAll(true).ToList();
                int count = publications.Count;
                publications = publicationRepository.GetAll().ToList();
                Assert.AreEqual(count, publications.Count);
                Assert.IsTrue(count > 0);
                Publication newPub = new Publication()
                {
                    PublicationId = count + 1,
                    Name = DateTime.Now.ToShortDateString(),
                };
                publicationRepository.Add(newPub);
                publicationRepository.SaveChanges();
                Assert.IsTrue(publicationRepository.GetAll().Count() == count + 1);
                publicationRepository.Delete(newPub);
                publicationRepository.SaveChanges();
                Assert.IsTrue(publicationRepository.GetAll().Count() == count);
            }
        }

        [TestMethod]
        public void ModelTest()
        {
            Publication pub1 = new Publication() { PublicationId = 1, Name = "test" };
            Publication pub2 = new Publication() { PublicationId = 1, Name = "test2" };
            Assert.IsFalse(pub1.Equals(null));
            Assert.AreEqual(pub1, pub2);
            Assert.AreEqual("Publication: 1 - test", pub1.ToString());
        }
    }
}