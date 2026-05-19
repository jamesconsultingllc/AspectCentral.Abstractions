using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
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

        [Fact]
        public void AddAspectThrowsArgumentNullExceptionWhenAspectFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddAspect(default!));
        }

        [Fact]
        public void AddAspectThrowsAspectExceptionWhenNoServiceHasBeenRegistered()
        {
            var ex = Assert.Throws<AspectException>(
                () => aspectRegistrationBuilder.AddAspect(TestAspect.Type));
            Assert.Equal(AspectErrorCodes.NoServiceRegisteredForAspect, ex.ErrorCode);
        }

        [Fact]
        public void AddAspectWithFactorySuccess()
        {
            aspectRegistrationBuilder.AddService(typeof(ITestInterface), _ => new MyTestInterface(),
                    ServiceLifetime.Scoped)
                .AddAspect(TestAspect.Type, null, typeof(MyTestInterface).GetMethods());
            var aspects = aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].GetAspects();
            Assert.Single(aspects);
        }

        [Fact]
        public void AddServiceSuccess()
        {
            aspectRegistrationBuilder.AddService(typeof(ITestInterface), MyTestInterface.Type, ServiceLifetime.Scoped);
            Assert.Equal(2, aspectRegistrationBuilder.Services.Count);
            Assert.Single(aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries);
            Assert.Equal(MyTestInterface.Type,
                aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor.ImplementationType);
        }

        [Fact]
        public void AddServiceThrowsAspectExceptionWhenImplementationDoesNotImplementService()
        {
            var ex = Assert.Throws<AspectException>(() =>
                aspectRegistrationBuilder.AddService(typeof(IAspectConfigurationProvider), GetType(),
                    ServiceLifetime.Scoped));
            Assert.Equal(AspectErrorCodes.InvalidServiceRegistration, ex.ErrorCode);
        }

        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenImplementationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                aspectRegistrationBuilder.AddService(typeof(IAspectConfigurationProvider), default(Type)!,
                    ServiceLifetime.Scoped));
        }

        [Fact]
        public void AddServiceThrowsArgumentNullExceptionWhenServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                aspectRegistrationBuilder.AddService(null!, default(Type)!, ServiceLifetime.Scoped));
        }

        [Fact]
        public void AddServiceWithFactorySuccess()
        {
            aspectRegistrationBuilder.AddService(
                typeof(ITestInterface),
                _ => new MyTestInterface(),
                ServiceLifetime.Scoped);
            Assert.Single(aspectRegistrationBuilder.Services);
            Assert.Single(aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries);
            Assert.NotNull(aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor.ImplementationFactory);
            Assert.Null(aspectRegistrationBuilder.AspectConfigurationProvider.ConfigurationEntries[0].ServiceDescriptor.ImplementationType);
        }

        [Fact]
        public void AddServiceWithFactoryThrowsArgumentNullExceptionWhenImplementationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => aspectRegistrationBuilder.AddService(typeof(ITestInterface),
                default(Func<IServiceProvider, object>)!, ServiceLifetime.Scoped));
        }

        [Fact]
        public void AddServiceWithFactoryThrowsArgumentNullExceptionWhenServiceIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                aspectRegistrationBuilder.AddService(null!, default(Func<IServiceProvider, object>)!,
                    ServiceLifetime.Scoped));
        }

        [Fact]
        public void ConstructorCreatesNewObject()
        {
            Assert.NotNull(aspectRegistrationBuilder);
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenAspectConfigurationProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new TestAspectRegistrationBuilder(new ServiceCollection(), null!));
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestAspectRegistrationBuilder(null!, null!));
        }

        [Fact]
        public void ValidateAddAspectThrowsAspectExceptionWhenTypeIsNotAConcreteClass()
        {
            var ex = Assert.Throws<AspectException>(
                () => aspectRegistrationBuilder.AddAspect(typeof(ITestInterface)));
            Assert.Equal(AspectErrorCodes.InvalidAspectType, ex.ErrorCode);
        }
    }
}
