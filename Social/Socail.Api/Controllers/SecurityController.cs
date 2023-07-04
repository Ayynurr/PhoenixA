using Application.Abstracts;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Socail.Api.Controllers;


[Authorize(AuthenticationSchemes = "Bearer",Roles = "SuperAdmin")]
[Route("api/[controller]")]
public class SecurityController : ControllerBase
{
    readonly ISecurityService _securityService;

    public SecurityController(ISecurityService securityService)
    {
        _securityService = securityService;
    }
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _securityService.GetUsers());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }

  
    [HttpPut("BlockUser/{userId}")]
    public async Task<IActionResult> BlockUser(int userId, bool blockStatus, DateTime? blockEndDate)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _securityService.BlockUser(userId, blockStatus, blockEndDate));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPut("DeletedUser/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId,bool deleteStatus)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK,await _securityService.DeletedUser(userId, deleteStatus));
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,new ResponseDto { Status = "Error",Message = ex.Message });
        }
    }
    [HttpGet("GetGroups")]
    public async Task<IActionResult> GetGroups()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _securityService.GetGroups());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPut("DeleteGroup/{groupId}")]
    public async Task<IActionResult> DeleteGroup(int groupId)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _securityService.DeleteGroup(groupId));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpGet("GetPost")]
    public async Task<IActionResult> GetPosts()
    {
        try 
        {
            return StatusCode(StatusCodes.Status200OK, await _securityService.GetPosts());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPut("DeletePost/{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _securityService.DeletePost(postId));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }

}
