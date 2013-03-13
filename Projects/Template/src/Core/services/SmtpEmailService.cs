using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.services
{
    public class SmtpEmailService : IEmailService
    {
        private bool ssl = true;
        private int port = 25;
        private string smtpAddress = null;
        private string from = null;

        public SmtpEmailService()
        {
            //required to avoid the issue with "The remote certificate is invalid according to the validation procedure"
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; }; 
        }

        public static SmtpEmailService GetService(string smtpAddress, int port, string from, bool ssl)
        {
            SmtpEmailService emailService = new SmtpEmailService();
            emailService.smtpAddress = smtpAddress;
            emailService.port = port;
            emailService.from = from;
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
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmtpPort"]))
                            port = Int32.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                        from = ConfigurationManager.AppSettings["SmtpFrom"];
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
            SmtpServer.EnableSsl = ssl;
            mailObject.From = new MailAddress(from);
            mailObject.To.Add(mail.To);
            mailObject.Subject = mail.Subject;
            mailObject.Body = mail.Body;
            SmtpServer.Send(mailObject);
        }
    }
}
