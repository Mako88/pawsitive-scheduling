using PawsitivityScheduler.Entities.Dogs.Breeds;
using System.Collections.Generic;

namespace PawsitiveScheduling.Entities.Users
{
    /// <summary>
    /// The groomer entity
    /// </summary>
    public class Groomer : User
    {
        public Dictionary<string, int> ServiceTimeAdjustments { get; set; } = new();

        public Dictionary<Size, int> SizeTimeAdjustments { get; set; } = new();
    }
}
