using Application.Abstracts;
using Application.DTOs;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.DataContext;

namespace Persistance.Concretes;

public class UserService : IUserService
{
    readonly AppDbContext _dbcontext;

    public UserService(AppDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task<UserUpdateDto> UdateAsync([FromRoute] int id,[FromBody] UserUpdateDto user)
    {
        AppUser? newUser = await _dbcontext.Users.FirstOrDefaultAsync(s => s.Id == id)
        ?? throw new NotfoundException();
        newUser.BirthDate = user.BirthDate;
        newUser.Bio = user.Bio;
        newUser.Address = user.Address;
        newUser.Gender = user.Gender;
        await _dbcontext.SaveChangesAsync();
        return new()
        {
            Address =newUser.Address,
            Gender =newUser.Gender,
            Bio=newUser.Bio,
            BirthDate = newUser.BirthDate
        };
    }

  
}
