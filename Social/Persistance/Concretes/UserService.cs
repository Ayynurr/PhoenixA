using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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

    public Task DeleteImage(int imageId)
    {
        throw new NotImplementedException();
    }

    public async Task PrfileCreate(ProfileCreateDto profileCreate)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId)
        ?? throw new NotfoundException();
        UserImage image = new()
        {
            Id = (int)loginId,

            CreatedDate = DateTime.Now,
        };
        user = new()
        {
            Bio = profileCreate.Bio,
        };

        if (profileCreate.ImageFile.CheckFileSize(2048)) throw new FileSizeException();

        if (!profileCreate.ImageFile.CheckFileType("image/")) throw new FileTypeException();


        string newFileName = await profileCreate.ImageFile.FileUploadAsync(_hostEvnironment.WebRootPath, "Images");
        image.ProfileImageName = newFileName;


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
    ////Adminle qarisdirilib buneeee
}



