using Application.DTOs;

namespace Application.Abstracts;

public interface IUserService
{
    Task<UserUpdateDto> UdateAsync(int id,UserUpdateDto user);
}
