using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Application.DTOs.PostDto;

namespace Application.Abstracts;

public interface IPostService
{
    Task<PostGetDto> CreateAsync(PostCreateDto post);
    Task<PostGetDto> GetByIdAsync(int id);
    Task<List<PostGetDto>> GetAllAsync();
    Task<PostGetDto> UpdateAsync(PostUpdateDto post,int id);
    Task<List<ImageGetDto>> UpdateImagesAsync(UpdateImageDto updateImage,int postId);
    Task DeleteAsync(int id);


}
