using Domain.Entities.Base;
using Domain.Entities.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Image : BaseAuditable
{
    public string ImgName { get; set; }

    [ForeignKey("PostId")]
    public Post? Post { get; set; }
    public int PostId { get; set; }
    

   
}
