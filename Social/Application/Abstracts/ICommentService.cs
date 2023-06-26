using Application.DTOs;
using Domain.Entities;
namespace Application.Abstracts;

public interface ICommentService
{
    Task<CommentGetDto> CreateAsync(CommentCreateDto comment);
    Task CommentDeleteAsync(int id);
    //Task<List<CommentGetDto>> GetPostComment(int id);

}
