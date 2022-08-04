using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Entities.Users;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Entities
{
    /// <summary>
    /// Utility for managing users
    /// </summary>
    public interface IUserUtility
    {
        /// <summary>
        /// Create a user of the proper type
        /// </summary>
        Task<User> CreateUser(RegisterUserRequest request);
    }
}