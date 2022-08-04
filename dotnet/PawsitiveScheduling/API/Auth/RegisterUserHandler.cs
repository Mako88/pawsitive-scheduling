using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.DI;
using PawsitiveScheduling.Utility.Entities;
using System;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth
{
    /// <summary>
    /// Handler for registering a user
    /// </summary>
    [Component]
    public class RegisterUserHandler : Handler
    {
        private readonly IUserUtility userUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterUserHandler(IUserUtility userUtility, ILog log) : base(log)
        {
            this.userUtility = userUtility;
            this.log = log;
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
            if (!ValidateRequest(request, out var response))
            {
                return response!;
            }

            try
            {
                var user = await userUtility.CreateUser(request);
                return CreateResponse(new { Id = user.Id });
            }
            catch (DuplicateNameException ex)
            {
                return CreateResponse(HttpStatusCode.Conflict, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.UnprocessableEntity, ex.Message);
            }
        }
    }
}
