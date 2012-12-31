using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.database;
using asi.asicentral.model;
using System.Collections.Generic;
using asi.asicentral.services.interfaces;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class ASIInternetTest
    {
        public const string connectionString = "name=ASIInternetContext";
        [TestMethod]
        public void PublicationTest()
        {
            //basic crud operations for Publication
            using (var context = new ASIInternetContext())
            {
                int count = context.Publications.Count();
                //make sure we have some
                Assert.IsTrue(count > 0);
                Publication pub = context.Publications.First();
                Assert.IsNotNull(pub);
                Assert.IsTrue(pub.Issues.Count > 0);
                //add a new one and then remove it
                Publication publication = new Publication()
                {
                    Name = DateTime.Now.ToShortDateString(),
                    PublicationId = count + 1,
                };
                context.Publications.Add(publication);
                context.SaveChanges();
                Assert.IsTrue(context.Publications.Count() == count + 1);
                context.Publications.Remove(publication);
                context.SaveChanges();
                Assert.IsTrue(context.Publications.Count() == count);
            }
        }

        [TestMethod()]
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

        [TestMethod()]
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
    }
}