﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs
{
    public class TokenApiDto
    {
        public string AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; }
    }
}
