//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IAspectRegistrationBuilder.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMember.Global

// ReSharper disable UnusedMemberInSuper.Global

namespace AspectCentral.Abstractions
{
    /// <summary>
    ///     The AspectRegistrationBuilder interface.
    /// </summary>
    public interface IAspectRegistrationBuilder
    {
        /// <summary>
        ///     Gets the aspect configuration provider.
        /// </summary>
        IAspectConfigurationProvider AspectConfigurationProvider { get; }

        /// <summary>
        ///     Gets the services.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        ///     The add aspect.
        /// </summary>
        /// <param name="aspectType">
        ///     The aspect type.
        /// </param>
        /// <param name="sortOrder">
        ///     The sort order.
        /// </param>
        /// <param name="methodsToIntercept">
        ///     The methods to intercept.
        /// </param>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        IAspectRegistrationBuilder AddAspect(Type? aspectType, int? sortOrder = null,
            params MethodInfo[] methodsToIntercept);

        /// <summary>
        ///     The add service.
        /// </summary>
        /// <param name="service">
        ///     The service.
        /// </param>
        /// <param name="implementation">
        ///     The implementation.
        /// </param>
        /// <param name="serviceLifetime">
        ///     The service lifetime.
        /// </param>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        IAspectRegistrationBuilder AddService(Type service, Type? implementation, ServiceLifetime serviceLifetime);

        /// <summary>
        ///     The add service.
        /// </summary>
        /// <param name="service">
        ///     The service.
        /// </param>
        /// <param name="factory">
        ///     The factory.
        /// </param>
        /// <param name="serviceLifetime">
        ///     The service lifetime.
        /// </param>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        IAspectRegistrationBuilder AddService(Type service, Func<IServiceProvider, object> factory,
            ServiceLifetime serviceLifetime);

        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="aspectConfiguration"></param>
        /// <returns></returns>
        object InvokeCreateFactory(IServiceProvider serviceProvider, AspectConfiguration aspectConfiguration);
    }
}