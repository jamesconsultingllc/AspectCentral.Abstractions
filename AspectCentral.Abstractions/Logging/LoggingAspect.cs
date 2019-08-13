// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingAspect.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The logging aspect.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using AspectCentral.Abstractions.Configuration;
using JamesConsulting.Reflection;
using Microsoft.Extensions.Logging;

namespace AspectCentral.Abstractions.Logging
{
    /// <summary>
    /// The logging aspect.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class LoggingAspect<T> : BaseAspect<T>
    {
        /// <summary>
        /// </summary>
        public static readonly Type LoggingAspectType = typeof(LoggingAspect<>);

        /// <summary>
        ///     The logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="loggerFactory">
        /// The logger.
        /// </param>
        /// <param name="aspectConfigurationProvider">
        /// </param>
        /// <param name="loggingAspectFactoryType">
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Create(T instance, Type type, ILoggerFactory loggerFactory, IAspectConfigurationProvider aspectConfigurationProvider, Type loggingAspectFactoryType)
        {
            object proxy = Create<T, LoggingAspect<T>>();
            ((LoggingAspect<T>)proxy).Instance = instance;
            ((LoggingAspect<T>)proxy).ObjectType = type;
            ((LoggingAspect<T>)proxy).logger = loggerFactory.CreateLogger(type.FullName);
            ((LoggingAspect<T>)proxy).AspectConfigurationProvider = aspectConfigurationProvider;
            ((LoggingAspect<T>)proxy).FactoryType = loggingAspectFactoryType;
            return (T)proxy;
        }

        /// <summary>
        /// The post invoke.
        /// </summary>
        /// <param name="aspectContext">
        /// The aspect context.
        /// </param>
        protected override void PostInvoke(AspectContext aspectContext)
        {
            if (aspectContext.TargetMethod.HasReturnValue()) logger.LogInformation($"Return value : {aspectContext.ReturnValue}");

            logger.LogInformation($"{aspectContext.InvocationString} End");
        }

        /// <summary>
        /// The pre invoke.
        /// </summary>
        /// <param name="aspectContext">
        /// The aspect context.
        /// </param>
        protected override void PreInvoke(AspectContext aspectContext)
        {
            logger.LogInformation($"{aspectContext.InvocationString} Start");
        }
    }
}