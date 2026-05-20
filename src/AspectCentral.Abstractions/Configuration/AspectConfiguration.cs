using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Configuration;

/// <summary>
/// Aggregates a single service registration together with the ordered set of aspects that should wrap
/// it. Built up by <see cref="IAspectRegistrationBuilder" /> implementations and queried by the runtime
/// proxy factories.
/// </summary>
public sealed class AspectConfiguration : IEquatable<AspectConfiguration?>
{
    private readonly List<AspectConfigurationEntry> aspectConfigurationEntries = new();

    /// <summary>Initializes a new instance of the <see cref="AspectConfiguration" /> class.</summary>
    /// <param name="serviceDescriptor">The service descriptor being configured. The service type must be an interface.</param>
    /// <exception cref="ArgumentNullException"><paramref name="serviceDescriptor" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="serviceDescriptor" /> is registered against a non-interface service type.</exception>
    public AspectConfiguration(ServiceDescriptor serviceDescriptor)
    {
        Guard.NotNull(serviceDescriptor);
        if (!serviceDescriptor.ServiceType.IsInterface)
            throw new ArgumentException("The ServiceType property must be an interface", nameof(serviceDescriptor));

        ServiceDescriptor = serviceDescriptor;
    }

    /// <summary>Gets the underlying service descriptor that this configuration extends.</summary>
    public ServiceDescriptor ServiceDescriptor { get; }

    /// <inheritdoc />
    public bool Equals(AspectConfiguration? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return ServiceDescriptor.ServiceType == other.ServiceDescriptor.ServiceType
               && ServiceDescriptor.ImplementationType == other.ServiceDescriptor.ImplementationType
               && ServiceDescriptor.ImplementationFactory == other.ServiceDescriptor.ImplementationFactory
               && ServiceDescriptor.ImplementationInstance == other.ServiceDescriptor.ImplementationInstance
               && ServiceDescriptor.Lifetime == other.ServiceDescriptor.Lifetime;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as AspectConfiguration);

    /// <summary>Equality operator.</summary>
    public static bool operator ==(AspectConfiguration? left, AspectConfiguration? right) => Equals(left, right);

    /// <summary>Inequality operator.</summary>
    public static bool operator !=(AspectConfiguration? left, AspectConfiguration? right) => !Equals(left, right);

    /// <summary>Adds or extends an aspect entry on this configuration.</summary>
    /// <param name="aspectFactoryType">The concrete aspect type.</param>
    /// <param name="sortOrder">Optional sort order. When omitted, computed as max + 1 over existing entries (or 1 if none).</param>
    /// <param name="methodsToIntercept">Methods to intercept. When null/empty, every method on the service interface is intercepted.</param>
    /// <exception cref="ArgumentNullException"><paramref name="aspectFactoryType" /> is <c>null</c>.</exception>
    public void AddEntry(Type aspectFactoryType, int? sortOrder = null, params MethodInfo?[]? methodsToIntercept)
    {
        Guard.NotNull(aspectFactoryType);

        if (!sortOrder.HasValue)
        {
            sortOrder = aspectConfigurationEntries.Count == 0
                ? 1
                : aspectConfigurationEntries.Max(x => x.SortOrder) + 1;
        }

        var aspectConfigurationEntry = aspectConfigurationEntries.Find(x => x.AspectType == aspectFactoryType);

        var resolvedMethodsToIntercept = methodsToIntercept is null || methodsToIntercept.Length == 0
            ? ServiceDescriptor.ServiceType.GetMethods()
            : methodsToIntercept.Where(static x => x is not null).Select(static x => x!).ToArray();

        // Methods must belong to the service interface — interception matches by MethodInfo identity
        // and DispatchProxy dispatches off the interface, so an implementation MethodInfo would never
        // match at runtime and the aspect would silently never apply.
        foreach (var m in resolvedMethodsToIntercept)
        {
            if (m.DeclaringType is null || !m.DeclaringType.IsAssignableFrom(ServiceDescriptor.ServiceType))
                throw new AspectException(AspectErrorCodes.InvalidServiceRegistration,
                    $"Method '{m.Name}' is declared on '{m.DeclaringType?.FullName ?? "<unknown>"}', which is not assignable from the service type '{ServiceDescriptor.ServiceType.FullName}'. Pass interface MethodInfo values (e.g. typeof(IFoo).GetMethod(\"Bar\")).");
        }

        if (aspectConfigurationEntry is null)
            aspectConfigurationEntries.Add(new AspectConfigurationEntry(aspectFactoryType, sortOrder.Value,
                resolvedMethodsToIntercept));
        else
            aspectConfigurationEntry.AddMethodsToIntercept(resolvedMethodsToIntercept);
    }

    /// <summary>Returns aspects attached to this configuration ordered by descending <see cref="AspectConfigurationEntry.SortOrder" />.</summary>
    public IOrderedEnumerable<AspectConfigurationEntry> GetAspects() =>
        aspectConfigurationEntries.OrderByDescending(x => x.SortOrder);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 31 + (ServiceDescriptor.ServiceType?.GetHashCode() ?? 0);
            hash = hash * 31 + (ServiceDescriptor.ImplementationType?.GetHashCode() ?? 0);
            hash = hash * 31 + (ServiceDescriptor.ImplementationFactory?.GetHashCode() ?? 0);
            hash = hash * 31 + (ServiceDescriptor.ImplementationInstance?.GetHashCode() ?? 0);
            hash = hash * 31 + (int)ServiceDescriptor.Lifetime;
            return hash;
        }
    }

    /// <summary>Indicates whether <paramref name="methodInfo" /> should be intercepted by aspect <paramref name="factoryType" />.</summary>
    /// <param name="factoryType">The aspect type.</param>
    /// <param name="methodInfo">The method being invoked.</param>
    /// <returns><c>true</c> when an entry matching both exists; otherwise <c>false</c>.</returns>
    public bool ShouldIntercept(Type factoryType, MethodInfo methodInfo)
    {
        return aspectConfigurationEntries.Any(x =>
            x.AspectType == factoryType && x.ContainsMethod(methodInfo));
    }
}
