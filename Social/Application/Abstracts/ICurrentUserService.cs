using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Application.Abstracts;

public interface ICurrentUserService
{
    public int? UserId { get; }

    public string? Username { get; }
    public string? Email { get; }
}
