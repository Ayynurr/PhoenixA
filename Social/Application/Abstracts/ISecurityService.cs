using Application.DTOs;
namespace Application.Abstracts;

public interface ISecurityService
{
    Task<List<UserAdminDto>> GetUsers();
    Task<bool> BlockUser(int userId, bool blockStatus, DateTime? blockEndDate);
    Task<bool> DeletedUser(int userId, bool deletedStatus);
    Task<List<GetGroupDto>> GetGroups();
    Task<bool> DeleteGroup(int groupId);
    Task<bool> DeletePost(int postId);
    Task<List<PostGetDto>> GetPosts();

}
