using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth
{
    /// <summary>
    /// Handler for api/auth/* endpoints
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AuthController
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly IPasswordUtility passwordUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthController(IDatabaseUtility dbUtility, IPasswordUtility passwordUtility)
        {
            this.dbUtility = dbUtility;
            this.passwordUtility = passwordUtility;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<TokenResponse> RegisterUser([FromBody] CreateUserRequest request)
        {
            // TODO: Authenticate the caller before registering a user

            var user = new User
            {
                Username = request.Username
            };

            passwordUtility.EncryptPassword(request.Password, user);

            await dbUtility.AddEntity(user);

            // TODO: Generate tokens and return them
            return new TokenResponse();
        }
    }
}
