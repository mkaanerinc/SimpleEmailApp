using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace SimpleEmailApp.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailDto request)
        {
            // Fake email account created from https://ethereal.email

            // Email Object

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailConfiguration").GetSection("UserName").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Body
            };

            // Sending Email

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration.GetSection("EmailConfiguration").GetSection("SmtpServer").Value,
                Convert.ToInt32(_configuration.GetSection("EmailConfiguration").GetSection("SmtpPort").Value),
                SecureSocketOptions.Auto);
            // for gmail => "smtp.gmail.com"
            smtp.Authenticate(_configuration.GetSection("EmailConfiguration").GetSection("UserName").Value,
                _configuration.GetSection("EmailConfiguration").GetSection("Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
