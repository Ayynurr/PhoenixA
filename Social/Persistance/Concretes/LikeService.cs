using Application.Abstracts;
using Application.DTOs.LikeDto;

namespace Persistance.Concretes;

public class LikeService : ILikeService
{
    private readonly DbContext _dbContext;

    public LikeService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CreateAsync(LikeCreateDto like)
    {
        throw new NotImplementedException();
    }

    //public Task CreateAsync(LikeCreateDto like)
    //{
    //    // Yeni bir "Like" nesnesi oluşturun
    //    var newLike = new Like
    //    {
    //        CommentId = likeDto.CommentId,
    //        UserId = likeDto.UserId
    //    };

    //    // "Like" nesnesini veritabanına ekleyin
    //    _dbContext.Likes.Add(newLike);
    //    await _dbContext.SaveChangesAsync();
    //}


}
