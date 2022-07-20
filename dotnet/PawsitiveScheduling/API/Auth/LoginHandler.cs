using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Repositories;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Auth;
using PawsitiveScheduling.Utility.DI;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth
{
    /// <summary>
    /// Handler for logging in
    /// </summary>
    [Component]
    public class LoginHandler : Handler
    {
        private readonly IHashingUtility hashingUtility;
        private readonly IUserRepository userRepo;
        private readonly ITokenUtility tokenUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoginHandler(IHashingUtility hashingUtility,
            IUserRepository userRepo,
            ITokenUtility tokenUtility,
            Func<string, ILog> logFactory)
        {
            this.hashingUtility = hashingUtility;
            this.userRepo = userRepo;
            this.tokenUtility = tokenUtility;
            log = logFactory("Login");
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapPost("api/auth/login", Handle);

        /// <summary>
        /// Login
        /// </summary>
        [AllowAnonymous]
        public async Task<IResult> Handle([FromBody] LoginRequest request, HttpContext context)
        {
            log.Info($"Authenticating {request.Username}");

            var user = await userRepo.GetUserByUsername(request.Username);

            if (user == null)
            {
                log.Info($"Could not find username '{request.Username}'");

                return CreateResponse(HttpStatusCode.NotFound, "Username not found");
            }

            if (!hashingUtility.Verify(request.Password, user.Password))
            {
                log.Info("Invalid password");

                return CreateResponse(HttpStatusCode.Forbidden, "Invalid password");
            }

            log.Info("Successfully authenticated");

            return CreateResponse(await tokenUtility.CreateTokens(user, context.Connection.RemoteIpAddress?.ToString()));
        }
    }
}
