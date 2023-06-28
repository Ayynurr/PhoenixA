using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class GroupPostDto
{
    public int PostId { get; set; }
    public int GroupId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<IFormFile> Images { get; set; }
    public string ImageName { get; set; }
}
