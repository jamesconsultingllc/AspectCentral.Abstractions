// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAspectFactory.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The AspectFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace AspectCentral.Abstractions
{
    /// <summary>
    ///     The AspectFactory interface.
    /// </summary>
    public interface IAspectFactory
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Create<T>(T instance, Type implementationType);
    }
}