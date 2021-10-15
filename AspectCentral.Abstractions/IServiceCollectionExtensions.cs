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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using JamesConsulting.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspectCentral.Abstractions
{
    // ReSharper disable once InconsistentNaming
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="aspectRegistrationBuilderType"></param>
        /// <param name="aspectConfigurationProvider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IAspectRegistrationBuilder AddAspectSupport(this IServiceCollection serviceCollection,
            Type aspectRegistrationBuilderType, IAspectConfigurationProvider? aspectConfigurationProvider = null)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));
            if (aspectRegistrationBuilderType == null)
                throw new ArgumentNullException(nameof(aspectRegistrationBuilderType));
            if (!typeof(IAspectRegistrationBuilder).IsAssignableFrom(aspectRegistrationBuilderType))
                throw new ArgumentException(
                    $"Parameter {nameof(aspectRegistrationBuilderType)} must implement {typeof(IAspectRegistrationBuilder)}",
                    nameof(aspectRegistrationBuilderType));

            aspectConfigurationProvider ??= new InMemoryAspectConfigurationProvider();

            var builder = (IAspectRegistrationBuilder) Activator.CreateInstance(aspectRegistrationBuilderType,
                serviceCollection.RegisterAspects(),
                aspectConfigurationProvider)!;
            serviceCollection.TryAddSingleton(aspectConfigurationProvider);
            serviceCollection.TryAddSingleton(builder);
            return builder;
        }

        /// <summary>
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IServiceCollection RegisterAspects(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            var types = from type in LoadedTypes()
                where type.IsConcreteClass() && type.GetCustomAttribute(typeof(AspectAttribute), true) != null
                select type;

            foreach (var type in types) serviceCollection.TryAddSingleton(type);

            return serviceCollection;
        }

        private static IEnumerable<Type> LoadedTypes()
        {
            try
            {
                return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    select type);
            }
            catch (ReflectionTypeLoadException reflectionTypeLoadException)
            {
                return reflectionTypeLoadException.Types.Where(x => x is not null);
            }
        }
    }
}