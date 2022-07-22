using PawsitiveScheduling.Entities.Users;

namespace PawsitiveScheduling.Utility.Auth
{
    public interface IHashingUtility
    {
        /// <summary>
        /// Generate a hash for the given text
        /// </summary>
        HashInfo CreateHash(string plaintext);

        /// <summary>
        /// Verify the given text matches the given hash
        /// </summary>
        bool Verify(string plaintext, HashInfo hashInfo);
    }
}