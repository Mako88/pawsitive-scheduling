using System.ComponentModel.DataAnnotations;

namespace PawsitiveScheduling.API.Auth.DTO
{
    /// <summary>
    /// A login request
    /// </summary>
    public class LoginRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
