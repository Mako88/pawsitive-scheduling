using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using PawsitivityScheduler.Entities.Dogs;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Appointments
{
    /// <summary>
    /// Handler for getting the length of an appointment
    /// </summary>
    public class GetAppointmentTimeHandler : Handler
    {
        private readonly IDatabaseUtility dbUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public GetAppointmentTimeHandler(IDatabaseUtility dbUtility, ILog log) : base(log)
        {
            this.dbUtility = dbUtility;
            this.log = log;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public override void MapEndpoint(WebApplication app) => app.MapGet("api/appointments/time", Handle);

        /// <summary>
        /// Handle the request
        /// </summary>
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Receptionist},{UserRoles.Groomer},{UserRoles.Customer}")]
        public async Task<IResult> Handle([FromQuery] string dogId, [FromQuery] List<string> serviceIds, [FromQuery] bool bathOnly, [FromQuery] string groomerId)
        {
            log.Info($"Getting appointment time for groomer {groomerId}, dog {dogId}, with services: {string.Join(", ", serviceIds)} and bathOnly: {bathOnly}");

            var groomer = await dbUtility.GetEntity<Groomer>(groomerId);

            if (groomer == null)
            {
                return CreateResponse(HttpStatusCode.NotFound, "Invalid groomerId");
            }

            var services = new List<Service>();

            foreach (var serviceId in serviceIds)
            {
                var service = await dbUtility.GetEntity<Service>(serviceId);

                if (service == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, $"Invalid serviceId: {serviceId}");
                }

                services.Add(service);
            }

            var dog = await dbUtility.GetEntity<Dog>(dogId);

            if (dog == null)
            {
                return CreateResponse(HttpStatusCode.NotFound, "Invalid dogId");
            }

            var breed = await dbUtility.GetBreedByName(dog.Breed);

            if (breed == null)
            {
                return CreateResponse(HttpStatusCode.NotFound, $"Could not find breed '{dog.Breed}'");
            }

            var time = breed.BathMinutes + dog.AdditionalBathMinutes;

            if (!bathOnly)
            {
                time += breed.GroomMinutes + dog.AdditionalGroomMinutes;
            }

            time += groomer.SizeTimeAdjustments[breed.Size];

            foreach (var service in services)
            {
                time += service.Time;

                if (groomer.ServiceTimeAdjustments.TryGetValue(service.Id, out var timeAdjust))
                {
                    time += timeAdjust;
                }
            }

            log.Info($"Caluclated time: {time}");

            return CreateResponse(new { Time = time });
        }
    }
}
