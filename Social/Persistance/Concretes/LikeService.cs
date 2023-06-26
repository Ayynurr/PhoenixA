using Application.Abstracts;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistance.Datacontext.Migrations;
using Persistance.DataContext;

namespace Persistance.Concretes;

public class LikeService : ILikeService
{
    private readonly AppDbContext _dbcontext;
    public readonly ICurrentUserService _currentUserService;
    public LikeService(AppDbContext dbcontext, ICurrentUserService userService)
    {
        _dbcontext = dbcontext;
        _currentUserService = userService;
    }
    //public async Task<int> ILikeService.CreateAsync(LikeCreateDto likeCreate)
    //{

    //    var loginId = _currentUserService.UserId;
    //    AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
    //    ?? throw new NotfoundException("User Not Found");
    //    var newLike = new Like
    //    {
    //        CommentId = likeCreate.CommentId,
    //        UserId = likeCreate.UserId,
    //        PostId = likeCreate.PostId,
    //    };


    //    _dbcontext.Likes.Add(newLike);
    //    await _dbcontext.SaveChangesAsync();
    //    return (int)loginId;
    //}
    public async Task<int> LikeComment(int commentId)
    {
        var loginId = _currentUserService.UserId;
        var existingLike = await _dbcontext.LikeComments
            .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == loginId);
        if (existingLike != null)
        {
            throw new Exception("You have already liked this comment.");
        }

        var newLike = new LikeComment
        {
            CommentId = commentId,
            UserId = (int)loginId
        };
        _dbcontext.LikeComments.Add(newLike);
        int totalLikes;
        int like = ( totalLikes = await _dbcontext.LikeComments.CountAsync(l => l.CommentId == commentId)+1);
        //int like = totalLikes + 1;
        await _dbcontext.SaveChangesAsync();
        return like;
    }

    public async Task<int> LikePost(int postId)
    {
        var loginId = _currentUserService.UserId;
        var existingLike = await _dbcontext.LikePosts
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == loginId);

        if (existingLike != null)
        {
            throw new Exception("You have already liked this post.");
        }

        var newLike = new LikePost
        {
            PostId = postId,
            UserId = (int)loginId
        };

        _dbcontext.LikePosts.Add(newLike);
        await _dbcontext.SaveChangesAsync();

        int totalLikes = await _dbcontext.LikePosts.CountAsync(l => l.PostId == postId);
        return totalLikes;
    }

  
}
