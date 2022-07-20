using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Repositories;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Auth;
using PawsitiveScheduling.Utility.DI;
using System;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth
{
    /// <summary>
    /// Handler for registering a user
    /// </summary>
    [Component]
    public class RegisterUserHandler : Handler
    {
        private readonly IHashingUtility hashingUtility;
        private readonly IUserRepository userRepo;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterUserHandler(IHashingUtility hashingUtility,
            IUserRepository userRepo,
            Func<string, ILog> logFactory)
        {
            log = logFactory("RegisterUser");
            this.hashingUtility = hashingUtility;
            this.userRepo = userRepo;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapPost("api/auth/register", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        public async Task<IResult> Handle([FromBody] RegisterUserRequest request)
        {
            log.Info($"Creating user {request.Username}");

            var user = new User
            {
                Username = request.Username
            };

            user.Password = hashingUtility.CreateHash(request.Password);

            user = await userRepo.AddEntity(user);

            log.Info("User saved");

            return CreateResponse(new { Id = user.Id });
        }
    }
}
