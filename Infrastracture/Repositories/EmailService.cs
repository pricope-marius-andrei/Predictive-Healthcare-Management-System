using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task<Result<string>> SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Predictive Healthcare System", _emailConfiguration.From));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true); // true for SSL
                await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return Result<string>.Success("Email sent successfully");
            }
            catch(Exception ex)
            {
                return Result<string>.Failure(ex.InnerException?.ToString() ?? ex.Message);
            }
            

        }
    }
}
