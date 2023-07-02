using Hangfire;

namespace Infrastructure.Services;

public class ArchiveJob : IArchiveJob
{
    private readonly IArchiveService _archiveService;

    public ArchiveJob(IArchiveService archiveService)
    {
        _archiveService = archiveService;
    }

    public void ScheduleArchiveJob()
    {
        RecurringJob.AddOrUpdate(() => _archiveService.ArchiveStoriesAsync(), Cron.Daily);
    }
}
