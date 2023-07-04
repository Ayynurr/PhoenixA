using Application.Abstracts;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] string? username , [FromQuery] string? groupname)
    {
        try
        {
            List<AppUser> userSearch = await _searchService.Search(username, groupname);
            if (userSearch.Count > 0) return Ok(userSearch);

            else return NotFound("No Search results found");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
