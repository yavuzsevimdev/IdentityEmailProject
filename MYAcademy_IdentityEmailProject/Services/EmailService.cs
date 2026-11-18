using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MYAcademy_IdentityEmailProject.Models;

namespace MYAcademy_IdentityEmailProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string receiverEmail, string subject, string message)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(
                _emailSettings.SenderName,
                _emailSettings.SenderEmail));

            mimeMessage.To.Add(new MailboxAddress(
                "User",
                receiverEmail));

            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                TextBody = message
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(
                _emailSettings.Host,
                _emailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(
                _emailSettings.Username,
                _emailSettings.Password);

            await smtpClient.SendAsync(mimeMessage);

            await smtpClient.DisconnectAsync(true);
        }

        public async Task SendHtmlEmailAsync(
            string receiverEmail,
            string subject,
            string htmlMessage,
            string textMessage)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(
                _emailSettings.SenderName,
                _emailSettings.SenderEmail));

            mimeMessage.To.Add(MailboxAddress.Parse(receiverEmail));

            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage,
                TextBody = textMessage
            };

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();

            await smtpClient.ConnectAsync(
                _emailSettings.Host,
                _emailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(
                _emailSettings.Username,
                _emailSettings.Password);

            await smtpClient.SendAsync(mimeMessage);

            await smtpClient.DisconnectAsync(true);
        }
    }
}
