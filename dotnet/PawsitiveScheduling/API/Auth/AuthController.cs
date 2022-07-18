using Be.Vlaanderen.Basisregisters.Generators.Guid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MongoDB.Bson;
using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Repositories;
using PawsitiveScheduling.Utility.Auth;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth
{
    /// <summary>
    /// Controller for api/auth/* endpoints
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepo;
        private readonly IHashingUtility hashingUtility;
        private readonly ITokenUtility tokenUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthController(IUserRepository userRepo, IHashingUtility hashingUtility, ITokenUtility tokenUtility)
        {
            this.userRepo = userRepo;
            this.hashingUtility = hashingUtility;
            this.tokenUtility = tokenUtility;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task RegisterUser([FromBody] AuthenticationRequest request)
        {
            var user = new User
            {
                Username = request.Username
            };

            user.Password = hashingUtility.CreateHash(request.Password);

            await userRepo.AddEntity(user);
        }

        /// <summary>
        /// Authenticate a user
        /// </summary>
        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public async Task<ObjectResult> Authenticate([FromBody] AuthenticationRequest request)
        {
            var user = await userRepo.GetUserByUsername(request.Username);

            if (user == null)
            {
                return StatusCode(404, "Username not found");
            }

            if (!hashingUtility.Verify(request.Password, user.Password))
            {
                return StatusCode(403, "Invalid password");
            }

            return StatusCode(200, await tokenUtility.CreateTokens(user, HttpContext.Connection.RemoteIpAddress?.ToString()));
        }

        /// <summary>
        /// Fetch new tokens
        /// </summary>
        [HttpGet]
        [Route("token")]
        public async Task<ObjectResult> RefreshToken()
        {
            ObjectId userId;

            try
            {
                userId = tokenUtility.GetUserId(User);
            }
            catch (AuthenticationException)
            {
                return StatusCode(403, "Sid claim missing from token");
            }

            var user = await userRepo.GetEntity<User>(userId);

            if (user == null)
            {
                return StatusCode(403, "Invalid user in token");
            }

            var hashedIp = Deterministic.Create(Constants.IpAddressDeterministicNamespace, HttpContext.Connection.RemoteIpAddress?.ToString());

            var refreshToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (!user.RefreshTokens.TryGetValue(hashedIp.ToString(), out var currentToken))
            {
                return StatusCode(403, "Refresh token IP does not match current request IP");
            }

            if (refreshToken != currentToken)
            {
                return StatusCode(403, "Invalid refresh token");
            }

            return StatusCode(200, await tokenUtility.CreateTokens(user, HttpContext.Connection.RemoteIpAddress?.ToString()));
        }
    }
}
