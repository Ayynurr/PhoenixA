﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs;

public class AppUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime Birthdate { get; set; }
}
