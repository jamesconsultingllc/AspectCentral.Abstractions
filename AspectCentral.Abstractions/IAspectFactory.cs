// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAspectFactory.cs" company="James Consulting LLC">
//   Copyright © 2019. All rights reserved.
// </copyright>
// <summary>
//   The AspectFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AspectCentral.Abstractions
{
    using System;

    /// <summary>
    ///     The AspectFactory interface.
    /// </summary>
    public interface IAspectFactory
    {
        /// <summary>
        /// Wraps the given object instance an aspect
        /// </summary>
        /// <param name="instance">
        /// The object that will be intercepted.
        /// </param>
        /// <param name="instanceType">
        /// The instance Type.
        /// </param>
        /// <param name="serviceProvider">
        /// The service Provider.
        /// </param>
        /// <typeparam name="T">
        /// The interface that will be wrapped by the aspect
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/> the given instance wrapped by the aspect.
        /// </returns>
        T Create<T>(T instance, Type instanceType, IServiceProvider serviceProvider);
    }
}