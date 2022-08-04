using System.ComponentModel.DataAnnotations;

namespace PawsitiveScheduling.API.Auth.DTO
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}
