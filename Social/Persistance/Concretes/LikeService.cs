using Application.Abstracts;
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
    public async Task<int> LikeComment(int commentId, int userId)
    {
        var existingLike = await _dbcontext.Likes
            .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);

        if (existingLike != null)
        {
            throw new Exception("You have already liked this comment.");
        }

        var newLike = new Like
        {
            CommentId = commentId,
            UserId = userId
        };

        _dbcontext.Likes.Add(newLike);
        await _dbcontext.SaveChangesAsync();

        int totalLikes = await _dbcontext.Likes.CountAsync(l => l.CommentId == commentId);
        return totalLikes;
    }

    public async Task<int> LikePost(int postId, int userId)
    {
        var existingLike = await _dbcontext.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

        if (existingLike != null)
        {
            throw new Exception("You have already liked this post.");
        }

        var newLike = new Like
        {
            PostId = postId,
            UserId = userId
        };

        _dbcontext.Likes.Add(newLike);
        await _dbcontext.SaveChangesAsync();

        int totalLikes = await _dbcontext.Likes.CountAsync(l => l.PostId == postId);
        return totalLikes;
    }

  
}
