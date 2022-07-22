namespace PawsitiveScheduling.Entities
{
    /// <summary>
    /// Entity that stores values that need to be tracked between requests
    /// </summary>
    public class Tracker : Entity
    {
        public string LastAutoAssignedGroomerId { get; set; }
    }
}
