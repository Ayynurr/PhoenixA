using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;
using Persistance.Extentions;

namespace Persistance.Concretes;

public class UserService : IUserService
{
    readonly AppDbContext _dbcontext;
    readonly ICurrentUserService _currentUserService;
    readonly IWebHostEnvironment _hostEvnironment;

    public UserService(AppDbContext dbcontext, ICurrentUserService currentUserService, IWebHostEnvironment hostEvnironment)
    {
        _dbcontext = dbcontext;
        _currentUserService = currentUserService;
        _hostEvnironment = hostEvnironment;
    }

    public async Task BackCreateAsync(ProfileCreateDto profilCreate)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.Include(c=>c.UserImages).FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException();
        UserImage image = new()
        {
            Id = (int)loginId,
            IsBack = true,
            CreatedDate = DateTime.Now,
        };

        
        if (profilCreate.ImageFile.CheckFileSize(2048)) throw new FileSizeException();

        if (!profilCreate.ImageFile.CheckFileType("image/")) throw new FileTypeException();


        string newFileName = await profilCreate.ImageFile.FileUploadAsync(_hostEvnironment.WebRootPath, "UserImages");
        image.Path = newFileName;

        user.UserImages.Add(image);
        await _dbcontext.SaveChangesAsync();
    }

    public Task DeleteImage(int imageId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetGroupDto>> GetUserGroups()
    {
        var loginId = _currentUserService.UserId;

        var groups = await _dbcontext.GroupMemberships
            .Where(gm => gm.UserId == loginId && gm.Status == Status.Accepted)
            .Select(gm => gm.Group)
            .ToListAsync();

        var groupDtos = groups.Select(g => new GetGroupDto
        {
            Name = g.Name,
        }).ToList();

        return groupDtos;
    }

    public async Task InviteUserToGroup(int groupId)
    {
        var loginId = _currentUserService.UserId;
        bool membershipExists = _dbcontext.GroupMemberships
            .Any(gm => gm.UserId == loginId && gm.GroupId == groupId);

        if (!membershipExists)
        {
            GroupMembership membership = new ()
            {
                UserId = (int)loginId,
                GroupId = groupId,
                Status = Status.Pending
            };

            _dbcontext.GroupMemberships.Add(membership);
            _dbcontext.SaveChanges();
        }
    }
    public async Task RemoveUserFromGroup( int groupId)
    {
        var loginId = _currentUserService.UserId;
        GroupMembership? membership = await _dbcontext.GroupMemberships
            .FirstOrDefaultAsync(gm => gm.UserId == loginId && gm.GroupId == groupId);

        if (membership != null)
        {
            _dbcontext.GroupMemberships.Remove(membership);
            await _dbcontext.SaveChangesAsync();
        }
    }
    public async Task RespondToGroupInvitation( int groupId, bool acceptInvitation)
    {
        var loginId = _currentUserService.UserId;
        GroupMembership? membership = await _dbcontext.GroupMemberships
            .FirstOrDefaultAsync(gm => gm.UserId == loginId && gm.GroupId == groupId && gm.Status == Status.Pending);

        if (membership != null)
        {
            if (acceptInvitation)
            {
                membership.Status = Status.Accepted;
            }
            else
            {
                membership.Status = Status.Rejected;
            }

           await _dbcontext.SaveChangesAsync();
        }
    }

    public async Task<bool> IsUserInGroup(int groupId)
    {
        var loginId = _currentUserService.UserId;

        await _dbcontext.SaveChangesAsync();

        return _dbcontext.GroupMemberships
            .Any(gm => gm.UserId == loginId && gm.GroupId == groupId && gm.Status == Status.Accepted);
    }

    public async Task PrfileCreate(ProfileCreateDto profileCreate)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException();
        UserImage image = new()
        {
            Id = (int)loginId,
            IsProfile = true,
            CreatedDate = DateTime.Now,
        };
       

        if (profileCreate.ImageFile.CheckFileSize(2048)) throw new FileSizeException();

        if (!profileCreate.ImageFile.CheckFileType("image/")) throw new FileTypeException();


        string newFileName = await profileCreate.ImageFile.FileUploadAsync(_hostEvnironment.WebRootPath, "UserImages");
        image.Path = newFileName;


    }

    public async Task<GetProfileDto> ProfileUpdate(ProfileUpdateDto profileUpdate)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException();
        user.UserName = profileUpdate.Username;
        user.Bio = profileUpdate.Bio;
        user.Email = profileUpdate.Email;
        user.Gender = profileUpdate.Gender;
        user.Address = profileUpdate.Address;
        await _dbcontext.SaveChangesAsync();
        return new()
        {
            Username = user.UserName,
            Address = user.Address,
            Gender = user.Gender,
            Bio = user.Bio,
            Email = user.Email,
        };
    }

    public Task<List<GetProfileImage>> UpdateImage(UpdateProfileImage updateImage)
    {
        throw new NotImplementedException();
    }




    //public async Task<List<GetProfileImage>> UpdateImage(UpdateProfileImage updateImage)
    //{
    //    var loginId = _currentUserService.UserId;
    //    AppUser? user = await _dbcontext.Users.Include(u=>u.UserImages).FirstOrDefaultAsync(u => u.Id == loginId) ??
    //      throw new NotfoundException();
    //    user.Images ??= new List<Image>();
    //    List<GetProfileImage> updateImages = new();


    //    if (updateImage.ProfileImage.CheckFileSize(2048))
    //        throw new FileTypeException();
    //    if (!updateImage.ProfileImage.CheckFileType("image/"))
    //        throw new FileSizeException();
    //    if (updateImage.BackImage.CheckFileSize(2048))
    //        throw new FileTypeException();
    //    if (!updateImage.BackImage.CheckFileType("image/"))
    //        throw new FileSizeException();
    //    string newFileNameProfile = await updateImage.ProfileImage.FileUploadAsync(_hostEvnironment.WebRootPath, "UserImages");
    //    string newFileNameBackraound = await updateImage.BackImage.FileUploadAsync(_hostEvnironment.WebRootPath, "UserImages");

    //    UserImage newImage = new()
    //    {
    //        ProfileImageName = newFileNameProfile,
    //        BackraundImageName = newFileNameBackraound,
    //        UserId = (int)loginId,
    //        PathProfile = Path.Combine(_hostEvnironment.WebRootPath, "UserImages"),
    //        PathBack = Path.Combine(_hostEvnironment.WebRootPath, "UserImages"),
    //        UpdatedDate = DateTime.Now
    //    };
    //    user.UserImages.Add(newImage);
    //    updateImages.Add(new GetProfileImage
    //    {
    //        ProfileImage = newImage.ProfileImageName,
    //        BackraoundImage = newImage.BackraundImageName,
    //        UserId = (int)loginId,
    //        UrlProfile = $"https://localhost:7275/api/Post/Images/{user.UserImages}",
    //        UrlBackraound = $"https://localhost:7275/api/Post/Images/{user.UserImages}"
    //    });
    //    await _dbcontext.SaveChangesAsync();
    //    return updateImages;


    //}
    //public async Task DeleteImage(int imageId)
    //{
    //    var loginId = _currentUserService.UserId;
    //    AppUser? user = await _dbcontext.Users.Include(u => u.UserImages).FirstOrDefaultAsync(u => u.Id == loginId) ??
    //        throw new NotfoundException();

    //    UserImage image = user.UserImages.FirstOrDefault(img => img.Id == imageId);
    //    if (image != null)
    //    {
    //        string profileImagePath = Path.Combine(image.PathProfile, image.ProfileImageName);
    //        string backImagePath = Path.Combine(image.PathBack, image.BackraundImageName);

    //        if (File.Exists(profileImagePath))
    //        {
    //            File.Delete(profileImagePath);
    //        }
    //        if (File.Exists(backImagePath))
    //        {
    //            File.Delete(backImagePath);
    //        }

    //        user.UserImages.Remove(image);

    //        await _dbcontext.SaveChangesAsync();
    //    }
    //}


    public Task<GetProfileDto> UserGet()
    {
        throw new NotImplementedException();
    }

    //public async Task<GetProfileDto> UserGet()
    //{
    //    List<AppUser>? user = await _dbcontext.Users.ToListAsync() ?? throw new NotfoundException();



    //    return user;
    //}

    public async Task<UserGetDto> UserGetByUsername(string username)
    {
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(s => s.UserName == username) ??
           throw new NotfoundException();

        return new UserGetDto() { Name = user.Name,Address= user.Address,Surname = user.Surname,Bio = user.Bio,Gender = user.Gender};
    }

    public Task RespondToGroupInvitation(int userId, int groupId, bool acceptInvitation)
    {
        throw new NotImplementedException();
    }
}



