using Application.Abstracts;
using Application.DTOs;
using Application.DTOs.ImagePostDto;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

    public Task<GetProfileDto> GetAll(GetProfileDto getAll)
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



    public async Task<List<GetProfileImage>> UpdateImage(UpdateProfileImage updateImage)
    {
        var loginId = _currentUserService.UserId;
        AppUser? user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Id == loginId) ??
          throw new NotfoundException();
        user.Images ??= new List<Image>();
        List<GetProfileImage> updateImages = new();


        if (updateImage.Images.CheckFileSize(2048))
            throw new FileTypeException();
        if (!updateImage.Images.CheckFileType("image/"))
            throw new FileSizeException();
        string newFileName = await updateImage.Images.FileUploadAsync(_hostEvnironment.WebRootPath, "Images");

        UserImage newImage = new()
        {
            ProfileImageName = newFileName,
            BackraundImageName = newFileName,
            UserId = (int)loginId,
            Path = Path.Combine(_hostEvnironment.WebRootPath, "UserImages"),
            UpdatedDate = DateTime.Now
        };
        user.UserImages.Add(newImage);
        updateImages.Add(new GetProfileImage
        {
            ProfileImage = newImage.ProfileImageName,
            BackraoundImage = newImage.BackraundImageName,
            UserId = (int)loginId,
            UrlProfile = $"https://localhost:7275/api/Post/Images/{user.UserImages}",
            UrlBackraound = $"https://localhost:7275/api/Post/Images/{user.UserImages}"
        });
        await _dbcontext.SaveChangesAsync();
        return updateImages;


    }
}



