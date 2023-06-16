using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Persistance.DataContext;
using Persistance.Extentions;

namespace Persistance.Concretes;
public class PostService : IPostService
{
    private readonly AppDbContext _dbcontext;
    public readonly ICurrentUserService _currentUserService;
    private readonly IWebHostEnvironment _hostEnvironment;
    public PostService(AppDbContext dbcontext, ICurrentUserService userService, IWebHostEnvironment hostEnvironment)
    {
        _dbcontext = dbcontext;
        _currentUserService = userService;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<PostGetDto> CreateAsync(PostCreateDto post)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException("User Not Found");
        //dbcontext interceptor

        Post newPost = new()
        {
            UserId = (int)loginId,
            Content = post.Content,
        };
        if (post.Images != null)
        {
            foreach (var file in post.Images)
            {
                if (file.CheckFileSize(2048))
                    throw new FileTypeException();
                if (!file.CheckFileType("image/"))
                    throw new FileSizeException();
                string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "Images");
                newPost.ImageName = newFileName;
            }


            //{
            //    AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == post.UserId)
            //    ?? throw new NotfoundException("User Not Found");
            //    if (post.ImageName != null)
            //    {
            //        foreach (IFormFile file in post.ImageName)
            //        {
            //            if (!file.CheckFileSize(2048))
            //                throw new FileTypeException();
            //            if (!file.CheckFileType("image/"))
            //                throw new FileSizeException();
            //            string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "Images");
            //        }


            //        post.Images = new List<Image>();

            //        foreach (IFormFile item in post.ImageName)
            //        {
            //            Post image = new()
            //            {
            //                UserId = loginId,
            //                Content = post.Content,
            //            };

            //        }
            //    }

        }

        await _dbcontext.Posts.AddAsync(newPost);
        await _dbcontext.SaveChangesAsync();
        return new PostGetDto() { Content = newPost.Content, Id = newPost.Id };

    }
    public async Task<PostGetDto> GetByIdAsync(int id)
    {
        Post? post = await _dbcontext.Posts.FirstOrDefaultAsync(s => s.Id == id) ??
           throw new NotfoundException();

        return new PostGetDto() { Content = post.Content, Id = post.Id };

    }
    public async Task<PostGetDto> GetAllAsync()
    {
        List<Post>? posts = await _dbcontext.Posts.ToListAsync() ?? throw new NotfoundException();
        List<PostGetDto> postDtos = posts.Select(p => new PostGetDto
        {
            Content = p.Content,
            Images = (ICollection<Image>)p.Images.Select(i => i.ImgName).ToList()
        }).ToList();

        return new PostGetDto { };

    }


}
