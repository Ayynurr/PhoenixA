using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Application.DTOs.PostDto;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Persistance.Concretes;
using Persistance.DataContext;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]

public class PostController : ControllerBase
{
    readonly IPostService _postService;
    readonly AppDbContext _dbcontext;
    readonly IWebHostEnvironment  _hostEnvironment;
    readonly ILikeService _likeService;
    public PostController(IPostService postService, AppDbContext dbcontext, IWebHostEnvironment hostEnvironment, ILikeService likeService)
    {
        _postService = postService;
        _dbcontext = dbcontext;
        _hostEnvironment = hostEnvironment;
        _likeService = likeService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PostCreateDto post)
    {
        try
        {
        return Ok(await _postService.CreateAsync(post));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message}); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet()]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            return StatusCode(200, await _dbcontext.Posts.ToListAsync());
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
            return StatusCode(200, await _dbcontext.Posts.FirstOrDefaultAsync(s => s.Id == id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }
        #region 
        // Post? post = await _dbcontext.Posts.FirstOrDefaultAsync(s => s.Id == id);
        // if (post == null)
        // {
        //     return NotFound();
        // }
        //return StatusCode(StatusCodes.Status200OK, post);
        #endregion
    }

    [HttpGet("Images/{ImageName}")]
    public async Task<IActionResult> GetImagesAsync([FromRoute] string ImageName)
    {
        var file = await _dbcontext.Posts.FirstOrDefaultAsync(f => f.ImageName == ImageName)
            ?? throw new Exception("Image not found");

        string path = Path.Combine(_hostEnvironment.WebRootPath, "Images", file.ImageName);
        if (!System.IO.File.Exists(path))
            throw new Exception("File not found");

        FileExtensionContentTypeProvider provider = new();
        byte[] imageBytes = System.IO.File.ReadAllBytes(path);


        if (provider.TryGetContentType(path, out string? contentType))
            contentType = "application/octet-stream";

        return File(imageBytes, contentType);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost( int id, [FromBody] PostUpdateDto post)
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

    [HttpPost("{postId}/Images")]
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
    [HttpPost("post/{postId}")]
    public async Task<IActionResult> LikePost([FromRoute] int postId)
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
}
