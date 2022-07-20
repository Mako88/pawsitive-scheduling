using Autofac;
using PawsitiveScheduling.Utility.Extensions;
using Serilog;
using System;

namespace PawsitiveScheduling.Utility.DI
{
    /// <summary>
    /// Module for registering services with Autofac
    /// </summary>
    public class AutofacModule : Module
    {
        /// <summary>
        /// Load services
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((string requestType) => CreateLogger(requestType)).InstancePerLifetimeScope();

            //builder.RegisterInstance(CreateLogger()).SingleInstance();

            ComponentAttribute.RegisterComponents(builder);
        }

        /// <summary>
        /// Create a logger
        /// </summary>
        private ILog CreateLogger(string requestType = null)
        {
            var path = requestType.HasValue() ?
                $"{Environment.CurrentDirectory}\\Logs\\Requests\\{requestType}\\{DateTime.Now:hh-mm-ss-fff}.txt" :
                $"{Environment.CurrentDirectory}\\Logs\\log.txt";

            return new LogWrapper(new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Async(x => x.File(path, shared: true, flushToDiskInterval: TimeSpan.FromSeconds(10), rollOnFileSizeLimit: true))
                .WriteTo.Async(x => x.Debug())
                .CreateLogger());
        }
    }
}
