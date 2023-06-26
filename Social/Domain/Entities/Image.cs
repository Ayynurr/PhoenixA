using Domain.Entities.Base;

namespace Domain.Entities;

public class Image : BaseAuditable
{
    public string ImgName { get; set; }

    public Post? Post { get; set; }
    public int PostId { get; set; }
    public string Path { get; set; } = null!;
  
    

   
}
