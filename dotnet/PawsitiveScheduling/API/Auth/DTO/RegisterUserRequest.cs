using System.Collections.Generic;

namespace PawsitiveScheduling.API.Auth.DTO
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
}
