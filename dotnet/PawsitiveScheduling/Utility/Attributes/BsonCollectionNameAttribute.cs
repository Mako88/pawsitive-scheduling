using System;

namespace PawsitiveScheduling.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BsonCollectionNameAttribute : Attribute
    {
        public string CollectionName { get; set; }

        public BsonCollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
