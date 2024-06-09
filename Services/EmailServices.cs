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

    private async Task SendEmailAsync(string emailTo, string subject, string message)
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
    
    public async Task SendForgotPasswordMail(string email, string resetToken)
    {
        const string subject = "Reset Your Password";
        var resetLink = $"https://yourapp.com/reset-password?token={resetToken}";

        var htmlMessage = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='UTF-8'>
            <title>Reset Your Password</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    color: #333;
                    line-height: 1.6;
                }}
                .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    padding: 20px;
                    background: #fff;
                    border: 1px solid #ddd;
                    border-radius: 10px;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    margin: 20px 0;
                    background: #007BFF;
                    color: #fff;
                    text-decoration: none;
                    border-radius: 5px;
                }}
                .button:hover {{
                    background: #0056b3;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Reset Your Password</h2>
                <p>Hi,</p>
                <p>You requested to reset your password. Please click the button below to reset your password:</p>
                <a href='{resetLink}' class='button'>Reset Password</a>
                <p>If you did not request this, please ignore this email.</p>
                <p>Thanks,<br>YourApp Team</p>
            </div>
        </body>
        </html>";
        
        await SendEmailAsync(email, subject, htmlMessage);
    }
}