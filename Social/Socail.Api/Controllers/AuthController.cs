using Application.Abstracts;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Socail.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    readonly UserManager<AppUser> _userManager;
    readonly IConfiguration _configuration;
    readonly RoleManager<Role> _roleManager;
    readonly IEmailService _emailService;
    public AuthController(UserManager<AppUser> userManager, IConfiguration confifuration, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _configuration = confifuration;
        _roleManager = roleManager;
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
      
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = Url.Action("ConfirmUser", "Account", new { token = token, email = user.Email });
        _emailService.SendMessage($"<a href=\"{link}\"> click for email confirmation</a>", "Confirmation link", register.Email);
        //await _userManager.AddToRoleAsync(user, Roles.User.ToString());
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
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmUser(string email, string token)
    {
        AppUser user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new Exception("user not found");


        IdentityResult identityResult = await _userManager.ConfirmEmailAsync(user, token);
        if (!identityResult.Succeeded)
        {
            ModelState.AddModelError("", "Token confirm incorrect");
            return BadRequest(identityResult.Errors);
        }

        return Ok();
    }

}
