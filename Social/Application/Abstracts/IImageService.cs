using Application.DTOs.ImagePostDto;

namespace Application.Abstracts;

public interface IImageService
{
    Task<ImageGetDto> CreateAsync(ImageGetDto image);
}
