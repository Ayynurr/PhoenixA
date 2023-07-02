namespace Infrastructure.Services;

public class BackraundEmailService : IBackraundEmailService
{
    private readonly IEmailService _emailService;

    public BackraundEmailService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendBirthdayMessagesAsync(List<string> emails, string message)
    {
        foreach (var email in emails)
        {
            await _emailService.SendBirthdayMessage(email, message);
        }
    }


}
