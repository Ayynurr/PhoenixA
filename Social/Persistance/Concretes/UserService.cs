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


    public async Task<List<GetGroupDto>> GetUserGroups()
    {
        var loginId = _currentUserService.UserId;

        var groups = await _dbcontext.GroupMemberships
            .Where(gm => gm.UserId == loginId && gm.Status == Status.Accepted)
            .Select(gm => gm.Group)
            .ToListAsync() ?? throw new NotfoundException() ;   

        var groupDtos = groups.Select(g => new GetGroupDto
        {
            Name = g.Name,
        }).ToList();

        return groupDtos;
    }

    public async Task InviteUserToGroup(int groupId)
    {
        var loginId = _currentUserService.UserId;
        bool membershipExists =  _dbcontext.GroupMemberships
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
            .FirstOrDefaultAsync(gm => gm.UserId == loginId && gm.GroupId == groupId && gm.Status == Status.Pending) ?? throw new NotfoundException();

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

    public async Task<List<GetProfileImage>> UpdateImage(UpdateProfileImage updateImage)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.Include(u => u.UserImages).FirstOrDefaultAsync(u => u.Id == loginId) ??
            throw new NotfoundException();
        user.UserImages ??= new List<UserImage>();
        List<GetProfileImage> updatedImages = new List<GetProfileImage>();

        if (updateImage.Image.CheckFileSize(2048))
            throw new FileTypeException();
        if (!updateImage.Image.CheckFileType("image/"))
            throw new FileSizeException();

        string newFileName = await updateImage.Image.FileUploadAsync(_hostEvnironment.WebRootPath, "UserImages");

        var newImage = new UserImage
        {
            UserId = (int)loginId,
            Path = newFileName
        };

        if (updateImage.IsProfile)
        {
            var existingProfileImage = user.UserImages.FirstOrDefault(image => image.IsProfile);
            if (existingProfileImage != null)
                existingProfileImage.IsProfile = false;

            newImage.IsProfile = true;
        }

        if (updateImage.IsBack)
        {
            var existingBackImage = user.UserImages.FirstOrDefault(image => image.IsBack);
            if (existingBackImage != null)
                existingBackImage.IsBack = false;

            newImage.IsBack = true;
        }

        user.UserImages.Add(newImage);

        await _dbcontext.SaveChangesAsync();

        updatedImages.Add(new GetProfileImage
        {
            Image = newImage.Path,
            UserId = (int)loginId,
            ImageUrl = $"https://localhost:7275/api/Post/Images/{newImage.Path}"
        });

        return updatedImages;
    }

    public async Task DeleteImage(int imageId)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.Include(u => u.UserImages).FirstOrDefaultAsync(u => u.Id == loginId) ??
            throw new NotfoundException();

        var imageToDelete = user.UserImages.FirstOrDefault(image => image.Id == imageId);
        if (imageToDelete == null)
            throw new NotfoundException("Image not found");

        user.UserImages.Remove(imageToDelete);

        
        if (imageToDelete.IsProfile)
        {
            var newProfileImage = user.UserImages.FirstOrDefault(image => image.IsBack);
            if (newProfileImage != null)
                newProfileImage.IsProfile = true;
        }

        if (imageToDelete.IsBack)
        {
            var newBackImage = user.UserImages.FirstOrDefault(image => image.IsProfile);
            if (newBackImage != null)
                newBackImage.IsBack = true;
        }

        await _dbcontext.SaveChangesAsync();
    }

    public async Task<UserGetDto> UserGetByUsername(string username)
    {
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(s => s.UserName == username) ??
           throw new NotfoundException();
        
        return new UserGetDto() { Name = user.Name,Address= user.Address,Surname = user.Surname,Bio = user.Bio,Gender = user.Gender};
    }

    public async Task AddProfileViewAsync(int profileOwnerId, int visitorId)
    {
        var view = new ProfileView
        {
            ProfileOwnerId = profileOwnerId,
            VisitorId = visitorId,
            VisitDate = DateTime.Now
        };

        _dbcontext.ProfileViews.Add(view);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<int> GetProfileViewCountAsync(int profileOwnerId)
    {
        var viewCount = await _dbcontext.ProfileViews
       .Where(view => view.ProfileOwnerId == profileOwnerId)
       .CountAsync();

        return viewCount;
    }

   
   
}



