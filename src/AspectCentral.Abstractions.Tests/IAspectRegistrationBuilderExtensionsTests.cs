using System;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    // ReSharper disable once InconsistentNaming
    public class IAspectRegistrationBuilderExtensionsTests
    {
        public IAspectRegistrationBuilderExtensionsTests()
        {
            aspectRegistrationBuilder = Substitute.For<IAspectRegistrationBuilder>();
        }

        private readonly IAspectRegistrationBuilder aspectRegistrationBuilder;

        [Fact]
        public void AddAspectRegistersAspect()
        {
            aspectRegistrationBuilder.AddAspect<TestAspect>();
            aspectRegistrationBuilder.Received(1).AddAspect(typeof(TestAspect), null);
        }

        [Fact]
        public void AddAspectThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => default(IAspectRegistrationBuilder)!.AddAspect<TestAspect>());
        }

        [Fact]
        public void AddScopedCallsAddServiceWhenArgumentsAreValid()
        {
            aspectRegistrationBuilder.AddScoped<ITestInterface, MyTestInterface>();
            aspectRegistrationBuilder.Received(1)
                .AddService(typeof(ITestInterface), MyTestInterface.Type, ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddScopedThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                default(IAspectRegistrationBuilder)!.AddScoped<ITestInterface, MyTestInterface>());
        }

        [Fact]
        public void AddScopedWithFactoryCallsAddServiceWhenArgumentsAreValid()
        {
            aspectRegistrationBuilder.AddScoped<ITestInterface>(_ => new MyTestInterface());
            aspectRegistrationBuilder.Received(1)
                .AddService(typeof(ITestInterface), Arg.Any<Func<IServiceProvider, object>>(),
                    ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddScopedWithFactoryThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilder",
                () => default(IAspectRegistrationBuilder)!.AddScoped<ITestInterface>(null!));
        }

        [Fact]
        public void AddScopedWithFactoryThrowsArgumentNullExceptionWhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>("factory",
                () => aspectRegistrationBuilder.AddScoped<ITestInterface>(null!));
        }

        [Fact]
        public void AddSingletonCallsAddServiceWhenArgumentsAreValid()
        {
            aspectRegistrationBuilder.AddSingleton<ITestInterface, MyTestInterface>();
            aspectRegistrationBuilder.Received(1)
                .AddService(typeof(ITestInterface), MyTestInterface.Type, ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddSingletonThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                default(IAspectRegistrationBuilder)!.AddSingleton<ITestInterface, MyTestInterface>());
        }

        [Fact]
        public void AddSingletonWithFactoryCallsAddServiceWhenArgumentsAreValid()
        {
            aspectRegistrationBuilder.AddSingleton<ITestInterface>(_ => new MyTestInterface());
            aspectRegistrationBuilder.Received(1)
                .AddService(typeof(ITestInterface), Arg.Any<Func<IServiceProvider, object>>(),
                    ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddSingletonWithFactoryThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilder",
                () => default(IAspectRegistrationBuilder)!.AddSingleton<ITestInterface>(null!));
        }

        [Fact]
        public void AddSingletonWithFactoryThrowsArgumentNullExceptionWhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>("factory",
                () => aspectRegistrationBuilder.AddSingleton<ITestInterface>(null!));
        }

        [Fact]
        public void AddTransientCallsAddServiceWhenArgumentsAreValid()
        {
            aspectRegistrationBuilder.AddTransient<ITestInterface, MyTestInterface>();
            aspectRegistrationBuilder.Received(1)
                .AddService(typeof(ITestInterface), MyTestInterface.Type, ServiceLifetime.Transient);
        }

        [Fact]
        public void AddTransientThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                default(IAspectRegistrationBuilder)!.AddTransient<ITestInterface, MyTestInterface>());
        }

        [Fact]
        public void AddTransientWithFactoryCallsAddServiceWhenArgumentsAreValid()
        {
            aspectRegistrationBuilder.AddTransient<ITestInterface>(_ => new MyTestInterface());
            aspectRegistrationBuilder.Received(1)
                .AddService(typeof(ITestInterface), Arg.Any<Func<IServiceProvider, object>>(),
                    ServiceLifetime.Transient);
        }

        [Fact]
        public void AddTransientWithFactoryThrowsArgumentExceptionWhenTypeDoesNotHaveAspectAttribute()
        {
            Assert.Throws<ArgumentException>(() => aspectRegistrationBuilder.AddAspect<MyTestInterface>());
        }

        [Fact]
        public void AddTransientWithFactoryThrowsArgumentNullExceptionWhenAFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>("factory",
                () => aspectRegistrationBuilder.AddTransient<ITestInterface>(null!));
        }

        [Fact]
        public void AddTransientWithFactoryThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilder",
                () => default(IAspectRegistrationBuilder)!.AddTransient<ITestInterface>(null!));
        }
    }
}
