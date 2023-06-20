using Application.DTOs;
using Domain.Entities;
namespace Application.Abstracts;

public interface ICommentService
{
    Task<CommentGetDto> CreateAsync(CommentCreateDto comment);
    Task<CommentGetDto> GetAll(int id);
    Task CommentDeleteAsync(int id);

}
