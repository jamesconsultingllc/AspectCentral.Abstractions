// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectConfigurationEntryTests.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The aspect configuration tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Logging;
using AspectCentral.Abstractions.Profiling;
using JamesConsulting.Collections;
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

        private readonly AspectConfigurationEntry instance;

        public AspectConfigurationEntryTests()
        {
            instance = new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods);
        }

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
            Assert.Throws<ArgumentException>(() => new AspectConfigurationEntry(GetType(), 1));
        }

        /// <summary>
        ///     The constructor throws argument exception when type is not concrete class.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenTypeIsNotConcreteClass()
        {
            Assert.Throws<ArgumentException>(() => new AspectConfigurationEntry(Constants.InterfaceIAspectFactoryType, 1));
        }

        /// <summary>
        ///     The constructor throws argument null exception when type is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfigurationEntry(null, 1));
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldBeEqual()
        {
            Assert.True(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods) == new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods));
        }

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            Assert.True(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods) != new AspectConfigurationEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, 1, Methods));
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [Fact]
        public void ShouldBeEqual()
        {
            Assert.Equal(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods),
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods));
        }

        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [Fact]
        public void ShouldNotBeEqual()
        {
            Assert.NotEqual(
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods),
                new AspectConfigurationEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, 1, Methods));
        }

        [Fact]
        public void ShouldNotBeEqualWhenOtherIsNull()
        {
            new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods).Equals(null)
                .Should().BeFalse();
        }

        [Fact]
        public void ShouldBeTrueWhenReferencingSameObject()
        {
            instance.Equals(instance).Should().BeTrue();
        }

        [Fact]
        public void GetHashCodeValueShouldBeHashCodeOfFactoryType()
        {
            new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods).GetHashCode()
                .Should().Be(LoggingAspectFactory.LoggingAspectFactoryType.GetHashCode());
        }

        [Fact]
        public void AddMethodsToInterceptNullArgumentThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods)
                    .AddMethodsToIntercept(null));
        }
        
        [Fact]
        public void AddMethodsToInterceptEmptyListThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new AspectConfigurationEntry(LoggingAspectFactory.LoggingAspectFactoryType, 1, Methods)
                    .AddMethodsToIntercept(new MethodInfo[0]));
        }

        [Fact]
        public void RemoveMethodsToInterceptThrowsArgumentNullExceptionWhenMethodsToBeRemovedIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => instance.RemoveMethodsToIntercept(null));
        }
        
        [Fact]
        public void RemoveMethodsToInterceptThrowsArgumentExceptionWhenMethodsToBeRemovedIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => instance.RemoveMethodsToIntercept(new MethodInfo[0]));
        }

        [Fact]
        public void RemoveMethodsToInterceptRemovesGivenMethods()
        {
            instance.RemoveMethodsToIntercept(Methods.Skip(2).ToArray());
            instance.GetMethodsToIntercept().IsEqualTo(Methods.Take(2));
        }
    }
}