using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Internal;
using JamesConsulting.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// Living under the canonical MEDI namespace so consumers see AddAspectSupport without an extra using.
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>Service-collection entry points for wiring AspectCentral into a host's DI container.</summary>
public static class AspectCentralServiceCollectionExtensions
{
    /// <summary>
    /// Wires AspectCentral into <paramref name="serviceCollection" /> using the supplied concrete
    /// <see cref="IAspectRegistrationBuilder" /> implementation. Discovers and registers every
    /// <see cref="AspectAttribute" />-decorated type in the current <see cref="AppDomain" /> as a singleton,
    /// then constructs the builder, registers it and the configuration provider, and returns the builder.
    /// </summary>
    /// <param name="serviceCollection">The DI container being configured.</param>
    /// <param name="aspectRegistrationBuilderType">A concrete type implementing <see cref="IAspectRegistrationBuilder" />.</param>
    /// <param name="aspectConfigurationProvider">Optional provider; defaults to <see cref="InMemoryAspectConfigurationProvider" />.</param>
    /// <returns>The constructed <see cref="IAspectRegistrationBuilder" />, also registered as a singleton.</returns>
    /// <exception cref="ArgumentNullException">Either required argument is <c>null</c>.</exception>
    /// <exception cref="AspectException">
    /// Thrown with <see cref="AspectErrorCodes.InvalidRegistrationBuilderType" /> when
    /// <paramref name="aspectRegistrationBuilderType" /> does not implement <see cref="IAspectRegistrationBuilder" />.
    /// </exception>
    public static IAspectRegistrationBuilder AddAspectSupport(this IServiceCollection serviceCollection,
        Type aspectRegistrationBuilderType, IAspectConfigurationProvider? aspectConfigurationProvider = null)
    {
        Guard.NotNull(serviceCollection);
        Guard.NotNull(aspectRegistrationBuilderType);

        if (!typeof(IAspectRegistrationBuilder).IsAssignableFrom(aspectRegistrationBuilderType))
            throw new AspectException(AspectErrorCodes.InvalidRegistrationBuilderType,
                $"Parameter {nameof(aspectRegistrationBuilderType)} must implement {typeof(IAspectRegistrationBuilder)}");

        aspectConfigurationProvider ??= new InMemoryAspectConfigurationProvider();

        var builder = (IAspectRegistrationBuilder)Activator.CreateInstance(aspectRegistrationBuilderType,
            serviceCollection.RegisterAspects(),
            aspectConfigurationProvider)!;
        serviceCollection.TryAddSingleton(aspectConfigurationProvider);
        serviceCollection.TryAddSingleton(builder);
        return builder;
    }

    private static IServiceCollection RegisterAspects(this IServiceCollection serviceCollection)
    {
        var types = LoadedTypes()
            .Where(type => type.IsConcreteClass()
                           && type.GetCustomAttribute(typeof(AspectAttribute), true) != null);

        foreach (var type in types) serviceCollection.TryAddSingleton(type);

        return serviceCollection;
    }

    private static IEnumerable<Type> LoadedTypes()
    {
        // Per-assembly try/catch: a single ReflectionTypeLoadException in one assembly used to
        // discard every type discovered so far via SelectMany's lazy enumeration. Iterate
        // assemblies explicitly, collect what we can get, and on a partial-load failure salvage
        // the non-null Types reported by the exception.
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var loaded = new List<Type>();
        foreach (var assembly in assemblies)
        {
            Type?[] assemblyTypes;
            try
            {
                assemblyTypes = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                assemblyTypes = ex.Types;
            }

            foreach (var t in assemblyTypes)
                if (t is not null) loaded.Add(t);
        }

        return loaded;
    }
}
