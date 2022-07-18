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

        public UserLevel UserLevel { get; set; }

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

    public enum UserLevel
    {
        Customer = 0,
        Employee = 1,
        Admin = 2,
    }
}
