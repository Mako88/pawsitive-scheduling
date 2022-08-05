using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Auth;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
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
        private readonly IDatabaseUtility dbUtility;
        private readonly ITokenUtility tokenUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoginHandler(IHashingUtility hashingUtility,
            IDatabaseUtility dbUtility,
            ITokenUtility tokenUtility,
            ILog log) : base(log)
        {
            this.hashingUtility = hashingUtility;
            this.dbUtility = dbUtility;
            this.tokenUtility = tokenUtility;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapPost("api/auth/login", Handle);

        /// <summary>
        /// Login
        /// </summary>
        [AllowAnonymous]
        public async Task<IResult> Handle(LoginRequest request, HttpContext context) =>
            await HandleCommon(request, context);

        /// <summary>
        /// Perform login
        /// </summary>
        protected override async Task<IResult> HandleInternal(object requestObject, HttpContext context)
        {
            var request = (LoginRequest) requestObject;

            log.Info($"Authenticating {request.Email}");

            var user = await dbUtility.GetUserByEmail(request.Email!);

            if (user == null)
            {
                log.Info($"Could not find user with email '{request.Email}'");

                return CreateResponse(HttpStatusCode.NotFound, "User not found");
            }

            if (!hashingUtility.Verify(request.Password!, user.Password))
            {
                log.Info("Invalid password");

                return CreateResponse(HttpStatusCode.Forbidden, "Invalid password");
            }

            log.Info("Successfully authenticated");

            return CreateResponse(await tokenUtility.CreateTokens(user, context.Connection.RemoteIpAddress?.ToString()));
        }
    }
}
