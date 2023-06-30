using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class UpdateProfileImage
{


    public IFormFile Image { get; set; }
    public bool IsBack { get; set; }
    public bool IsProfile { get; set; }
   

}
