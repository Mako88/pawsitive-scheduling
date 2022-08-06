using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.API.Appointments.DTO;
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
        public async Task<IResult> Handle([FromBody] GetAppointmentTimeRequest request) =>
            await HandleCommon(request);

        /// <summary>
        /// Get the time for a pending appointment
        /// </summary>
        protected override async Task<IResult> HandleInternal(object requestObject)
        {
            var request = (GetAppointmentTimeRequest) requestObject;

            log.Info($"Getting appointment time for groomer {request.GroomerId}, dog {request.DogId}, with services: {string.Join(", ", request.ServiceIds)} and bathOnly: {request.BathOnly}");

            var groomer = await dbUtility.GetEntity<Groomer>(request.GroomerId);

            if (groomer == null)
            {
                return CreateResponse(HttpStatusCode.NotFound, "Invalid groomerId");
            }

            var services = new List<Service>();

            foreach (var serviceId in request.ServiceIds)
            {
                var service = await dbUtility.GetEntity<Service>(serviceId);

                if (service == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, $"Invalid serviceId: {serviceId}");
                }

                services.Add(service);
            }

            var dog = await dbUtility.GetEntity<Dog>(request.DogId);

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

            if (!request.BathOnly)
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

            log.Info($"Calculated time: {time}");

            return CreateResponse(new { Time = time });
        }
    }
}
