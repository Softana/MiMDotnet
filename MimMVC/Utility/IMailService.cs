using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimMVC.Utility
{
    public interface IMailService
    {
        Task SendEmailOnlyBody(string To, string Subject, string body);
        Task SendEmailAsync(MailRequest mailrequest);
    }
}
