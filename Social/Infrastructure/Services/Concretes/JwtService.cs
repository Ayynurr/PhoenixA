using Domain.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configure;
    public JwtService(IConfiguration configure)
    {
        _configure = configure;
    }

    public string GetJwt(AppUser user, IList<string> roles)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                
            };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configure.GetSection("Jwt:securityKey").Value));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken securityToken = new JwtSecurityToken(
            issuer: _configure.GetSection("Jwt:issuer").Value,
            audience: _configure.GetSection("Jwt:audience").Value,
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(2),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}






