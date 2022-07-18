using MongoDB.Bson.Serialization.Attributes;
using PawsitiveScheduling;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility.Extensions;
using PawsitivityScheduler.Data.Breeds;
using System;

namespace PawsitivityScheduler.Data
{
    public class Dog : Entity
    {
        public override string CollectionName => Constants.DogCollectionName;

        public string Name { get; set; }

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

                // Subtract a year if 
                if (BirthDate > DateTime.Today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
            set
            {
                BirthDate = DateTime.Today.AddYears(-value);
            }
        }
    }
}
