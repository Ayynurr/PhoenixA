using Application.Abstracts;
using Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;

namespace Persistance.Concretes;

public class SecurityService : ISecurityService
{
    readonly AppDbContext _dbcontext;
    public SecurityService(AppDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task<bool> BlockUser(int userId, bool blockStatus, DateTime? blockEndDate)
    {
        AppUser? user = await _dbcontext.Users.FindAsync(userId);
        if (user == null)
        {
            return false;
        }

        user.IsBlock = blockStatus;
        user.BlockEndDate = blockEndDate;
        await _dbcontext.SaveChangesAsync();

        return true;
    }


    public async Task<List<UserAdminDto>> GetUsers()
    {
        List<UserAdminDto> userList = await _dbcontext.Users
            .Select(u => new UserAdminDto
            {
                UserId = u.Id,
                Username = u.UserName,
                IsBlocked = u.IsBlock,
                BlockEndDate = u.BlockEndDate
            })
            .ToListAsync();

        return userList;
    }

}
