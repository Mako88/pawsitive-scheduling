using System;

namespace PawsitiveScheduling
{
    /// <summary>
    /// Class to manage environment variables
    /// </summary>
    public static class EnvironmentVariables
    {
        public static bool IsLocal = GetEnvironmentVariable("IsLocal", true);

        /// <summary>
        /// Get the connection string for the database
        /// </summary>
        public static string GetDBConnectionString()
        {
            if (IsLocal)
            {
                return "mongodb://localhost:27017";
            }

            // TODO: Update the production connection string
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASS");
            return $"server={host};port={port};database={database};user={user};password={password}";
        }

        /// <summary>
        /// Get an environment variable as a boolean
        /// </summary>
        public static bool GetEnvironmentVariable(string variableName, bool defaultValue)
        {
            try
            {
                return bool.Parse(Environment.GetEnvironmentVariable(variableName));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
