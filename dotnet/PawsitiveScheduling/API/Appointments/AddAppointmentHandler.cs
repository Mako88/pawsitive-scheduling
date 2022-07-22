using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using System;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Appointments
{
    /// <summary>
    /// Handler for adding an appointment
    /// </summary>
    [Component]
    public class AddAppointmentHandler : Handler
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddAppointmentHandler(IDatabaseUtility dbUtility, ILog log) : base(log)
        {
            this.dbUtility = dbUtility;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapPost("api/appointments/add", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Receptionist},{UserRoles.Groomer},{UserRoles.Customer}")]
        public async Task<IResult> Handle([FromBody] CreateAppointmentRequest request)
        {
            log.Info($"Creating new appointment for GroomerId '{request.GroomerId}' starting on '{request.StartDate}' and lasting '{request.Duration}' minutes");

            var scheduledTime = new TimeBlock(request.StartDate, TimeSpan.FromMinutes(request.Duration));

            var appointment = new Appointment
            {
                GroomerId = request.GroomerId,
                ScheduledTime = scheduledTime,
            };

            var savedAppointment = await dbUtility.AddEntity(appointment).ConfigureAwait(false);

            if (request.AutoAssigned)
            {
                var tracker = await dbUtility.GetTracker();

                log.Info($"Setting LastAutoAssignedGroomerId to '{request.GroomerId}'");
                tracker.LastAutoAssignedGroomerId = request.GroomerId;
                await dbUtility.UpdateEntity(tracker);
            }

            return CreateResponse(new { Id = savedAppointment.Id });
        }
    }
}
