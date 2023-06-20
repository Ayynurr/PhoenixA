using Application.DTOs.LikeDto;

namespace Application.Abstracts;

public interface ILikeService
{
    Task CreateAsync(LikeCreateDto like);
}
