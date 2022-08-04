using PawsitiveScheduling.Utility.Attributes;
using System.Collections.Generic;

namespace PawsitiveScheduling.Entities.Users
{
    /// <summary>
    /// The user entity
    /// </summary>
    [BsonCollectionName(Constants.UserCollectionName)]
    public class User : Entity
    {
        public string Email { get; set; } = null!;

        public HashInfo Password { get; set; } = new();

        public string Role { get; set; } = null!;

        public Dictionary<string, string> RefreshTokens { get; set; } = new();
    }

    public class HashInfo
    {
        public byte[] HashedText { get; set; } = null!;

        public byte[] Salt { get; set; } = null!;

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
