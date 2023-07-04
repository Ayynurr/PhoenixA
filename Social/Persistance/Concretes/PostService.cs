using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Application.DTOs.PostDto;
using Application.FileService;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Persistance.DataContext;
using Persistance.Extentions;
using static System.Net.Mime.MediaTypeNames;
using Image = Domain.Entities.Image;

namespace Persistance.Concretes;
public class PostService : IPostService
{
    private readonly AppDbContext _dbcontext;
    public readonly ICurrentUserService _currentUserService;
    private readonly IWebHostEnvironment _hostEnvironment;
    readonly IAzureFileService _azureService;
    public PostService(AppDbContext dbcontext, ICurrentUserService userService, IWebHostEnvironment hostEnvironment, IAzureFileService azureService = null)
    {
        _dbcontext = dbcontext;
        _currentUserService = userService;
        _hostEnvironment = hostEnvironment;
        _azureService = azureService;
    }


    public async Task<PostGetDto> CreateAsync(PostCreateDto post)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException("User Not Found");
        //dbcontext interceptor
        if (user.IsBlock)
        {
            throw new NotAuthorizedException("User is blocked. Cannot create a post.");
        }

        Post newPost = new()
        {
            UserId = (int)loginId,
            Content = post.Content,
            CreatedDate = DateTime.Now,
        };
        if (post.Images != null)
        {
            foreach (var file in post.Images)
            {
                if (file.CheckFileSize(2048))
                    throw new FileTypeException();
                if (!file.CheckFileType("image/"))
                    throw new FileSizeException();
                //string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "Images");
                FileUploadResult fileUploadResult = await _azureService.UploadFileAsync("postimages", file);
                //newPost.ImageName = newFileName;
                newPost.Images.Add(new Image { Path = Path.Combine(_hostEnvironment.WebRootPath, "Images"), ImgName = fileUploadResult.fileName });

                //newPost.ImageName = fileUploadResult.fileName;
            }
            #region 
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
        List<ImageGetDto> imageDtos = newPost.Images.Select(i => new ImageGetDto()
        {
            Url = $"https://socialapi.blob.core.windows.net/postimages/{i.ImgName}"
        }).ToList();

        _dbcontext.Posts.Add(newPost);
        await _dbcontext.SaveChangesAsync();
        return new PostGetDto() { Content = newPost.Content, Id = newPost.Id, Images = imageDtos };

    }

    public async Task<PostGetDto> GetByIdAsync(int id)
    {
        Post? post = await _dbcontext.Posts.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotfoundException();

        List<ImageGetDto> imageDtos = post.Images.Select(i => new ImageGetDto()
        {

            Url = $"https://socialapi.blob.core.windows.net/postimages/{i.ImgName}"
        }).ToList();


        PostGetDto postGetDto = new PostGetDto()
        {
            Content = post.Content,
            Id = post.Id,
            Images = imageDtos
        };


        return postGetDto;
    }
    public async Task<List<PostGetDto>> GetAllAsync()
    {
        var posts = await _dbcontext.Posts.Include(i => i.Images)
            .Select(s => new PostGetDto { Id = s.Id, Content = s.Content })
            .ToListAsync();

        foreach (var post in posts)
        {
            var images = await _dbcontext.Images
                .Where(i => i.PostId == post.Id)
                .Select(i => new ImageGetDto
                {

                    Url = $"https://socialapi.blob.core.windows.net/postimages/{i.ImgName}"
                })
                .ToListAsync();

            post.Images.AddRange(images);
        }

        return posts;
    }

    public async Task<PostGetDto> UpdateAsync(PostUpdateDto post, int id)
    {
        Post? newPost = await _dbcontext.Posts.Include(i => i.Images).FirstOrDefaultAsync(s => s.Id == id) ??
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
        List<ImageGetDto> updateImages = new();

        foreach (var file in updateImage.Images)
        {
            if (file.CheckFileSize(2048))
                throw new FileTypeException();
            if (!file.CheckFileType("image/"))
                throw new FileSizeException();
            //string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "Images");
            FileUploadResult fileUploadResult = await _azureService.UploadFileAsync("postimages", file);

            Image newImage = new()
            {
                ImgName = fileUploadResult.fileName,
                PostId = postId,
                Path = $"https://socailapi.blob.core.windows.net/{fileUploadResult.filePath}",
                UpdatedDate = DateTime.Now
            };
            newPost.Images.Add(newImage);
            updateImages.Add(new ImageGetDto
            {

                Url = $"https://socailapi.blob.core.windows.net/{fileUploadResult.filePath}"
            });
        }
        await _dbcontext.SaveChangesAsync();
        return updateImages;

        ///COUNTRY
    }

    public async Task DeleteAsync(int id)
    {
        Post? post = await _dbcontext.Posts.FirstOrDefaultAsync(i => i.Id == id) ?? throw new NotfoundException();

        foreach (var image in post.Images)
        {
            string blobName = image.Path.Substring(image.Path.LastIndexOf('/') + 1);
        }

        _dbcontext.Posts.Remove(post);
        await _dbcontext.SaveChangesAsync();
    }
}




