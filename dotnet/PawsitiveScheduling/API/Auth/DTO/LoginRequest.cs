using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PawsitiveScheduling.API.Auth.DTO
{
    /// <summary>
    /// A login request
    /// </summary>
    public class LoginRequest
    {
        [Required]
        [FromBody]
        public string? Email { get; set; }

        [Required]
        [FromBody]
        public string? Password { get; set; }
    }
}
