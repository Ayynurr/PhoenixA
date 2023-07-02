using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{

    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _smtpClient = new SmtpClient
        {
            Host = _configuration["Email:Host"],
            Port = int.Parse(_configuration["Email:Port"]),
            EnableSsl = bool.Parse(_configuration["Email:EnableSsl"]),
            Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:AppPassword"])
        };
    }


    public async Task<bool> SendBirthdayMessage(string email, string message)
    {
        try
        {
            var fromEmail = _configuration["Email:From"];
            var subject = "Happy Birthday!";
            var mailMessage = new MailMessage(fromEmail, email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);

            return true; 
        }
        catch (Exception ex)
        {
           
            using (StreamWriter writer = new StreamWriter("error.log", true))
            {
                await writer.WriteLineAsync($"E-posta gönderme hatası: {ex.Message}");
            }

            throw; 
        }
    }

    public void SendMessage(string message, string subject, string to)
    {
        MailMessage newMessage = new MailMessage(_configuration["Email:From"], to)
        {
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        _smtpClient.Send(newMessage);
    }

    public async Task SendResetPasswordEmail(string emailAddress, string resetLink)
    {
        // E-posta gönderme işlemlerini gerçekleştirin
        // Örneğin, SMTP protokolünü kullanarak e-posta sunucusuna bağlanabilirsiniz

        // E-posta gönderme kodu örneği
        using (var client = new SmtpClient())
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("noreply@example.com"),
                Subject = "Reset Your Password",
                Body = $"Please reset your password using the following link: {resetLink}"
            };

            mailMessage.To.Add(new MailAddress(emailAddress));

            await client.SendMailAsync(mailMessage);
        }

    }
}