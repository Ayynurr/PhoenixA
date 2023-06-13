using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    readonly IPostService _postService;
    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostCreateDto post)
    {
        try
        {
        return Ok(await _postService.CreateAsync(post));
        }
        catch (NotfoundException ex) { return NotFound(new ResponseDto { Message = ex.Message}); }

        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
