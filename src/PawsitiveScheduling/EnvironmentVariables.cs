using System;

namespace PawsitiveScheduling
{
    /// <summary>
    /// Class to manage environment variables
    /// </summary>
    public static class EnvironmentVariables
    {
        /// <summary>
        /// Get the connection string for the database
        /// </summary>
        public static string GetDBConnectionString()
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASS");
            return $"server={host};port={port};database={database};user={user};password={password}";
        }
    }
}
