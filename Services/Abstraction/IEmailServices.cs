namespace Services.Abstraction;

public interface IEmailServices
{
    Task SendEmailAsync(string emailTo, string subject, string message);
}