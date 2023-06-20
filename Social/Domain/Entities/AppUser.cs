﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;

namespace Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime BirthDate { get; set; } //backround service 
    public string? Bio { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; }
    public bool IsBlock { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Story> Stories { get; set; }
    public ICollection<UserImage> UserImages { get; set; }
}