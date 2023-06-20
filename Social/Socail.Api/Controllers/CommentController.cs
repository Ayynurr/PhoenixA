using Application.Abstracts;
using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Concretes;
using Persistance.DataContext;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CommentController : ControllerBase
{
    readonly ICommentService _commentService;
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
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
    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete([FromRoute] int id)
    //{

    //    try
    //    {
    //       return Ok(await _commentService.CommentDeleteAsync(id));
    //    }
    //    catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message }); }

    //    catch (Exception)
    //    {
    //        return StatusCode(StatusCodes.Status500InternalServerError);
    //    }
    //}
}
