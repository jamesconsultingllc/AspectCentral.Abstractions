//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="InMemoryAspectConfigurationProviderTests.cs" company="James Consulting LLC">
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

namespace AspectCentral.Abstractions.Tests.Configuration
{
    public class InMemoryAspectConfigurationProviderTests
    {
        public InMemoryAspectConfigurationProviderTests()
        {
            inMemoryAspectConfigurationProvider = new InMemoryAspectConfigurationProvider();
        }

        private readonly InMemoryAspectConfigurationProvider inMemoryAspectConfigurationProvider;

        [Fact]
        public void AddEntryReplacesExistingEntry()
        {
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            inMemoryAspectConfigurationProvider.ConfigurationEntries.Count.Should().Be(1);
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            inMemoryAspectConfigurationProvider.ConfigurationEntries.Count.Should().Be(1);
        }

        [Fact]
        public void AddEntryThrowsArgumentNullExceptionWhenAspectConfigurationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => inMemoryAspectConfigurationProvider.AddEntry(null));
        }

        [Fact]
        public void GetTypeAspectConfigurationReturnsConfigurationWhenTypeIsRegistered()
        {
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(configuration.ServiceDescriptor.ServiceType,
                configuration.ServiceDescriptor.ImplementationType!).Should().NotBeNull();
        }

        [Fact]
        public void GetTypeAspectConfigurationReturnsNullWhenTypeIsNotRegistered()
        {
            inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(typeof(ITestInterface), MyTestInterface.Type)
                .Should().BeNull();
        }

        [Fact]
        public void GetTypeAspectConfigurationThrowsArgumentNullExceptionWhenContractTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("contractType",
                () => inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(null, null));
        }

        [Fact]
        public void GetTypeAspectConfigurationThrowsArgumentNullExceptionWhenImplementationTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("implementationType",
                () => inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(typeof(ITestInterface), null));
        }

        [Fact]
        public void LoadConfigurationThrowsNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => inMemoryAspectConfigurationProvider.LoadConfiguration());
        }

        [Fact]
        public void ShouldInterceptReturnsFalseWhenRegistrationFoundButNoAspectsRegistered()
        {
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type, typeof(ITestInterface),
                MyTestInterface.Type, MyTestInterface.Type.GetMethods().First()).Should().BeFalse();
        }

        [Fact]
        public void ShouldInterceptReturnsFalseWhenRegistrationFoundButNoMatchingAspectRegistered()
        {
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            configuration.AddEntry(MyTestInterface.Type);
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface2.Type, typeof(ITestInterface),
                MyTestInterface.Type, MyTestInterface.Type.GetMethods().First()).Should().BeFalse();
        }

        [Fact]
        public void ShouldInterceptReturnsFalseWhenRegistrationFoundButNoMatchingMethodFound()
        {
            var methods = typeof(ITestInterface).GetMethods();
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            configuration.AddEntry(MyTestInterface.Type, methodsToIntercept: methods.First());
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            inMemoryAspectConfigurationProvider
                .ShouldIntercept(MyTestInterface.Type, typeof(ITestInterface), MyTestInterface.Type, methods.Last())
                .Should().BeFalse();
        }

        [Fact]
        public void ShouldInterceptReturnsFalseWhenRegistrationNotFound()
        {
            inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type, typeof(ITestInterface),
                MyTestInterface.Type, MyTestInterface.Type.GetMethods().First()).Should().BeFalse();
        }

        [Fact]
        public void ShouldInterceptReturnsTrueWhenAllConditionsAreMet()
        {
            var methods = typeof(ITestInterface).GetMethods();
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            configuration.AddEntry(MyTestInterface.Type, methodsToIntercept: methods.First());
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            inMemoryAspectConfigurationProvider
                .ShouldIntercept(MyTestInterface.Type, typeof(ITestInterface), MyTestInterface.Type, methods.First())
                .Should().BeTrue();
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenFactoryTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("factoryType",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(null, null, null, null));
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenImplementationTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("implementationType",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type, typeof(ITestInterface),
                    null, null));
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenMethodInfoIsNull()
        {
            Assert.Throws<ArgumentNullException>("methodInfo",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type, typeof(ITestInterface),
                    MyTestInterface.Type, null));
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenServiceTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceType",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type, null, null, null));
        }
    }
}