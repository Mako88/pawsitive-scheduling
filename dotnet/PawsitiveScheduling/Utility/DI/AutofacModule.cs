using Autofac;

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
            ComponentAttribute.RegisterComponents(builder);
        }
    }
}
