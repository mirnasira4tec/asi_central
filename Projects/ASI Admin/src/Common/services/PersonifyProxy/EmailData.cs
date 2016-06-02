using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using asi.asicentral.interfaces;

namespace asi.asicentral.services.PersonifyProxy
{
    public class EmailData
    {
        public string[] MailTo { get; set; }

        public string Subject { get; set; }

        public string EmailBody { get; set; }

        private ILogService log = null;

        public EmailData()
        {
            log = LogService.GetLog(this.GetType());
            MailTo = GetMailTo();
        }

        public string[] GetMailTo()
        {
            MailTo = null;
            var em = ConfigurationManager.AppSettings["CreateOrderInPersonifyErrorEmail"];
            if (!string.IsNullOrEmpty(em))
            {
                MailTo = ConfigurationManager.AppSettings["CreateOrderInPersonifyErrorEmail"].Split(new char[] { ';', ',', ' ' });
            }
            else
            {
                log.Debug("Entry of \"CreateOrderInPersonifyErrorEmail\" doesn't exist in configuration file.");
            }

            return MailTo;
        }

        public void SendEmail(IEmailService emailService)
        {
            try
            {
                if (MailTo == null)
                {
                    log.Error("MailTo is null");
                    if (!string.IsNullOrEmpty(Subject))
                    {
                        log.Debug(Subject);
                    }
                    if (!string.IsNullOrEmpty(EmailBody))
                    {
                        log.Debug(EmailBody);
                    }
                    return;
                }
                MailMessage mail = new MailMessage();
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                foreach (var a in MailTo)
                {
                    if (!string.IsNullOrWhiteSpace(a))
                    {
                        mail.To.Add(new MailAddress(a.Trim()));
                    }
                }
                mail.Subject = Subject;
                mail.Body = EmailBody;
                emailService.SendMail(mail);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error in sending email: {0} {1}", ex.Message, ex.StackTrace));
            }
        }

        public static string GetMessageSuffix(string url)
        {
            var s = "<br /><br />Thanks,<br /><br />ASICentral team";
            if (!string.IsNullOrEmpty(url)) s = "<br /><br />" + url + s;
            return s;
        }
    }
}
