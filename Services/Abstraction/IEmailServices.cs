namespace Services.Abstraction;

public interface IEmailServices
{
    Task SendForgotPasswordMail(string email, string resetToken);
}