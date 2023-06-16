using Application.Abstracts;
using Application.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]

public class PostController : ControllerBase
{
    readonly IPostService _postService;
    readonly AppDbContext _dbcontext;
    public PostController(IPostService postService, AppDbContext dbcontext )
    {
        _postService = postService;
        _dbcontext = dbcontext;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] PostCreateDto post)
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
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            return StatusCode(200, await _dbcontext.Posts.ToListAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = ex.Message
            });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        Post? post = await _dbcontext.Posts.FirstOrDefaultAsync(s => s.Id == id);
        if (post == null)
        {
            return NotFound();
        }
       return StatusCode(StatusCodes.Status200OK, post);
    }

}
