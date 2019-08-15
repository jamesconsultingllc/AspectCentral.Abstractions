// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectConfigurationTests.cs" company="CBRE">
//   
// </copyright>
// // <summary>
//   The aspect configuration entry tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Logging;
using AspectCentral.Abstractions.Profiling;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The aspect configuration entry tests.
    /// </summary>
    public class AspectConfigurationTests
    {
        /// <summary>
        ///     The constructor contract type is not interface throws argument exception.
        /// </summary>
        [Fact]
        public void ConstructorContractTypeIsNotInterfaceThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new AspectConfiguration(new ServiceDescriptor(GetType(), GetType(), ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The constructor contract type null throws argument null exception.
        /// </summary>
        [Fact]
        public void ConstructorContractTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(null, null)));
        }

        /// <summary>
        ///     The constructor implementation type null throws argument null exception.
        /// </summary>
        [Fact]
        public void ConstructorImplementationTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, default(Type), ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldBeEqual()
        {
            Assert.True(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient))
                == new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            Assert.True(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient))
                != new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, ProfilingAspectFactory.ProfilingAspectFactoryType, ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [Fact]
        public void ShouldBeEqual()
        {
            Assert.Equal(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)),
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [Fact]
        public void ShouldNotBeEqual()
        {
            Assert.NotEqual(
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)),
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, ProfilingAspectFactory.ProfilingAspectFactoryType, ServiceLifetime.Transient)));
        }

        [Fact]
        public void EqualsOtherIsNullShouldBeFalse()
        {
            new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType,
                    LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)).Equals(null).Should()
                .BeFalse();
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServiceDescriptorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(null));
        }
        
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServiceDescriptorServiceTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(default(Type), null)));
        } 
        
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenServiceDescriptorServiceTypeIsNotInterface()
        {
            Assert.Throws<ArgumentException>(() => new AspectConfiguration(new ServiceDescriptor(typeof(MyTestInterface), new MyTestInterface())));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddNewEntrySortOrderLessThanOrEqualToZeroThrowsOutOfRangeException(int sortOrder)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType,
                LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient)).AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, sortOrder));
        }

        [Fact]
        public void GetHashCodeShouldEqualServiceDescriptorHashCode()
        {
            var serviceDescriptor = new ServiceDescriptor(Constants.InterfaceIAspectFactoryType,
                LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient);
            new AspectConfiguration(serviceDescriptor).GetHashCode().Should().Be(
                serviceDescriptor.GetHashCode() * 397);
        }
    }
}