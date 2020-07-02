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
using JamesConsulting.Reflection;

namespace AspectCentral.Abstractions.Configuration
{
    /// <summary>
    ///     The aspect configuration.
    /// </summary>
    public class AspectConfigurationEntry : IEqualityComparer<AspectConfigurationEntry?>
    {
        /// <summary>
        ///     Gets or sets the methods to intercept.
        /// </summary>
        private List<MethodInfo> methodsToIntercept;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AspectConfigurationEntry" /> class.
        /// </summary>
        /// <param name="aspectType">
        ///     The aspect factory type.
        /// </param>
        /// <param name="sortOrder">
        ///     The sort Order.
        /// </param>
        /// <param name="methodsToIntercept">
        ///     The methods To Intercept.
        /// </param>
        internal AspectConfigurationEntry(Type aspectType, int sortOrder, params MethodInfo[] methodsToIntercept)
        {
            AspectType = aspectType ?? throw new ArgumentNullException(nameof(aspectType));

            if (!aspectType.IsConcreteClass())
                throw new ArgumentException("Type must be a concrete class", nameof(aspectType));

            SortOrder = sortOrder;
            this.methodsToIntercept = methodsToIntercept == null
                ? new List<MethodInfo>()
                : new List<MethodInfo>(methodsToIntercept);
        }

        /// <summary>
        ///     Gets or sets the aspect type.
        /// </summary>
        public Type AspectType { get; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        public int SortOrder { get; }

        public virtual bool Equals(AspectConfigurationEntry? x, AspectConfigurationEntry? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.AspectType == y.AspectType;
        }

        public virtual int GetHashCode(AspectConfigurationEntry? obj)
        {
            return obj?.AspectType != null ? obj.AspectType.GetHashCode() : 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(AspectConfigurationEntry? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return AspectType == other.AspectType;
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
            if (ReferenceEquals(left, right)) return true;
            if (ReferenceEquals(left, null)) return false;
            return !ReferenceEquals(right, null) && left.Equals(right);
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
            return !(left == right);
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
            if (newMethodsToIntercept.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(newMethodsToIntercept));

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
            return GetHashCode(this);
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
            methodsToIntercept.RemoveAll(methodsToBeRemoved.Contains!);
        }
    }
}