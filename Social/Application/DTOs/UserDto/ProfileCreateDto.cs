﻿using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class ProfileCreateDto
{
   
    public IFormFile ImageFile { get; set; }

}
