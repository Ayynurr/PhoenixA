using Application.DTOs;
using System.Text.RegularExpressions;

namespace Application.Abstracts;

public interface IUserService
{
    Task PrfileCreate(ProfileCreateDto profileCreate);
    Task BackCreateAsync(ProfileCreateDto profilCreate);
    Task<GetProfileDto> ProfileUpdate(ProfileUpdateDto profileUpdate);
    Task<List<GetProfileImage>> UpdateImage(UpdateProfileImage updateImage);
    Task<UserGetDto> UserGetByUsername(string username);
    Task DeleteImage(int imageId);
    Task InviteUserToGroup(int groupId);
    Task<List<GetGroupDto>> GetUserGroups();
    Task<bool> IsUserInGroup(int groupId);
    Task RemoveUserFromGroup(int groupId);
    Task RespondToGroupInvitation( int groupId, bool acceptInvitation);
    Task AddProfileViewAsync(int profileOwnerId, int visitorId);
    Task<int> GetProfileViewCountAsync(int profileOwnerId);
}
