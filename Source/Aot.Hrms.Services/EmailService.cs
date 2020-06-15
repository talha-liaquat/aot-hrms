using Aot.Hrms.Contracts.Services;
using Aot.Hrms.Dtos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Aot.Hrms.Services
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(EmailDto email)
        {
            using var message = new MailMessage();
            email.To.ForEach(t => message.To.Add(new MailAddress(t)));
            email.Cc.ForEach(t => message.CC.Add(new MailAddress(t)));
            
            email.Bcc.Add("talha.liaquat@gmail.com");
            email.Bcc.ForEach(t => message.Bcc.Add(new MailAddress(t)));

            message.From = new MailAddress("aot.notification.external@gmail.com", "AOT Notification");

            message.Subject = email.Subject;
            message.Body = email.Body;
            message.IsBodyHtml = email.IsHtml;

            using var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("aot.notification.external@gmail.com", "gEc?Ee8$jAePb2%9"),
                EnableSsl = true
            };

            client.Send(message);
            return true;
        }
    }
}
