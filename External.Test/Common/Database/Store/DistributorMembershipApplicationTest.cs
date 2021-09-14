using System;
using System.Linq;
using asi.asicentral.model.store;
using System.Collections.Generic;
using NUnit.Framework;

namespace asi.asicentral.Tests.Model.store
{
    [TestFixture]
    public class DistributorMembershipApplicationTest
    {
        [Test]
        public void CopyTo_SyncDistContacts()
        {
            // prepare for DistributorMembershipApplication's CopyTo(DistributorMembershipApplication target)
            // for syncing contacts
            LegacyDistributorMembershipApplication distributorApplication = new LegacyDistributorMembershipApplication();
            LegacyDistributorMembershipApplication model = new LegacyDistributorMembershipApplication();
            distributorApplication.Contacts.Add(new LegacyDistributorMembershipApplicationContact() { Id = 1, Name = "contact2" });
            distributorApplication.Contacts.Add(new LegacyDistributorMembershipApplicationContact() { Id = 0, Name = "contact1" });
            distributorApplication.Contacts.Add(new LegacyDistributorMembershipApplicationContact() { Id = 3, Name = "contact3" });
            model.Contacts.Add(new LegacyDistributorMembershipApplicationContact() { Id = 0, Name = "new contact" });
            model.Contacts.Add(new LegacyDistributorMembershipApplicationContact() { Id = 1, Name = "new contact" });
            
            // model copy to target, target should have information provided by model.
            model.CopyTo(distributorApplication);
            Assert.AreEqual(distributorApplication.Contacts.Count, model.Contacts.Count);
            LegacyDistributorMembershipApplicationContact contact = null;
            contact = distributorApplication.Contacts.Where(c => c.Id == 3).SingleOrDefault();
            Assert.IsNull(contact);
            contact = distributorApplication.Contacts.Where(c => c.Id == 0).SingleOrDefault();
            Assert.IsNotNull(contact);
            contact = distributorApplication.Contacts.Where(c => c.Id == 1).SingleOrDefault();
            Assert.IsNotNull(contact);
        }

        [Test]
        public void CopyTo_SyncAccountTypes()
        {
            // prepare for DistributorMembershipApplication's CopyTo(DistributorMembershipApplication target)
            // for syncing account types
            LegacyDistributorMembershipApplication distributorApplication = new LegacyDistributorMembershipApplication();
            LegacyDistributorMembershipApplication model = new LegacyDistributorMembershipApplication();
            distributorApplication.AccountTypes.Add(new LegacyDistributorAccountType() { Id = 0 });
            distributorApplication.AccountTypes.Add(new LegacyDistributorAccountType() { Id = 1 });
            model.AccountTypes.Add(new LegacyDistributorAccountType() { Id = 1 });
            model.AccountTypes.Add(new LegacyDistributorAccountType() { Id = 2 });
            model.AccountTypes.Add(new LegacyDistributorAccountType() { Id = 3 });

            // model copy to target, target should have model's new information
            model.CopyTo(distributorApplication);
            Assert.AreEqual(model.AccountTypes.Count, distributorApplication.AccountTypes.Count);
            LegacyDistributorAccountType type = null;
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 0).SingleOrDefault();
            Assert.IsNull(type);
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 1).SingleOrDefault();
            Assert.IsNotNull(type);
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 2).SingleOrDefault();
            Assert.IsNotNull(type);
            type = distributorApplication.AccountTypes.Where(theType => theType.Id == 3).SingleOrDefault();
            Assert.IsNotNull(type);
        }

        [Test]
        public void CopyTo_SyncProductLines()
        {
            // prepare for DistributorMembershipApplication's CopyTo(DistributorMembershipApplication target)
            // for syncing product lines
            LegacyDistributorMembershipApplication distributorApplication = new LegacyDistributorMembershipApplication();
            LegacyDistributorMembershipApplication model = new LegacyDistributorMembershipApplication();
            distributorApplication.ProductLines.Add(new LegacyDistributorProductLine() { Id = 0 });
            distributorApplication.ProductLines.Add(new LegacyDistributorProductLine() { Id = 1 });
            distributorApplication.ProductLines.Add(new LegacyDistributorProductLine() { Id = 2 });
            model.ProductLines.Add(new LegacyDistributorProductLine() { Id = 1 });
            model.ProductLines.Add(new LegacyDistributorProductLine() { Id = 5 });

            // model copy to target, target should have model's new information
            model.CopyTo(distributorApplication);
            Assert.AreEqual(model.ProductLines.Count, distributorApplication.ProductLines.Count);
            LegacyDistributorProductLine productline = null;
            productline = distributorApplication.ProductLines.Where(line => line.Id == 0).SingleOrDefault();
            Assert.IsNull(productline);
            productline = distributorApplication.ProductLines.Where(line => line.Id == 5).SingleOrDefault();
            Assert.IsNotNull(productline);
            productline = distributorApplication.ProductLines.Where(line => line.Id == 1).SingleOrDefault();
            Assert.IsNotNull(productline);
        }
    }
}
