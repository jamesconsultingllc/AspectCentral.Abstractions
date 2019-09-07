//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IServiceCollectionExtensionsTests.cs" company="James Consulting LLC">
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
    // ReSharper disable once InconsistentNaming
    public class IServiceCollectionExtensionsTests
    {
        private readonly IServiceCollection serviceCollection;

        public IServiceCollectionExtensionsTests()
        {
            serviceCollection = new ServiceCollection();
        }
        
        [Fact]
        public void RegisterAspectsThrowsArgumentNullExceptionWhenServiceCollectionNull()
        {
            Assert.Throws<ArgumentNullException>("serviceCollection",() => default(IServiceCollection).RegisterAspects());
        }

        [Fact]
        public void RegisterAspectsSucceeds()
        {
            serviceCollection.RegisterAspects();
            serviceCollection.Count.Should().Be(1);
        }

        [Fact]
        public void AddAspectSupportThrowsArgumentNullExceptionWhenServiceCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceCollection",() => default(IServiceCollection).AddAspectSupport<TestAspectRegistrationBuilder>());
        }
        
        [Fact]
        public void AddAspectSupportThrowsArgumentNullExceptionWhenAspectRegistrationBuilderTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilderType",() => serviceCollection.AddAspectSupport(default));
        }
        
        [Fact]
        public void AddAspectSupportThrowsArgumentExceptionWhenAspectRegistrationBuilderTypeDoesNotImplementIAspectRegistrationBuilder()
        {
            Assert.Throws<ArgumentException>("aspectRegistrationBuilderType",() => serviceCollection.AddAspectSupport(this.GetType()));
        }
        
        [Fact]
        public void AddAspectSupportSucceeds()
        {
            var builder = serviceCollection.AddAspectSupport<TestAspectRegistrationBuilder>();
            builder.Services.Count.Should().Be(3);
            builder.Services.Count(x => x.ServiceType == typeof(IAspectRegistrationBuilder)).Should().Be(1);
            builder.Services.Count(x => x.ServiceType == typeof(TestAspect)).Should().Be(1);
            builder.Services.Count(x => x.ServiceType == typeof(IAspectConfigurationProvider)).Should().Be(1);
        }

        [Fact]
        public void AddAspectSupportWithPreLoadedConfiguration()
        {
            var configuration = new InMemoryAspectConfigurationProvider();
            configuration.AddEntry(new AspectConfiguration(ServiceDescriptor.Describe(typeof(ITestInterface), typeof(MyTestInterface), ServiceLifetime.Transient)));
            var builder = serviceCollection.AddAspectSupport<TestAspectRegistrationBuilder>(configuration);
            builder.Services.Count.Should().Be(5);
            builder.Services.Count(x => x.ServiceType == typeof(ITestInterface)).Should().Be(1);
            builder.Services.Count(x => x.ServiceType == typeof(MyTestInterface)).Should().Be(1);
            builder.Services.Count(x => x.ServiceType == typeof(IAspectRegistrationBuilder)).Should().Be(1);
            builder.Services.Count(x => x.ServiceType == typeof(TestAspect)).Should().Be(1);
            builder.Services.Count(x => x.ServiceType == typeof(IAspectConfigurationProvider)).Should().Be(1);
        }
    }
}