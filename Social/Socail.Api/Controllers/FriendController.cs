using Application;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance;
using Persistance.DataContext;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class FriendController : ControllerBase
{
    readonly AppDbContext _dbcontext;
    readonly IFriendService _friendService;

    public FriendController(AppDbContext dbcontext, IFriendService friendService)
    {
        _dbcontext = dbcontext;
        _friendService = friendService;
    }

    [HttpPost("confirm/{id}")]
    public async Task<ActionResult> ConfirmFriend(int id)
    {
        try
        {
            await _friendService.ConfirmFriendAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Confirm successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPost("add/{id}")]
    public async Task<ActionResult> AddFriend(int id)
    {
        try
        {
            await _friendService.AddFriendAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Add Friend successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteFriend(int id)
    {
        try
        {
            await _friendService.DeleteFriendAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Delete friend successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpDelete("block/{id}")]
    public async Task<ActionResult> FriendBlock(int id)
    {
        try
        {
            await _friendService.FriendBlockAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Friend block successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPost("unblock/{id}")]
    public async Task<ActionResult> FriendUnblock(int id)
    {
        try
        {
            await _friendService.FriendUnBlockAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Friend unblock successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpDelete("declined/{id}")]
    public async Task<ActionResult> Declined(int id)
    {
        try
        {
            await _friendService.DeclinedAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Friend declined successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPost("all")]
    public async Task<ActionResult> UserGetFriends()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK ,await _friendService.GetAllFriendsAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpPost("allRequest")]
    public async Task<ActionResult> UserRequestFriends()
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _friendService.GetRequestFriends());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }

}
