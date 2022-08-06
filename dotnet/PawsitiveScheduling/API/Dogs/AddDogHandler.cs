using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using PawsitivityScheduler.Entities.Dogs;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Dogs
{
    /// <summary>
    /// Handler for adding a dog
    /// </summary>
    [Component(Singleton = true)]
    public class AddDogHandler : Handler
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddDogHandler(IDatabaseUtility dbUtility, ILog log) : base(log)
        {
            this.dbUtility = dbUtility;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapPost("api/dogs", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Receptionist}")]
        public async Task<IResult> Handle([FromBody] Dog dog) =>
            await HandleCommon(dog);

        /// <summary>
        /// Add a dog
        /// </summary>
        protected override async Task<IResult> HandleInternal(object requestObject)
        {
            // TODO: Receive a request object, not a Dog entity directly

            var dog = (Dog) requestObject;

            log.Info("Adding dog");

            var savedDog = await dbUtility.AddEntity(dog);

            log.Info($"Saved {savedDog.Id}");

            return CreateResponse(new { Id = savedDog.Id });
        }
    }
}
