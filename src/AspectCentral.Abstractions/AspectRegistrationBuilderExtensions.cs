using System;
using System.Reflection;
using AspectCentral.Abstractions.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions;

/// <summary>Convenience extensions for <see cref="IAspectRegistrationBuilder" /> covering the common service-lifetime + aspect-attach patterns.</summary>
public static class AspectRegistrationBuilderExtensions
{
    /// <summary>Attaches an aspect of type <typeparamref name="T" /> to the most recently registered service.</summary>
    /// <param name="aspectRegistrationBuilder">The builder.</param>
    /// <param name="sortOrder">Optional ordinal controlling aspect application order.</param>
    /// <param name="methodsToIntercept">Optional subset of methods to intercept. If empty, every method on the service interface is intercepted.</param>
    /// <typeparam name="T">An aspect type decorated with <see cref="AspectAttribute" /> (directly or via a base type).</typeparam>
    /// <returns>The builder to support chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="aspectRegistrationBuilder" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><typeparamref name="T" /> is not decorated with <see cref="AspectAttribute" />.</exception>
    public static IAspectRegistrationBuilder AddAspect<T>(this IAspectRegistrationBuilder aspectRegistrationBuilder,
        int? sortOrder = null, params MethodInfo[] methodsToIntercept)
    {
        Guard.NotNull(aspectRegistrationBuilder);
        var type = typeof(T);
        if (type.GetCustomAttribute(typeof(AspectAttribute), true) == null)
            throw new ArgumentException(
                "The given type T must be decorated with the AspectAttribute or inherit from a type decorated with it");
        return aspectRegistrationBuilder.AddAspect(type, sortOrder, methodsToIntercept);
    }

    /// <summary>Registers <typeparamref name="TService" /> with implementation <typeparamref name="TImplementation" /> as scoped.</summary>
    /// <param name="aspectRegistrationBuilder">The builder.</param>
    /// <returns>The builder to support chaining.</returns>
    public static IAspectRegistrationBuilder AddScoped<TService, TImplementation>(
        this IAspectRegistrationBuilder aspectRegistrationBuilder) where TImplementation : TService
    {
        Guard.NotNull(aspectRegistrationBuilder);
        aspectRegistrationBuilder.AddService(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
        return aspectRegistrationBuilder;
    }

    /// <summary>Registers <typeparamref name="TService" /> with a factory as scoped.</summary>
    /// <param name="aspectRegistrationBuilder">The builder.</param>
    /// <param name="factory">A factory that produces the implementation.</param>
    /// <returns>The builder to support chaining.</returns>
    public static IAspectRegistrationBuilder AddScoped<TService>(
        this IAspectRegistrationBuilder aspectRegistrationBuilder, Func<IServiceProvider, object> factory)
    {
        Guard.NotNull(aspectRegistrationBuilder);
        Guard.NotNull(factory);
        aspectRegistrationBuilder.AddService(typeof(TService), factory, ServiceLifetime.Scoped);
        return aspectRegistrationBuilder;
    }

    /// <summary>Registers <typeparamref name="TService" /> with implementation <typeparamref name="TImplementation" /> as singleton.</summary>
    /// <param name="aspectRegistrationBuilder">The builder.</param>
    /// <returns>The builder to support chaining.</returns>
    public static IAspectRegistrationBuilder AddSingleton<TService, TImplementation>(
        this IAspectRegistrationBuilder aspectRegistrationBuilder) where TImplementation : TService
    {
        Guard.NotNull(aspectRegistrationBuilder);
        aspectRegistrationBuilder.AddService(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
        return aspectRegistrationBuilder;
    }

    /// <summary>Registers <typeparamref name="TService" /> with a factory as singleton.</summary>
    /// <param name="aspectRegistrationBuilder">The builder.</param>
    /// <param name="factory">A factory that produces the implementation.</param>
    /// <returns>The builder to support chaining.</returns>
    public static IAspectRegistrationBuilder AddSingleton<TService>(
        this IAspectRegistrationBuilder aspectRegistrationBuilder, Func<IServiceProvider, object> factory)
    {
        Guard.NotNull(aspectRegistrationBuilder);
        Guard.NotNull(factory);
        aspectRegistrationBuilder.AddService(typeof(TService), factory, ServiceLifetime.Singleton);
        return aspectRegistrationBuilder;
    }

    /// <summary>Registers <typeparamref name="TService" /> with implementation <typeparamref name="TImplementation" /> as transient.</summary>
    /// <param name="aspectRegistrationBuilder">The builder.</param>
    /// <returns>The builder to support chaining.</returns>
    public static IAspectRegistrationBuilder AddTransient<TService, TImplementation>(
        this IAspectRegistrationBuilder aspectRegistrationBuilder) where TImplementation : TService
    {
        Guard.NotNull(aspectRegistrationBuilder);
        aspectRegistrationBuilder.AddService(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
        return aspectRegistrationBuilder;
    }

    /// <summary>Registers <typeparamref name="TService" /> with a factory as transient.</summary>
    /// <param name="aspectRegistrationBuilder">The builder.</param>
    /// <param name="factory">A factory that produces the implementation.</param>
    /// <returns>The builder to support chaining.</returns>
    public static IAspectRegistrationBuilder AddTransient<TService>(
        this IAspectRegistrationBuilder aspectRegistrationBuilder, Func<IServiceProvider, object> factory)
    {
        Guard.NotNull(aspectRegistrationBuilder);
        Guard.NotNull(factory);
        aspectRegistrationBuilder.AddService(typeof(TService), factory, ServiceLifetime.Transient);
        return aspectRegistrationBuilder;
    }
}
