//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectContext.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using JamesConsulting.Reflection;

namespace AspectCentral.Abstractions
{
    /// <summary>
    ///     The aspect context.
    /// </summary>
    public class AspectContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AspectContext" /> class.
        /// </summary>
        /// <param name="targetMethod">
        ///     The target method.
        /// </param>
        /// <param name="parameterValues">
        ///     The parameter values.
        /// </param>
        public AspectContext(MethodInfo targetMethod, object[] parameterValues)
        {
            TargetMethod = targetMethod ?? throw new ArgumentNullException(nameof(targetMethod));
            ParameterValues = parameterValues;
            SetMethodType();
        }

        /// <summary>
        ///     Gets or sets the invocation string.
        /// </summary>
        public string InvocationString { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether invoke method.
        /// </summary>
        public bool InvokeMethod { get; set; } = true;

        /// <summary>
        ///     Gets or sets the method type.
        /// </summary>
        public MethodTypeOptions MethodType { get; set; }

        /// <summary>
        ///     Gets the parameter values.
        /// </summary>
        public object[] ParameterValues { get; }

        /// <summary>
        ///     Gets or sets the return value.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        ///     Gets the target method.
        /// </summary>
        public MethodInfo TargetMethod { get; }

        /// <summary>
        ///     Gets the target method.
        /// </summary>
        public MethodInfo InstanceMethod { get; set; }

        /// <summary>
        ///     The set method type.
        /// </summary>
        private void SetMethodType()
        {
            if (TargetMethod.IsAsync()) MethodType = TargetMethod.IsAsyncWithResult() ? MethodTypeOptions.AsyncFunction : MethodTypeOptions.AsyncAction;
            else MethodType = TargetMethod.HasReturnValue() ? MethodTypeOptions.SyncFunction : MethodTypeOptions.SyncAction;
        }
    }
}