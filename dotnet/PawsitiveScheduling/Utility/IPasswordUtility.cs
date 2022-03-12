using PawsitiveScheduling.Entities;

namespace PawsitiveScheduling.Utility
{
    public interface IPasswordUtility
    {
        /// <summary>
        /// Encrypt a plaintext password and add it to a user
        /// </summary>
        void EncryptPassword(string password, User user);

        /// <summary>
        /// Verify a plaintext password for a user
        /// </summary>
        bool VerifyPassword(string password, User user);
    }
}