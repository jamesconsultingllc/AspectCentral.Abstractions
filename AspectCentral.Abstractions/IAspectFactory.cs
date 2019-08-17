//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IAspectFactory.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;

namespace AspectCentral.Abstractions
{
    /// <summary>
    ///     The AspectFactory interface.
    /// </summary>
    public interface IAspectFactory
    {
        /// <summary>
        ///     The create.
        /// </summary>
        /// <param name="instance">
        ///     The instance.
        /// </param>
        /// <param name="implementationType">
        ///     The implementation type.
        /// </param>
        /// <param name="args">Additional arguments</param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        T Create<T>(T instance, Type implementationType, params object[] args);
    }
}