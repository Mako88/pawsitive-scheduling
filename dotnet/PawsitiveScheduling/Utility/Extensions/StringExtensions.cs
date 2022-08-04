namespace PawsitiveScheduling.Utility.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Whether or not a string has a value
        /// </summary>
        public static bool HasValue(this string? theString) => !string.IsNullOrWhiteSpace(theString);
    }
}
