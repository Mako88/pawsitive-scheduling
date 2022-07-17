using MongoDB.Driver;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Repositories
{
    /// <summary>
    /// Repo for Users
    /// </summary>
    public class UserRepository : DatabaseUtility, IUserRepository
    {
        /// <summary>
        /// Get a user by username
        /// </summary>
        public async Task<User> GetUserByUsername(string username) =>
            await GetCollection<User>().Find(x => x.Username == username).FirstOrDefaultAsync().ConfigureAwait(false);
    }
}
