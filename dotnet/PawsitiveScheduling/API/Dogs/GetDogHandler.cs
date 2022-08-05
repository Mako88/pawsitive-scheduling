using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using PawsitivityScheduler.Entities.Dogs;
using System.Net;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Dogs
{
    /// <summary>
    /// Handler for getting a dog by ID
    /// </summary>
    [Component(Singleton = true)]
    public class GetDogHandler : Handler
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public GetDogHandler(IDatabaseUtility dbUtility, ILog log) : base(log)
        {
            this.dbUtility = dbUtility;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapGet("api/dogs/{id}", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Receptionist}")]
        public async Task<IResult> Handle(string id)
        {
            // TODO: Receive a request object instead of just an ID string
            // and implement the template method pattern

            log.Info($"Getting dog with ID {id}");

            var dog = await dbUtility.GetEntity<Dog>(id);

            if (dog == null)
            {
                return CreateResponse(HttpStatusCode.NotFound, "Invalid dogId");
            }

            return CreateResponse(dog);
        }
    }
}
