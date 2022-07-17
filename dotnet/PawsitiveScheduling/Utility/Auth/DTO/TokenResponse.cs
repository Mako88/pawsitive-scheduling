using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Auth.DTO
{
    public class TokenResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
