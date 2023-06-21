using Application.DTOs;

namespace Application.Abstracts;

public interface IUserService
{
    Task<UserUpdateDto> UdateAsync(int id,UserUpdateDto user);
    Task PrfileCreate(ProfileCreateDto profileCreate);
    Task<GetProfileDto> ProfileUpdate(ProfileUpdateDto profileUpdate);
    Task<GetProfileDto> GetAll(GetProfileDto getAll);

}
