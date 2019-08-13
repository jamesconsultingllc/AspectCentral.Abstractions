// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectConfigurationTests.cs" company="CBRE">
//   
// </copyright>
// // <summary>
//   The aspect configuration entry tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The aspect configuration entry tests.
    /// </summary>
    [TestClass]
    public class AspectConfigurationTests
    {
        /// <summary>
        ///     The constructor contract type is not interface throws argument exception.
        /// </summary>
        [TestMethod]
        public void ConstructorContractTypeIsNotInterfaceThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new AspectConfiguration(new ServiceDescriptor(GetType(), GetType(), ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The constructor contract type null throws argument null exception.
        /// </summary>
        [TestMethod]
        public void ConstructorContractTypeNullThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(null, null)));
        }

        /// <summary>
        ///     The constructor implementation type null throws argument null exception.
        /// </summary>
        [TestMethod]
        public void ConstructorImplementationTypeNullThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, default(Type), ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [TestMethod]
        public void OperatorShouldBeEqual()
        {
            Assert.IsTrue(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient))
                == new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [TestMethod]
        public void OperatorShouldNotBeEqual()
        {
            Assert.IsTrue(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient))
                != new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, ProfilingAspectFactory.ProfilingAspectFactoryType, ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [TestMethod]
        public void ShouldBeEqual()
        {
            Assert.AreEqual(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)),
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [TestMethod]
        public void ShouldNotBeEqual()
        {
            Assert.AreNotEqual(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)),
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, ProfilingAspectFactory.ProfilingAspectFactoryType, ServiceLifetime.Transient)));
        }
    }
}