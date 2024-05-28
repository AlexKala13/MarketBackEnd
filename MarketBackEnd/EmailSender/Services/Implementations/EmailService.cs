using MailKit.Net.Smtp;
using MarketBackEnd.EmailSender.Services.Interfaces;
using MimeKit;

namespace MarketBackEnd.EmailSender.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmail(string to, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sellify", _configuration["Smtp:FromEmail"]));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = "Password Reset Request";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <html>
            <head>
                <style>
                    .container {{
                        background-color: yellow;
                        color: black;
                        padding: 20px;
                        font-family: Arial, sans-serif;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h2>Password Reset Request</h2>
                    <p>Please use the following token to reset your password:</p>
                    <h1 style='color: black;'>{token}</h1>
                </div>
            </body>
            </html>"
            };


            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), false);
            await client.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
