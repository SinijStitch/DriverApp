using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Net;

namespace SearchForDriversWebApp.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }

        public void SendEmailDefault(string sender, string messageText)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Yavir Admin", "admin@mycompany.com"));
                message.To.Add(new MailboxAddress("Recipient Name", $"{sender}"));
                message.Subject = "Yavir";
                message.Body = new TextPart("html") { Text = $"<div style=\"color: black;\">{messageText}</div>" };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("your email", "your password for email");
                    client.Send(message);
                    client.Disconnect(true);
                    logger.LogInformation("Повідомлення успішно відправлено!");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.GetBaseException().Message);
            }
        }
    }
}
