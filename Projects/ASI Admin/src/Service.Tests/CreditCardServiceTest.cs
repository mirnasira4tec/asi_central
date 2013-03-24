using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using asi.asicentral.model;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class CreditCardServiceTest
    {
        [TestMethod]
        public void Create()
        {
            ICreditCardService cardService = new CreditCardService();
            string cardIdentifier = cardService.Store(GetVisaCC());
            Assert.IsFalse(string.IsNullOrEmpty(cardIdentifier));
            Assert.AreNotEqual(Guid.Empty.ToString(), cardIdentifier);
        }

        [TestMethod]
        public void StoreGetAndDelete()
        {
            CreditCard card = GetVisaCC();
            ICreditCardService cardService = new CreditCardService();
            string cardIdentifier = cardService.Store(card);
            cardService.Delete(cardIdentifier);
        }

        private CreditCard GetVisaCC()
        {
            CreditCard card = new CreditCard()
            {
                Type = "Visa",
                Number = "4222222222222",
                ExpirationDate = new DateTime(2014, 11, 15),
                CardHolderName = "ASI Store",
                Address = "Street",
                City = "City",
                State = "NJ",
                Country = "USA",
                PostalCode = "98123",
            };
            return card;
        }
    }
}
