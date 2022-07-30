using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using PawsitiveScheduling.Utility.Extensions;
using System;
using System.Linq;
using System.Net;
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
        public override void MapEndpoint(WebApplication app) => app.MapGet("api/appointments", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Receptionist},{UserRoles.Groomer},{UserRoles.Customer}")]
        public async Task<IResult> Handle(GetAppointmentsRequest request)
        {
            if (!ValidateRequest(request, out var response))
            {
                return response;
            }

            var groomerId = request.GroomerId;

            if (groomerId.Equals("any", StringComparison.OrdinalIgnoreCase))
            {
                log.Info("Getting next groomer to auto-assign");

                var groomers = await dbUtility.GetEntities<Groomer>(sortBy: x => x.Id);
                var groomer = groomers.FirstOrDefault();

                if (groomer == null)
                {
                    return CreateResponse(HttpStatusCode.UnprocessableEntity, "Could not find a groomer to auto-assign");
                }

                var tracker = await dbUtility.GetTracker();

                if (tracker.LastAutoAssignedGroomerId.HasValue())
                {
                    // Get the first groomer after the last auto-assigned groomer alphabetically by Id
                    groomer = groomers.FirstOrDefault(x => string.Compare(x.Id, tracker.LastAutoAssignedGroomerId) > 0) ?? groomer;

                    tracker.LastAutoAssignedGroomerId = groomer.Id;

                    await dbUtility.UpdateEntity(tracker);
                }

                groomerId = groomer.Id;
            }

            log.Info($"Getting appointments for GroomerId '{groomerId}'");

            var appointments = await dbUtility.GetEntities<Appointment>(x => x.GroomerId == groomerId);

            return CreateResponse(new { Appointments = appointments });
        }
    }
}
