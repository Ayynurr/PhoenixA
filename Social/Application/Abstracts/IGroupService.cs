using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Abstracts;

public interface IGroupService
{
    //Task CreateUserGroup(UserGroupDto groupDTO);
    //Task SendInvitation(int groupId);
    //Task AcceptInvitation(int groupId);
    //Task RejectInvitation(int groupId);
    //Task AddPost(AddPostDto dto);
    Task<GroupDto> CreateGroup(string groupName);
    Task AcceptUserInvitation(int userId, int groupId);
    Task RemoveUserFromGroup(int userId, int groupId);
    Task<bool> DeleteGroup(int groupId);
    Task<GroupPostDto> CreatePost(int groupId, string content, List<IFormFile> images);
    Task InviteUserToGroup(int userId, int groupId);
    Task<GroupPostDto> UpdateGroupPost(GroupPostUpdate updateDto);
    Task DeleteGroupPost(int groupId, int postId);
    Task<List<GetUserDto>> GetUserAcceptedGroups(int groupId);
}
