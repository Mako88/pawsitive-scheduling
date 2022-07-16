using Autofac;
using Microsoft.Extensions.Logging;

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
            builder.Register(x =>
                LoggerFactory.Create(log =>
                {
                    log.SetMinimumLevel(LogLevel.Debug);
                    log.AddConsole();
                }))
                .As<ILoggerFactory>()
                .SingleInstance()
                .AutoActivate();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            ComponentAttribute.RegisterComponents(builder);
        }
    }
}
