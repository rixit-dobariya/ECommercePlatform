using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;


namespace ECommercePlatform.Helpers.EmailHelper
{
    
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings["SmtpServer"], int.Parse(emailSettings["Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings["SenderEmail"], emailSettings["Password"]);
            smtp.Send(message);
            smtp.Disconnect(true);
        }

    }

}
