using Application.Abstracts;
using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Persistance.Concretes;
using Persistance.DataContext;

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
        //await _commentService.CommentDeleteAsync(id);
        //return StatusCode(StatusCodes.Status200OK);
        try
        {
            await _commentService.CommentDeleteAsync(id);
            return StatusCode(StatusCodes.Status204NoContent, new ResponseDto { Status = "Successs", Message = "Comment delete successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new ResponseDto { Status = "Error", Message = ex.Message });
        }

        //try
        //{
        //    return Ok(await _commentService.CommentDeleteAsync(id));
        //}
        //catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

        //catch (Exception)
        //{
        //    return StatusCode(StatusCodes.Status500InternalServerError);
        //}
    }
    [HttpPost("/api/Comments")]
    public async Task<IActionResult> GettAll()
    {
        try
        {
            return StatusCode(200, await _dbcontext.Comments.ToListAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                ex.Message
            });
        }
    }
    [HttpPost("comment/{commentId}")]
    public async Task<IActionResult> LikeComment([FromRoute] int commentId, [FromRoute] int userId)
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
}
