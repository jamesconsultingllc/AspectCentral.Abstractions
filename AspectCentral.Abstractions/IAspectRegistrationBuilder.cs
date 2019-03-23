// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAspectRegistrationBuilder.cs" company="James Consulting LLC">
//   Copyright © 2019. All rights reserved.
// </copyright>
// <summary>
//   The AspectRegistrationBuilder interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AspectCentral.Abstractions
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The AspectRegistrationBuilder interface.
    /// </summary>
    /// <typeparam name="TContract">
    /// The object interface to intercept
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The interface implementation type
    /// </typeparam>
    public interface IAspectRegistrationBuilder<TContract, TImplementation>
        where TImplementation : TContract
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// The add factory.
        /// </summary>
        /// <param name="factory">
        /// The factory.
        /// </param>
        /// <returns>
        /// The <see cref="IAspectRegistrationBuilder{TContract, TImplementation}"/>.
        /// </returns>
        IAspectRegistrationBuilder<TContract, TImplementation> AddFactory(IAspectFactory factory);

        /// <summary>
        /// The add factory.
        /// </summary>
        /// <typeparam name="T">
        /// The IAspectFactory implementation to register
        /// </typeparam>
        /// <returns>
        /// The <see cref="IAspectRegistrationBuilder{TContract, TImplementation}"/>.
        /// </returns>
        IAspectRegistrationBuilder<TContract, TImplementation> AddFactory<T>()
            where T : IAspectFactory, new();

        /// <summary>
        /// Wraps the object with the registered aspects
        /// </summary>
        void Build();
    }
}