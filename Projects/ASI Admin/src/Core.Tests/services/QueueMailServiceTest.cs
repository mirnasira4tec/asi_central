using System;
using ASI.Contracts.Messages.Email;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using System.Net.Mail;
using System.Net.Configuration;
using System.Collections.Generic;
using ASI.Services.Messaging;
using asi.asicentral.model;
using System.Threading;
using NUnit.Framework;

namespace Core.Tests
{
	[TestFixture]
    public class QueueMailServiceTest
	{
		[Test]
        [Ignore("Ignore a test")]
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
            QueueMailService smtpEmailService = new QueueMailService();
            var result = smtpEmailService.SendMail(mail);
            Assert.IsTrue(result);
            
		}

        [Test]
        [Ignore("Ignore a test")]
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

        [Test]
        [Ignore("Ignore a test")]
        public void SendMailWithoutFrom()
        {
            MailMessage mail = new MailMessage();

            mail.Subject = "Test Mail";
            mail.Body = "This is Test Mail";
            mail.To.Add(new MailAddress("pkumar@asicentral.com"));

            QueueMailService smtpEmailService = new QueueMailService();
            bool result = smtpEmailService.SendMail(mail);
            Assert.IsTrue(result);
        }

        [Test]
        [Ignore("Ignore a test")]
        public void SendMailWithMailObject()
        {
            Mail mail = new Mail();
            mail.To = "pkumar@asicentral.com";
            mail.Subject = "Mail from asi.asicetral.model.Mail object";
            mail.Body = "Body - Mail from asi.asicetral.model.Mail object";
            QueueMailService smtpEmailService = new QueueMailService();
            bool result = smtpEmailService.SendMail(mail);
            Assert.IsTrue(result);
        }
	}
}
