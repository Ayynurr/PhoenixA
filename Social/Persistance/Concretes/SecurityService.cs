using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
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
            .Where(u=>!u.IsDeleted)
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
    public async Task<bool> DeletedUser(int userId,bool deletedStatus)
    {
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(i => i.Id == userId);
        if (user is null) return false;
        user.IsDeleted = deletedStatus;
        await _dbcontext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteGroup(int groupId)
    {
        Group? group = await _dbcontext.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
        if (group is null) return false;

        group.IsDeleted = true;
        await _dbcontext.SaveChangesAsync();
        return true;
    }
    public async Task<List<GetGroupDto>> GetGroups()
    {
        List<GetGroupDto> groupList = await _dbcontext.Groups.Include(g => g.GroupMemberships)
            .Where(g => !g.IsDeleted)
            .Select(g => new GetGroupDto
            {
                Name = g.Name,
                GroupMembers = g.GroupMemberships.Select(m => new AppUserDto
                { 
                    Name = m.User.UserName,
                    Email = m.User.Email,
                    Birthdate=(DateTime)m.User.BirthDate
                }).ToList()
            })
            .ToListAsync();

        return groupList;
    }
    public async Task<bool> DeletePost(int postId)
    {
        Post? post = await _dbcontext.Posts.FirstOrDefaultAsync(g => g.Id == postId);
        if (post is null) return false;

        post.IsDeleted = true;
        await _dbcontext.SaveChangesAsync();
        return true;
    }
    public async Task<List<PostGetDto>> GetPosts()
    {
        var posts = await _dbcontext.Posts.Include(i => i.Images)
            .Select(s => new PostGetDto { Id = s.Id, Content = s.Content })
            .ToListAsync();

        foreach (var post in posts)
        {
            var images = await _dbcontext.Images
                .Where(i => i.PostId == post.Id)
                .Select(i => new ImageGetDto
                {
                    Url = $"https://localhost:7046/api/Hotel/Images/{i.ImgName}"
                })
                .ToListAsync();

            post.Images.AddRange(images);
        }

        return posts;
    }

}
