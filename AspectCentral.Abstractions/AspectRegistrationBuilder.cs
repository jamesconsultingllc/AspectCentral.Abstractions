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
        public AspectRegistrationBuilder(IServiceCollection services,
            IAspectConfigurationProvider aspectConfigurationProvider)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            AspectConfigurationProvider = aspectConfigurationProvider ??
                                          throw new ArgumentNullException(nameof(aspectConfigurationProvider));
        }

        /// <inheritdoc />
        public IAspectConfigurationProvider AspectConfigurationProvider { get; }

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        /// <inheritdoc />
        public IAspectRegistrationBuilder AddAspect(Type aspectType, int? sortOrder = null, params MethodInfo[] methodsToIntercept)
        {
            if (aspectType == null) throw new ArgumentNullException(nameof(aspectType));
            if (!aspectType.IsConcreteClass())
                throw new ArgumentException(
                    $"The {nameof(aspectType)} must be a concrete class",
                    nameof(aspectType));

            if (AspectConfigurationProvider.ConfigurationEntries.Count == 0)
                throw new InvalidOperationException("A service must be registered to apply an aspect to.");
            AspectConfigurationProvider.ConfigurationEntries.Last()
                .AddEntry(aspectType, sortOrder, methodsToIntercept);
            return this;
        }

        /// <inheritdoc />
        public abstract IAspectRegistrationBuilder AddService(Type service, Type implementation, ServiceLifetime serviceLifetime);

        /// <inheritdoc />
        public abstract IAspectRegistrationBuilder AddService(Type service, Func<IServiceProvider, object> factory, ServiceLifetime serviceLifetime);
    }
}