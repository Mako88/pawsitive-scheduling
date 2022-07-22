using PawsitiveScheduling;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility.Attributes;

namespace PawsitivityScheduler.Entities.Dogs.Breeds
{
    [BsonCollectionName(Constants.BreedCollectionName)]
    public class Breed : Entity
    {
        public BreedName Name { get; set; }

        public int GroomMinutes { get; set; }

        public int BathMinutes { get; set; }

        public Size Size { get; set; }

        public Group Group { get; set; }
    }
}
