using Konscious.Security.Cryptography;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility.DI;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PawsitiveScheduling.Utility.Auth
{
    /// <summary>
    /// Utility for interacting with passwords
    /// </summary>
    [Component]
    public class HashingUtility : IHashingUtility
    {
        /// <summary>
        /// Generate a hash for the given text
        /// </summary>
        public HashInfo CreateHash(string plaintext)
        {
            var hashInfo = new HashInfo();

            hashInfo.Salt = CreateSalt();
            hashInfo.DegreeOfParallelism = EnvironmentVariables.Argon2DegreeOfParallelism;
            hashInfo.Iterations = EnvironmentVariables.Argon2Iterations;
            hashInfo.MemorySize = EnvironmentVariables.Argon2MemorySize;

            hashInfo.HashedText = PerformHash(plaintext, hashInfo);

            return hashInfo;
        }

        /// <summary>
        /// Verify the given text matches the given hash
        /// </summary>
        public bool Verify(string plaintext, HashInfo hashInfo)
        {
            var newHash = PerformHash(plaintext, hashInfo);
            return hashInfo.HashedText.SequenceEqual(newHash);
        }

        /// <summary>
        /// Generate a salt
        /// </summary>
        private byte[] CreateSalt() => RandomNumberGenerator.GetBytes(16);

        /// <summary>
        /// Hash the plaintext
        /// </summary>
        private byte[] PerformHash(string plaintext, HashInfo hashInfo)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(plaintext))
            {
                Salt = hashInfo.Salt,
                DegreeOfParallelism = hashInfo.DegreeOfParallelism,
                Iterations = hashInfo.Iterations,
                MemorySize = hashInfo.MemorySize
            };

            return argon2.GetBytes(16);
        }
    }
}
