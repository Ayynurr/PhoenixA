using Application;
using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;

namespace Persistance;

public class FriendService : IFriendService
{
    private readonly AppDbContext _dbcontext;
    public readonly ICurrentUserService _currentUserService;
    public FriendService(AppDbContext dbcontext, ICurrentUserService userService)
    {
        _dbcontext = dbcontext;
        _currentUserService = userService;
    }
    public async Task AddFriendAsync(int id)
    {
        var userLoginId = _currentUserService.UserId;
        if (userLoginId == id)
        {
            throw new Exception("You cannot send friend request to yourself.");
        }

        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new NotfoundException("User is not found");
        var userfriends = await _dbcontext.UserFriends.FirstOrDefaultAsync(f => f.UserId == user.Id && f.FriendId == userLoginId);
        if (userfriends != null)
        {
            throw new Exception("Friendly or blocked user");
        }
        UserFriend userFriend = new UserFriend
        {
            UserId = id,
            FriendId = (int)userLoginId,
            Status = FriendStatus.Pending
        };
        await _dbcontext.AddAsync(userFriend);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task ConfirmFriendAsync(int id)
    {
        var userLoginId = _currentUserService.UserId;
        UserFriend? friend = await _dbcontext.UserFriends.FirstOrDefaultAsync(u => u.UserId == userLoginId && u.FriendId == id);
        if (friend is null)
        {
            throw new NotfoundException("User is not found");
        }
        friend.Status = FriendStatus.Accepted;

        await _dbcontext.SaveChangesAsync();
    }

    public async Task DeclinedAsync(int id)
    {
        var userLoginId = _currentUserService.UserId;
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            throw new NotfoundException("User is not found");
        }
        var userfriends = await _dbcontext.UserFriends.FirstOrDefaultAsync(f => f.UserId == userLoginId && f.FriendId == id);
        if (userfriends is null)
        {
            throw new Exception("Friend is not found");
        }
        userfriends.Status = FriendStatus.Declined;
        _dbcontext.UserFriends.Update(userfriends);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task DeleteFriendAsync(int id)
    {
        var userLoginId = _currentUserService.UserId;
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            throw new NotfoundException("User is not found");
        }
        var userfriends = await _dbcontext.UserFriends.FirstOrDefaultAsync(f => (f.UserId == userLoginId && f.FriendId == id) ||
                                                                           (f.UserId == id && f.FriendId == userLoginId));
        if (userfriends is null)
        {
            throw new NotfoundException("Friend is not found");
        }
        _dbcontext.UserFriends.Remove(userfriends);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task FriendBlockAsync(int id)
    {
        var userLoginId = _currentUserService.UserId;
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            throw new NotfoundException("User is not found");
        }
        var userfriends = await _dbcontext.UserFriends.FirstOrDefaultAsync(f => f.UserId == userLoginId && f.FriendId == id);
        if (userfriends is null)
        {
            throw new Exception("Friend is not found");
        }
        userfriends.Status = FriendStatus.Blocked;
        _dbcontext.UserFriends.Update(userfriends);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task FriendUnBlockAsync(int id)
    {
        var UserId = _currentUserService.UserId;
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            throw new NotfoundException("User is not found");
        }
        var userfriends = await _dbcontext.UserFriends.FirstOrDefaultAsync(f => f.UserId == user.Id && f.FriendId == UserId);
        if (userfriends is null)
        {
            throw new Exception("Friend is not found");
        }
        userfriends.Status = FriendStatus.Declined;
        _dbcontext.UserFriends.Update(userfriends);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<List<UserGetDto>> GetAllFriendsAsync()
    {
        var userLoginId = _currentUserService.UserId;
        var friends = await _dbcontext.UserFriends
            .Where(u => u.UserId == userLoginId && u.Status == FriendStatus.Accepted)
            .Include(u => u.Friend)
            .Select(u => new UserGetDto
            {
                Name = u.Friend.Name,
                Surname = u.Friend.Surname,
                BirthDate = u.Friend.BirthDate,
                Bio = u.Friend.Bio,
                Gender = u.Friend.Gender,
                Address = u.Friend.Address
            })
            .ToListAsync();

        return friends;
    }

    public async Task<List<UserGetDto>> GetRequestFriends()
    {
        var userLoginId = _currentUserService.UserId;

        var friends = await _dbcontext.UserFriends
            .Where(f => f.UserId == userLoginId && f.Status == FriendStatus.Pending)
            .Join(_dbcontext.Users,
                friend => friend.FriendId,
                user => user.Id,
                (friend, user) => new UserGetDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthDate = user.BirthDate,
                    Bio = user.Bio,
                    Gender = user.Gender,
                    Address = user.Address
                })
            .ToListAsync();

        return friends;
    }
}
