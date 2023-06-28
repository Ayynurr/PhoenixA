using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class GroupPostUpdate
{
    public int PostId { get; set; }
    public string Content { get; set; }
    public List<IFormFile> Images { get; set; }
}
