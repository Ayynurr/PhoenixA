using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Application.DTOs.PostDto;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistance.Concretes;
using Persistance.DataContext;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class UserController : ControllerBase
{
    readonly IUserService _userService;
    readonly AppDbContext _dbcontext;
    readonly IWebHostEnvironment _hostEnvironment;
    readonly ILikeService _likeService;
    public UserController(IUserService userService, AppDbContext dbcontext, IWebHostEnvironment hostEnvironment, ILikeService likeService)
    {
        _userService = userService;
        _dbcontext = dbcontext;
        _hostEnvironment = hostEnvironment;
        _likeService = likeService;
    }


    [HttpPost("/api/User/CreateProfile")]
    public async Task<IActionResult> CreateAsync([FromForm] ProfileCreateDto profile)
    {
        try
        {
            await _userService.PrfileCreate(profile);
            return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Success", Message = "Profile create successfully!" });
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpPost("/api/User/UpdateProfile")]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto update)
    {
        try
        {
            return Ok(await _userService.ProfileUpdate(update));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

    }
    [HttpPost("/api/User/UpdateImage")]
    public async Task<ActionResult> UpdateImage([FromForm] UpdateProfileImage images)
    {
        try
        {
            return Ok(await _userService.UpdateImage(images));
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
}
