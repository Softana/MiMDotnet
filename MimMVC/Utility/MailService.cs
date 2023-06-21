using System.Net.Mail;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace MimMVC.Utility
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailsettings)
        {
            _mailSettings = mailsettings.Value;
        }


        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(mailRequest.ToEmail);
                message.From = new MailAddress(_mailSettings.Mail);
                message.Subject = mailRequest.Subject;
                message.Body = mailRequest.Body;
                message.IsBodyHtml = true;
                var clientclient = new SmtpClient(_mailSettings.Host, _mailSettings.Port);
                clientclient.EnableSsl = true;
                clientclient.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);

                clientclient.Send(message);
            }
            catch (Exception e)
            {
            }
        }

        public async Task SendEmailOnlyBody(string To, string Subject, string body)
        {
            var req = new MailRequest();
            req.Body = body;
            req.ToEmail = To;
            req.Subject = Subject;

            await SendEmailAsync(req);
        }
    }
}
