using MongoDB.Bson.Serialization.Attributes;
using PawsitiveScheduling;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility.Attributes;
using PawsitiveScheduling.Utility.Extensions;
using PawsitivityScheduler.Entities.Dogs.Breeds;
using System;

namespace PawsitivityScheduler.Entities.Dogs
{
    /// <summary>
    /// The dog entity
    /// </summary>
    [BsonCollectionName(Constants.DogCollectionName)]
    public class Dog : Entity
    {
        public string Name { get; set; } = null!;

        public BreedName Breed { get; set; }

        [BsonIgnore]
        public string BreedDisplayName => Breed.GetDescription();

        public int AdditionalGroomMinutes { get; set; }

        public int AdditionalBathMinutes { get; set; }

        public DateTime BirthDate { get; set; }

        [BsonIgnore]
        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - BirthDate.Year;

                // Subtract a year if their birthday hasn't happened yet this year
                if (BirthDate > DateTime.Today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
            set => BirthDate = DateTime.Today.AddYears(-value);
        }
    }
}
