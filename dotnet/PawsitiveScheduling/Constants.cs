using System;

namespace PawsitiveScheduling
{
    public static class Constants
    {
        public const string DatabaseName = "pawsitivity";

        // Collection names
        public const string BreedCollectionName = "breeds";
        public const string DogCollectionName = "dogs";
        public const string UserCollectionName = "users";
        public const string AppointmentCollectionName = "appointments";

        // Index names
        public const string BreedNameIndexName = "breedNameIndex";
        public const string AppointmentScheduledTimeIndexName = "appointmentScheduledTimeIndex";
        public const string AppointmentGroomerIdIndexName = "appointmentGroomerIdIndex";
        public const string UserEmailIndexName = "userEmailIndex";

        public static readonly Guid IpAddressDeterministicNamespace = Guid.Parse("F43FA0D2-1D0E-45EF-B0DA-CD7A92A34937");
    }
}
