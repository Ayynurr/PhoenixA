﻿using Application.DTOs.AuthDto;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Socail.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly UserManager<AppUser> _userManager;
    readonly IEmailService _emailService;
    readonly IJwtService _jwtService;
    public AuthController(UserManager<AppUser> userManager, IEmailService emailService, IJwtService jwtService)
    {
        _userManager = userManager;
        _emailService = emailService;
        _jwtService = jwtService;
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

        //IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "SuperAdmin");
        //if (!roleResult.Succeeded)
        //{
        //    var errors = roleResult.Errors.Select(error => error.Description).ToList();
        //    return BadRequest(new { errors });
        //}

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = Url.Action("ConfirmUser", "Auth", new { email = user.Email, token = token },HttpContext.Request.Scheme);
        _emailService.SendMessage(token,"Confirm", user.Email);
        return Ok(new
        {
            user.Name,
            user.Email
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var user = await _userManager.FindByNameAsync(login.Username);
        if (user == null)
        {
            return BadRequest(new { Message = "Username or password incorrect!!!" });
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, login.Password);
        if (!passwordValid)
        {
            return BadRequest(new { Message = "Username or password incorrect!!!" });
        }
        var roles =  _userManager.GetRolesAsync(user).Result;
        var jwt =  _jwtService.GetJwt(user,roles);
       

        return Ok(new { Token = jwt });
    }

  
    //[HttpPost("createRoles")]
    //public async Task CreateRoles()
    //{
    //    foreach (var item in Enum.GetValues(typeof(Roles)))
    //    {
    //        if (!(await _roleManager.RoleExistsAsync(item.ToString())))
    //        {
    //            await _roleManager.CreateAsync(new IdentityRole
    //            {
    //                Name = item.ToString()
    //            });
    //        }
    //    }
    //}
  

    [HttpPost("ConfirmUser")]
    public async Task<IActionResult> ConfirmUser(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return BadRequest("Invalid email. User not found.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            return BadRequest("Email confirmation failed. Invalid token or expired link.");
        }

        return Ok("Email confirmed successfully.");
    }


    [HttpPost("forgetPassword")]
    public async Task<IActionResult> ForgotPassword([FromForm] ForgetPasswordDto forgotPassword)
    {
        var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
        if (user is null)
        {
            return BadRequest("User not found.");
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string? link = Url.Action("ResetPassword", "Auth", new { UserId=user.Id, token = token }, HttpContext.Request.Scheme);
        string message = $"Please reset your password by clicking the following link: {link}";
        string subject = "Password Reset";
        _emailService.SendMessage(message, subject, user.Email);
        return Ok(link);
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto resetPasswordDto,string UserId,string token)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user is null)
        {
            return NotFound();
        }
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, resetPasswordDto.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest();
        }

        return Ok();
    }


}
