using System;
using System.Linq;
using asi.asicentral.model.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace asi.asicentral.WebApplication.Tests.Model.store
{
    [TestClass]
    public class SupplierMembershipApplicationTest
    {
        [TestMethod]
        public void CopyTo_SyncSuppContacts()
        {
            // prepare for SupplierMembershipApplication's CopyTo(SupplierMembershipApplication target)
            // for syncing contacts
            SupplierMembershipApplication supplierApplication = new SupplierMembershipApplication();
            SupplierMembershipApplication model = new SupplierMembershipApplication();
            supplierApplication.Contacts.Add(new SupplierMembershipApplicationContact() { Id = 0, Name = "contact0" });
            supplierApplication.Contacts.Add(new SupplierMembershipApplicationContact() { Id = 1, Name = "contact1" });
            supplierApplication.Contacts.Add(new SupplierMembershipApplicationContact() { Id = 2, Name = "contact2" });
            model.Contacts.Add(new SupplierMembershipApplicationContact() { Id = 0, Name = "new contact" });
            model.Contacts.Add(new SupplierMembershipApplicationContact() { Id = 1, Name = "new contact" });
            
            // model copy to the target, target should have 2 contacts which are provided by the model
            model.CopyTo(supplierApplication);
            Assert.AreEqual(model.Contacts.Count, supplierApplication.Contacts.Count);
            SupplierMembershipApplicationContact contact = null;
            contact = supplierApplication.Contacts.Where(theContact => theContact.Id == 2).SingleOrDefault();
            Assert.IsNull(contact);
            contact = supplierApplication.Contacts.Where(theContact => theContact.Id == 1).SingleOrDefault();
            Assert.IsNotNull(contact);
            contact = supplierApplication.Contacts.Where(theContact => theContact.Id == 0).SingleOrDefault();
            Assert.IsNotNull(contact);
        }

        [TestMethod]
        public void CopyTo_SyncDecorationTypes()
        {
            // prepare for SupplierMembershipApplication's CopyTo(SupplierMembershipApplication target)
            // for syncing decoration types
            SupplierMembershipApplication supplierApplication = new SupplierMembershipApplication();
            SupplierMembershipApplication model = new SupplierMembershipApplication();
            supplierApplication.DecoratingTypes.Add(new SupplierDecoratingType() { Name = "type0" });
            supplierApplication.DecoratingTypes.Add(new SupplierDecoratingType() { Name = "type1" });
            supplierApplication.DecoratingTypes.Add(new SupplierDecoratingType() { Name = "type2" });
            model.DecoratingTypes.Add(new SupplierDecoratingType() { Name = "type7" });
            model.DecoratingTypes.Add(new SupplierDecoratingType() { Name = "type2" });

            // model copy to target, target should have new data from the model
            model.CopyTo(supplierApplication);
            Assert.AreEqual(model.DecoratingTypes.Count, supplierApplication.DecoratingTypes.Count);
            SupplierDecoratingType type = null;
            type = supplierApplication.DecoratingTypes.Where(p => p.Name == "type2").SingleOrDefault();
            Assert.IsNotNull(type);
            type = supplierApplication.DecoratingTypes.Where(p => p.Name == "type7").SingleOrDefault();
            Assert.IsNotNull(type);
        }
    }
}
