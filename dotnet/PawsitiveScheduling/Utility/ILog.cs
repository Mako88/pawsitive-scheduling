using Serilog;
using System;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Wrapper for Serilog's logging
    /// </summary>
    public interface ILog : ILogger
    {
        /// <summary>
        /// Log a message
        /// </summary>
        void Info(string message);

        /// <summary>
        /// Log a warning
        /// </summary>
        void Warn(string message);

        /// <summary>
        /// Log a warning with an exception
        /// </summary>
        void Warn(string message, Exception? ex);

        /// <summary>
        /// Log an error with an exception
        /// </summary>
        void Error(string message, Exception? ex);
    }
}
