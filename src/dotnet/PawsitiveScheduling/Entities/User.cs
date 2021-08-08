using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// The user entity
    /// </summary>
    public class User : Entity
    {
        public User()
        {
            CollectionName = "users";
        }

        public string Username { get; set; }

        public PasswordInfo Password { get; set; }
    }

    public class PasswordInfo
    {
        public byte[] HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public int DegreeOfParallelism { get; set; }

        public int Iterations { get; set; }

        public int MemorySize { get; set; }
    }
}
