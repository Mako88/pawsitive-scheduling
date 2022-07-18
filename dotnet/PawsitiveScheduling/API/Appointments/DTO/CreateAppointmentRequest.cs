using System;

namespace PawsitiveScheduling.API.Appointments.DTO
{
    public class CreateAppointmentRequest
    {
        public DateTime StartDate { get; set; }

        public int Duration { get; set; }

        public string GroomerId { get; set; }
    }
}
