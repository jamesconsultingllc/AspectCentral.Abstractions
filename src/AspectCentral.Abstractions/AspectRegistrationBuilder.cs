using System;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Internal;
using JamesConsulting.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspectCentral.Abstractions;

/// <summary>
/// Base class for concrete <see cref="IAspectRegistrationBuilder" /> implementations (DispatchProxy,
/// Castle DynamicProxy, etc.). Provides shared book-keeping: service registration, aspect attachment,
/// and configuration-provider integration. Implementors only need to supply
/// <see cref="InvokeCreateFactory" />.
/// </summary>
/// <remarks>
/// <para>
/// <b>Registration on <see cref="Services" /> is <i>replace</i>, not <i>add</i>.</b> Both
/// <see cref="AddService(Type, Type, ServiceLifetime)" /> overloads remove any prior descriptor
/// for the same service type from <see cref="Services" /> before adding the aspect-wrapped one,
/// so a second <c>AddService</c> call for the same service interface fully supersedes the first
/// at resolve time. AspectCentral does not support multi-registration
/// (<c>IEnumerable&lt;TService&gt;</c>) of the same interface with different aspects; if that
/// pattern is needed, register each implementation under its own interface and combine at the
/// call site.
/// </para>
/// <para>
/// The <see cref="AspectConfigurationProvider" /> uses a narrower key —
/// <c>(ServiceType, ImplementationType)</c>. For factory-based registrations
/// <see cref="Microsoft.Extensions.DependencyInjection.ServiceDescriptor.ImplementationType" />
/// is <c>null</c>, so factory entries are effectively keyed by <c>(ServiceType, null)</c> —
/// at most one factory entry per service type. Type-based and factory-based entries for the
/// same service type can therefore coexist in the provider; only the latest descriptor on
/// <see cref="Services" /> is resolved at runtime, and the latest call's
/// <see cref="AspectConfiguration" /> is the one the factory closes over.
/// </para>
/// <para>
/// Because of this, consumers should treat <c>AddAspectSupport</c>/<see cref="AddService(Type, Type, ServiceLifetime)" />
/// as the canonical registration entry point for services that participate in aspect interception —
/// not as an addition to a pre-existing manual registration.
/// </para>
/// </remarks>
public abstract class AspectRegistrationBuilder : IAspectRegistrationBuilder
{
    /// <summary>Initializes a new instance of the <see cref="AspectRegistrationBuilder" /> class.</summary>
    /// <param name="services">The underlying <see cref="IServiceCollection" /> being populated.</param>
    /// <param name="aspectConfigurationProvider">Storage for the aspect configuration this builder produces.</param>
    /// <exception cref="ArgumentNullException">Either argument is <c>null</c>.</exception>
    protected AspectRegistrationBuilder(IServiceCollection services,
        IAspectConfigurationProvider aspectConfigurationProvider)
    {
        Guard.NotNull(services);
        Guard.NotNull(aspectConfigurationProvider);

        Services = services;
        AspectConfigurationProvider = aspectConfigurationProvider;

        aspectConfigurationProvider.ConfigurationEntries.ForEach(RegisterAspectConfiguration);
    }

    /// <inheritdoc />
    public IAspectConfigurationProvider AspectConfigurationProvider { get; }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public IAspectRegistrationBuilder AddAspect(Type aspectType, int? sortOrder = null,
        params MethodInfo[] methodsToIntercept)
    {
        ValidateAddAspect(aspectType);

        if (AspectConfigurationProvider.ConfigurationEntries.Count == 0)
            throw new AspectException(AspectErrorCodes.NoServiceRegisteredForAspect,
                "A service must be registered before an aspect can be attached. " +
                "Call AddScoped/AddTransient/AddSingleton before AddAspect.");

        AspectConfigurationProvider.ConfigurationEntries.Last()
            .AddEntry(aspectType, sortOrder, methodsToIntercept);
        return this;
    }

    /// <inheritdoc />
    public IAspectRegistrationBuilder AddService(Type service, Type implementation,
        ServiceLifetime serviceLifetime)
    {
        Guard.NotNull(service);
        Guard.NotNull(implementation);

        if (!implementation.IsConcreteClass() || !service.IsAssignableFrom(implementation))
            throw new AspectException(AspectErrorCodes.InvalidServiceRegistration,
                $"The {nameof(implementation)} ({implementation.FullName}) must be a concrete class that implements the {nameof(service)} ({service.Name})");

        var aspectConfiguration =
            new AspectConfiguration(ServiceDescriptor.Describe(service, implementation, serviceLifetime));
        RegisterAspectConfiguration(aspectConfiguration);
        AspectConfigurationProvider.AddEntry(aspectConfiguration);
        return this;
    }

    /// <inheritdoc />
    public IAspectRegistrationBuilder AddService(Type service, Func<IServiceProvider, object> factory,
        ServiceLifetime serviceLifetime)
    {
        Guard.NotNull(service);
        Guard.NotNull(factory);

        var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(service, factory, serviceLifetime));
        AspectConfigurationProvider.AddEntry(aspectConfiguration);
        // Replace, don't append: InMemoryAspectConfigurationProvider.AddEntry dedups by
        // (ServiceType, ImplementationType), so a second AddService for the same service must
        // not leave the prior ServiceDescriptor behind on the IServiceCollection — that would
        // surface unexpectedly via IEnumerable<T> resolution and bind a factory closed over a
        // stale AspectConfiguration.
        Services.RemoveAll(service);
        Services.Add(new ServiceDescriptor(service,
            serviceProvider => InvokeCreateFactory(serviceProvider, aspectConfiguration), serviceLifetime));
        return this;
    }

    /// <inheritdoc />
    public abstract object InvokeCreateFactory(IServiceProvider serviceProvider,
        AspectConfiguration aspectConfiguration);

    private void RegisterAspectConfiguration(AspectConfiguration aspectConfiguration)
    {
        if (aspectConfiguration.ServiceDescriptor.ImplementationType != null)
            Services.TryAdd(ServiceDescriptor.Describe(aspectConfiguration.ServiceDescriptor.ImplementationType,
                aspectConfiguration.ServiceDescriptor.ImplementationType,
                aspectConfiguration.ServiceDescriptor.Lifetime));

        // Replace, don't append: keep IServiceCollection in lock-step with
        // InMemoryAspectConfigurationProvider.AddEntry, which dedups by (ServiceType,
        // ImplementationType). Without this RemoveAll, registering the same service twice
        // would leave a stale ServiceDescriptor on the collection that resolves via a
        // factory closed over the previous AspectConfiguration.
        Services.RemoveAll(aspectConfiguration.ServiceDescriptor.ServiceType);
        Services.Add(ServiceDescriptor.Describe(aspectConfiguration.ServiceDescriptor.ServiceType,
            serviceProvider => InvokeCreateFactory(serviceProvider, aspectConfiguration),
            aspectConfiguration.ServiceDescriptor.Lifetime));
    }

    /// <summary>Validates that <paramref name="aspectType" /> is non-null and a concrete class.</summary>
    /// <param name="aspectType">The aspect type to validate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="aspectType" /> is <c>null</c>.</exception>
    /// <exception cref="AspectException"><paramref name="aspectType" /> is not a concrete class (<see cref="AspectErrorCodes.InvalidAspectType" />).</exception>
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void ValidateAddAspect(Type aspectType)
    {
        Guard.NotNull(aspectType);
        if (!aspectType.IsConcreteClass())
            throw new AspectException(AspectErrorCodes.InvalidAspectType,
                $"The {nameof(aspectType)} ({aspectType.FullName}) must be a concrete class.");
    }
}
