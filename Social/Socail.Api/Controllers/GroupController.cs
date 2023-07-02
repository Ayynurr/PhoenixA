using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;
using Persistance.Concretes;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class GroupController : ControllerBase
{
    readonly IGroupService _groupService;

    public GroupController(IGroupService gorupService)
    {
        _groupService = gorupService;
    }



    [HttpPost("Create/{groupName}")]
    public async Task<IActionResult> CreateGroup(string groupName)
    {
        try
        {
            await _groupService.CreateGroup(groupName);
            return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Create Group" });
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpPost("Accepted")]
    public async Task<IActionResult> AcceptedInvation(int groupId, int userId)
    {
        try
        {
            await _groupService.AcceptUserInvitation(groupId, userId);
            return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Accepted Invation" });
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpDelete("groups/{groupId}/users/{userId}")]
    public async Task<IActionResult> RemoveUserFromGroup(int groupId, int userId)
    {
        try
        {
            await _groupService.RemoveUserFromGroup(groupId, userId);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "User delete successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }
    [HttpGet("Users")]
    public async Task<IActionResult> GetUser(int groupId)
    {
        try
        {
            var users = await _groupService.GetUserAcceptedGroups(groupId);
            return StatusCode(StatusCodes.Status200OK, users);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }



    [HttpPost("groups/{groupId}/Post")]
    public async Task<IActionResult> AddPost(int groupId, string content, [FromForm] List<IFormFile> images)
    {
        try

        {
            await _groupService.CreatePost(groupId, content, images);
            return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Created Post" });
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpPost("groups/{groupId}/Post/Upload")]
    public async Task<IActionResult> UpdateGroupPost([FromForm] GroupPostUpdate updateDto)
    {
        try

        {
            return StatusCode(StatusCodes.Status200OK, await _groupService.UpdateGroupPost(updateDto));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpPost("InviteUserToGroup")]
    public async Task<IActionResult> InviteUserToGroup(int userId, int groupId)
    {
        try
        {
            await _groupService.InviteUserToGroup(userId, groupId);
            return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Invation user to group" });
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpDelete("groups/{groupId}/Post/{postId}")]
    public async Task<IActionResult> DeleteGroupPost(int groupId, int postId)
    {
        try
        {
            await _groupService.DeleteGroupPost(groupId,postId);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Remove Post Succesfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }

    #region First

    //[HttpPost("Create")]
    //public async Task<IActionResult> CreateGroup(UserGroupDto groupDTO)
    //{
    //    try
    //    {
    //        await _gorupService.CreateUserGroup(groupDTO);
    //        return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Create Group" });
    //    }
    //    catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

    //    catch (Exception)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError);
    //    }
    //}
    //[HttpPost("SendInvation")]
    //public async Task<IActionResult> Send(int groupId)
    //{
    //    try
    //    {
    //        await _gorupService.SendInvitation(groupId);
    //        return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Send Invation" });
    //    }
    //    catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

    //    catch (Exception)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError);
    //    }
    //}
    //[HttpPost("Accepted")]
    //public async Task<IActionResult> AcceptedInvation(int groupId)
    //{
    //    try
    //    {
    //        await _gorupService.AcceptInvitation(groupId);
    //        return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Accepted Invation" });
    //    }
    //    catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

    //    catch (Exception)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError);
    //    }
    //}
    //[HttpPost("Post")]
    //public async Task<IActionResult> AddPost([FromForm]AddPostDto dto)
    //{
    //    try
    //    {
    //        await _gorupService.AddPost(dto);
    //        return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Successfully", Message = "Created Post" });
    //    }
    //    catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

    //    catch (Exception)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError);
    //    }
    //}
    #endregion
}
