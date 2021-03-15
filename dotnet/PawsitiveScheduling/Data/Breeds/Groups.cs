using System.ComponentModel;

namespace PawsitivityScheduler.Data.Breeds
{
    public enum Groups
    {
        [Description("Herding")]
        Herding,

        [Description("Hounds")]
        Hounds,

        [Description("Non-Sporting")]
        NonSporting,

        [Description("Sporting")]
        Sporting,

        [Description("Terriers")]
        Terriers,

        [Description("Toy Breeds")]
        ToyBreeds,

        [Description("Working")]
        Working,
    }
}
