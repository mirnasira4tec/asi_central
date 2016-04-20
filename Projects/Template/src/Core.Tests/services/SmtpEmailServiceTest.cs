using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using System.Net.Mail;
using System.Net.Configuration;
using ASI.Contracts.Messages.Email;
using System.Collections.Generic;
using ASI.Services.Messaging;
using asi.asicentral.model;
using System.Threading;

namespace Core.Tests
{
	[TestClass]
    public class SmtpEmailServiceTest
	{
		[TestMethod]
		public void TestSendMail()
		{
            MailMessage mail = new MailMessage();

            mail.Subject = "Test Mail";
            mail.Body = "This is Test Mail";
            mail.From = new MailAddress("pkumar@asicentral.com", "ASI Product Support");
            mail.To.Add(new MailAddress("pkumar@asicentral.com"));

            //contentEmailMessage.Attachments = new List<String>() { @"C:\TestMailAttachment.txt" };
            //contentEmailMessage.Attachments = new List<String>() { @"C:\TestMailAttachment.txt", @"C:\UniqueProductsWithID1.txt" };
            //contentEmailMessage.Attachments = new List<String>() { @"C:\UniqueProductsWithID1.txt" };
            SmtpEmailService smtpEmailService = new SmtpEmailService();
            var result = smtpEmailService.SendMail(mail);
            Assert.IsTrue(result);
            
		}

        [TestMethod]
        public void TestSendMailAsBarista()
        {
            var contentEmailMessage = new ContentEmailMessage();

            contentEmailMessage.Subject = "Test Mail";
            contentEmailMessage.Body = "This is Test Mail";
            contentEmailMessage.FromEmail = "support@asicentral.com|ASI Product Support";
            contentEmailMessage.ToEmailList = new List<string>() { "pkumar@asicentral.com" };
            
            var result = contentEmailMessage.SendAck();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SendMailWithoutFrom()
        {
            MailMessage mail = new MailMessage();

            mail.Subject = "Test Mail";
            mail.Body = "This is Test Mail";
            mail.To.Add(new MailAddress("pkumar@asicentral.com"));

            SmtpEmailService smtpEmailService = new SmtpEmailService();
            bool result = smtpEmailService.SendMail(mail);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SendMailWithMailObject()
        {
            Mail mail = new Mail();
            mail.To = "pkumar@asicentral.com";
            mail.Subject = "Mail from asi.asicetral.model.Mail object";
            mail.Body = "Body - Mail from asi.asicetral.model.Mail object";
            SmtpEmailService smtpEmailService = new SmtpEmailService();
            bool result = smtpEmailService.SendMail(mail);
            Assert.IsTrue(result);
        }
	}
}
