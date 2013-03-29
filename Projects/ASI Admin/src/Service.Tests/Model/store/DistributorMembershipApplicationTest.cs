using System;
using System.Linq;
using asi.asicentral.model.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace asi.asicentral.Tests.Model.store
{
    [TestClass]
    public class DistributorMembershipApplicationTest
    {
        [TestMethod]
        public void CopyTo_SyncDistContacts()
        {
            // prepare for DistributorMembershipApplication's CopyTo(DistributorMembershipApplication target)
            // for syncing contacts
            DistributorMembershipApplication distributorApplication = new DistributorMembershipApplication();
            DistributorMembershipApplication model = new DistributorMembershipApplication();
            distributorApplication.Contacts.Add(new DistributorMembershipApplicationContact() { Id = 1, Name = "contact2" });
            distributorApplication.Contacts.Add(new DistributorMembershipApplicationContact() { Id = 0, Name = "contact1" });
            distributorApplication.Contacts.Add(new DistributorMembershipApplicationContact() { Id = 3, Name = "contact3" });
            model.Contacts.Add(new DistributorMembershipApplicationContact() { Id = 0, Name = "new contact" });
            model.Contacts.Add(new DistributorMembershipApplicationContact() { Id = 1, Name = "new contact" });
            
            // model copy to target, target should have information provided by model.
            model.CopyTo(distributorApplication);
            Assert.AreEqual(distributorApplication.Contacts.Count, model.Contacts.Count);
            DistributorMembershipApplicationContact contact = null;
            contact = distributorApplication.Contacts.Where(c => c.Id == 3).SingleOrDefault();
            Assert.IsNull(contact);
            contact = distributorApplication.Contacts.Where(c => c.Id == 0).SingleOrDefault();
            Assert.IsNotNull(contact);
            contact = distributorApplication.Contacts.Where(c => c.Id == 1).SingleOrDefault();
            Assert.IsNotNull(contact);
        }

        [TestMethod]
        public void CopyTo_SyncAccountTypes()
        {
            // prepare for DistributorMembershipApplication's CopyTo(DistributorMembershipApplication target)
            // for syncing account types
            DistributorMembershipApplication distributorApplication = new DistributorMembershipApplication();
            DistributorMembershipApplication model = new DistributorMembershipApplication();
            distributorApplication.AccountTypes.Add(new DistributorAccountType() { Id = 0 });
            distributorApplication.AccountTypes.Add(new DistributorAccountType() { Id = 1 });
            model.AccountTypes.Add(new DistributorAccountType() { Id = 1 });
            model.AccountTypes.Add(new DistributorAccountType() { Id = 2 });
            model.AccountTypes.Add(new DistributorAccountType() { Id = 3 });

            // model copy to target, target should have model's new information
            model.CopyTo(distributorApplication);
            Assert.AreEqual(model.AccountTypes.Count, distributorApplication.AccountTypes.Count);
            DistributorAccountType type = null;
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 0).SingleOrDefault();
            Assert.IsNull(type);
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 1).SingleOrDefault();
            Assert.IsNotNull(type);
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 2).SingleOrDefault();
            Assert.IsNotNull(type);
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 3).SingleOrDefault();
            Assert.IsNotNull(type);
        }

        [TestMethod]
        public void CopyTo_SyncProductLines()
        {
            // prepare for DistributorMembershipApplication's CopyTo(DistributorMembershipApplication target)
            // for syncing product lines
            DistributorMembershipApplication distributorApplication = new DistributorMembershipApplication();
            DistributorMembershipApplication model = new DistributorMembershipApplication();
            distributorApplication.ProductLines.Add(new DistributorProductLine() { Id = 0 });
            distributorApplication.ProductLines.Add(new DistributorProductLine() { Id = 1 });
            distributorApplication.ProductLines.Add(new DistributorProductLine() { Id = 2 });
            model.ProductLines.Add(new DistributorProductLine() { Id = 1 });
            model.ProductLines.Add(new DistributorProductLine() { Id = 5 });

            // model copy to target, target should have model's new information
            model.CopyTo(distributorApplication);
            Assert.AreEqual(model.ProductLines.Count, distributorApplication.ProductLines.Count);
            DistributorProductLine productline = null;
            productline = distributorApplication.ProductLines.Where(line => line.Id == 0).SingleOrDefault();
            Assert.IsNull(productline);
            productline = distributorApplication.ProductLines.Where(line => line.Id == 5).SingleOrDefault();
            Assert.IsNotNull(productline);
            productline = distributorApplication.ProductLines.Where(line => line.Id == 1).SingleOrDefault();
            Assert.IsNotNull(productline);
        }
    }
}
