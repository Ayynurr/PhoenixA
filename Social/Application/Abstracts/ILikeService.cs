using Application.DTOs;
using Application.DTOs;

namespace Application.Abstracts;

public interface ILikeService
{
    Task<int> LikeComment(int commentId);
    Task<int> LikePost(int postId);
    //like delete
}
