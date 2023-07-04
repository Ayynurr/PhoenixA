using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.DTOs.AuthDto;

public class ForgetPasswordDto
{
    public string? Email { get; set; }
}
