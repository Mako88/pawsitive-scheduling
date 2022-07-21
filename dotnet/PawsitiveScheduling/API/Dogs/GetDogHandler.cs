using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.DI;
using PawsitivityScheduler.Data;
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
        public async Task<Dog> Handle(string id)
        {
            log.Info($"Getting dog with ID {id}");

            return await dbUtility.GetEntity<Dog>(id).ConfigureAwait(false);
        }
    }
}
