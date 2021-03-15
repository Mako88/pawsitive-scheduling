namespace PawsitivityScheduler.Data.Breeds
{
    public class Breed
    {
        public BreedNames Name { get; set; }

        public int GroomMinutes { get; set; }

        public int BathMinutes { get; set; }

        public Sizes Size { get; set; }

        public Groups Group { get; set; }
    }
}
