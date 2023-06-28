using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Persistance.Extentions;
using Persistance.DataContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class GroupService : IGroupService
{
    readonly AppDbContext _dbcontext;
    readonly ICurrentUserService _currentUserService;
    readonly IWebHostEnvironment _hostEnvironment;

    public GroupService(AppDbContext dbcontext, ICurrentUserService currentUserService, IWebHostEnvironment hostEnvironment)
    {
        _dbcontext = dbcontext;
        _currentUserService = currentUserService;
        _hostEnvironment = hostEnvironment;
    }

    public async Task AcceptUserInvitation(int userId, int groupId)
    {
        GroupMembership? membership = await _dbcontext.GroupMemberships
            .FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId == groupId && gm.Status == Status.Pending)
            ?? throw new NotfoundException();

        if (membership != null)
        {
            membership.Status = Status.Accepted;
            await _dbcontext.SaveChangesAsync();
        }
    }

    public async Task<GroupDto> CreateGroup(string groupName)
    {
        Group group = new Group
        {
            Name = groupName
        };


        GroupDto groupDto = new()
        {
            Id = group.Id,
            Name = group.Name
        };

        _dbcontext.Groups.Add(group);
        await _dbcontext.SaveChangesAsync();
        return groupDto;
    }
    public async Task InviteUserToGroup(int userId, int groupId)
    {
        Group group = await _dbcontext.Groups.FindAsync(groupId) ?? throw new NotfoundException();
        AppUser user = await _dbcontext.Users.FindAsync(userId) ?? throw new NotfoundException();

        if (group != null && user != null)
        {
            // Check if the user is already a member of the group with accepted status
            bool isAcceptedMember = await _dbcontext.GroupMemberships
                .AnyAsync(gm => gm.UserId == userId && gm.GroupId == groupId && gm.Status == Status.Accepted);

            if (!isAcceptedMember)
            {
                GroupMembership membership = new GroupMembership
                {
                    UserId = userId,
                    GroupId = groupId,
                    Status = Status.Pending
                };

                _dbcontext.GroupMemberships.Add(membership);
                await _dbcontext.SaveChangesAsync();
            }
        }
    }

    public async Task RemoveUserFromGroup(int userId, int groupId)
    {
        GroupMembership? membership = await _dbcontext.GroupMemberships
            .FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId == groupId);

        if (membership != null)
        {
            _dbcontext.GroupMemberships.Remove(membership);
            await _dbcontext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("The user is not in the group");
        }
    }
    public async Task<List<GetUserDto>> GetUserAcceptedGroups(int groupId)
    {
        var group = await _dbcontext.Groups.FirstOrDefaultAsync(i => i.Id == groupId) ?? throw new NotfoundException();

        var memberships = await _dbcontext.GroupMemberships
            .Where(gm => gm.GroupId == groupId && gm.Status == Status.Accepted)
            .Include(gm => gm.User)
            .ToListAsync();

        var users = memberships.Select(gm => new GetUserDto
        {
            UserId = gm.UserId,
            UserName = gm.User.UserName
        }).ToList();

        return users;
    }

    public async Task<bool> DeleteGroup(int groupId)
    {
        Group? group = await _dbcontext.Groups.FindAsync(groupId);

        if (group != null)
        {
            _dbcontext.Groups.Remove(group);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<GroupPostDto> CreatePost(int groupId, string content, List<IFormFile> images)
    {
        var loginId = _currentUserService.UserId;
        Group? group = await _dbcontext.Groups.Include(u => u.Posts).FirstOrDefaultAsync(i => i.Id == groupId) ?? throw new ArgumentException("Invalid groupId");
        Post post = new Post
        {
            GroupId = groupId,
            UserId = (int)loginId,
            Content = content,
            CreatedDate = DateTime.Now
        };

        GroupPostDto postDto = new()
        {
            PostId = post.Id,
            GroupId = post.GroupId,
            UserId = post.UserId,
            Content = post.Content,
            CreatedAt = post.CreatedDate
        };
        if (images != null)
        {
            foreach (var file in images)
            {
                if (file.CheckFileSize(2048))
                    throw new FileTypeException();
                if (!file.CheckFileType("image/"))
                    throw new FileSizeException();
                string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "GroupPostImages");
                post.ImageName = newFileName;
            }
        }

        _dbcontext.Posts.Add(post);
        await _dbcontext.SaveChangesAsync();
        return postDto;
    }
    public async Task<GroupPostDto> UpdateGroupPost(GroupPostUpdate updateDto)
    {
        Post? post = await _dbcontext.Posts.FindAsync(updateDto.PostId) ?? throw new NotfoundException();
        if (updateDto.Images != null)
        {
            foreach (var file in updateDto.Images)
            {
                if (file.CheckFileSize(2048))
                    throw new FileTypeException();
                if (!file.CheckFileType("image/"))
                    throw new FileSizeException();
                string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "GroupPostImages");
                post.ImageName = newFileName;
            }
        }
        post.Content = updateDto.Content;
        await _dbcontext.SaveChangesAsync();

        GroupPostDto postDto = new ()
        {
            PostId = post.Id,
            GroupId = post.GroupId,
            UserId = post.UserId,
            Content = post.Content,
            CreatedAt = post.CreatedDate,
        };

        return postDto;
    }
    public async Task DeleteGroupPost(int groupId, int postId)
    {
        Group? group = await _dbcontext.Groups.Include(c=>c.Posts).FirstOrDefaultAsync(i=>i.Id==groupId) ?? throw new NotfoundException();
        Post? post = group.Posts.FirstOrDefault(p => p.Id == postId) ?? throw new NotfoundException();
        string path = Path.Combine(_hostEnvironment.WebRootPath, "GroupPostImages");
        if (path != null)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        _dbcontext.Posts.Remove(post);
        await _dbcontext.SaveChangesAsync();

       
    }

}
#region FirstWrite
//    public async Task AcceptInvitation(int groupId)
//    {
//        var loginId = _currentUserService.UserId;
//        AppUser? user = await _dbcontext.Users.Include(c => c.Groups)
//                                             .FirstOrDefaultAsync(i => i.Id == loginId)
//                                             ?? throw new NotfoundException();

//        Group? group = await _dbcontext.Groups.FindAsync(groupId);

//        if (group != null)
//        {
//            if (!group.Members.Any(m => m.Id == loginId))
//            {
//                group.Members.Add(await _dbcontext.Users.FindAsync(loginId));
//            }

//            if (!group.Status.Equals(InvitationStatus.Accepted))
//            {
//                group.Status = InvitationStatus.Accepted; // Grup durumunu Accepted olarak güncelle
//            }

//            await _dbcontext.SaveChangesAsync();
//        }
//        else
//        {
//            throw new NotfoundException();
//        }
//    }

//    public async Task AddPost(AddPostDto dto)
//    {
//        var loginId = _currentUserService.UserId;
//        Group? group = await _dbcontext.Groups.Include(g => g.Posts)
//                                            .FirstOrDefaultAsync(g => g.Id == dto.GroupId);

//        if (group != null && group.Members.Any(m => m.Id == loginId) && group.Status == InvitationStatus.Accepted)
//        {
//            Post post = new Post
//            {
//                GroupId = dto.GroupId,
//                UserId = (int)loginId,
//                Content = dto.Content,
//                CreatedDate = DateTime.UtcNow
//            };

//            if (dto.Images != null)
//            {
//                foreach (var file in dto.Images)
//                {
//                    if (file.CheckFileSize(2048))
//                        throw new FileTypeException();
//                    if (!file.CheckFileType("image/"))
//                        throw new FileSizeException();
//                    string newFileName = await file.FileUploadAsync(_hostEnvironment.WebRootPath, "GroupPostImages");
//                    post.ImageName = newFileName;
//                }
//            }

//            group.Posts.Add(post);
//            await _dbcontext.SaveChangesAsync();

//        }
//    }

//    public async Task CreateUserGroup(UserGroupDto groupDTO)
//    {
//        var loginId = _currentUserService.UserId;
//        AppUser? user = await _dbcontext.Users.Include(c => c.Groups).FirstOrDefaultAsync(i => i.Id == loginId)
//                                                                                                      ?? throw new NotfoundException();
//        Group group = new Group
//        {
//            Name = groupDTO.Name,
//            Members = new List<AppUser>()
//        };

//        foreach (int memberId in groupDTO.MemberIds)
//        {
//            user = await _dbcontext.Users.FindAsync(memberId);
//            if (user != null)
//            {
//                group.Members.Add(user);
//            }
//        }

//        _dbcontext.Groups.Add(group);
//        await _dbcontext.SaveChangesAsync();
//    }

//    public async Task RejectInvitation(int groupId)
//    {
//        var loginId = _currentUserService.UserId;
//        AppUser? user = await _dbcontext.Users.Include(c => c.Groups).FirstOrDefaultAsync(i => i.Id == loginId)
//                                                                                                      ?? throw new NotfoundException();

//        Group? group = await _dbcontext.Groups.FindAsync(groupId);

//        if (group != null)
//        {
//            if (!group.Members.Any(m => m.Id == loginId) && !group.Status.Equals(InvitationStatus.Rejected))
//            {
//                group.Members.Add(await _dbcontext.Users.FindAsync(loginId));
//                await _dbcontext.SaveChangesAsync();
//            }
//        }
//    }

//    public async Task SendInvitation(int groupId)
//    {
//        var loginId = _currentUserService.UserId;
//        AppUser? user = await _dbcontext.Users.Include(c => c.Groups).FirstOrDefaultAsync(i => i.Id == loginId)
//                                                                                                      ?? throw new NotfoundException();

//        Group? group = await _dbcontext.Groups.FindAsync(groupId);

//        if (group != null)
//        {
//            if (!group.Members.Any(m => m.Id == loginId) && !group.Status.Equals(InvitationStatus.Pending))
//            {
//                group.Members.Add(await _dbcontext.Users.FindAsync(loginId));
//                await _dbcontext.SaveChangesAsync();
//            }
//        }
//    }
//}
#endregion