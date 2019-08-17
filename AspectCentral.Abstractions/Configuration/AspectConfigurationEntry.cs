//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectConfigurationEntry.cs" company="James Consulting LLC">
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

namespace AspectCentral.Abstractions.Configuration
{
    /// <summary>
    ///     The aspect configuration.
    /// </summary>
    public class AspectConfigurationEntry : IEquatable<AspectConfigurationEntry>
    {
        /// <summary>
        ///     Gets or sets the methods to intercept.
        /// </summary>
        private List<MethodInfo> methodsToIntercept;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AspectConfigurationEntry" /> class.
        /// </summary>
        /// <param name="aspectFactoryType">
        ///     The aspect factory type.
        /// </param>
        /// <param name="sortOrder">
        ///     The sort Order.
        /// </param>
        /// <param name="methodsToIntercept">
        ///     The methods To Intercept.
        /// </param>
        internal AspectConfigurationEntry(Type aspectFactoryType, int sortOrder, params MethodInfo[] methodsToIntercept)
        {
            AspectFactoryType = aspectFactoryType ?? throw new ArgumentNullException(nameof(aspectFactoryType));

            if (aspectFactoryType.IsInterface || aspectFactoryType.IsAbstract || !Constants.InterfaceIAspectFactoryType.IsAssignableFrom(aspectFactoryType))
                throw new ArgumentException($"Type must implement {nameof(IAspectFactory)}", nameof(aspectFactoryType));

            SortOrder = sortOrder;
            this.methodsToIntercept = methodsToIntercept == null ? new List<MethodInfo>() : new List<MethodInfo>(methodsToIntercept);
        }

        /// <summary>
        ///     Gets or sets the aspect factory type.
        /// </summary>
        public Type AspectFactoryType { get; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        public int SortOrder { get; }

        /// <inheritdoc />
        public bool Equals(AspectConfigurationEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return AspectFactoryType == other.AspectFactoryType;
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
        public static bool operator ==(AspectConfigurationEntry left, AspectConfigurationEntry right)
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
        public static bool operator !=(AspectConfigurationEntry left, AspectConfigurationEntry right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     The add methods to intercept.
        /// </summary>
        /// <param name="newMethodsToIntercept">
        ///     The methods to intercept.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public void AddMethodsToIntercept(params MethodInfo[] newMethodsToIntercept)
        {
            if (newMethodsToIntercept == null) throw new ArgumentNullException(nameof(newMethodsToIntercept));
            if (newMethodsToIntercept.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(newMethodsToIntercept));

            methodsToIntercept = methodsToIntercept.Union(newMethodsToIntercept).ToList();
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
            return Equals(obj as AspectConfigurationEntry);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return AspectFactoryType.GetHashCode();
        }

        /// <summary>
        ///     The get methods to intercept.
        /// </summary>
        /// <returns>
        ///     The <see cref="List{MethodInfo}" />.
        /// </returns>
        public List<MethodInfo> GetMethodsToIntercept()
        {
            return methodsToIntercept.ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="methodsToBeRemoved">
        /// </param>
        public void RemoveMethodsToIntercept(params MethodInfo[] methodsToBeRemoved)
        {
            if (methodsToBeRemoved == null || methodsToBeRemoved.Length == 0) return;
            methodsToIntercept.RemoveAll(methodsToBeRemoved.Contains);
        }
    }
}