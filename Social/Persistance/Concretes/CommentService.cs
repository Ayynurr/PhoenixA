using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistance.DataContext;

namespace Persistance.Concretes;

public class CommentService : ICommentService
{
    private readonly AppDbContext _dbcontext;
    public readonly ICurrentUserService _currentUserService;
    public CommentService(AppDbContext dbcontext, ICurrentUserService userService)
    {
        _dbcontext = dbcontext;
        _currentUserService = userService;
    }
    public async Task CommentDeleteAsync(int id)
    {
        Comment? comment = await _dbcontext.Comments.FindAsync(id) ?? throw new NotfoundException();
         _dbcontext.Comments.Remove(comment);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<CommentGetDto> CreateAsync(CommentCreateDto commentDto)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException("User Not Found");

        Comment? newComment = new Comment
        {
            Content = commentDto.Content,
            UserId = (int)loginId,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = user.Name
        };

        if (commentDto.ReplyCommentId.HasValue)
        {
            Comment? topComment = await _dbcontext.Comments.FindAsync(commentDto.ReplyCommentId.Value);
            if (topComment != null)
            {
                newComment.TopComment = topComment;
            }
        }


        _dbcontext.Comments.Add(newComment);
        await _dbcontext.SaveChangesAsync();


        return new CommentGetDto { Content=newComment.Content,Id = newComment.Id};
    }

    

    public async Task<CommentGetDto> GetAll(int id)
    {
        List<Comment>? comment = await _dbcontext.Comments.ToListAsync() ?? throw new NotfoundException();
        List<CommentGetDto> postDtos = comment.Select(p => new CommentGetDto
        {
            Id = p.Id,
            Content = p.Content,
        }).ToList();

        return new CommentGetDto { };
    }
   

}



