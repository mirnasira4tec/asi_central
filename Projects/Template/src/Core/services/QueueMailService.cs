using asi.asicentral.interfaces;
using ASI.Contracts.Messages.Email;
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
using ASI.Services.Messaging;

namespace asi.asicentral.services
{
    public class QueueMailService : IEmailService
    {
        private log4net.ILog log;
        public QueueMailService()
        {
            log = log4net.LogManager.GetLogger(GetType());
            //required to avoid the issue with "The remote certificate is invalid according to the validation procedure"
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        }

        public virtual bool SendMail(model.Mail mail)
        {
            MailMessage mailObject = new MailMessage();
            mailObject.To.Add(mail.To);
            mailObject.Subject = mail.Subject;
            mailObject.Body = mail.Body;
            return SendMail(mailObject);
        }

        private bool SendMailSmtp(MailMessage mail)
        {
            if (mail == null) throw new Exception("Invalid mail details");
            bool result = false;
            var content = new ContentEmailMessage();

            content.Subject = mail.Subject;
            content.Body = mail.Body;
            if(mail.From != null) content.FromEmail = string.Format("{0}|{1}", mail.From.Address, mail.From.DisplayName);
            if (string.IsNullOrEmpty(content.FromEmail) && ConfigurationManager.AppSettings["SmtpFrom"] != null && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["SmtpFrom"]))
            {
                content.FromEmail = ConfigurationManager.AppSettings["SmtpFrom"];
            }
            if(mail.To != null)
            {
                if(mail.To.Any())
                {
                    content.ToEmailList = new List<string>();
                    foreach(var to in mail.To){
                        content.ToEmailList.Add(to.Address);
                    }
                }
            }
            if (mail.CC != null)
            {
                if (mail.CC.Any())
                {
                    content.CCEmailList = new List<string>();
                    foreach (var cc in mail.CC)
                    {
                        content.CCEmailList.Add(cc.Address);
                    }
                }
            }
            try
            {
                result = content.SendAck();
            }
            catch (Exception ex)
            {
                result = false;
                log.Error(string.Format("Failed email sending in SmtpEmailService-SendMailSmtp(): {0}", ex.Message));
            }
            return result;
        }

        public virtual bool SendMail(MailMessage mail)
        {
            return SendMailSmtp(mail);
        }
    }
}
