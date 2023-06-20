using Application.DTOs;
using Application.DTOs;

namespace Application.Abstracts;

public interface ILikeService
{
    Task<int> LikeComment(int commentId, int userId);
    Task<int> LikePost(int postId, int userId);

}
