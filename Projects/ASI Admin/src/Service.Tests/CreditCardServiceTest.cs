using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using asi.asicentral.model;
using System.Configuration;

namespace asi.asicentral.Tests
{
    [TestClass]
    public class CreditCardServiceTest
    {
        [TestMethod]
        public void ValidateCreditCard()
        {
            string webAPIUrl = ConfigurationManager.AppSettings["ConnectUrl"];
            if (!string.IsNullOrEmpty(webAPIUrl)) {
                CreditCard card = GetVisaCC();
                ICreditCardService cardService = new CreditCardService(new PersonifyService());
                Assert.IsTrue(cardService.Validate(card));
                card.Number = "4123456789012345";
                Assert.IsFalse(cardService.Validate(card));
            }
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
