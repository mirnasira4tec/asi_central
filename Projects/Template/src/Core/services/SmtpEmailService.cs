using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class SmtpEmailService : IEmailService
    {
        private bool ssl = true;
        private int port;
        private string smtpAddress = null;
        private string from = null;
        private string password = null;

        public static SmtpEmailService GetService(string smtpAddress, int port, string from, string password, bool ssl)
        {
            SmtpEmailService emailService = new SmtpEmailService();
            emailService.smtpAddress = smtpAddress;
            emailService.port = port;
            emailService.from = from;
            emailService.password = password;
            emailService.ssl = ssl;
            return emailService;
        }

        public virtual void SendMail(model.Mail mail)
        {
            if (string.IsNullOrEmpty(smtpAddress) || string.IsNullOrEmpty(from))
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmtpServer"]))
                {
                    try
                    {
                        smtpAddress = ConfigurationManager.AppSettings["SmtpServer"];
                        port = Int32.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                        from = ConfigurationManager.AppSettings["SmtpFrom"];
                        password = ConfigurationManager.AppSettings["SmtpPassword"];
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Could not initialize the class: " + exception.Message);
                    }
                }
                else
                {
                    throw new Exception("The SmtpEmailService was not initialized properly");
                }
            }
            MailMessage mailObject = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(smtpAddress);
            SmtpServer.Port = port;
            SmtpServer.Credentials = new System.Net.NetworkCredential(from, password);
            SmtpServer.EnableSsl = ssl;
            mailObject.From = new MailAddress(from);
            mailObject.To.Add(mail.To);
            mailObject.Subject = mail.Subject;
            mailObject.Body = mail.Body;
            SmtpServer.Send(mailObject);
        }
    }
}
