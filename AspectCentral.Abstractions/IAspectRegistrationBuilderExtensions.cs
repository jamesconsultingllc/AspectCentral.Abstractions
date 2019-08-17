//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IAspectRegistrationBuilderExtensions.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions
{
    /// <summary>
    ///     The extension methods for <see cref="IAspectRegistrationBuilder"/> 
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IAspectRegistrationBuilderExtensions
    {
        /// <summary>
        ///     The add aspect.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        ///     The aspect registration builder.
        /// </param>
        /// <param name="sortOrder">
        ///     The sort order.
        /// </param>
        /// <param name="methodsToIntercept">
        ///     The methods to intercept.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IAspectRegistrationBuilder AddAspect<T>(this IAspectRegistrationBuilder aspectRegistrationBuilder, int? sortOrder = null, params MethodInfo[] methodsToIntercept)
            where T : IAspectFactory
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            aspectRegistrationBuilder.AddAspect(typeof(T), sortOrder, methodsToIntercept);
            return aspectRegistrationBuilder;
        }

        /// <summary>
        ///     The add scoped.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        ///     The aspect registration builder.
        /// </param>
        /// <typeparam name="TService">
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IAspectRegistrationBuilder AddScoped<TService, TImplementation>(this IAspectRegistrationBuilder aspectRegistrationBuilder) where TImplementation : TService
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            aspectRegistrationBuilder.AddService(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
            return aspectRegistrationBuilder;
        }

        /// <summary>
        ///     The add scoped.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        ///     The aspect registration builder.
        /// </param>
        /// <param name="factory">
        ///     The factory.
        /// </param>
        /// <typeparam name="TService">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IAspectRegistrationBuilder AddScoped<TService>(this IAspectRegistrationBuilder aspectRegistrationBuilder, Func<IServiceProvider, object> factory)
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            aspectRegistrationBuilder.AddService(typeof(TService), factory, ServiceLifetime.Scoped);
            return aspectRegistrationBuilder;
        }

        /// <summary>
        ///     The add singleton.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        ///     The aspect registration builder.
        /// </param>
        /// <typeparam name="TService">
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IAspectRegistrationBuilder AddSingleton<TService, TImplementation>(this IAspectRegistrationBuilder aspectRegistrationBuilder) where TImplementation : TService
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            aspectRegistrationBuilder.AddService(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
            return aspectRegistrationBuilder;
        }

        /// <summary>
        ///     The add singleton.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        ///     The aspect registration builder.
        /// </param>
        /// <param name="factory">
        ///     The factory.
        /// </param>
        /// <typeparam name="TService">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IAspectRegistrationBuilder AddSingleton<TService>(this IAspectRegistrationBuilder aspectRegistrationBuilder, Func<IServiceProvider, object> factory)
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            aspectRegistrationBuilder.AddService(typeof(TService), factory, ServiceLifetime.Singleton);
            return aspectRegistrationBuilder;
        }

        /// <summary>
        ///     The add transient.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        ///     The aspect registration builder.
        /// </param>
        /// <typeparam name="TService">
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IAspectRegistrationBuilder AddTransient<TService, TImplementation>(this IAspectRegistrationBuilder aspectRegistrationBuilder) where TImplementation : TService
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            aspectRegistrationBuilder.AddService(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
            return aspectRegistrationBuilder;
        }

        /// <summary>
        ///     The add transient.
        /// </summary>
        /// <param name="aspectRegistrationBuilder">
        ///     The aspect registration builder.
        /// </param>
        /// <param name="factory">
        ///     The factory.
        /// </param>
        /// <typeparam name="TService">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IAspectRegistrationBuilder" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static IAspectRegistrationBuilder AddTransient<TService>(this IAspectRegistrationBuilder aspectRegistrationBuilder, Func<IServiceProvider, object> factory)
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            aspectRegistrationBuilder.AddService(typeof(TService), factory, ServiceLifetime.Transient);
            return aspectRegistrationBuilder;
        }
    }
}