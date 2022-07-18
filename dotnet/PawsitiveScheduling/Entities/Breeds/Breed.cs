using PawsitiveScheduling;
using PawsitiveScheduling.Entities;

namespace PawsitivityScheduler.Data.Breeds
{
    public class Breed : Entity
    {
        public override string CollectionName => Constants.BreedCollectionName;

        public BreedName Name { get; set; }

        public int GroomMinutes { get; set; }

        public int BathMinutes { get; set; }

        public Size Size { get; set; }

        public Group Group { get; set; }
    }
}
