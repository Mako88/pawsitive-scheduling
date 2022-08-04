using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PawsitiveScheduling.API.Appointments.DTO
{
    /// <summary>
    /// Request for getting the time for an appointment
    /// </summary>
    public class GetAppointmentTimeRequest
    {
        [FromQuery]
        [Required]
        public string? DogId { get; set; }

        [FromQuery]
        public List<string> ServiceIds { get; set; } = new();

        [FromQuery]
        public bool BathOnly { get; set; }

        [FromQuery]
        [Required]
        public string? GroomerId { get; set; }
    }
}
