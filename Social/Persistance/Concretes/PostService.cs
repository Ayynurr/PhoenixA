using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Application.DTOs.PostDto;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
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

            #region Lazimsiz
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
            #endregion
        }

         _dbcontext.Posts.Add(newPost);
        await _dbcontext.SaveChangesAsync();
        return new PostGetDto() { Content = newPost.Content, Id = newPost.Id ,};

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
            Id = p.Id,
            Content = p.Content,
        }).ToList();

        return new PostGetDto { };

    }
    public async Task<PostGetDto> UpdateAsync(PostUpdateDto post, int id)
    {
        Post? newPost = await _dbcontext.Posts.FirstOrDefaultAsync(s => s.Id == id) ??
           throw new NotfoundException();
        newPost.Content = post.Content;
        newPost.Id = id;
        await _dbcontext.SaveChangesAsync();
        return new PostGetDto
        {
            Id = newPost.Id,
            Content = newPost.Content,
        };

    }
    public async Task<List<ImageGetDto>> UpdateImagesAsync(UpdateImageDto updateImage, int postId)
    {
        Post? newPost = await _dbcontext.Posts.FirstOrDefaultAsync(s => s.Id == postId) ??
          throw new NotfoundException();
        newPost.Images ??= new List<Image>();
        List<ImageGetDto> updateImages = new();

        foreach (var file in updateImage.Images)
        {
            if (file.CheckFileSize(2048))
                throw new FileTypeException();
            if (!file.CheckFileType("image/"))
                throw new FileSizeException();
            string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "Images");
        Image newImage = new()
        {
            ImgName = newFileName,
            PostId = postId,
            Path = Path.Combine(_hostEnvironment.WebRootPath, "Images")
        };
        newPost.Images.Add(newImage);
        updateImages.Add(new ImageGetDto
        {
            ImageName = newImage.ImgName,
            PostId = postId,
            Url = $"https://localhost:7275/api/Post/Images/{newPost.ImageName}"
        });
        }
        await _dbcontext.SaveChangesAsync();
        return updateImages;

        ///COUNTRY
    }

   
}




