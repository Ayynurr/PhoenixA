﻿using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Socail.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly UserManager<AppUser> _userManager;
    readonly IConfiguration _configuration;
    public AuthController(UserManager<AppUser> userManager, IConfiguration confifuration)
    {
        _userManager = userManager;
        _configuration = confifuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto register)
    {
        AppUser user = new()
        {
            Name = register.Firstname,
            Surname = register.Lastname,
            Email = register.Email,
            UserName = register.Username,
            Gender = register.Gender,
            Address = register.Address,
            BirthDate = register.Birthday
        };
        IdentityResult result = await _userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok(new
        {
            user.Name,
            user.Email
        });

    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        AppUser user = await _userManager.FindByNameAsync(login.Username);
        if (user is null)
        {
            return BadRequest(new
            {
                Message = "Username or password incorrect!!!"
            });
        }
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email)
        };
        string privateKey = _configuration["JWT:SecurityKey"];
        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        //token generate
        JwtSecurityToken token = new JwtSecurityToken
            (
            issuer: _configuration["JWT:audience"],
            audience: _configuration["JWT:issuer"],
            claims: claims,
            expires: DateTime.Now.AddDays(100),
            signingCredentials: signingCredentials

            );
        return Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        });

    }

}
