namespace Infrastructure.Services;

public interface IBackraundEmailService
{
    Task SendBirthdayMessagesAsync(List<string> emails, string message);

}
