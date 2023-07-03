using Application;
using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Persistance.DataContext;
using Persistance.Extentions;
using Microsoft.AspNetCore.Hosting;

namespace Persistance;

public class StoryService : IStoryService
{
    readonly ICurrentUserService _currentUserService;
    readonly AppDbContext _dbcontext;
    readonly IWebHostEnvironment _hostEnvironment;
    public StoryService(ICurrentUserService currentUserService, AppDbContext dbcontext, IWebHostEnvironment hostEnvironment)
    {
        _currentUserService = currentUserService;
        _dbcontext = dbcontext;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<List<StoryGetDto>> ArchiveAsync(int id)
    {
        var archivedStories = await _dbcontext.Stories
        .Where(story => story.IsArchived)
        .Select(story => new StoryGetDto
        {
            Id = story.Id,
            Content = story.Content
        })
        .ToListAsync();

        return archivedStories;
    }

    public async Task<StoryGetDto> CreateStoryImageAsync(CreateStoryDto story)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException("User Not Found");

        Story newStory = new()
        {
            UserId = (int)loginId,
            Content = story.Content,
            CreatedDate = DateTime.Now.Date,
        };
        if (story.Image != null)
        {
           
                if (story.Image.CheckFileSize(2048))
                    throw new FileTypeException();
                if (!story.Image.CheckFileType("image/"))
                    throw new FileSizeException();
                string newFileName = await story.Image.FileUploadAsync(_hostEnvironment.WebRootPath, "StoryImages");
                newStory.ImageName = newFileName;
           
        }
        //var currentDate = DateTime.Now.Date;
        //var archiveDate = currentDate.AddDays(-1);


        //var currentDate = DateTime.Now.Date; 
        //var archiveDate = currentDate.AddDays(1); 

        //if (newStory.CreatedDate.Date < archiveDate)
        //{
        //    newStory.IsArchived = true;
        //}
        //else
        //{
        //    newStory.IsArchived = false; 
        //}



        _dbcontext.Stories.Add(newStory);
        await _dbcontext.SaveChangesAsync();
        return new StoryGetDto() { Content = newStory.Content, Id = newStory.Id, };

    }

    public async Task<StoryGetDto> CreateVideoAsync(CreateVideo story)
    {

        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException("User Not Found");
        Story newStory = new()
        {
            UserId = (int)loginId,
            Content = story.Content,
            CreatedDate = DateTime.Now,
        };
        if (story.Videos != null)
        {

            if (!story.Videos.IsVideoFile())
            {
                throw new FileSupportedException();
            }

            string newFileName = await story.Videos.VideoUploadAsync(_hostEnvironment.WebRootPath, "StoryVideo");
            newStory.VideoName = newFileName;

        }

        //var currentDate = DateTime.Now.Date;
        //var archiveDate = currentDate.AddDays(-1);
        //if (newStory.CreatedDate.Date <= archiveDate)
        //{
        //    newStory.IsArchived = true;
        //    await _dbcontext.SaveChangesAsync();
        //}
        var currentDate = DateTime.Now.Date;
        var archiveDate = currentDate.AddDays(1);

        if (newStory.CreatedDate.Date < archiveDate)
        {
            newStory.IsArchived = false;
        }
        else
        {
            newStory.IsArchived = true;
        }


        _dbcontext.Stories.Add(newStory);
        await _dbcontext.SaveChangesAsync();
        return new StoryGetDto() { Content = newStory.Content, Id = newStory.Id, };
    }

    public async Task DeleteAsync(int id)
    {
        var story = await _dbcontext.Stories.FindAsync(id);

        if (story == null)
        {
            throw new NotfoundException("Story is not found");
        }
        if (!string.IsNullOrEmpty(story.ImageName))
        {
            string imagePath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "StoryImages", story.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        if (!string.IsNullOrEmpty(story.VideoName))
        {
            string videoPath = System.IO.Path.Combine(_hostEnvironment.WebRootPath, "StoryVideos", story.VideoName);
            if (System.IO.File.Exists(videoPath))
            {
                System.IO.File.Delete(videoPath);
            }
        }

        _dbcontext.Stories.Remove(story);
        await _dbcontext.SaveChangesAsync();


    }


    public async Task<List<StoryGetDto>> GetAllAsync()
    {
        var allStories = await _dbcontext.Stories
       .Select(story => new StoryGetDto
       {
           Id = story.Id,
           Content = story.Content
       })
       .ToListAsync();

        return allStories;
    }


    public async Task<List<StoryGetDto>> GetFriendAsync()
    {
        var loginId = _currentUserService.UserId;
        var friendStoryIds = await _dbcontext.UserFriends
            .Where(f => f.UserId == loginId && f.Status == FriendStatus.Accepted)
            .Select(f => f.FriendId)
            .ToListAsync();

        var friendStories = await _dbcontext.Stories
            .Where(story => friendStoryIds.Contains(story.UserId))
            .Select(story => new StoryGetDto
            {
                Id = story.Id,
                Content = story.Content
            })
            .ToListAsync();

        return friendStories;
    }

    public async Task<List<StoryGetDto>> GetUserAsync(string username)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.UserName == username)
       ?? throw new NotfoundException("User Not Found");

        var userStories = await _dbcontext.Stories
            .Where(story => story.UserId == user.Id)
            .Select(story => new StoryGetDto
            {
                Id = story.Id,
                Content = story.Content
            })
            .ToListAsync();

        return userStories;
    }
}
