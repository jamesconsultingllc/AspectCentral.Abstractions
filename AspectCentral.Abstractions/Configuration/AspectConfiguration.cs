﻿//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectConfiguration.cs" company="James Consulting LLC">
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
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Configuration
{
    /// <summary>
    ///     The aspect configuration entry.
    /// </summary>
    public sealed class AspectConfiguration : IEquatable<AspectConfiguration?>
    {
        /// <summary>
        ///     Gets or sets the factory type with methods to intercept.
        /// </summary>
        private readonly List<AspectConfigurationEntry> aspectConfigurationEntries =
            new List<AspectConfigurationEntry>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="AspectConfiguration" /> class.
        /// </summary>
        /// <param name="serviceDescriptor">
        ///     The service Descriptor.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public AspectConfiguration(ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor == null) throw new ArgumentNullException(nameof(serviceDescriptor));
            if (!serviceDescriptor.ServiceType.IsInterface)
                throw new ArgumentException("The ServiceType property must be an interface", nameof(serviceDescriptor));

            ServiceDescriptor = serviceDescriptor;
        }

        /// <summary>
        ///     Gets the service descriptor.
        /// </summary>
        public ServiceDescriptor ServiceDescriptor { get; }

        /// <inheritdoc />
        public bool Equals(AspectConfiguration? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ServiceDescriptor.ServiceType == other.ServiceDescriptor.ServiceType && ServiceDescriptor
                                                                                            .ImplementationType ==
                                                                                        other.ServiceDescriptor
                                                                                            .ImplementationType
                                                                                        && ServiceDescriptor
                                                                                            .ImplementationFactory ==
                                                                                        other.ServiceDescriptor
                                                                                            .ImplementationFactory;
        }

        /// <summary>
        ///     The ==.
        /// </summary>
        /// <param name="left">
        ///     The left.
        /// </param>
        /// <param name="right">
        ///     The right.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator ==(AspectConfiguration left, AspectConfiguration right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     The !=.
        /// </summary>
        /// <param name="left">
        ///     The left.
        /// </param>
        /// <param name="right">
        ///     The right.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator !=(AspectConfiguration left, AspectConfiguration right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     The add entry.
        /// </summary>
        /// <param name="aspectFactoryType">
        ///     The aspect factory type.
        /// </param>
        /// <param name="sortOrder">
        ///     The sort Order.
        /// </param>
        /// <param name="methodsToIntercept">
        ///     The methods to intercept. Defaults to all methods in interface if none specified
        /// </param>
        public void AddEntry(Type aspectFactoryType, int? sortOrder = null, params MethodInfo[] methodsToIntercept)
        {
            if (aspectFactoryType == null) throw new ArgumentNullException(nameof(aspectFactoryType));

            if (!sortOrder.HasValue)
            {
                if (aspectConfigurationEntries.Count == 0)
                    sortOrder = 1;
                else
                    sortOrder = aspectConfigurationEntries.Max(x => x.SortOrder) + 1;
            }

            var aspectConfigurationEntry = aspectConfigurationEntries.Find(x => x.AspectType == aspectFactoryType);

            if (methodsToIntercept == null || methodsToIntercept.Length == 0)
                methodsToIntercept = ServiceDescriptor.ServiceType.GetMethods();
            else
                methodsToIntercept = methodsToIntercept.Where(x => x != null).ToArray();

            if (aspectConfigurationEntry == null)
                aspectConfigurationEntries.Add(new AspectConfigurationEntry(aspectFactoryType, sortOrder.Value,
                    methodsToIntercept));
            else
                aspectConfigurationEntry.AddMethodsToIntercept(methodsToIntercept);
        }

        /// <summary>
        ///     The equals.
        /// </summary>
        /// <param name="obj">
        ///     The obj.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as AspectConfiguration);
        }

        /// <summary>
        ///     The get aspects.
        /// </summary>
        /// <returns>
        ///     The <see cref="IOrderedEnumerable{AspectConfigurationEntry}" />.
        /// </returns>
        public IEnumerable<AspectConfigurationEntry> GetAspects()
        {
            return aspectConfigurationEntries.OrderByDescending(x => x.SortOrder);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (ServiceDescriptor != null ? ServiceDescriptor.GetHashCode() : 0) * 397;
            }
        }

        /// <summary>
        ///     Determines if the given method should be intercepted by the aspect
        /// </summary>
        /// <param name="factoryType"></param>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public bool ShouldIntercept(Type factoryType, MethodInfo methodInfo)
        {
            return aspectConfigurationEntries.Any(x =>
                x.AspectType == factoryType && x.GetMethodsToIntercept().Contains(methodInfo));
        }
    }
}