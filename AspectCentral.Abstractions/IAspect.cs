//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IAspect.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace AspectCentral.Abstractions
{
    public interface IAspect
    {
        /// <summary>
        ///     Generates an instance of <see cref="AspectContext" />
        /// </summary>
        /// <param name="targetMethod">The method the aspect targets</param>
        /// <param name="args">The method arguments</param>
        /// <returns>An instance of <see cref="AspectContext" /></returns>
        AspectContext GenerateAspectContext(MethodInfo targetMethod, object[] args);

        /// <summary>
        ///     A <see cref="string" /> representing the method with passed in arguments
        /// </summary>
        /// <param name="targetMethod">The method the aspect targets</param>
        /// <param name="args">The method arguments</param>
        /// <param name="implementationMethod">
        ///     The <see cref="MethodInfo" /> of the object the target method is executed against.
        ///     The target <see cref="MethodInfo" /> may be from the interface and this returns the <see cref="MethodInfo" /> of
        ///     the object instance
        /// </param>
        /// <returns>A <see cref="string" /> representing the method with passed in arguments</returns>
        string GenerateMethodNameWithArguments(MethodInfo targetMethod, object[] args,
            out MethodInfo implementationMethod);

        /// <summary>
        ///     Invoked after the <see cref="Invoke" /> method has been called
        /// </summary>
        /// <param name="aspectContext">The <see cref="AspectContext" /> for the method interception</param>
        void PostInvoke(AspectContext aspectContext);

        /// <summary>
        ///     Invoked before the <see cref="Invoke" /> method has been called
        /// </summary>
        /// <param name="aspectContext">The <see cref="AspectContext" /> for the method interception</param>
        void PreInvoke(AspectContext aspectContext);

        /// <summary>
        ///     Determines if the method should be intercepted
        /// </summary>
        /// <param name="aspectContext">The <see cref="AspectContext" /> for the method interception</param>
        /// <returns>
        ///     Returns <see langword="true" /> if the method should be intercepted, otherwise <see langword="false" />
        /// </returns>
        bool ShouldIntercept(AspectContext aspectContext);
    }
}