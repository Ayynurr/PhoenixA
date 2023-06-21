namespace Application.Abstracts;

public interface IEmailService
{
    void SendMessage(string message, string subject, string to);
}
