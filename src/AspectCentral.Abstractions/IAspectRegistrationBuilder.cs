using System;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions;

/// <summary>
/// The contract that concrete aspect runtimes (DispatchProxy, Castle DynamicProxy, …) implement. Hosts
/// the underlying <see cref="IServiceCollection" /> and the <see cref="IAspectConfigurationProvider" />
/// describing which services should be wrapped with which aspects.
/// </summary>
public interface IAspectRegistrationBuilder
{
    /// <summary>Gets the configuration provider that stores the aspect registrations applied to this builder.</summary>
    IAspectConfigurationProvider AspectConfigurationProvider { get; }

    /// <summary>Gets the underlying service collection being populated with proxied service descriptors.</summary>
    IServiceCollection Services { get; }

    /// <summary>Attaches an aspect to the most-recently-added service.</summary>
    /// <param name="aspectType">A concrete aspect class.</param>
    /// <param name="sortOrder">Optional ordinal controlling aspect application order. If omitted, appended after existing aspects.</param>
    /// <param name="methodsToIntercept">Optional subset of methods to intercept. If empty, every method on the service interface is intercepted.</param>
    /// <returns>This builder to support chaining.</returns>
    /// <exception cref="AspectException">Thrown with <see cref="AspectErrorCodes.NoServiceRegisteredForAspect" /> when no service is registered yet.</exception>
    IAspectRegistrationBuilder AddAspect(Type aspectType, int? sortOrder = null,
        params MethodInfo[] methodsToIntercept);

    /// <summary>Registers a service with its concrete implementation.</summary>
    /// <param name="service">The service interface.</param>
    /// <param name="implementation">A concrete class implementing <paramref name="service" />.</param>
    /// <param name="serviceLifetime">The DI lifetime to register under.</param>
    /// <returns>This builder to support chaining.</returns>
    IAspectRegistrationBuilder AddService(Type service, Type implementation, ServiceLifetime serviceLifetime);

    /// <summary>Registers a service via a factory delegate.</summary>
    /// <param name="service">The service interface.</param>
    /// <param name="factory">A factory that produces the implementation instance.</param>
    /// <param name="serviceLifetime">The DI lifetime to register under.</param>
    /// <returns>This builder to support chaining.</returns>
    IAspectRegistrationBuilder AddService(Type service, Func<IServiceProvider, object> factory,
        ServiceLifetime serviceLifetime);

    /// <summary>
    /// Produces an instance of the proxied service for the given aspect configuration. Called from the
    /// service descriptor's factory at resolution time. Implementations construct their proxy here.
    /// </summary>
    /// <param name="serviceProvider">The runtime service provider used to resolve dependencies (target instance, aspects).</param>
    /// <param name="aspectConfiguration">The configuration describing the service and its attached aspects.</param>
    /// <returns>The proxied service instance.</returns>
    object InvokeCreateFactory(IServiceProvider serviceProvider, AspectConfiguration aspectConfiguration);
}
