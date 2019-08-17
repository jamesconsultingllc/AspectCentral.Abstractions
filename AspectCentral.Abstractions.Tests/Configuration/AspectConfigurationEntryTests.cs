//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectConfigurationEntryTests.cs" company="James Consulting LLC">
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
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    /// <summary>
    ///     The aspect configuration tests.
    /// </summary>
    public class AspectConfigurationEntryTests
    {
        public AspectConfigurationEntryTests()
        {
            instance = new AspectConfigurationEntry(this.GetType(), 1, Methods);
        }

        /// <summary>
        ///     The methods.
        /// </summary>
        private static readonly MethodInfo[] Methods = typeof(ITestInterface).GetMethods();

        private readonly AspectConfigurationEntry instance;

        [Fact]
        public void AddMethodsToInterceptEmptyListThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>("newMethodsToIntercept", () => instance.AddMethodsToIntercept());
        }

        [Fact]
        public void AddMethodsToInterceptNullArgumentThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("newMethodsToIntercept", () => instance.AddMethodsToIntercept(null));
        }

        /// <summary>
        ///     The constructor creates object successfully when type is concrete class that implements i aspect factory.
        /// </summary>
        [Fact]
        public void ConstructorCreatesObjectSuccessfullyWhenTypeIsConcreteClassThatImplementsIAspectFactory()
        {
            var aspectConfiguration = new AspectConfigurationEntry(this.GetType(), 1, Methods);
            aspectConfiguration.Should().NotBeNull();
        }

        /// <summary>
        ///     The constructor throws argument exception when type is not concrete class.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenTypeIsNotConcreteClass()
        {
            Assert.Throws<ArgumentException>("aspectFactoryType", () => new AspectConfigurationEntry(typeof(ITestInterface), 1));
        }

        /// <summary>
        ///     The constructor throws argument null exception when type is null.
        /// </summary>
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectFactoryType", () => new AspectConfigurationEntry(null, 1));
        }

        [Fact]
        public void EqualsOtherObjectIsNullShouldBeFalse()
        {
            instance.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void EqualsReferencesSameObjectShouldBeTrue()
        {
            instance.Equals(instance).Should().BeTrue();
        }

        [Fact]
        public void GetHashCodeValueShouldBeHashCodeOfFactoryType()
        {
            instance.GetHashCode()
                .Should().Be(GetType().GetHashCode());
        }

        /// <summary>
        ///     The operator should be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldBeEqual()
        {
            var result = instance == new AspectConfigurationEntry(GetType(), 1, Methods);
            result.Should().BeTrue();
        }

        /// <summary>
        ///     The operator should not be equal.
        /// </summary>
        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            var result = instance != new AspectConfigurationEntry(MyUnitTestClass.Type, 1, Methods);
            result.Should().BeTrue();
        }

        [Fact]
        public void RemoveMethodsToInterceptMethodsToBeRemovedIsNullReturns()
        {
            instance.RemoveMethodsToIntercept(null);
        }

        [Fact]
        public void RemoveMethodsToInterceptRemovesGivenMethods()
        {
            instance.RemoveMethodsToIntercept(Methods.Skip(2).ToArray());
            instance.GetMethodsToIntercept().IsEqualTo(Methods.Take(2));
        }

        /// <summary>
        ///     The should be equal.
        /// </summary>
        [Fact]
        public void ShouldBeEqual()
        {
            instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods)).Should().BeTrue();
        }

        [Fact]
        public void ShouldBeTrueWhenReferencingSameObject()
        {
            instance.Equals(instance).Should().BeTrue();
        }

        /// <summary>
        ///     The should not be equal.
        /// </summary>
        [Fact]
        public void ShouldNotBeEqual()
        {
            Assert.NotEqual(
                new AspectConfigurationEntry(GetType(), 1, Methods),
                new AspectConfigurationEntry(MyUnitTestClass.Type, 1, Methods));
        }

        [Fact]
        public void ShouldNotBeEqualWhenOtherIsNullShouldBeFalse()
        {
            new AspectConfigurationEntry(GetType(), 1, Methods).Equals(null)
                .Should().BeFalse();
        }

        [Fact]
        public void EqualityComparerEqualsReferenceEqualsShouldBeTrue()
        {
            instance.Equals(instance, instance).Should().BeTrue();
        }
        
        [Fact]
        public void EqualityComparerEqualsXEqualsNullShouldBeFalse()
        {
            instance.Equals(null, instance).Should().BeFalse();
        }
        
        [Fact]
        public void EqualityComparerEqualsYEqualsNullShouldBeFalse()
        {
            instance.Equals(instance,null).Should().BeFalse();
        }

        [Fact]
        public void EqualityComparerEqualsShouldBeTrue()
        {
            instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods), 
                new AspectConfigurationEntry(GetType(), 1, Methods)).Should().BeTrue();
        }

        [Fact] public void EqualityComparerEqualsShouldBeFalseWhenNotEqual()
        {
            instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods), 
                new AspectConfigurationEntry(MyUnitTestClass.Type, 1, Methods)).Should().BeFalse();
        }

        [Fact]
        public void EqualityComparerEqualsReturnsFalseWhenObjectsAreNotOfTheSameType()
        {
            instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods), 
                new TestConfigurationEntry(GetType(), 1, Methods)).Should().BeFalse();
        }

        class TestConfigurationEntry : AspectConfigurationEntry
        {
            /// <inheritdoc />
            internal TestConfigurationEntry(Type aspectFactoryType, int sortOrder, params MethodInfo[] methodsToIntercept) : base(aspectFactoryType, sortOrder, methodsToIntercept)
            {
            }
        }
    }
}