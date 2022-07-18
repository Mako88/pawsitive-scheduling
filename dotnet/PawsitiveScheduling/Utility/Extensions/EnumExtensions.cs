using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PawsitiveScheduling.Utility.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.FirstOrDefault()?.Description ?? value.ToString();
        }
    }
}