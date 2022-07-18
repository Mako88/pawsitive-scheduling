using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// Base entity class
    /// </summary>
    public abstract class Entity
    {
        public virtual ObjectId Id { get; set; }

        [BsonIgnore]
        public abstract string CollectionName { get; }
    }
}
