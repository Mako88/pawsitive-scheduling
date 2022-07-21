using Microsoft.AspNetCore.Http;
using PawsitiveScheduling.Utility.DI;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;

namespace PawsitiveScheduling.Utility
{
    /// <summary>
    /// Wrapper for Serilog's logging
    /// </summary>
    [Component(Singleton = true)]
    public class LogManager : ILog, ILogger
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly Dictionary<string, DateLogger> loggers = new();

        private const string DayFormat = "yyyy-MM-dd";
        private readonly string LogBasePath = $"{Environment.CurrentDirectory}\\Logs\\{{0}}";

        /// <summary>
        /// Constructor
        /// </summary>
        public LogManager(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Log a message
        /// </summary>
        public void Info(string message)
        {
            try
            {
                GetLogger().Information(message);
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
                GetLogger().Warning(message);
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
                GetLogger().Warning(ex, message);
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
                GetLogger().Error(ex, message);
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
                GetLogger().Write(logEvent);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Get a logger
        /// </summary>
        private ILogger GetLogger()
        {
            var path = contextAccessor?.HttpContext?.Request?.Path.Value?.Replace('/', '\\');

            // Use the main logger if we're not in a request context
            if (path == null)
            {
                return GetOrCreateLogger("main", "log.txt");
            }

            return GetOrCreateLogger(path, $"{path}\\log.txt");
        }

        /// <summary>
        /// Get the logger with the given key from the dictionary
        /// or create it with the given path if it doesn't exist
        /// </summary>
        private ILogger GetOrCreateLogger(string key, string path)
        {
            var day = DateTime.Now.ToString(DayFormat);

            if (loggers.TryGetValue(key, out var logger) && logger.Day == day)
            {
                return logger.Logger;
            }

            logger = new DateLogger
            {
                Day = day,
                Logger = CreateLogger($"{string.Format(LogBasePath, day)}\\{path}"),
            };

            loggers[key] = logger;

            return logger.Logger;
        }

        /// <summary>
        /// Create a logger for the given path
        /// </summary>
        private static ILogger CreateLogger(string path) =>
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Async(x => x.File(path, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(10), rollOnFileSizeLimit: true))
                .WriteTo.Async(x => x.Debug())
                .CreateLogger();

        /// <summary>
        /// A class for per-day loggers
        /// </summary>
        private class DateLogger
        {
            public ILogger Logger { get; set; }

            public string Day { get; set; }
        }
    }
}
