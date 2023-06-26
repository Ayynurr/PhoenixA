using Application.DTOs;
namespace Application.Abstracts;

public interface IUserService
{
    Task PrfileCreate(ProfileCreateDto profileCreate);
    Task<GetProfileDto> ProfileUpdate(ProfileUpdateDto profileUpdate);
    Task<List<GetProfileImage>> UpdateImage(UpdateProfileImage updateImage);
    Task<GetProfileDto> UserGet();
    Task<UserGetDto> UserGetByUsername(string username);
    Task DeleteImage(int imageId);


}
