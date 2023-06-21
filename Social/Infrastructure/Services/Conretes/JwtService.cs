using Domain.Entities;
using Infrastructure.Services.Interface;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.Conretes;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GetJwt(AppUser user, IList<string> roles)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                
            };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:securityKey").Value));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: _config.GetSection("Jwt:issuer").Value,
            audience: _config.GetSection("Jwt:audience").Value,
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(2),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }


