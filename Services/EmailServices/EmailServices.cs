using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.EmailServices
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailSenderModel setting;

        public EmailServices(IOptions<EmailSenderModel> options)
        {
            setting = options.Value;
        }
        public async Task SendEmailAsync(string to, string Subject, string body,CancellationToken cancellationToken=default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(setting.SenderName, setting.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = Subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", setting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(setting.SenderEmail, setting.Password);

            await smtp.SendAsync(message,cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
