using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.DataContext;
using System.IdentityModel.Tokens.Jwt;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CommentController : ControllerBase
{
    readonly ICommentService _commentService;
    readonly AppDbContext _dbcontext;
    readonly ILikeService _likeService;
    public CommentController(ICommentService commentService, AppDbContext dbcontext, ILikeService likeService)
    {
        _commentService = commentService;
        _dbcontext = dbcontext;
        _likeService = likeService;
    }
    private string GetInnermostExceptionMessage(Exception ex)
    {
        var innerException = ex;
        while (innerException.InnerException != null)
        {
            innerException = innerException.InnerException;
        }

        return innerException.Message;
    }
    [HttpPost("{postId}")]
    public async Task<IActionResult> Create([FromRoute] int postId,[FromBody] CommentCreateDto comment)
    {
        try
        {
            return Ok(await _commentService.CreateAsync(postId,comment));
        }
        catch (NotfoundException ex)
        {
            return NotFound(new ResponseDto { Message = ex.Message });
        }
        catch (Exception ex)
        {
            var innerExceptionMessage = GetInnermostExceptionMessage(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Message = "Sunucu hatası: " + innerExceptionMessage });
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        
        try
        {
            await _commentService.CommentDeleteAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Comment delete successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }

    }
  
    [HttpPost("/api/Comment/Like/{commentId}")]
    public async Task<IActionResult> LikeComment(int commentId)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");

            if (userIdClaim != null)
            {
                var userId = userIdClaim.Value;

                int totalLikes = await _likeService.
                    LikeComment( commentId);
                return Ok(new { TotalLikes = totalLikes });
            }   
            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    [HttpGet("{postId}")]
    public async Task<IActionResult> GetComments(int postId)
    {
        try
        {
            return StatusCode(StatusCodes.Status200OK, await _commentService.GetPostComment(postId));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = ex.Message });
        }
    }

}

