using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Repositories
{
    /// <summary>
    /// Repo for Users
    /// </summary>
    public interface IUserRepository : IDatabaseUtility
    {
        /// <summary>
        /// Get a user by username
        /// </summary>
        Task<User> GetUserByUsername(string username);
    }
}