using System.Collections.Generic;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// The user entity
    /// </summary>
    public class User : Entity
    {
        public override string CollectionName => Constants.UserCollectionName;

        public string Username { get; set; }

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
