using Domain.Entities;

namespace Infrastructure.Services.Interface;

public interface IJwtService 
{
    public string GetJwt(AppUser user, IList<string> roles);
}
