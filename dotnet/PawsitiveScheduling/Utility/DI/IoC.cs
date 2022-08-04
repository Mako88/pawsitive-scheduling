using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;

namespace PawsitiveScheduling.Utility.DI
{
    /// <summary>
    /// Class for creating lifetime scopes
    /// </summary>
    public class IoC
    {
        /// <summary>
        /// The root Autofac container
        /// </summary>
        public static ILifetimeScope? RootContainer { get; private set; }

        /// <summary>
        /// Perform initialization
        /// </summary>
        public static void Initialize(WebApplication app) => RootContainer = app.Services.GetAutofacRoot();

        /// <summary>
        /// Create a lifetime scope to resolve services from
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope() => RootContainer?.BeginLifetimeScope() ?? throw new Exception("Must call Initialize before BeginLifetimeScope");
    }
}
