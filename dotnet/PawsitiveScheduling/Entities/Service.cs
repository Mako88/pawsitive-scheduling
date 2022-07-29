using PawsitiveScheduling.Utility.Attributes;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// A grooming service
    /// </summary>
    [BsonCollectionName("services")]
    public class Service : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public int Time { get; set; }
    }
}
