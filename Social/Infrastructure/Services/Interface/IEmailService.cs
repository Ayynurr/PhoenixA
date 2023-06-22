namespace Infrastructure.Services.Interface;

public interface IEmailService
{
    void SendMessage(string message, string subject, string to);
}
