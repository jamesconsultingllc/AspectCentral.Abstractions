// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectConfigurationEntryTests.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The aspect configuration tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using FluentAssertions;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Logging;
using AspectCentral.Abstractions.Profiling;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The aspect configuration tests.
    /// </summary>
    public class AspectConfigurationEntryTests
    {
        /// <summary>
        ///     The methods.
        /// </summary>
        private static readonly MethodInfo[] Methods = typeof(ITestInterface).GetMethods();

        /// <summary>
        ///     The constructor creates object successfully when type is concrete class that implements i aspect factory.
        /// </summary>
        [Fact]
        public void ConstructorCreatesObjectSuccessfullyWhenTypeIsConcreteClassThatImplementsIAspectFactory()
        {
            var aspectConfiguration = new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods);
            aspectConfiguration.Should().NotBeNull();
        }

        /// <summary>
        ///     The constructor throws argument exception when concrete class does not implement i aspect factory.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenConcreteClassDoesNotImplementIAspectFactory()
        {
            Assert.ThrowsException<ArgumentException>(() => new AspectConfigurationEntry(GetType(), 1));
        }

        /// <summary>
        ///     The constructor throws argument exception when type is not concrete class.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenTypeIsNotConcreteClass()
        {
            Assert.ThrowsException<ArgumentException>(() => new AspectConfigurationEntry(Constants.InterfaceIAspectFactoryType, 1));
        }

        /// <summary>
        ///     The constructor throws argument null exception when type is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTypeIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new AspectConfigurationEntry(null, 1));
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldBeEqual()
        {
            Assert.IsTrue(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods) == new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods));
        }

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            Assert.IsTrue(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods) != new AspectConfigurationEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, 1, Methods));
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [Fact]
        public void ShouldBeEqual()
        {
            Assert.AreEqual(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods),
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods));
        }

        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [Fact]
        public void ShouldNotBeEqual()
        {
            Assert.AreNotEqual(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods),
                new AspectConfigurationEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, 1, Methods));
        }
    }
}