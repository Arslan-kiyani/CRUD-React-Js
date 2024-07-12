using System;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ReadFile_Mini.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string recipientEmail, string subject, string messageBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Arslan kiyani", "arslankiyani946@gmail.com"));
            message.To.Add(new MailboxAddress("Recipient", recipientEmail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = messageBody
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("arslankiyani946@gmail.com", "Kiyani@123");

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    // Log or handle the error
                    throw new InvalidOperationException($"An error occurred while sending email: {ex.Message}", ex);
                }
            }
        }
    }
}
