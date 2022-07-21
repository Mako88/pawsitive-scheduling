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
        /// Get a user by email
        /// </summary>
        public async Task<User> GetUserByEmail(string email) =>
            await GetCollection<User>().Find(x => x.Email == email).SingleOrDefaultAsync().ConfigureAwait(false);
    }
}
