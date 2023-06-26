using Application.DTOs.CommentDto.AuthDto;
using Domain.Entities;
using Infrastructure.Services.Interface;
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
        //email a
        //
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

        // JWT oluşturma işlemine geç

        // JWT ayarları ve imzalama bilgileri
        var jwtSettings = _configuration.GetSection("JWT");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecurityKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // JWT taleplerini oluştur
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("sub",user.Id.ToString())
        // İhtiyaç duyduğunuz diğer talepleri ekleyin
    };

        // JWT oluşturma
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7), // Tokenın süresi
            signingCredentials: credentials
        );

        // JWT tokenı oluştur
        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedToken = tokenHandler.WriteToken(token);

        // Cevap olarak tokenı döndür
        return Ok(new { Token = encodedToken });
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
   

    [HttpPost("forgetPassword")]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordDto forgotPassword)
    {
        var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
        if (user is null)
            return BadRequest("User not found.");

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        string link = Url.Action("ResetPassword", "Account", new { userId = user.Id, token });
        return Ok(link);
    }
    

    [HttpPost("resetp")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword, string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return BadRequest("User ID and token are required.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            throw new Exception("Not Found");

        var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.ConfirmPassword);
        if (result.Succeeded)
            return RedirectToAction(nameof(Login));

        return BadRequest("Failed to reset password.");
    }

}
