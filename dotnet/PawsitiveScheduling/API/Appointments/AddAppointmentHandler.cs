using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Repositories;
using PawsitiveScheduling.Utility;
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
        private readonly IAppointmentRepository appointmentRepo;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddAppointmentHandler(IAppointmentRepository appointmentRepo, ILog log) : base(log)
        {
            this.appointmentRepo = appointmentRepo;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapPost("api/appointments/add", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Receptionist},{UserRoles.Groomer}")]
        public async Task<IResult> Handle([FromBody] CreateAppointmentRequest request)
        {
            var scheduledTime = new TimeBlock(request.StartDate, TimeSpan.FromMinutes(request.Duration));

            var appointment = new Appointment
            {
                GroomerId = request.GroomerId,
                ScheduledTime = scheduledTime,
            };

            var savedAppointment = await appointmentRepo.AddEntity(appointment).ConfigureAwait(false);

            return CreateResponse(new { Id = savedAppointment.Id });
        }
    }
}
