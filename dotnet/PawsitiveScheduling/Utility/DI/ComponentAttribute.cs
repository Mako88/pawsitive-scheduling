using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PawsitiveScheduling.Utility.DI
{
    /// <summary>
    /// Attribute to register a component with Autofac
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ComponentAttribute : Attribute
    {
        public RegistrationType RegistrationType { get; set; }

        public bool Singleton { get; set; }

        public Type Interface { get; set; }

        public object Key { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentAttribute(RegistrationType RegistrationType = RegistrationType.AllInterfaces,
            bool Singleton = false,
            Type Interface = null,
            object Key = null
            )
        {
            this.RegistrationType = RegistrationType;
            this.Singleton = Singleton;
            this.Interface = Interface;
            this.Key = Key;
        }

        /// <summary>
        /// Perform registration of components
        /// </summary>
        public static void RegisterComponents(ContainerBuilder builder)
        {
            var typesToRegister = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes().Where(type => type.IsDefined(typeof(ComponentAttribute))).Select(type =>
            {
                var componentAttribute = type.GetCustomAttribute<ComponentAttribute>();

                return new KeyValuePair<Type, ComponentAttribute>(type, componentAttribute);
            }));

            foreach (var typeAttribute in typesToRegister)
            {
                var type = typeAttribute.Key;
                var attribute = typeAttribute.Value;

                var registration = builder.RegisterType(type);

                switch (attribute.RegistrationType)
                {
                    case RegistrationType.AllInterfaces:
                        var interfaces = type.GetInterfaces();

                        if (!interfaces.Any())
                        {
                            throw new DependencyResolutionException($"Error registering component '{nameof(type)}'. This component does not implement any interfaces, did you intend to register it as a class?");
                        }

                        foreach (var implementedInterface in type.GetInterfaces())
                        {
                            if (implementedInterface != typeof(IDisposable))
                            {
                                registration.As(implementedInterface);
                            }
                        }

                        break;
                    case RegistrationType.Interface:
                        var registeredInterface = attribute.Interface;

                        if (registeredInterface == null)
                        {
                            throw new DependencyResolutionException($"Error registering component '{nameof(type)}'. This component was registered as an interface, but Interface was not defined");
                        }

                        registration.As(registeredInterface);

                        break;
                    case RegistrationType.Keyed:
                        if (attribute.Key == null)
                        {
                            throw new DependencyResolutionException($"Error registering component '{nameof(type)}'. This component was registered as keyed, but Key was not defined");
                        }

                        if (attribute.Interface == null)
                        {
                            throw new DependencyResolutionException($"Error registering component '{nameof(type)}'. This component was registered as keyed, but Interface was not defined");
                        }

                        registration.Keyed(attribute.Key, attribute.Interface);

                        break;
                }

                if (attribute.Singleton)
                {
                    registration.SingleInstance();
                }
            }
        }
    }
}
