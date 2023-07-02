using Application.DTOs;
namespace Application.Abstracts;

public interface ISecurityService
{
    Task<List<UserAdminDto>> GetUsers();
    Task<bool> BlockUser(int userId, bool blockStatus, DateTime? blockEndDate);
    
}
