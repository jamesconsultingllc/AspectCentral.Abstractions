using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
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
            Assert.Single(inMemoryAspectConfigurationProvider.ConfigurationEntries);
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            Assert.Single(inMemoryAspectConfigurationProvider.ConfigurationEntries);
        }

        [Fact]
        public void AddEntryThrowsArgumentNullExceptionWhenAspectConfigurationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => inMemoryAspectConfigurationProvider.AddEntry(null!));
        }

        [Fact]
        public void GetTypeAspectConfigurationReturnsConfigurationWhenTypeIsRegistered()
        {
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            Assert.NotNull(inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(
                configuration.ServiceDescriptor.ServiceType,
                configuration.ServiceDescriptor.ImplementationType!));
        }

        [Fact]
        public void GetTypeAspectConfigurationReturnsNullWhenTypeIsNotRegistered()
        {
            Assert.Null(inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(
                typeof(ITestInterface), MyTestInterface.Type));
        }

        [Fact]
        public void GetTypeAspectConfigurationThrowsArgumentNullExceptionWhenContractTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("contractType",
                () => inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(null!, null!));
        }

        [Fact]
        public void GetTypeAspectConfigurationThrowsArgumentNullExceptionWhenImplementationTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("implementationType",
                () => inMemoryAspectConfigurationProvider.GetTypeAspectConfiguration(typeof(ITestInterface), null!));
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
            Assert.False(inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type,
                typeof(ITestInterface), MyTestInterface.Type, MyTestInterface.Type.GetMethods().First()));
        }

        [Fact]
        public void ShouldInterceptReturnsFalseWhenRegistrationFoundButNoMatchingAspectRegistered()
        {
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            configuration.AddEntry(MyTestInterface.Type);
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            Assert.False(inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface2.Type,
                typeof(ITestInterface), MyTestInterface.Type, MyTestInterface.Type.GetMethods().First()));
        }

        [Fact]
        public void ShouldInterceptReturnsFalseWhenRegistrationFoundButNoMatchingMethodFound()
        {
            var methods = typeof(ITestInterface).GetMethods();
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            configuration.AddEntry(MyTestInterface.Type, methodsToIntercept: methods.First());
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            Assert.False(inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type,
                typeof(ITestInterface), MyTestInterface.Type, methods.Last()));
        }

        [Fact]
        public void ShouldInterceptReturnsFalseWhenRegistrationNotFound()
        {
            Assert.False(inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type,
                typeof(ITestInterface), MyTestInterface.Type, MyTestInterface.Type.GetMethods().First()));
        }

        [Fact]
        public void ShouldInterceptReturnsTrueWhenAllConditionsAreMet()
        {
            var methods = typeof(ITestInterface).GetMethods();
            var configuration =
                new AspectConfiguration(ServiceDescriptor.Scoped(typeof(ITestInterface), MyTestInterface.Type));
            configuration.AddEntry(MyTestInterface.Type, methodsToIntercept: methods.First());
            inMemoryAspectConfigurationProvider.AddEntry(configuration);
            Assert.True(inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type,
                typeof(ITestInterface), MyTestInterface.Type, methods.First()));
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenFactoryTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("factoryType",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(null!, null!, null!, null!));
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenImplementationTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("implementationType",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type,
                    typeof(ITestInterface), null!, null!));
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenMethodInfoIsNull()
        {
            Assert.Throws<ArgumentNullException>("methodInfo",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type,
                    typeof(ITestInterface), MyTestInterface.Type, null!));
        }

        [Fact]
        public void ShouldInterceptThrowsArgumentNullExceptionWhenServiceTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceType",
                () => inMemoryAspectConfigurationProvider.ShouldIntercept(MyTestInterface.Type, null!, null!, null!));
        }
    }
}
