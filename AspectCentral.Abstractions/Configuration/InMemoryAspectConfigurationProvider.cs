// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryAspectConfigurationProvider.cs" company="CBRE">
//   
// </copyright>
// // <summary>
//   The in memory aspect configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspectCentral.Abstractions.Configuration
{
    /// <summary>
    ///     The in memory aspect configuration.
    /// </summary>
    public class InMemoryAspectConfigurationProvider : IAspectConfigurationProvider
    {
        /// <inheritdoc />
        public List<AspectConfiguration> ConfigurationEntries { get; } = new List<AspectConfiguration>();

        /// <inheritdoc />
        public void AddEntry(AspectConfiguration aspectConfiguration)
        {
            if (aspectConfiguration == null) throw new ArgumentNullException(nameof(aspectConfiguration));

            if (ConfigurationEntries.Contains(aspectConfiguration))
                ConfigurationEntries.Remove(aspectConfiguration);

            ConfigurationEntries.Add(aspectConfiguration);
        }

        /// <inheritdoc />
        public AspectConfiguration GetTypeAspectConfiguration(Type contractType, Type implementationType)
        {
            if (contractType == null) throw new ArgumentNullException(nameof(contractType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            return ConfigurationEntries.Find(
                x => x.ServiceDescriptor.ServiceType == contractType && x.ServiceDescriptor.ImplementationType == implementationType
                     || x.ServiceDescriptor.ServiceType == contractType && x.ServiceDescriptor.ImplementationFactory != null);
        }

        /// <inheritdoc />
        public void LoadConfiguration()
        {
            // Everything already loaded in memory
        }

        /// <inheritdoc />
        public bool ShouldIntercept(Type factoryType, Type serviceType, Type implementationType, MethodInfo methodInfo)
        {
            if (factoryType == null) throw new ArgumentNullException(nameof(factoryType));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            var aspectConfiguration = GetTypeAspectConfiguration(serviceType, implementationType);

            if (aspectConfiguration == null)
                return false;

            return aspectConfiguration.GetAspects().Any(x => x.AspectFactoryType == factoryType && (!x.GetMethodsToIntercept().Any() || x.GetMethodsToIntercept().Contains(methodInfo)));
        }
    }
}