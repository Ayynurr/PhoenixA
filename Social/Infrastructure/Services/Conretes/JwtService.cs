using Domain.Entities;
using Infrastructure.Services.Interface;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services.Conretes;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configration;
    public JwtService(IConfiguration config)
    {
        _configration = config;
    }

    public string GetJwt(AppUser user, IList<string> roles)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email)
        };
        string privateKey = _configration["JWT:SecurityKey"];
        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
        //token generate
        JwtSecurityToken token = new JwtSecurityToken
        (
            issuer: _configration["JWT:audience"],
            audience: _configration["JWT:issuer"],
            claims: claims,
            expires: DateTime.Now.AddDays(100),
            signingCredentials: signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}





