using PawsitivityScheduler.Data.Breeds;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PawsitivityScheduler.Data
{
    public class Dog
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public Breed Breed { get; set; }

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
