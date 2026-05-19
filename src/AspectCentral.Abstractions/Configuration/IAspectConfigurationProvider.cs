using System;
using System.Collections.Generic;
using System.Reflection;

namespace AspectCentral.Abstractions.Configuration;

/// <summary>
/// Storage abstraction for aspect configuration. Implementations may persist entries to disk, query
/// them from a config source, or hold them in memory (see <see cref="InMemoryAspectConfigurationProvider" />).
/// </summary>
public interface IAspectConfigurationProvider
{
    /// <summary>Gets the configuration entries known to this provider.</summary>
    List<AspectConfiguration> ConfigurationEntries { get; }

    /// <summary>Adds (or replaces) a configuration entry.</summary>
    /// <param name="aspectConfiguration">The configuration to add.</param>
    void AddEntry(AspectConfiguration aspectConfiguration);

    /// <summary>Looks up the configuration matching a service / implementation pair.</summary>
    /// <param name="contractType">The service interface type.</param>
    /// <param name="implementationType">The concrete implementation type.</param>
    /// <returns>The matching configuration, or <c>null</c> if none exists.</returns>
    AspectConfiguration? GetTypeAspectConfiguration(Type contractType, Type implementationType);

    /// <summary>Loads configuration from the provider's backing store. In-memory implementations may throw.</summary>
    void LoadConfiguration();

    /// <summary>Indicates whether an invocation should be intercepted by the given aspect.</summary>
    /// <param name="factoryType">The aspect type.</param>
    /// <param name="serviceType">The service interface being invoked.</param>
    /// <param name="implementationType">The concrete implementation.</param>
    /// <param name="methodInfo">The method being invoked.</param>
    /// <returns><c>true</c> when interception applies; otherwise <c>false</c>.</returns>
    bool ShouldIntercept(Type factoryType, Type serviceType, Type implementationType, MethodInfo methodInfo);
}
