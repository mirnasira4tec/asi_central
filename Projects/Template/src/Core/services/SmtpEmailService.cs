using asi.asicentral.interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace asi.asicentral.services
{
    public class SmtpEmailService : IEmailService
    {
        private bool ssl = true;
        private int port = 25;
        private string smtpAddress = null;
        private string from = null;
        private string username = null;
        private string password = null;

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
            MailMessage mailObject = new MailMessage();
            mailObject.To.Add(mail.To);
            mailObject.Subject = mail.Subject;
            mailObject.Body = mail.Body;
            SendMail(mailObject);
        }


        public virtual void SendMail(MailMessage mail)
        {
            if (HttpContext.Current != null && (string.IsNullOrEmpty(smtpAddress) || string.IsNullOrEmpty(from)))
            {
                var config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                var settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
                if (settings != null && settings.Smtp != null && settings.Smtp.Network != null)
                {
                    smtpAddress = settings.Smtp.Network.Host;
                    port = settings.Smtp.Network.Port;
                    from = settings.Smtp.From;
                    username = settings.Smtp.Network.UserName;
                    password = settings.Smtp.Network.Password;
                }
            }

            //load from individual app settings if address is not set in mail settings
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
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                username = ConfigurationManager.AppSettings["smtpUserName"];
                password = ConfigurationManager.AppSettings["smtpPassword"];
            }
            
            SmtpClient SmtpServer = new SmtpClient(smtpAddress);
            SmtpServer.Port = port;
            SmtpServer.EnableSsl = ssl;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
            }
            if (mail.From == null) mail.From = new MailAddress(from);

            try
            {
                new Thread(() => SmtpServer.Send(mail)).Start();
            }
            catch(Exception ex)
            {
                throw new Exception("Error occurred during sending Email");
            }
            
        }
    }
}
