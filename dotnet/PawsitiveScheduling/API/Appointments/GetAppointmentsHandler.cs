using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using PawsitiveScheduling.Utility.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Appointments
{
    /// <summary>
    /// Handler for getting appointments
    /// </summary>
    [Component]
    public class GetAppointmentsHandler : Handler
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public GetAppointmentsHandler(IDatabaseUtility dbUtility, ILog log) : base(log)
        {
            this.dbUtility = dbUtility;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapGet("api/appointments/get", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Receptionist},{UserRoles.Groomer},{UserRoles.Customer}")]
        public async Task<IResult> Handle([FromQuery] string groomerId)
        {
            if (groomerId.Equals("any", StringComparison.OrdinalIgnoreCase))
            {
                var tracker = await dbUtility.GetTracker();

                var groomers = await dbUtility.GetEntities<Groomer>(sortBy: x => x.Id);
                var groomer = groomers.FirstOrDefault();

                if (tracker.LastAutoAssignedGroomerId.HasValue())
                {
                    var index = groomers.FindIndex(x => x.Id == tracker.LastAutoAssignedGroomerId);

                    if (index != -1)
                    {
                        groomer = groomers[index + 1];
                    }
                }

                groomerId = groomer.Id;
            }

            log.Info($"Getting appointments for GroomerId '{groomerId}'");

            var appointments = await dbUtility.GetEntities<Appointment>(x => x.GroomerId == groomerId);

            return CreateResponse(new { Appointments = appointments });
        }
    }
}
