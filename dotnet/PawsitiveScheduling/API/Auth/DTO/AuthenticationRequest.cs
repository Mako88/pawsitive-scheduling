﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth.DTO
{
    public class AuthenticationRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
