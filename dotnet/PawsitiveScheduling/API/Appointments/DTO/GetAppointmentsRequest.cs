using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PawsitiveScheduling.API.Appointments.DTO
{
    /// <summary>
    /// Request to get appointments
    /// </summary>
    public class GetAppointmentsRequest
    {
        [Required]
        [FromQuery]
        public string? GroomerId { get; set; }
    }
}
