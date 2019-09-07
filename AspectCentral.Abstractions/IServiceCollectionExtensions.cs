//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IServiceCollectionExtensions.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspectCentral.Abstractions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="aspectConfigurationProvider"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IAspectRegistrationBuilder AddAspectSupport<T>(this IServiceCollection serviceCollection, IAspectConfigurationProvider aspectConfigurationProvider = null) 
            where T : IAspectRegistrationBuilder
        {
            return AddAspectSupport(serviceCollection, typeof(T), aspectConfigurationProvider);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="type"></param>
        /// <param name="aspectConfigurationProvider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IAspectRegistrationBuilder AddAspectSupport(this IServiceCollection serviceCollection, Type type,
            IAspectConfigurationProvider aspectConfigurationProvider = null)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (!typeof(IAspectRegistrationBuilder).IsAssignableFrom(type)) throw  new ArgumentException(nameof(type), $"Parameter {nameof(type)} must implement {typeof(IAspectRegistrationBuilder)}");

            aspectConfigurationProvider = aspectConfigurationProvider ?? new InMemoryAspectConfigurationProvider();
            
            serviceCollection.TryAddSingleton(aspectConfigurationProvider);
            var builder = (IAspectRegistrationBuilder) Activator.CreateInstance(type, RegisterAspects(serviceCollection, type),
                aspectConfigurationProvider);
            serviceCollection.TryAddSingleton<IAspectRegistrationBuilder>(builder);
            return builder;
        }
        
        /// <summary>
        ///     The register aspect factories.
        /// </summary>
        /// <param name="serviceCollection">
        ///     The service collection.
        /// </param>
        /// <returns>
        ///     The <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection RegisterAspects<T>(this IServiceCollection serviceCollection)
        {
            return RegisterAspects(serviceCollection, typeof(T));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection RegisterAspects(this IServiceCollection serviceCollection, Type baseType)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            
            var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where !type.IsAbstract && !type.IsInterface &&
                      baseType.IsAssignableFrom(type)
                select type;

            foreach (var type in types) serviceCollection.TryAddSingleton(type);

            return serviceCollection;
        }
        
        /// <summary>
        ///     The configure aspects.
        /// </summary>
        /// <param name="serviceCollection">
        ///     The service collection.
        /// </param>
        /// <param name="aspectConfigurationProvider">
        ///     The aspect configuration provider.
        /// </param>
        /// <returns>
        ///     The <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection ConfigureAspects(this IServiceCollection serviceCollection,
            IAspectConfigurationProvider aspectConfigurationProvider)
        {
            for (var index = 0; index < serviceCollection.Count; index++)
            {
                var service = serviceCollection[index];

                if (service.ServiceType.IsInterface && service.ImplementationType != null)
                {
                    var aspectConfiguration =
                        aspectConfigurationProvider.GetTypeAspectConfiguration(service.ServiceType,
                            service.ImplementationType);

                    if (aspectConfiguration == null) continue;

                    serviceCollection.TryAdd(ServiceDescriptor.Describe(service.ImplementationType, service.ImplementationType, service.Lifetime));
                    serviceCollection[index] = new ServiceDescriptor(service.ServiceType,
                        serviceProvider => serviceProvider.GetService<IAspectRegistrationBuilder>().InvokeCreateFactory(serviceProvider, aspectConfiguration), service.Lifetime);
                }
            }

            return serviceCollection;
        }
    }
}