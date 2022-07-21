using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// Base entity class
    /// </summary>
    public abstract class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
    }
}
