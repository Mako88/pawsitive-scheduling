using Be.Vlaanderen.Basisregisters.Generators.Guid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Repositories;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Auth;
using PawsitiveScheduling.Utility.DI;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth
{
    /// <summary>
    /// Handler for refreshing a token
    /// </summary>
    [Component]
    public class RefreshTokenHandler : Handler
    {
        private readonly IUserRepository userRepo;
        private readonly ITokenUtility tokenUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public RefreshTokenHandler(IUserRepository userRepo, ITokenUtility tokenUtility, ILog log) : base(log)
        {
            this.userRepo = userRepo;
            this.tokenUtility = tokenUtility;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapGet("api/auth/token", Handle);

        /// <summary>
        /// Fetch new tokens
        /// </summary>
        [Authorize]
        public async Task<IResult> Handle(ClaimsPrincipal claimsUser, HttpContext context, HttpRequest request)
        {
            string userId;

            try
            {
                userId = tokenUtility.GetUserId(claimsUser);

                log.Info($"Refreshing token for userId {userId}");
            }
            catch (AuthenticationException ex)
            {
                log.Error(ex.Message, ex);
                return CreateResponse(HttpStatusCode.Forbidden, "Sid claim missing from token");
            }

            var user = await userRepo.GetEntity<User>(userId);

            if (user == null)
            {
                return CreateResponse(HttpStatusCode.Forbidden, "Invalid user in token");
            }

            var hashedIp = Deterministic.Create(Constants.IpAddressDeterministicNamespace, context.Connection.RemoteIpAddress?.ToString());

            var refreshToken = request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            if (!user.RefreshTokens.TryGetValue(hashedIp.ToString(), out var currentToken))
            {
                return CreateResponse(HttpStatusCode.Forbidden, "Refresh token IP does not match current request IP");
            }

            if (refreshToken != currentToken)
            {
                return CreateResponse(HttpStatusCode.Forbidden, "Invalid refresh token");
            }

            return CreateResponse(await tokenUtility.CreateTokens(user, context.Connection.RemoteIpAddress?.ToString()));
        }
    }
}
