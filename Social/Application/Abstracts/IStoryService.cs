using Application.DTOs;
using Domain.Entities;
namespace Application.Abstracts;

public interface IStoryService
{
    Task<StoryGetDto> CrateAsync(CreateStoryDto story);
}
