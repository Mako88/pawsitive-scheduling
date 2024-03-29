﻿using PawsitiveScheduling.Utility.Attributes;

namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// Entity that stores values that need to be tracked between requests
    /// </summary>
    [BsonCollectionName("Tracker")]
    public class Tracker : Entity
    {
        public override string Id => "tracker";

        public string? LastAutoAssignedGroomerId { get; set; }
    }
}
