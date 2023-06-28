using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance;
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
    readonly ICurrentUserService _currentUserService;
    public UserController(IUserService userService, AppDbContext dbcontext, IWebHostEnvironment hostEnvironment, ILikeService likeService, ICurrentUserService currentUserService )
    {
        _userService = userService;
        _dbcontext = dbcontext;
        _hostEnvironment = hostEnvironment;
        _likeService = likeService;
        _currentUserService = currentUserService;
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
   
    //[HttpPost("/api/Users")]
    //public async Task<IActionResult> UserGet()
    //{
    //    try
    //    {
    //        return StatusCode(200, await _userService.UserGet());
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError, new
    //        {
    //            ex.Message
    //        });
    //    }
    //}
    //[HttpGet("GetImages/")]
    //public async Task<IActionResult> GetImagesAsync()
    //{
    //    var loginId = _currentUserService.UserId;
    //    var fileProfile = await _dbcontext.UserImages.FirstOrDefaultAsync(f => f.UserId == loginId)
    //        ?? throw new Exception("Image not found");

    //    UriBuilder? uriBuilder = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port ?? -1);

    //    string publicImage = Path.Combine(uriBuilder.Uri.AbsoluteUri, "UserImages", fileProfile.ProfileImageName);
    //    string publicBack = Path.Combine(uriBuilder.Uri.AbsoluteUri, "UserImages", fileProfile.BackraundImageName);


    //    #region
    //    //string pathProfile = Path.Combine(_hostEnvironment.WebRootPath, "UserImages", fileProfile.ProfileImageName);
    //    //string pathBack = Path.Combine(_hostEnvironment.WebRootPath, "UserImages", fileProfile.BackraundImageName);
    //    //if (!System.IO.File.Exists(pathProfile))
    //    //    throw new Exception("File not found");
    //    //if (!System.IO.File.Exists(pathBack))
    //    //    throw new Exception("File not found");

    //    //FileExtensionContentTypeProvider provider = new();
    //    //byte[] imageBytes1 = System.IO.File.ReadAllBytes(pathProfile);
    //    //byte[] imageBytes2 = System.IO.File.ReadAllBytes(pathBack);

    //    //string contentTypeProfile;
    //    //string contentTypeBack;

    //    //if (!provider.TryGetContentType(pathProfile, out contentTypeProfile))
    //    //    contentTypeProfile = "application/octet-stream";
    //    //if (!provider.TryGetContentType(pathBack, out contentTypeBack))
    //    //    contentTypeBack = "application/octet-stream";

    //    //var base64Profile = Convert.ToBase64String(imageBytes1);
    //    //var base64Back = Convert.ToBase64String(imageBytes2);

    //    //var result = new
    //    //{
    //    //    ProfileImage = base64Profile,
    //    //    BackgroundImage = base64Back
    //    //};
    //    #endregion

    //    return Ok(new {profile= publicImage,profileback = publicBack });
    //}


    [HttpPost("{username}")]
    public async Task<IActionResult> UserGetByUsername(string username)
    {
        try
        {
            return StatusCode(200, await _userService.UserGetByUsername(username));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }

    }
    [HttpDelete("api/User/DeleteImage")]
    public async Task<IActionResult> DeleteAsync(int imageId)
    {
        try
        {
            await _userService.DeleteImage(imageId);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Post delete successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }

    }
    [HttpPost("backimage")]
    public async Task<IActionResult> BackImages([FromForm] ProfileCreateDto profilCreate)
    {

        try
        {
            await _userService.BackCreateAsync(profilCreate);
            return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Success", Message = "BackImage create successfully!" });
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("GetUserGroup")]
    public async Task<IActionResult> GetUserGroups()
    {
        try
        {
            return StatusCode(200, await _userService.GetUserGroups());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }

    }
    [HttpPost("IsUserInGroup")]
    public async Task<IActionResult> IsUserInGroup(int groupId)
    {
        try
        {
            return StatusCode(200, await _userService.IsUserInGroup(groupId));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }

    }
    [HttpPost("InviteUserToGroup")]
    public async Task<IActionResult> InviteUserToGroup(int groupId)
    {
        try
        {
            await _userService.InviteUserToGroup(groupId);
            return StatusCode(200,new ResponseDto { Status="Succesfully",Message="User Invited to Group"});
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }

    }
    [HttpDelete("groups/{groupId}")]
    public async Task<IActionResult> RemoveUserFromGroup(int groupId)
    {
        try
        { 
            await _userService.RemoveUserFromGroup(groupId);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "I left this group" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPost("Respond")]
    public async Task<IActionResult> RespondToGroupInvitation(int groupId, bool acceptInvitation)
    {
        try
        {
            await _userService.RespondToGroupInvitation(groupId, acceptInvitation);
            return StatusCode(200, new ResponseDto { Status = "Succesfully", Message = "Respond To Group Invitation" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }

    }

}
