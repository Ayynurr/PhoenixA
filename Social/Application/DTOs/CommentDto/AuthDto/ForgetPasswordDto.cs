using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.DTOs.CommentDto.AuthDto;

public class ForgetPasswordDto
{
    public string? Email { get; set; }
}
