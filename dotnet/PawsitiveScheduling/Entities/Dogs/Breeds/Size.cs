using System.ComponentModel;

namespace PawsitivityScheduler.Entities.Dogs.Breeds
{
    public enum Size
    {
        [Description("Extra Small")]
        ExtraSmall,

        [Description("Small")]
        Small,

        [Description("Medium")]
        Medium,

        [Description("Large")]
        Large,

        [Description("Extra Large")]
        ExtraLarge,
    }
}
