using System;

namespace PawsitiveScheduling
{
    /// <summary>
    /// Class to manage environment variables
    /// </summary>
    public static class EnvironmentVariables
    {
        public static bool IsLocal = GetEnvironmentVariable("IS_LOCAL", true);

        public static int Argon2DegreeOfParallelism = GetEnvironmentVariable("ARGON2_DEGREE_OF_PARALLELISM", 8);

        public static int Argon2Iterations = GetEnvironmentVariable("ARGON2_ITERATIONS", 4);

        public static int Argon2MemorySize = GetEnvironmentVariable("ARGON2_MEMORY_SIZE", 1024*1024);

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

        /// <summary>
        /// Get an environment variable as an int
        /// </summary>
        public static int GetEnvironmentVariable(string variableName, int defaultValue)
        {
            try
            {
                return int.Parse(Environment.GetEnvironmentVariable(variableName));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
