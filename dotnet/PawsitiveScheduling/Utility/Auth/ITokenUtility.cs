using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility.Auth.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Utility.Auth
{
    /// <summary>
    /// Utility for managing JWT tokens
    /// </summary>
    public interface ITokenUtility
    {
        /// <summary>
        /// Create JWT tokens for the given user
        /// </summary>
        Task<TokenResponse> CreateTokens(User user, string ipAddress);

        /// <summary>
        /// Get a user ID from a ClaimsPrincipal
        /// </summary>
        string GetUserId(ClaimsPrincipal user);
    }
}