// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectConfigurationTests.cs" company="James Consulting LLC">
//   
// </copyright>
// // <summary>
//   The aspect configuration entry tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using FluentAssertions;
using JamesConsulting.Collections;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The aspect configuration entry tests.
    /// </summary>
    public class AspectConfigurationTests
    {
        private readonly AspectConfiguration instance;

        public AspectConfigurationTests()
        {
            instance = new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType,
                TestAspectFactory.TestAspectFactoryType, ServiceLifetime.Transient));
        }
        /// <summary>
        ///     The constructor contract type is not interface throws argument exception.
        /// </summary>
        [Fact]
        public void ConstructorContractTypeIsNotInterfaceThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new AspectConfiguration(new ServiceDescriptor(GetType(), GetType(), ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The constructor contract type null throws argument null exception.
        /// </summary>
        [Fact]
        public void ConstructorContractTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(null, null)));
        }

        /// <summary>
        ///     The constructor implementation type null throws argument null exception.
        /// </summary>
        [Fact]
        public void ConstructorImplementationTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, default(Type), ServiceLifetime.Transient)));
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldBeEqual()
        {
            var config2 = new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, TestAspectFactory.TestAspectFactoryType, ServiceLifetime.Transient)); 
            var result = instance == config2; 
            result.Should().BeTrue();
        }

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            var config2 = new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, TestAspectFactory2.TestAspectFactory2Type, ServiceLifetime.Transient)); 
            var result = instance != config2; 
            result.Should().BeTrue();
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [Fact]
        public void ShouldBeEqual()
        {
            instance.Equals(new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, TestAspectFactory.TestAspectFactoryType, ServiceLifetime.Transient))).Should().BeTrue();
        }

        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [Fact]
        public void ShouldNotBeEqual()
        {
            instance.Equals(new AspectConfiguration(new ServiceDescriptor(Constants.InterfaceIAspectFactoryType, TestAspectFactory2.TestAspectFactory2Type, ServiceLifetime.Transient))).Should().BeFalse();
        }

        [Fact]
        public void EqualsOtherIsNullShouldBeFalse()
        {
            instance.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void EqualsOtherReferencesSameObjectShouldBeTrue()
        {
            instance.Equals(instance).Should().BeTrue();
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServiceDescriptorIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceDescriptor",() => new AspectConfiguration(default(ServiceDescriptor)));
        }
        
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenServiceDescriptorServiceTypeIsNotInterface()
        {
            Assert.Throws<ArgumentException>("serviceDescriptor", () => new AspectConfiguration(new ServiceDescriptor(typeof(MyTestInterface), new MyTestInterface())));
        }

        [Fact]
        public void GetHashCodeShouldEqualServiceDescriptorHashCode()
        {
            var serviceDescriptor = new ServiceDescriptor(Constants.InterfaceIAspectFactoryType,
                TestAspectFactory.TestAspectFactoryType, ServiceLifetime.Transient);
            new AspectConfiguration(serviceDescriptor).GetHashCode().Should().Be(
                serviceDescriptor.GetHashCode() * 397);
        }

        [Fact]
        public void AddEntryCreatesNewConfigurationEntry()
        {
            instance.AddEntry(TestAspectFactory.TestAspectFactoryType, 0, MyTestInterface.MyTestInterfaceType.GetMethods());
            instance.GetAspects().First().GetMethodsToIntercept().Count.Should()
                  .Be(MyTestInterface.MyTestInterfaceType.GetMethods().Length);
        }
        
        [Fact]
        public void AddEntryThrowsArgumentNullExceptionWhenAspectFactoryTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectFactoryType", () => instance.AddEntry(null));
        }

        [Fact]
        public void AddEntryAddsMethodsToExistingConfigurationEntry()
        {
            instance.AddEntry(TestAspectFactory.TestAspectFactoryType, 0, MyTestInterface.MyTestInterfaceType.GetMethods().Skip(1).ToArray());
            instance.AddEntry(TestAspectFactory.TestAspectFactoryType, 0, MyTestInterface.MyTestInterfaceType.GetMethods().Take(1).ToArray());
            instance.GetAspects().First().GetMethodsToIntercept()
                    .IsEqualTo(MyTestInterface.MyTestInterfaceType.GetMethods());
        }

        [Fact]
        public void AddEntryAddsAllMethodsWhenMethodsToInterceptIsNull()
        {
            instance.AddEntry(TestAspectFactory.TestAspectFactoryType, 0);
            instance.GetAspects().First().GetMethodsToIntercept()
                    .IsEqualTo(MyTestInterface.MyTestInterfaceType.GetMethods());
        }
        
        [Fact]
        public void AddEntryAddsAllMethodsWhenMethodsToInterceptIsEmptyArray()
        {
            instance.AddEntry(TestAspectFactory.TestAspectFactoryType, 0, new MethodInfo[0]);
            instance.GetAspects().First().GetMethodsToIntercept()
                    .IsEqualTo(MyTestInterface.MyTestInterfaceType.GetMethods());
        }
        
        [Fact]
        public void AddEntryAddsRemovesNullMethodInfoEntries()
        {
            instance.AddEntry(TestAspectFactory.TestAspectFactoryType, 0, MyTestInterface.MyTestInterfaceType.GetMethods().Concat(new MethodInfo[] { default(MethodInfo) }).ToArray());
            instance.GetAspects().First().GetMethodsToIntercept()
                    .IsEqualTo(MyTestInterface.MyTestInterfaceType.GetMethods());
        }
    }
}