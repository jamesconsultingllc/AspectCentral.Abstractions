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
        /// Registers the aspect of Type {type:T}
        /// </summary>
        /// <param name="aspectRegistrationBuilder"></param>
        /// <param name="sortOrder"></param>
        /// <param name="methodsToIntercept"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IAspectRegistrationBuilder AddAspect<T>(this IAspectRegistrationBuilder aspectRegistrationBuilder, int? sortOrder = null, params MethodInfo[] methodsToIntercept) where T : IAspect
        {
            if (aspectRegistrationBuilder == null) throw new ArgumentNullException(nameof(aspectRegistrationBuilder));
            return aspectRegistrationBuilder.AddAspect(typeof(T), sortOrder, methodsToIntercept);
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