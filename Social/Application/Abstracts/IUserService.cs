using Application.DTOs;

namespace Application.Abstracts;

public interface IUserService
{
    Task PrfileCreate(ProfileCreateDto profileCreate);
    Task<GetProfileDto> ProfileUpdate(ProfileUpdateDto profileUpdate);
    Task<GetProfileDto> GetAll(GetProfileDto getAll);
    Task<List<GetProfileImage>> UpdateImage(UpdateProfileImage updateImage);

}
