//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectConfigurationTests.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

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
        public AspectConfigurationTests()
        {
            instance = new AspectConfiguration(new ServiceDescriptor(ITestInterfaceType,
                MyTestInterface.Type, ServiceLifetime.Transient));
        }

        private readonly AspectConfiguration instance;

        [Fact]
        public void AddEntryAddsAllMethodsWhenMethodsToInterceptIsEmptyArray()
        {
            instance.AddEntry(MyTestInterface.Type, 0);
            instance.GetAspects().First().GetMethodsToIntercept()
                .IsEqualTo(MyTestInterface.Type.GetMethods());
        }

        [Fact]
        public void AddEntryAddsAllMethodsWhenMethodsToInterceptIsNull()
        {
            instance.AddEntry(MyTestInterface.Type, 0);
            instance.GetAspects().First().GetMethodsToIntercept()
                .IsEqualTo(MyTestInterface.Type.GetMethods());
        }

        [Fact]
        public void AddEntryAddsMethodsToExistingConfigurationEntry()
        {
            instance.AddEntry(MyTestInterface.Type, 0, MyTestInterface.Type.GetMethods().Skip(1).ToArray());
            instance.AddEntry(MyTestInterface.Type, 0, MyTestInterface.Type.GetMethods().Take(1).ToArray());
            instance.GetAspects().First().GetMethodsToIntercept()
                .IsEqualTo(MyTestInterface.Type.GetMethods());
        }

        [Fact]
        public void AddEntryAddsRemovesNullMethodInfoEntries()
        {
            instance.AddEntry(MyTestInterface.Type, null, MyTestInterface.Type.GetMethods().Concat(new[] {default(MethodInfo)}).ToArray());
            instance.GetAspects().First().GetMethodsToIntercept()
                .IsEqualTo(MyTestInterface.Type.GetMethods());
        }
        
        [Fact]
        public void AddEntryNullSortOrderWithNoEntriesShouldBeOne()
        {
            instance.AddEntry(MyTestInterface.Type, null, MyTestInterface.Type.GetMethods().Concat(new[] {default(MethodInfo)}).ToArray());
            instance.GetAspects().First().SortOrder.Should().Be(1);
        }
        
        [Fact]
        public void AddEntryNullSortOrderWithNoEntriesShouldBeTheMaxSortOrderPlus1()
        {
            instance.AddEntry(MyTestInterface.Type, 3, MyTestInterface.Type.GetMethods().Concat(new[] {default(MethodInfo)}).ToArray());
            instance.AddEntry(MyUnitTestClass.Type, null, MyTestInterface.Type.GetMethods().Concat(new[] {default(MethodInfo)}).ToArray());
            instance.GetAspects().First(x => x.AspectFactoryType == MyUnitTestClass.Type).SortOrder.Should().Be(4);
        }

        [Fact]
        public void AddEntryCreatesNewConfigurationEntry()
        {
            instance.AddEntry(MyTestInterface.Type, 0, MyTestInterface.Type.GetMethods());
            var aspect = instance.GetAspects().First();
            aspect.SortOrder.Should().Be(0);
                aspect.GetMethodsToIntercept().Count.Should()
                .Be(MyTestInterface.Type.GetMethods().Length);
        }

        [Fact]
        public void AddEntryThrowsArgumentNullExceptionWhenAspectFactoryTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectFactoryType", () => instance.AddEntry(null));
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
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(GetType(), default(Type), ServiceLifetime.Transient)));
        }

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenServiceDescriptorServiceTypeIsNotInterface()
        {
            Assert.Throws<ArgumentException>("serviceDescriptor", () => new AspectConfiguration(new ServiceDescriptor(typeof(MyTestInterface), new MyTestInterface())));
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServiceDescriptorIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceDescriptor", () => new AspectConfiguration(default));
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
        public void GetHashCodeShouldEqualServiceDescriptorHashCode()
        {
            var serviceDescriptor = new ServiceDescriptor(ITestInterfaceType,
                MyTestInterface.Type, ServiceLifetime.Transient);
            new AspectConfiguration(serviceDescriptor).GetHashCode().Should().Be(
                serviceDescriptor.GetHashCode() * 397);
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldBeEqual()
        {
            var config2 = new AspectConfiguration(new ServiceDescriptor(ITestInterfaceType, MyTestInterface.Type, ServiceLifetime.Transient));
            var result = instance == config2;
            result.Should().BeTrue();
        }

        private static readonly Type ITestInterfaceType = typeof(ITestInterface);

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            var config2 = new AspectConfiguration(new ServiceDescriptor(ITestInterfaceType, MyTestInterface2.Type, ServiceLifetime.Transient));
            var result = instance != config2;
            result.Should().BeTrue();
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [Fact]
        public void ShouldBeEqual()
        {
            instance.Equals(new AspectConfiguration(new ServiceDescriptor(ITestInterfaceType, MyTestInterface.Type, ServiceLifetime.Transient))).Should().BeTrue();
        }

        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [Fact]
        public void ShouldNotBeEqual()
        {
            instance.Equals(new AspectConfiguration(new ServiceDescriptor(ITestInterfaceType, MyTestInterface2.Type, ServiceLifetime.Transient))).Should().BeFalse();
        }
    }
}