using asi.asicentral.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace asi.asicentral.interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="mail"></param>
        bool SendMail(Mail mail);

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="mail"></param>
        bool SendMail(MailMessage mail);
    }
}
