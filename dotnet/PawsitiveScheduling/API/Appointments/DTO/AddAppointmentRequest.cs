using System;
using System.ComponentModel.DataAnnotations;

namespace PawsitiveScheduling.API.Appointments.DTO
{
    public class AddAppointmentRequest
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string? GroomerId { get; set; }

        public bool AutoAssigned { get; set; }
    }
}
