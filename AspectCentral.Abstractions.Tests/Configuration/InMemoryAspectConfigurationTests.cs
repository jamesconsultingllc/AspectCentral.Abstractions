// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryAspectConfigurationTests.cs" company="CBRE">
//   
// </copyright>
// // <summary>
//   The in memory aspect configuration a tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Logging;
using AspectCentral.Abstractions.Profiling;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The in memory aspect configuration a tests.
    /// </summary>
    public class InMemoryAspectConfigurationTests
    {
        /// <summary>
        ///     The aspect configuration.
        /// </summary>
        private IAspectConfigurationProvider aspectConfigurationProvider;

        /// <summary>
        ///     The add entry adds aspect configuration entry not previously added.
        /// </summary>
        [Fact]
        public void AddEntryAddsAspectConfigurationEntryNotPreviouslyAdded()
        {
            var aspectConfigurationEntry =
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient));
            aspectConfigurationProvider.AddEntry(aspectConfigurationEntry);
            Assert.Single(aspectConfigurationProvider.ConfigurationEntries);
        }

        /// <summary>
        ///     The add entry null aspect configuration entry throws argument null exception.
        /// </summary>
        [Fact]
        public void AddEntryNullAspectConfigurationEntryThrowsArgumentNullException()
        {
            AspectConfiguration aspectConfiguration = null;
            Assert.Throws<ArgumentNullException>(() => aspectConfigurationProvider.AddEntry(aspectConfiguration));
        }

        /// <summary>
        ///     The add entry removes existing entry and adds new entry.
        /// </summary>
        [Fact]
        public void AddEntryRemovesExistingEntryAndAddsNewEntry()
        {
            var aspectConfigurationEntry =
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient));
            aspectConfigurationProvider.AddEntry(aspectConfigurationEntry);
            aspectConfigurationProvider.AddEntry(aspectConfigurationEntry);
            Assert.Single(aspectConfigurationProvider.ConfigurationEntries);
        }

        /// <summary>
        ///     The get type aspect configuration contract type null throws argument null exception.
        /// </summary>
        [Fact]
        public void GetTypeAspectConfigurationContractTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => aspectConfigurationProvider.GetTypeAspectConfiguration(null, null));
        }

        /// <summary>
        ///     The get type aspect configuration implementation type null throws argument null exception.
        /// </summary>
        [Fact]
        public void GetTypeAspectConfigurationImplementationTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => aspectConfigurationProvider.GetTypeAspectConfiguration(JamesConsulting.Constants.GenericConfiguredTaskAwaitable, null));
        }

        /// <summary>
        ///     The get type aspect returns null if not found.
        /// </summary>
        [Fact]
        public void GetTypeAspectReturnsNullIfNotFound()
        {
            Assert.Null(aspectConfigurationProvider.GetTypeAspectConfiguration(JamesConsulting.Constants.GenericConfiguredTaskAwaitable, Constants.InterfaceIAspectFactoryType));
        }

        /// <summary>
        ///     The initialize.
        /// </summary>
        public InMemoryAspectConfigurationTests()
        {
            aspectConfigurationProvider = new InMemoryAspectConfigurationProvider();
        }

        /// <summary>
        /// The should intercept returns false when aspect is not registered.
        /// </summary>
        [Fact]
        public void ShouldInterceptReturnsFalseWhenAspectIsNotRegistered()
        {
            var method = AspectRegistrationTests.IInterfaceType.GetMethods().First();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            Assert.False(
                aspectConfigurationProvider.ShouldIntercept(
                    LoggingAspectFactory.LoggingAspectFactoryType,
                    AspectRegistrationTests.IInterfaceType,
                    AspectRegistrationTests.MyTestInterfaceType,
                    method));
        }

        /// <summary>
        /// The should intercept returns false when method is not registered.
        /// </summary>
        [Fact]
        public void ShouldInterceptReturnsFalseWhenMethodIsNotRegistered()
        {
            var method = AspectRegistrationTests.IInterfaceType.GetMethods().First();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods().Except(new[] { method }).ToArray());
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            Assert.False(
                aspectConfigurationProvider.ShouldIntercept(
                    LoggingAspectFactory.LoggingAspectFactoryType,
                    AspectRegistrationTests.IInterfaceType,
                    AspectRegistrationTests.MyTestInterfaceType,
                    method));
        }

        /// <summary>
        /// The should intercept returns true.
        /// </summary>
        [Fact]
        public void ShouldInterceptReturnsTrue()
        {
            var method = AspectRegistrationTests.IInterfaceType.GetMethods().First();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, method);
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            Assert.True(
                aspectConfigurationProvider.ShouldIntercept(
                    LoggingAspectFactory.LoggingAspectFactoryType,
                    AspectRegistrationTests.IInterfaceType,
                    AspectRegistrationTests.MyTestInterfaceType,
                    method));
        }

        /// <summary>
        /// The should intercept throws argument null exception when factory type is null.
        /// </summary>
        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenFactoryTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(null, null, null, null));
        }

        /// <summary>
        /// The should intercept throws argument null exception when implementation type is null.
        /// </summary>
        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenImplementationTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(GetType(), GetType(), null, null));
        }

        /// <summary>
        /// The should intercept throws argument null exception when method info is null.
        /// </summary>
        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenMethodInfoIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(GetType(), GetType(), GetType(), null));
        }

        /// <summary>
        /// The should intercept throws argument null exception when service type is null.
        /// </summary>
        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenServiceTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(GetType(), null, null, null));
        }
    }
}