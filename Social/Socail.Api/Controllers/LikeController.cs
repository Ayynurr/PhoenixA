using Application.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistance.Concretes;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(AuthenticationSchemes = "Bearer")]
public class LikeController : ControllerBase
{
    readonly ILikeService  _likeService;
    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost("comment/{commentId}")]
    public async Task<IActionResult> LikeComment([FromRoute]int commentId,[FromRoute]  int userId)
    {
        try
        {
            int totalLikes = await _likeService.LikeComment(commentId, userId);
            return Ok(new { TotalLikes = totalLikes });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("post/{postId}")]
    public async Task<IActionResult> LikePost([FromRoute] int postId, [FromRoute] int userId)
    {
        try
        {
            int totalLikes = await _likeService.LikePost(postId, userId);
            return Ok(new { TotalLikes = totalLikes });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
