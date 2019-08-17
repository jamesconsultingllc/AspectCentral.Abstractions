//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestAspect.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace AspectCentral.Abstractions.Tests
{
    public class TestAspect : IAspect
    {
        /// <inheritdoc />
        public AspectContext GenerateAspectContext(MethodInfo targetMethod, object[] args)
        {
            return new AspectContext(targetMethod, args);
        }

        /// <inheritdoc />
        public string GenerateMethodNameWithArguments(MethodInfo targetMethod, object[] args, out MethodInfo implementationMethod)
        {
            implementationMethod = default(MethodInfo);
            return targetMethod.ToString();
        }

        /// <inheritdoc />
        public void PostInvoke(AspectContext aspectContext)
        {
        }

        /// <inheritdoc />
        public void PreInvoke(AspectContext aspectContext)
        {
        }

        /// <inheritdoc />
        public bool ShouldIntercept(AspectContext aspectContext)
        {
            return true;
        }
    }
}