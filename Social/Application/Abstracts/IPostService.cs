
using Application.DTOs;

namespace Application.Abstracts;

public interface IPostService
{
    Task<PostGetDto> CreateAsync(PostCreateDto post);
    Task<PostGetDto> GetByIdAsync(int id);
    Task<PostGetDto> GetAllAsync();

}
