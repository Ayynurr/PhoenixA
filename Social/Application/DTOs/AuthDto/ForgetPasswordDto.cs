using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.DTOs;

public class ForgetPasswordDto
{
    public string? Email { get; set; }
}
