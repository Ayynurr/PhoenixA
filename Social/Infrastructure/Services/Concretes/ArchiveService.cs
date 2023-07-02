using Domain.Exceptions;
using Persistance.DataContext;
using Microsoft.EntityFrameworkCore;
using Application.Abstracts;

namespace Infrastructure.Services;
public class ArchiveService : IArchiveService
{
    readonly AppDbContext _dbcontext;
    readonly ICurrentUserService _currentUserService;
    public ArchiveService(AppDbContext dbcontext, ICurrentUserService currentUserService)
    {
        _dbcontext = dbcontext;
        _currentUserService = currentUserService;
    }

    public async Task ArchiveStoriesAsync()
    {
        var loginId = _currentUserService.UserId;
        var currentDate = DateTime.Now.Date;
        var archiveDate = currentDate.AddDays(1);

        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
            ?? throw new NotfoundException("User Not Found");

        var storiesToArchive = user.Stories
             .Where(s => s.CreatedDate.Date < archiveDate && !s.IsArchived)
            .ToList();

        foreach (var story in storiesToArchive)
        {
            story.IsArchived = true;
        }

        await _dbcontext.SaveChangesAsync();
    }
}
