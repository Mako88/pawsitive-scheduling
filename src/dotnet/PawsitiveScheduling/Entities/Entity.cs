using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// Base entity class
    /// </summary>
    public class Entity
    {
        public virtual ObjectId Id { get; set; }

        [BsonIgnore]
        public string CollectionName { get; set; }
    }
}
