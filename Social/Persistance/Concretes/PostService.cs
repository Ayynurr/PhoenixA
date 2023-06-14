using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Persistance.DataContext;

namespace Persistance.Concretes;

public class PostService : IPostService
{
    private readonly AppDbContext _dbcontext;
    
    public PostService(AppDbContext dbcontext)=> _dbcontext = dbcontext;
        
    

    public async Task<PostGetDto> CreateAsync(PostCreateDto post)
    {
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == post.UserId)
        ?? throw new NotfoundException("User Not Found");
        //dbcontext interceptor
        Post newPost = new Post()
        {
            UserId = user.Id,
            Content = post.Content,
        };
        await _dbcontext.Posts.AddAsync(newPost);
        await _dbcontext.SaveChangesAsync();
        return new PostGetDto() { Content = newPost.Content, Id= newPost.Id };

    }
}
