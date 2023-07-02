namespace Infrastructure.Services;

public interface IEmailService
{
    void SendMessage(string message, string subject, string to);
    Task SendResetPasswordEmail(string emailAddress, string resetLink);
    Task<bool> SendBirthdayMessage(string email, string message);
}
