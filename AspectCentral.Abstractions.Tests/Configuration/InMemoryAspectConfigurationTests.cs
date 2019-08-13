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
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The in memory aspect configuration a tests.
    /// </summary>
    [TestClass]
    public class InMemoryAspectConfigurationTests
    {
        /// <summary>
        ///     The aspect configuration.
        /// </summary>
        private IAspectConfigurationProvider aspectConfigurationProvider;

        /// <summary>
        ///     The add entry adds aspect configuration entry not previously added.
        /// </summary>
        [TestMethod]
        public void AddEntryAddsAspectConfigurationEntryNotPreviouslyAdded()
        {
            var aspectConfigurationEntry =
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient));
            aspectConfigurationProvider.AddEntry(aspectConfigurationEntry);
            Assert.AreEqual(1, aspectConfigurationProvider.ConfigurationEntries.Count);
        }

        /// <summary>
        ///     The add entry null aspect configuration entry throws argument null exception.
        /// </summary>
        [TestMethod]
        public void AddEntryNullAspectConfigurationEntryThrowsArgumentNullException()
        {
            AspectConfiguration aspectConfiguration = null;
            Assert.ThrowsException<ArgumentNullException>(() => aspectConfigurationProvider.AddEntry(aspectConfiguration));
        }

        /// <summary>
        ///     The add entry removes existing entry and adds new entry.
        /// </summary>
        [TestMethod]
        public void AddEntryRemovesExistingEntryAndAddsNewEntry()
        {
            var aspectConfigurationEntry =
                new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, LoggingAspectFactory.LoggingAspectFactoryType, ServiceLifetime.Transient));
            aspectConfigurationProvider.AddEntry(aspectConfigurationEntry);
            aspectConfigurationProvider.AddEntry(aspectConfigurationEntry);
            Assert.AreEqual(1, aspectConfigurationProvider.ConfigurationEntries.Count);
        }

        /// <summary>
        ///     The get type aspect configuration contract type null throws argument null exception.
        /// </summary>
        [TestMethod]
        public void GetTypeAspectConfigurationContractTypeNullThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => aspectConfigurationProvider.GetTypeAspectConfiguration(null, null));
        }

        /// <summary>
        ///     The get type aspect configuration implementation type null throws argument null exception.
        /// </summary>
        [TestMethod]
        public void GetTypeAspectConfigurationImplementationTypeNullThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => aspectConfigurationProvider.GetTypeAspectConfiguration(Constants.GenericConfiguredTaskAwaitable, null));
        }

        /// <summary>
        ///     The get type aspect returns null if not found.
        /// </summary>
        [TestMethod]
        public void GetTypeAspectReturnsNullIfNotFound()
        {
            Assert.IsNull(aspectConfigurationProvider.GetTypeAspectConfiguration(Constants.GenericConfiguredTaskAwaitable, Constants.InterfaceIAspectFactoryType));
        }

        /// <summary>
        ///     The initialize.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            aspectConfigurationProvider = new InMemoryAspectConfigurationProvider();
        }

        /// <summary>
        /// The should intercept returns false when aspect is not registered.
        /// </summary>
        [TestMethod]
        public void ShouldInterceptReturnsFalseWhenAspectIsNotRegistered()
        {
            var method = AspectRegistrationTests.IInterfaceType.GetMethods().First();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            Assert.IsFalse(
                aspectConfigurationProvider.ShouldIntercept(
                    LoggingAspectFactory.LoggingAspectFactoryType,
                    AspectRegistrationTests.IInterfaceType,
                    AspectRegistrationTests.MyTestInterfaceType,
                    method));
        }

        /// <summary>
        /// The should intercept returns false when method is not registered.
        /// </summary>
        [TestMethod]
        public void ShouldInterceptReturnsFalseWhenMethodIsNotRegistered()
        {
            var method = AspectRegistrationTests.IInterfaceType.GetMethods().First();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods().Except(new[] { method }).ToArray());
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            Assert.IsFalse(
                aspectConfigurationProvider.ShouldIntercept(
                    LoggingAspectFactory.LoggingAspectFactoryType,
                    AspectRegistrationTests.IInterfaceType,
                    AspectRegistrationTests.MyTestInterfaceType,
                    method));
        }

        /// <summary>
        /// The should intercept returns true.
        /// </summary>
        [TestMethod]
        public void ShouldInterceptReturnsTrue()
        {
            var method = AspectRegistrationTests.IInterfaceType.GetMethods().First();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, method);
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            Assert.IsTrue(
                aspectConfigurationProvider.ShouldIntercept(
                    LoggingAspectFactory.LoggingAspectFactoryType,
                    AspectRegistrationTests.IInterfaceType,
                    AspectRegistrationTests.MyTestInterfaceType,
                    method));
        }

        /// <summary>
        /// The should intercept throws argument null exception when factory type is null.
        /// </summary>
        [TestMethod]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenFactoryTypeIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(null, null, null, null));
        }

        /// <summary>
        /// The should intercept throws argument null exception when implementation type is null.
        /// </summary>
        [TestMethod]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenImplementationTypeIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(GetType(), GetType(), null, null));
        }

        /// <summary>
        /// The should intercept throws argument null exception when method info is null.
        /// </summary>
        [TestMethod]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenMethodInfoIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(GetType(), GetType(), GetType(), null));
        }

        /// <summary>
        /// The should intercept throws argument null exception when service type is null.
        /// </summary>
        [TestMethod]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenServiceTypeIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => aspectConfigurationProvider.ShouldIntercept(GetType(), null, null, null));
        }
    }
}