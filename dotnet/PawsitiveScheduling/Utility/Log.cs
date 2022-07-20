using Serilog;
using Serilog.Events;
using System;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Wrapper for Serilog's logging
    /// </summary>
    public class LogWrapper : ILog
    {
        private readonly ILogger log;

        /// <summary>
        /// Constructor
        /// </summary>
        public LogWrapper(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Log a message
        /// </summary>
        public void Info(string message)
        {
            try
            {
                log.Information(message);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Log a warning
        /// </summary>
        public void Warn(string message)
        {
            try
            {
                log.Warning(message);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Log a warning with an exception
        /// </summary>
        public void Warn(string message, Exception ex)
        {
            try
            {
                log.Warning(ex, message);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Log an error with an exception
        /// </summary>
        public void Error(string message, Exception ex)
        {
            try
            {
                log.Error(ex, message);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Write an event to the log
        /// </summary>
        public void Write(LogEvent logEvent)
        {
            try
            {
                log.Write(logEvent);
            }
            catch (Exception)
            {

            }
        }
    }
}
