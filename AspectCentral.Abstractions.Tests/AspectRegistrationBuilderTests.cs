//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectRegistrationBuilderTests.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    public class AspectRegistrationBuilderTests
    {
        public AspectRegistrationBuilderTests()
        {
            aspectRegistrationBuilder =
                new TestAspectRegistrationBuilder(new ServiceCollection(), new InMemoryAspectConfigurationProvider());
        }

        private readonly TestAspectRegistrationBuilder aspectRegistrationBuilder;

        /// <summary>
        ///     The add aspect throws argument null exception when aspect factory is null.
        /// </summary>
        [Fact]
        public void AddAspectThrowsArgumentNullExceptionWhenAspectFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddAspect(default));
        }

        /// <summary>
        ///     The add aspect throws invalid operation exception when services have been registered.
        /// </summary>
        [Fact]
        public void AddAspectThrowsInvalidOperationExceptionWhenServicesHaveBeenRegistered()
        {
            Assert.Throws<InvalidOperationException>(
                () => aspectRegistrationBuilder.AddAspect(TestAspect.Type));
        }

        /// <summary>
        ///     The add aspect with factory success.
        /// </summary>
        [Fact]
        public void AddAspectWithFactorySuccess()
        {
            aspectRegistrationBuilder.AddService(typeof(ITestInterface), _ => new MyTestInterface(),
                    ServiceLifetime.Scoped)
                .AddAspect(TestAspect.Type, null, typeof(MyTestInterface).GetMethods());
            var aspects = aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].GetAspects();
            aspects.Count().Should().Be(1);
        }

        /// <summary>
        ///     The add service success.
        /// </summary>
        [Fact]
        public void AddServiceSuccess()
        {
            aspectRegistrationBuilder.AddService(typeof(ITestInterface), MyTestInterface.Type, ServiceLifetime.Scoped);
            aspectRegistrationBuilder.Services.Count.Should().Be(2);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries.Count.Should().Be(1);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor
                .ImplementationType.Should().Be(MyTestInterface.Type);
        }

        /// <summary>
        ///     The add service throws argument null exception when implementation does not implement service.
        /// </summary>
        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenImplementationDoesNotImplementService()
        {
            Assert.Throws<ArgumentException>(() =>
                aspectRegistrationBuilder.AddService(typeof(IAspectConfigurationProvider), GetType(),
                    ServiceLifetime.Scoped));
        }

        /// <summary>
        ///     The add service throws argument null exception when implementation is null.
        /// </summary>
        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenImplementationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                aspectRegistrationBuilder.AddService(typeof(IAspectConfigurationProvider), default(Type),
                    ServiceLifetime.Scoped));
        }

        /// <summary>
        ///     The add service throws argument null exception when service is null.
        /// </summary>
        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                aspectRegistrationBuilder.AddService(null, default(Type), ServiceLifetime.Scoped));
        }

        /// <summary>
        ///     The add service with factory success.
        /// </summary>
        [Fact]
        public void AddServiceWithFactorySuccess()
        {
            aspectRegistrationBuilder.AddService(
                typeof(ITestInterface),
                _ => new MyTestInterface(),
                ServiceLifetime.Scoped);
            aspectRegistrationBuilder.Services.Count.Should().Be(1);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries.Count.Should().Be(1);
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor
                .ImplementationFactory.Should().NotBeNull();
            aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor
                .ImplementationType.Should().BeNull();
        }

        /// <summary>
        ///     The add service with factory throws argument null exception when implementation is null.
        /// </summary>
        [Fact]
        public void AddServiceWithFactoryThrowsArgumentNullExceptionWhenImplementationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddService(typeof(ITestInterface),
                default(Func<IServiceProvider, object>), ServiceLifetime.Scoped));
        }

        /// <summary>
        ///     The add service with factory throws argument null exception when service is null.
        /// </summary>
        [Fact]
        public void AddServiceWithFactoryThrowsArgumentNullExceptionWhenServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                aspectRegistrationBuilder.AddService(null, default(Func<IServiceProvider, object>),
                    ServiceLifetime.Scoped));
        }

        /// <summary>
        ///     The constructor creates new object.
        /// </summary>
        [Fact]
        public void ConstructorCreatesNewObject()
        {
            aspectRegistrationBuilder.Should().NotBeNull();
        }

        /// <summary>
        ///     The constructor throws argument null exception when aspect configuration provider is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenAspectConfigurationProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new TestAspectRegistrationBuilder(new ServiceCollection(), null));
        }

        /// <summary>
        ///     The constructor throws argument null exception when services is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestAspectRegistrationBuilder(null, null));
        }

        [Fact]
        public void ValidateAddAspectThrowsExceptionWhenTypeIsNotAConcreteClass()
        {
            Assert.Throws<ArgumentException>("aspectType",
                () => aspectRegistrationBuilder.AddAspect(typeof(ITestInterface)));
        }
    }
}