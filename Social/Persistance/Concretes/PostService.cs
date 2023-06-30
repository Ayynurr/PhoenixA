using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Application.DTOs.PostDto;
using Domain.Entities;
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
                string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "Images");
              
                //newPost.ImageName = newFileName;
                newPost.Images.Add(new Image { Path = Path.Combine(_hostEnvironment.WebRootPath, "Images"), ImgName=newFileName});

               newPost.ImageName = newFileName;
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
                Url = $"https://localhost:7046/api/Hotel/Images/{i.ImgName}"
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
          
            Url = $"https://localhost:7046/api/Hotel/Images/{i.ImgName}"
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
        var posts = await _dbcontext.Posts.Include(i=>i.Images)
            .Select(s => new PostGetDto { Id = s.Id, Content = s.Content })
            .ToListAsync();

        foreach (var post in posts)
        {
            var images = await _dbcontext.Images
                .Where(i => i.PostId == post.Id)
                .Select(i => new ImageGetDto
                {
                 
                    Url = $"https://localhost:7046/api/Hotel/Images/{i.ImgName}"
                })
                .ToListAsync();

            post.Images.AddRange(images);
        }

        return posts;
    }

    public async Task<PostGetDto> UpdateAsync(PostUpdateDto post, int id)
    {
        Post? newPost = await _dbcontext.Posts.Include(i=>i.Images).FirstOrDefaultAsync(s => s.Id == id) ??
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
                Path = Path.Combine(_hostEnvironment.WebRootPath, "Images"),
                UpdatedDate = DateTime.Now
            };
            newPost.Images.Add(newImage);
            updateImages.Add(new ImageGetDto
            {
               
                Url = $"https://localhost:7275/api/Post/Images/{newPost.ImageName}"
            });
        }
        await _dbcontext.SaveChangesAsync();
        return updateImages;

        ///COUNTRY
    }

    public async Task DeleteAsync(int id)
    {

        Post? post = await _dbcontext.Posts.FirstOrDefaultAsync(i => i.Id == id) ?? throw new NotfoundException();
        string path = Path.Combine(_hostEnvironment.WebRootPath,"Images");
        if (path != null)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        _dbcontext.Posts.Remove(post);
        await _dbcontext.SaveChangesAsync();

    }
}




