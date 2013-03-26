using System;
using asi.asicentral.model.store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace asi.asicentral.WebApplication.Tests.Model.store
{
    [TestClass]
    public class SupplierMembershipApplicationTest
    {
        [TestMethod]
        public void CopyTo()
        {
            // prepare for SupplierMembershipApplication.CopyTo(SupplierMembershipApplication target)
            SupplierMembershipApplication supplierApplication = new SupplierMembershipApplication();
            SupplierMembershipApplication target = new SupplierMembershipApplication();
            SupplierMembershipApplicationContact contact1 = new SupplierMembershipApplicationContact() { Id = 0, Name = "contact1" };
            SupplierMembershipApplicationContact contact2 = new SupplierMembershipApplicationContact() { Id = 1, Name = "contact2" };
            supplierApplication.Contacts = new List<SupplierMembershipApplicationContact>();
            supplierApplication.Contacts.Add(contact1);
            supplierApplication.Contacts.Add(contact2);

            // copy to a target that doesn't have a list of contacts.
            // Test to see that the target contacts get copied over with the source.
            supplierApplication.CopyTo(target);
            Assert.AreEqual(supplierApplication.Contacts, target.Contacts);

            // copy to a target that already has a list of contacts.
            // The target's list of contact should be updated if it already has the matching contacts as the source.
            target.Contacts = new List<SupplierMembershipApplicationContact>();
            supplierApplication.Contacts.Clear();
            target.Contacts.Add(contact1);
            target.Contacts.Add(contact2);
            supplierApplication.Contacts.Add(contact1);
            supplierApplication.Contacts.Add(contact2);
            target.Contacts[0].Name = "changed1";
            target.Contacts[1].Name = "changed2";

            supplierApplication.CopyTo(target);
            Assert.AreEqual(supplierApplication.Contacts[0].Name, target.Contacts[0].Name);


            // copy to a target that already has a list of contacts but missing one.
            // The target's existing contact should be updated, and the missing ones should be added.
        }
    }
}
