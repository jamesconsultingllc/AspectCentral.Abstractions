using System;
using System.Collections.Generic;
using System.Reflection;
using AspectCentral.Abstractions.Internal;

namespace AspectCentral.Abstractions.Configuration;

/// <summary>
/// In-memory implementation of <see cref="IAspectConfigurationProvider" />. Intended for tests and for
/// simple host configurations where aspect wiring is expressed in code rather than persisted.
/// </summary>
public class InMemoryAspectConfigurationProvider : IAspectConfigurationProvider
{
    /// <inheritdoc />
    public List<AspectConfiguration> ConfigurationEntries { get; } = new();

    /// <inheritdoc />
    public void AddEntry(AspectConfiguration aspectConfiguration)
    {
        Guard.NotNull(aspectConfiguration);

        if (ConfigurationEntries.Contains(aspectConfiguration))
            ConfigurationEntries.Remove(aspectConfiguration);

        ConfigurationEntries.Add(aspectConfiguration);
    }

    /// <inheritdoc />
    public AspectConfiguration? GetTypeAspectConfiguration(Type contractType, Type implementationType)
    {
        Guard.NotNull(contractType);
        Guard.NotNull(implementationType);
        return ConfigurationEntries.Find(
            x => x.ServiceDescriptor.ServiceType == contractType &&
                 x.ServiceDescriptor.ImplementationType == implementationType
                 || x.ServiceDescriptor.ServiceType == contractType &&
                 x.ServiceDescriptor.ImplementationFactory != null);
    }

    /// <summary>
    /// The in-memory provider has no backing store and therefore does not support loading. Always throws
    /// <see cref="NotImplementedException" />.
    /// </summary>
    /// <exception cref="NotImplementedException">Always.</exception>
    public void LoadConfiguration() => throw new NotImplementedException();

    /// <inheritdoc />
    public bool ShouldIntercept(Type factoryType, Type serviceType, Type implementationType, MethodInfo methodInfo)
    {
        Guard.NotNull(factoryType);
        Guard.NotNull(serviceType);
        Guard.NotNull(implementationType);
        Guard.NotNull(methodInfo);

        var aspectConfiguration = GetTypeAspectConfiguration(serviceType, implementationType);

        return aspectConfiguration != null && aspectConfiguration.ShouldIntercept(factoryType, methodInfo);
    }
}
