//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="IAspectRegistrationBuilderExtensionsTests.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    // ReSharper disable once InconsistentNaming
    public class IAspectRegistrationBuilderExtensionsTests
    {
        private readonly Mock<IAspectRegistrationBuilder> mockIAspectRegistrationBuilder;
        
        public IAspectRegistrationBuilderExtensionsTests()
        {
            mockIAspectRegistrationBuilder = new Mock<IAspectRegistrationBuilder>();
        }
        
        [Fact]
        public void AddAspectThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(IAspectRegistrationBuilder).AddAspect<TestAspectFactory>());
        }

        [Fact]
        public void AddAspectCallsAddAspectWhenArgumentsAreValid()
        {
            mockIAspectRegistrationBuilder.Object.AddAspect<TestAspectFactory>();
            mockIAspectRegistrationBuilder.Verify(x => x.AddAspect(TestAspectFactory.TestAspectFactoryType, null, new MethodInfo[0]), Times.Once);
        }

        [Fact]
        public void AddScopedThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(IAspectRegistrationBuilder).AddScoped<ITestInterface, MyTestInterface>());
        }
        
        [Fact]
        public void AddScopedCallsAddServiceWhenArgumentsAreValid()
        {
            mockIAspectRegistrationBuilder.Object.AddScoped<ITestInterface, MyTestInterface>();
            mockIAspectRegistrationBuilder.Verify(x => x.AddService(typeof(ITestInterface), MyTestInterface.MyTestInterfaceType, ServiceLifetime.Scoped), Times.Once);
        }
        
        [Fact]
        public void AddScopedWithFactoryThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilder",() => default(IAspectRegistrationBuilder).AddScoped<ITestInterface>(null));
        }
        
        [Fact]
        public void AddScopedWithFactoryThrowsArgumentNullExceptionWhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>("factory", () => mockIAspectRegistrationBuilder.Object.AddScoped<ITestInterface>(null));
        }
        
        [Fact]
        public void AddScopedWithFactoryCallsAddServiceWhenArgumentsAreValid()
        {
            mockIAspectRegistrationBuilder.Object.AddScoped<ITestInterface>(serviceProvider => new MyTestInterface());
            mockIAspectRegistrationBuilder.Verify(x => x.AddService(typeof(ITestInterface), It.IsAny<Func<IServiceProvider, object>>(), ServiceLifetime.Scoped), Times.Once);
        }
        
        [Fact]
        public void AddSingletonThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(IAspectRegistrationBuilder).AddSingleton<ITestInterface, MyTestInterface>());
        }
        
        [Fact]
        public void AddSingletonCallsAddServiceWhenArgumentsAreValid()
        {
            mockIAspectRegistrationBuilder.Object.AddSingleton<ITestInterface, MyTestInterface>();
            mockIAspectRegistrationBuilder.Verify(x => x.AddService(typeof(ITestInterface), MyTestInterface.MyTestInterfaceType, ServiceLifetime.Singleton), Times.Once);
        }
        
        [Fact]
        public void AddSingletonWithFactoryThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilder",() => default(IAspectRegistrationBuilder).AddSingleton<ITestInterface>(null));
        }
        
        [Fact]
        public void AddSingletonWithFactoryThrowsArgumentNullExceptionWhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>("factory", () => mockIAspectRegistrationBuilder.Object.AddSingleton<ITestInterface>(null));
        }
        
        [Fact]
        public void AddSingletonWithFactoryCallsAddServiceWhenArgumentsAreValid()
        {
            mockIAspectRegistrationBuilder.Object.AddSingleton<ITestInterface>(serviceProvider => new MyTestInterface());
            mockIAspectRegistrationBuilder.Verify(x => x.AddService(typeof(ITestInterface), It.IsAny<Func<IServiceProvider, object>>(), ServiceLifetime.Singleton), Times.Once);
        }
        
        [Fact]
        public void AddTransientThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => default(IAspectRegistrationBuilder).AddTransient<ITestInterface, MyTestInterface>());
        }
        
        [Fact]
        public void AddTransientCallsAddServiceWhenArgumentsAreValid()
        {
            mockIAspectRegistrationBuilder.Object.AddTransient<ITestInterface, MyTestInterface>();
            mockIAspectRegistrationBuilder.Verify(x => x.AddService(typeof(ITestInterface), MyTestInterface.MyTestInterfaceType, ServiceLifetime.Transient), Times.Once);
        }
        
        [Fact]
        public void AddTransientWithFactoryThrowsArgumentNullExceptionWhenAspectRegistrationBuilderIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilder",() => default(IAspectRegistrationBuilder).AddTransient<ITestInterface>(null));
        }
        
        [Fact]
        public void AddTransientWithFactoryThrowsArgumentNullExceptionWhenFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>("factory", () => mockIAspectRegistrationBuilder.Object.AddTransient<ITestInterface>(null));
        }
        
        [Fact]
        public void AddTransientWithFactoryCallsAddServiceWhenArgumentsAreValid()
        {
            mockIAspectRegistrationBuilder.Object.AddTransient<ITestInterface>(serviceProvider => new MyTestInterface());
            mockIAspectRegistrationBuilder.Verify(x => x.AddService(typeof(ITestInterface), It.IsAny<Func<IServiceProvider, object>>(), ServiceLifetime.Transient), Times.Once);
        }
    }
}