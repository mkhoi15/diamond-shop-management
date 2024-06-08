using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Services.Abstraction;

namespace Services;

public class EmailServices : IEmailServices
{
    private readonly IConfiguration _configuration;

    public EmailServices(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string emailTo, string subject, string message)
    {
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse(_configuration["EmailHost"]));
        email.To.Add(MailboxAddress.Parse(emailTo));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = message
        };
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        await smtp.ConnectAsync(_configuration["EmailHost"], 587,
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["EmailUserName"], _configuration["EmailPassword"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task ForgotPassword(string email)
    {
        var subject = "Reset Your Password";
        var resetLink = $"https://yourapp.com/reset-password?token={Guid.NewGuid()}";

        var htmlMessage = "\n        <!DOCTYPE html>\n        <html>\n        <head>\n            <meta charset='UTF-8'>\n            <title>Reset Your Password</title>\n            <style>\n                body {\n                    font-family: Arial, sans-serif;\n                    background-color: #f4f4f4;\n                    color: #333;\n                    line-height: 1.6;\n                }\n                .container {\n                    max-width: 600px;\n                    margin: 20px auto;\n                    padding: 20px;\n                    background: #fff;\n                    border: 1px solid #ddd;\n                    border-radius: 10px;\n                }\n                .button {\n                    display: inline-block;\n                    padding: 10px 20px;\n                    margin: 20px 0;\n                    background: #007BFF;\n                    color: #fff;\n                    text-decoration: none;\n                    border-radius: 5px;\n                }\n                .button:hover {\n                    background: #0056b3;\n                }\n            </style>\n        </head>\n        <body>\n            <div class='container'>\n                <h2>Reset Your Password</h2>\n                <p>Hi,</p>\n                <p>You requested to reset your password. Please click the button below to reset your password:</p>\n                <a href='" + resetLink + @"' class='button'>Reset Password</a>
                <p>If you did not request this, please ignore this email.</p>
                <p>Thanks,<br>YourApp Team</p>
            </div>
        </body>
        </html>";
        await SendEmailAsync(email, subject, htmlMessage);
    }
}