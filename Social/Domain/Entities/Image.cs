using Domain.Entities.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Image : BaseEntity
{
    public int Id { get; set; }
    public string ImageName { get; set; }
    public bool IsProfileImage { get; set; }
    public bool IsBacroundImage { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    [NotMapped]
    public IFormFile ImageUrl { get; set; }

}
