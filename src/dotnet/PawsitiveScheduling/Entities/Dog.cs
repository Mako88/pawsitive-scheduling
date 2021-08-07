using PawsitiveScheduling;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Extensions;
using PawsitivityScheduler.Data.Breeds;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PawsitivityScheduler.Data
{
    public class Dog : Entity
    {
        public Dog()
        {
            CollectionName = Constants.DogCollectionName;
        }

        public string Name { get; set; }

        public BreedName Breed { get; set; }

        [NotMapped]
        public string BreedDisplayName => Breed.GetDescription();

        public int AdditionalGroomMinutes { get; set; }

        public int AdditionalBathMinutes { get; set; }

        public DateTime BirthDate { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - BirthDate.Year;

                // Account for leap years
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
