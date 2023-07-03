using Application.DTOs;
using Domain.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application;
using Application.Abstracts;
using Persistance.DataContext;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Infrastructure.Services;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class StoryController : ControllerBase
{
    readonly IStoryService _storyService;
    readonly ICurrentUserService _currentUserService;
    readonly AppDbContext _dbcontext;
    readonly IWebHostEnvironment _hostEnvironment;
  
    public StoryController(IStoryService storyService, ICurrentUserService currentUserService, AppDbContext dbcontext, IWebHostEnvironment hostEnvironment)
    {
        _storyService = storyService;
        _currentUserService = currentUserService;
        _dbcontext = dbcontext;
        _hostEnvironment = hostEnvironment;
        
    }

    [HttpPost("video")]
    public async Task<IActionResult> CreateVideoAsync([FromForm] CreateVideo story)
    {
        try
        {

            //_archiveJob.ScheduleArchiveJob();
            return Ok(await _storyService.CreateVideoAsync(story));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpPost("image")]
    public async Task<IActionResult> CreateImageAsync([FromForm] CreateStoryDto story)
    {
        try
        {
            //_archiveJob.ScheduleArchiveJob();
            return Ok(await _storyService.CreateStoryImageAsync(story));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet("stories/{storyId}/image")]
    public async Task<IActionResult> GetStoryImage(int storyId)
    {
        Story? story = await _dbcontext.Stories.FindAsync(storyId);

        if (story == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(story.ImageName))
        {
            return NotFound();
        }

        string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "StoryImages", story.ImageName);

        byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

        return File(imageBytes, "image/jpeg");
    }

    [HttpGet("stories/{storyId}/video")]
    public async Task<IActionResult> GetStoryVideo(int storyId)
    {
        Story? story = await _dbcontext.Stories.FindAsync(storyId);
        if (story == null)
        {
            return NotFound();
        }

        string videoPath = Path.Combine(_hostEnvironment.WebRootPath, "StoryVideo", story.VideoName);
        if (!System.IO.File.Exists(videoPath))
        {
            return NotFound();
        }

        byte[] videoBytes = await System.IO.File.ReadAllBytesAsync(videoPath);
        string mimeType = "video/mp4";
        return File(videoBytes, mimeType, story.VideoName);
    }
    [HttpGet("archive/{id}")]
    public async Task<ActionResult> ArchiveAsync([FromRoute] int id)
    {
        try
        {
            
            return StatusCode(StatusCodes.Status200OK, await _storyService.ArchiveAsync(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpGet("archive/User/{username}")]
    public async Task<ActionResult> GetUserAsync([FromRoute] string username)
    {
        try
        {

            return StatusCode(StatusCodes.Status200OK, await _storyService.GetUserAsync(username));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpGet("GetFriends")]
    public async Task<ActionResult> GetFriend()
    {
        try
        {

            return StatusCode(StatusCodes.Status200OK, await _storyService.GetFriendAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpGet("GetAll")]
    public async Task<ActionResult> GetAll()
    {
        try
        {

            return StatusCode(StatusCodes.Status200OK, await _storyService.GetAllAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
  
    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await _storyService.DeleteAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Story delete successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
   
}
