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

        // Dedup by the ServiceDescriptor's ServiceType (and ImplementationType when present) rather
        // than by AspectConfiguration.Equals, because Equals compares ImplementationFactory by
        // delegate-reference identity. Two factory-based registrations for the same service with
        // different Func instances would otherwise accumulate instead of replacing.
        var serviceType = aspectConfiguration.ServiceDescriptor.ServiceType;
        var implementationType = aspectConfiguration.ServiceDescriptor.ImplementationType;
        ConfigurationEntries.RemoveAll(existing =>
            existing.ServiceDescriptor.ServiceType == serviceType
            && existing.ServiceDescriptor.ImplementationType == implementationType);

        ConfigurationEntries.Add(aspectConfiguration);
    }

    /// <inheritdoc />
    public AspectConfiguration? GetTypeAspectConfiguration(Type contractType, Type implementationType)
    {
        Guard.NotNull(contractType);
        Guard.NotNull(implementationType);

        // Prefer an exact (service, implementation) match. Only when no exact match exists do we
        // fall back to a factory-based registration for the same service type. A single Find with
        // an "OR" predicate would otherwise return whichever entry appears first in the list, not
        // the more specific one.
        var exactMatch = ConfigurationEntries.Find(x =>
            x.ServiceDescriptor.ServiceType == contractType
            && x.ServiceDescriptor.ImplementationType == implementationType);

        if (exactMatch is not null)
            return exactMatch;

        return ConfigurationEntries.Find(x =>
            x.ServiceDescriptor.ServiceType == contractType
            && x.ServiceDescriptor.ImplementationFactory != null);
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
