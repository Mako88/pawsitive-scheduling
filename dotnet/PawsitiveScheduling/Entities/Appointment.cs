using Itenso.TimePeriod;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// An appointment
    /// </summary>
    public class Appointment : Entity
    {
        public override string CollectionName => Constants.AppointmentCollectionName;

        public ITimePeriod ScheduledTime { get; set; }

        public string GroomerId { get; set; }
    }
}
