using Application.Abstracts;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Socail.Api.Controllers;


//[ApiController]
[Authorize(AuthenticationSchemes = "Bearer",Roles = "SuperAdmin")]
[Route("api/[controller]")]
//[Area("SuperAdmin")]
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
  
}
