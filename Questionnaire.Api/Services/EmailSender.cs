using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Questionnaire.Api.Configurations;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnaire.Api.Services
{
    public class EmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; set; }

        public EmailSender(IOptions<AuthMessageSenderOptions> options)
        {
            Options = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if(Options.SendGridKey != null)
            {
                await Execute(Options.SendGridKey, subject, htmlMessage, email);
            }
        }

        public async Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("", ""),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            var response = await client.SendEmailAsync(msg);
        }
    }
}
