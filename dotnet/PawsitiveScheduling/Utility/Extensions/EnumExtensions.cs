using System;
using System.ComponentModel;
using System.Linq;

namespace PawsitiveScheduling.Utility.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[]) (fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) ?? new DescriptionAttribute[0]);

            return attributes.FirstOrDefault()?.Description ?? value.ToString();
        }
    }
}