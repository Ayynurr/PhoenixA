
using Application.DTOs;

namespace Application.Abstracts;

public interface IPostService
{
    Task<PostGetDto> CreateAsync(PostCreateDto post);


}
