// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectConfigurationEntryTests.cs" company="James Consulting LLC">
//   
// </copyright>
// <summary>
//   The aspect configuration tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using JamesConsulting.Collections;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The aspect configuration tests.
    /// </summary>
    public class AspectConfigurationEntryTests
    {
        /// <summary>
        ///     The methods.
        /// </summary>
        private static readonly MethodInfo[] Methods = typeof(ITestInterface).GetMethods();

        private readonly AspectConfigurationEntry instance;

        public AspectConfigurationEntryTests()
        {
            instance = new AspectConfigurationEntry(TestAspectFactory.TestAspectFactoryType, 1, Methods);
        }

        /// <summary>
        ///     The constructor creates object successfully when type is concrete class that implements i aspect factory.
        /// </summary>
        [Fact]
        public void ConstructorCreatesObjectSuccessfullyWhenTypeIsConcreteClassThatImplementsIAspectFactory()
        {
            var aspectConfiguration = new AspectConfigurationEntry(TestAspectFactory.TestAspectFactoryType, 1, Methods);
            aspectConfiguration.Should().NotBeNull();
        }

        /// <summary>
        ///     The constructor throws argument exception when concrete class does not implement i aspect factory.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenConcreteClassDoesNotImplementIAspectFactory()
        {
            Assert.Throws<ArgumentException>("aspectFactoryType", () => new AspectConfigurationEntry(GetType(), 1));
        }

        /// <summary>
        ///     The constructor throws argument exception when type is not concrete class.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenTypeIsNotConcreteClass()
        {
            Assert.Throws<ArgumentException>("aspectFactoryType",() => new AspectConfigurationEntry(Constants.InterfaceIAspectFactoryType, 1));
        }

        /// <summary>
        ///     The constructor throws argument null exception when type is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectFactoryType",() => new AspectConfigurationEntry(null, 1));
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldBeEqual()
        {
            var result = instance == new AspectConfigurationEntry(TestAspectFactory.TestAspectFactoryType, 1, Methods);
            result.Should().BeTrue();
        }

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            var result = instance != new AspectConfigurationEntry(TestAspectFactory2.TestAspectFactory2Type, 1, Methods);
            result.Should().BeTrue();
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [Fact]
        public void ShouldBeEqual()
        {
            instance.Equals(new AspectConfigurationEntry(TestAspectFactory.TestAspectFactoryType, 1, Methods)).Should().BeTrue();
        }

        [Fact]
        public void EqualsReferencesSameObjectShouldBeTrue()
        {
            instance.Equals(instance).Should().BeTrue();
        }

        [Fact]
        public void EqualsOtherObjectIsNullShouldBeFalse()
        {
            instance.Equals(null).Should().BeFalse();
        }
        
        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [Fact]
        public void ShouldNotBeEqual()
        {
            Assert.NotEqual(
                new AspectConfigurationEntry(TestAspectFactory.TestAspectFactoryType, 1, Methods),
                new AspectConfigurationEntry(TestAspectFactory2.TestAspectFactory2Type, 1, Methods));
        }

        [Fact]
        public void ShouldNotBeEqualWhenOtherIsNull()
        {
            new AspectConfigurationEntry(TestAspectFactory.TestAspectFactoryType, 1, Methods).Equals(null)
                .Should().BeFalse();
        }

        [Fact]
        public void ShouldBeTrueWhenReferencingSameObject()
        {
            instance.Equals(instance).Should().BeTrue();
        }

        [Fact]
        public void GetHashCodeValueShouldBeHashCodeOfFactoryType()
        {
            instance.GetHashCode()
                .Should().Be(TestAspectFactory.TestAspectFactoryType.GetHashCode());
        }

        [Fact]
        public void AddMethodsToInterceptNullArgumentThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("newMethodsToIntercept", () => instance.AddMethodsToIntercept(null));
        }
        
        [Fact]
        public void AddMethodsToInterceptEmptyListThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>("newMethodsToIntercept",() => instance.AddMethodsToIntercept(new MethodInfo[0]));
        }

        [Fact]
        public void RemoveMethodsToInterceptRemovesGivenMethods()
        {
            instance.RemoveMethodsToIntercept(Methods.Skip(2).ToArray());
            instance.GetMethodsToIntercept().IsEqualTo(Methods.Take(2));
        }

        [Fact]
        public void RemoveMethodsToInterceptMethodsToBeRemovedIsNullReturns()
        {
            instance.RemoveMethodsToIntercept(null);
        }
    }
}