using MongoDB.Driver;
using PawsitiveScheduling.Entities.Users;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Database
{
    /// <summary>
    /// User-related database methods
    /// </summary>
    public partial class DatabaseUtility : IDatabaseUtility
    {
        /// <summary>
        /// Get a user by email
        /// </summary>
        public async Task<User> GetUserByEmail(string email) =>
            await GetCollection<User>().Find(x => x.Email == email).SingleOrDefaultAsync();
    }
}
