using Application.Abstracts;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Persistance.Concretes;
using Persistance.DataContext;

namespace Socail.Api.BackraoundServices;

public class DateTimeLogWriter : IHostedService
{
    private IServiceProvider _serviceProvider;

    private Timer timer;


    public DateTimeLogWriter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"{nameof(DateTimeLogWriter)}Service started....");
        timer = new Timer(writeDateTimeOnLog, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        timer = new Timer(StoryTime, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }
    

    private async void writeDateTimeOnLog(object state)
    {

        //Console.WriteLine("Salam");
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            UserManager<AppUser> scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();


            var today = DateTime.Today;
            var usersWithBirthday = await scopedProcessingService.Users
                .Where(u => u.BirthDate.HasValue && u.BirthDate.Value.Day == today.Day && u.BirthDate.Value.Month == today.Month)
                .ToListAsync();




            foreach (var user in usersWithBirthday)
            {
                SendBirthdayEmail(user);
            }

            Console.WriteLine($"DateTime is {DateTime.Now.ToLongTimeString()}");
        }
    }

    private async void StoryTime(object state)
    {
        using (IServiceScope scope = _serviceProvider.CreateScope())
        {
            UserManager<AppUser> scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            using (IServiceScope scope2 = _serviceProvider.CreateScope())
            {
                AppDbContext scopedProcessingService2 =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var currentDate = DateTime.Now.Date;
                var archiveDate = currentDate.AddDays(-1); 

                foreach (var user in await scopedProcessingService.Users.Include(i => i.Stories).ToListAsync())
                {
                    var storiesToArchive = user.Stories
                        .Where(s => s.CreatedDate.Date <= archiveDate && !s.IsArchived)
                        .ToList();

                    foreach (var story in storiesToArchive)
                    {
                        story.IsArchived = true;
                    }
                }

                await scopedProcessingService2.SaveChangesAsync();

                Console.WriteLine($"DateTime is {DateTime.Now.ToLongTimeString()}");
            }
        }
    }
    
    private void SendBirthdayEmail(AppUser user)
    {
        Console.WriteLine($"Sending birthday email to {user.Email}");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        Console.WriteLine($"{nameof(DateTimeLogWriter)}Service stopped....");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer = null!;
    }
}
