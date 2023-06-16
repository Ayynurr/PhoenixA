using Application.Abstracts;
using Microsoft.AspNetCore.Http;
using Persistance.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Concretes;


public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _claims;

    public CurrentUserService(IHttpContextAccessor claims)
    {
        _claims = claims;
    }
    public int? UserId => _claims.HttpContext?.User.GetLoginUserId();

    public string? Username => _claims.HttpContext?.User.GetLoginUserName();

    public string? Email => _claims.HttpContext?.User.GetLoginUserEmail();

}
