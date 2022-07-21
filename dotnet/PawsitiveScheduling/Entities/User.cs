using PawsitiveScheduling.Utility.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// The user entity
    /// </summary>
    [BsonCollectionName(Constants.UserCollectionName)]
    public class User : Entity
    {
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        public HashInfo Password { get; set; } = new();

        public List<string> Roles { get; set; } = new();

        public Dictionary<string, string> RefreshTokens { get; set; } = new();
    }

    public class HashInfo
    {
        public byte[] HashedText { get; set; }

        public byte[] Salt { get; set; }

        public int DegreeOfParallelism { get; set; }

        public int Iterations { get; set; }

        public int MemorySize { get; set; }
    }

    public static class UserRoles
    {
        public const string Customer = "Customer";
        public const string Groomer = "Groomer";
        public const string Receptionist = "Receptionist";
        public const string Admin = "Admin";
    }
}
