using PawsitiveScheduling.Utility.Attributes;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// A grooming service
    /// </summary>
    [BsonCollectionName("services")]
    public class Service : Entity
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Cost { get; set; }

        public int Time { get; set; }
    }
}
