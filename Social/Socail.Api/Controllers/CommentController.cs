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
    public CommentController(ICommentService commentService, AppDbContext dbcontext, ILikeService likeService )
    {
        _commentService = commentService;
        _dbcontext = dbcontext;
        _likeService = likeService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CommentCreateDto comment)
    {
        try
        {
            return Ok(await _commentService.CreateAsync(comment));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
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
    public async Task<IActionResult> LikeComment([FromRoute] int commentId)
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
}

