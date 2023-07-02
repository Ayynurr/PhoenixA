using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Application.DTOs.PostDto;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]

public class PostController : ControllerBase
{
    readonly IPostService _postService;
    readonly AppDbContext _dbcontext;
    readonly IWebHostEnvironment _hostEnvironment;
    readonly ILikeService _likeService;
    readonly ICurrentUserService _currentUserService;
    public PostController(IPostService postService, AppDbContext dbcontext, IWebHostEnvironment hostEnvironment, ILikeService likeService, ICurrentUserService currentUserService)
    {
        _postService = postService;
        _dbcontext = dbcontext;
        _hostEnvironment = hostEnvironment;
        _likeService = likeService;
        _currentUserService = currentUserService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PostCreateDto post)
    {
        try
        {
            return Ok(await _postService.CreateAsync(post));
        }
        catch (NotfoundException ex)
        {
            return NotFound(new ResponseDto { Message = ex.Message });
        }
        catch (FileTypeException ex)
        {
            return BadRequest(new ResponseDto { Message = ex.Message });
        }
        catch (FileSizeException ex)
        {
            return BadRequest(new ResponseDto { Message = ex.Message });
        }
        catch (Exception ex)
        {
            // Inner exception'ı loglama veya hata mesajında gösterme
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Message = ex.InnerException?.Message ?? ex.Message });
        }
    }
    [HttpGet("/api/Posts")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            return StatusCode(200, await _postService.GetAllAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }
    }
    [HttpGet("{id}")]

    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            return StatusCode(200, await _postService.GetByIdAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }
       
    }
    #region GetImage
    //[HttpGet("GetImages")]
    //public async Task<IActionResult> GetImagesAsync(int postId)
    //{
    //    //var loginId = _currentUserService.UserId;
    //    //var file = await _dbcontext.Posts.FirstOrDefaultAsync(f => f.UserId == loginId)
    //    //    ?? throw new Exception("Image not found");

    //    //var uriBuilder = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port ?? -1);

    //    //string publicImage = Path.Combine(uriBuilder.Uri.AbsoluteUri, "Images", file.ImageName);




    //    //return Ok(new { profile = publicImage });

    //    Post? post = await _dbcontext.Posts.FindAsync(postId);

    //    if (post == null)
    //    {
    //        return NotFound();
    //    }

    //    if (string.IsNullOrEmpty(post.ImageName))
    //    {
    //        return NotFound();
    //    }

    //    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", post.ImageName);

    //    byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

    //    return File(imageBytes, "image/jpeg");
    //    #region
    //    //var file = await _dbcontext.Posts.FirstOrDefaultAsync(f => f.ImageName == ImageName)
    //    //    ?? throw new Exception("Image not found");

    //    //string path = Path.Combine(_hostEnvironment.WebRootPath, "Images", file.ImageName);
    //    //if (!System.IO.File.Exists(path))
    //    //    throw new Exception("File not found");

    //    //FileExtensionContentTypeProvider provider = new();
    //    //byte[] imageBytes = System.IO.File.ReadAllBytes(path);


    //    //if (provider.TryGetContentType(path, out string? contentType))
    //    //    contentType = "application/octet-stream";

    //    //return File(imageBytes, contentType);
    //    #endregion
    //}
    //[HttpGet("GetImages")]
    //public async Task<IActionResult> GetImagesAsync(int postId)
    //{
    //    Post post = await _dbcontext.Posts.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == postId);

    //    if (post == null || post.Images.Count == 0)
    //    {
    //        return NotFound();
    //    }

    //    List<byte[]> imageBytesList = new List<byte[]>();

    //    foreach (var image in post.Images)
    //    {
    //        string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", image.ImgName);

    //        if (!System.IO.File.Exists(imagePath))
    //        {
    //            return NotFound();
    //        }

    //        byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
    //        imageBytesList.Add(imageBytes);
    //    }

    //    return Ok(imageBytesList);
    //}
    #endregion

    [HttpGet("Images/{ImageName}")]

    public async Task<IActionResult> GetImagesAsync([FromRoute] string ImageName)
    {
        var file = await _dbcontext.Images.FirstOrDefaultAsync(f => f.ImgName == ImageName)
        ?? throw new Exception("Image not found");

        string path = Path.Combine(_hostEnvironment.WebRootPath, "Images", file.ImgName);
        if (!System.IO.File.Exists(path))
            throw new Exception("File not found");

        FileExtensionContentTypeProvider provider = new();
        byte[] imageBytes = System.IO.File.ReadAllBytes(path);


        if (provider.TryGetContentType(path, out string? contentType))
            contentType = "application/octet-stream";

        return File(imageBytes, contentType);
    } 

    #region OneImageGosterirSwagger
    //[HttpGet("post/{postId}/image")]
    //public async Task<IActionResult> GetStoryImage(int postId)
    //{
    //    Post? post = await _dbcontext.Posts.FirstOrDefaultAsync(i => i.Id == postId);

    //    if (post == null)
    //    {
    //        return NotFound();
    //    }

    //    if (string.IsNullOrEmpty(post.ImageName))
    //    {
    //        return NotFound();
    //    }

    //    string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Images", post.ImageName);

    //    byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

    //    return File(imageBytes, "image/jpeg");

    //}
    #endregion

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdateDto post)
    {
        try
        {
            return Ok(await _postService.UpdateAsync(post, id));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }

    [HttpPut("{postId}/UpdateImages")]
    public async Task<ActionResult> UpdatePostAsync(int postId, [FromForm] UpdateImageDto images)
    {
        try
        {
            return Ok(await _postService.UpdateImagesAsync(images, postId));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (FileTypeException ex)
        {
            throw new FileTypeException();
        }
        catch (FileSizeException)
        {
            throw new FileSizeException();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    [HttpPost("/api/Post/Like")]
    public async Task<IActionResult> LikePost(int postId)
    {
        try
        {
            int totalLikes = await _likeService.LikePost(postId);
            return Ok(new { TotalLikes = totalLikes });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _postService.DeleteAsync(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }

}
