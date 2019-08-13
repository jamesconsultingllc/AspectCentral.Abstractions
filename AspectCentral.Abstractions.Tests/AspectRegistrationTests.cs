// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectRegistrationTests.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The aspect registration tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
using AspectCentral.Abstractions.Logging;
using AspectCentral.Abstractions.Profiling;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    /// <summary>
    ///     The aspect registration tests.
    /// </summary>
    public class AspectRegistrationTests
    {
        /// <summary>
        ///     The ITestInterface type.
        /// </summary>
        public static readonly Type IInterfaceType = typeof(ITestInterface);

        /// <summary>
        ///     The MyTestInterface type.
        /// </summary>
        public static readonly Type MyTestInterfaceType = typeof(MyTestInterface);

        /// <summary>
        ///     The services.
        /// </summary>
        private IServiceCollection services;

        /// <summary>
        ///     The initialize.
        /// </summary>
        public AspectRegistrationTests()
        {
            services = new ServiceCollection();
            services.AddAspectSupport();
            services.AddTransient<ITestInterface, MyTestInterface>();
            services.AddLogging(x => { x.AddConsole(); });
            IAspectConfigurationProvider aspectConfigurationProvider = new InMemoryAspectConfigurationProvider();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(IInterfaceType, MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, IInterfaceType.GetMethods());
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            services.AddAspectSupport(aspectConfigurationProvider);
        }

        /// <summary>
        ///     The test generic registration.
        /// </summary>
        public void TestGenericRegistration()
        {
            AssertServiceRegisteredCorrectly(services, ServiceLifetime.Transient);
        }

        ///// <summary>
        /////     The test registration with list of factories.
        ///// </summary>
        // [TestMethod]
        // public void TestRegistrationWithListOfFactories()
        // {
        // services.AddWithAspectFactories<ITestInterface, MyTestInterface>(ServiceLifetime.Transient, LoggingAspectFactory.LoggingAspectFactoryType, ProfilingAspectFactory.ProfilingAspectFactoryType);
        // AssertServiceRegisteredCorrectly(services, ServiceLifetime.Transient);
        // }

        /// <summary>
        /// The assert service registered correctly.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        /// <param name="lifetime">
        /// The lifetime.
        /// </param>
        private static void AssertServiceRegisteredCorrectly(IServiceCollection services, ServiceLifetime lifetime)
        {
            var sp = services.BuildServiceProvider();
            sp.GetService<ITestInterface>().Test(1, string.Empty, new MyUnitTestClass(1, string.Empty));
            services.Any(x => x.Lifetime == lifetime && x.ServiceType == MyTestInterfaceType && x.ImplementationType == MyTestInterfaceType).Should().BeTrue();
            services.Any(x => x.Lifetime == lifetime && x.ServiceType == IInterfaceType && x.ImplementationFactory != null).Should().BeTrue();
            sp.GetService<ITestInterface>().Should().NotBeNull();
        }
    }
}