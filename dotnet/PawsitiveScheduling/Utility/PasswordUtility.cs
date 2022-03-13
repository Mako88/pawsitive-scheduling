using Konscious.Security.Cryptography;
using PawsitiveScheduling.Entities;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Utility for interacting with passwords
    /// </summary>
    public class PasswordUtility : IPasswordUtility
    {
        /// <summary>
        /// Encrypt a plaintext password and add it to a user
        /// </summary>
        public void EncryptPassword(string password, User user)
        {
            if (user.Password == null)
            {
                user.Password = new PasswordInfo();
            }

            user.Password.Salt = CreateSalt();
            user.Password.DegreeOfParallelism = EnvironmentVariables.Argon2DegreeOfParallelism;
            user.Password.Iterations = EnvironmentVariables.Argon2Iterations;
            user.Password.MemorySize = EnvironmentVariables.Argon2MemorySize;

            user.Password.HashedPassword = HashPassword(password, user);
        }

        /// <summary>
        /// Verify a plaintext password for a user
        /// </summary>
        public bool VerifyPassword(string password, User user)
        {
            var newHash = HashPassword(password, user);
            return user.Password.HashedPassword.SequenceEqual(newHash);
        }

        /// <summary>
        /// Generate a password salt
        /// </summary>
        private byte[] CreateSalt() => RandomNumberGenerator.GetBytes(16);

        /// <summary>
        /// Hash the password
        /// </summary>
        private byte[] HashPassword(string password, User user)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = user.Password.Salt,
                DegreeOfParallelism = user.Password.DegreeOfParallelism,
                Iterations = user.Password.Iterations,
                MemorySize = user.Password.MemorySize
            };

            return argon2.GetBytes(16);
        }
    }
}
