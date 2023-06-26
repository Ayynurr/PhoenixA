﻿using Domain.Entities.Base;

namespace Domain.Entities;

public class Post : BaseAuditable
{
    

    public string Content { get; set; } = null!;
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
    public string? ImageName { get; set; }
    public ICollection<LikePost> Likes { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Comment> Comments { get; set; }
    //hardadi bu commmenntnntntnntnntntntnntntts

}
