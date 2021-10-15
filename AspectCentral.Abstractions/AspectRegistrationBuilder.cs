//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectRegistrationBuilder.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using JamesConsulting.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspectCentral.Abstractions
{
    public abstract class AspectRegistrationBuilder : IAspectRegistrationBuilder
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AspectRegistrationBuilder" /> class.
        /// </summary>
        /// <param name="services">
        ///     The services.
        /// </param>
        /// <param name="aspectConfigurationProvider">
        ///     The aspect configuration provider.
        /// </param>
        protected AspectRegistrationBuilder(IServiceCollection services,
            IAspectConfigurationProvider aspectConfigurationProvider)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            AspectConfigurationProvider = aspectConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(aspectConfigurationProvider));

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
                throw new InvalidOperationException("A service must be registered to apply an aspect to.");
            AspectConfigurationProvider.ConfigurationEntries.Last()
                .AddEntry(aspectType, sortOrder, methodsToIntercept);
            return this;
        }

        public IAspectRegistrationBuilder AddService(Type service, Type implementation,
            ServiceLifetime serviceLifetime)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (implementation == null) throw new ArgumentNullException(nameof(implementation));
            if (!implementation.IsConcreteClass() || !service.IsAssignableFrom(implementation))
                throw new ArgumentException(
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
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(service, factory, serviceLifetime));
            AspectConfigurationProvider.AddEntry(aspectConfiguration);
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

            Services.Add(ServiceDescriptor.Describe(aspectConfiguration.ServiceDescriptor.ServiceType,
                serviceProvider => InvokeCreateFactory(serviceProvider, aspectConfiguration),
                aspectConfiguration.ServiceDescriptor.Lifetime));
        }

        /// <summary>
        ///     Validates that the type given is not null and is a concrete class
        /// </summary>
        /// <param name="aspectType">The aspect type</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="aspectType" /> is null</exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="aspectType" /> represents and interface or abstract
        ///     class
        /// </exception>
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void ValidateAddAspect(Type aspectType)
        {
            if (aspectType == null) throw new ArgumentNullException(nameof(aspectType));
            if (!aspectType.IsConcreteClass())
                throw new ArgumentException(
                    $"The {nameof(aspectType)} must be a concrete class",
                    nameof(aspectType));
        }
    }
}