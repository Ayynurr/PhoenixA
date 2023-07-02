using Domain.Entities;

namespace Infrastructure.Services;

public interface IJwtService 
{
    public string GetJwt(AppUser user, IList<string> roles);
}
