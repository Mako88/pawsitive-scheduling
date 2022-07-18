using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Repositories;
using System;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Appointments
{
    /// <summary>
    /// Controller for api/appointments/* endpoints
    /// </summary>
    [Route("api/appointments")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository appointmentRepo;

        /// <summary>
        /// Constructor
        /// </summary>
        public AppointmentsController(IAppointmentRepository appointmentRepo)
        {
            this.appointmentRepo = appointmentRepo;
        }

        /// <summary>
        /// Create a new appointment
        /// </summary>
        [HttpPost]
        [Route("create")]
        public async Task<string> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            var scheduledTime = new TimeBlock(request.StartDate, TimeSpan.FromMinutes(request.Duration));

            var appointment = new Appointment
            {
                GroomerId = request.GroomerId,
                ScheduledTime = scheduledTime,
            };

            var savedAppointment = await appointmentRepo.AddEntity(appointment).ConfigureAwait(false);

            return savedAppointment.Id.ToString();
        }
    }
}
