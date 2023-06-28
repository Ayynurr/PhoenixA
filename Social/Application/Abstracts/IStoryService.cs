using Application.DTOs;
namespace Application;

public interface IStoryService
{
    Task<StoryGetDto> CreateStoryImageAsync(CreateStoryDto story);
    Task<List<StoryGetDto>> GetAllAsync();
    Task<List<StoryGetDto>> GetUserAsync(string username);
    Task<StoryGetDto> CreateVideoAsync(CreateVideo story);
    Task<List<StoryGetDto>> GetFriendAsync();
    Task DeleteAsync(int id);
    Task<List<StoryGetDto>> ArchiveAsync(int id);
}
