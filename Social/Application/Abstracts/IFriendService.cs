using Application.DTOs;

namespace Application;

public interface IFriendService 
{
    Task ConfirmFriendAsync(int id);
    Task AddFriendAsync(int id);
    Task DeleteFriendAsync(int id);
    Task FriendBlockAsync(int id);
    Task FriendUnBlockAsync(int id);
    Task DeclinedAsync(int id);
    Task<List<UserGetDto>> GetAllFriendsAsync();
    Task<List<UserGetDto>> GetRequestFriends();

}
