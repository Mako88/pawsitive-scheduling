using Itenso.TimePeriod;
using PawsitiveScheduling.Utility.Attributes;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// An appointment
    /// </summary>
    [BsonCollectionName(Constants.AppointmentCollectionName)]
    public class Appointment : Entity
    {
        public ITimePeriod ScheduledTime { get; set; } = null!;

        public string GroomerId { get; set; } = null!;
    }
}
