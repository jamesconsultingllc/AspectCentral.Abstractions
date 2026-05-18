using System;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    public class AspectConfigurationEntryTests
    {
        public AspectConfigurationEntryTests()
        {
            instance = new AspectConfigurationEntry(GetType(), 1, Methods);
        }

        private static readonly MethodInfo[] Methods = typeof(ITestInterface).GetMethods();

        private readonly AspectConfigurationEntry instance;

        private class TestConfigurationEntry : AspectConfigurationEntry
        {
            internal TestConfigurationEntry(Type aspectType, int sortOrder, params MethodInfo[] methodsToIntercept) :
                base(aspectType, sortOrder, methodsToIntercept)
            {
            }
        }

        [Fact]
        public void AddMethodsToInterceptEmptyListThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>("newMethodsToIntercept", () => instance.AddMethodsToIntercept());
        }

        [Fact]
        public void AddMethodsToInterceptNullArgumentThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("newMethodsToIntercept", () => instance.AddMethodsToIntercept(null!));
        }

        [Fact]
        public void ConstructorCreatesObjectSuccessfullyWhenTypeIsConcreteClassThatImplementsIAspectFactory()
        {
            var aspectConfiguration = new AspectConfigurationEntry(GetType(), 1, Methods);
            Assert.NotNull(aspectConfiguration);
        }

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenTypeIsNotConcreteClass()
        {
            Assert.Throws<ArgumentException>("aspectType",
                () => new AspectConfigurationEntry(typeof(ITestInterface), 1));
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectType", () => new AspectConfigurationEntry(null!, 1));
        }

        [Fact]
        public void EqualityComparerEqualsReferenceEqualsShouldBeTrue()
        {
            Assert.True(instance.Equals(instance, instance));
        }

        [Fact]
        public void EqualityComparerEqualsReturnsFalseWhenObjectsAreNotOfTheSameType()
        {
            Assert.False(instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods),
                new TestConfigurationEntry(GetType(), 1, Methods)));
        }

        [Fact]
        public void EqualityComparerEqualsShouldBeFalseWhenNotEqual()
        {
            Assert.False(instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods),
                new AspectConfigurationEntry(MyUnitTestClass.Type, 1, Methods)));
        }

        [Fact]
        public void EqualityComparerEqualsShouldBeTrue()
        {
            Assert.True(instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods),
                new AspectConfigurationEntry(GetType(), 1, Methods)));
        }

        [Fact]
        public void EqualityComparerEqualsXEqualsNullShouldBeFalse()
        {
            Assert.False(instance.Equals(null, instance));
        }

        [Fact]
        public void EqualityComparerEqualsYEqualsNullShouldBeFalse()
        {
            Assert.False(instance.Equals(instance, null));
        }

        [Fact]
        public void EqualsOtherObjectIsNullShouldBeFalse()
        {
            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void EqualsReferencesSameObjectShouldBeTrue()
        {
            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void GetHashCodeValueShouldBeHashCodeOfFactoryType()
        {
            Assert.Equal(GetType().GetHashCode(), instance.GetHashCode());
        }

        [Fact]
        public void OperatorShouldBeEqual()
        {
            var result = instance == new AspectConfigurationEntry(GetType(), 1, Methods);
            Assert.True(result);
        }

        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            var result = instance != new AspectConfigurationEntry(MyUnitTestClass.Type, 1, Methods);
            Assert.True(result);
        }

        [Fact]
        public void RemoveMethodsToInterceptMethodsToBeRemovedIsNullReturns()
        {
            instance.RemoveMethodsToIntercept(null!);
        }

        [Fact]
        public void RemoveMethodsToInterceptRemovesGivenMethods()
        {
            instance.RemoveMethodsToIntercept(Methods.Skip(2).ToArray());
            Assert.True(instance.GetMethodsToIntercept().SequenceEqual(Methods.Take(2)));
        }

        [Fact]
        public void ShouldBeEqual()
        {
            Assert.True(instance.Equals(new AspectConfigurationEntry(GetType(), 1, Methods)));
        }

        [Fact]
        public void ShouldBeTrueWhenReferencingSameObject()
        {
            Assert.True(instance.Equals(instance));
        }

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
            Assert.False(new AspectConfigurationEntry(GetType(), 1, Methods).Equals(null));
        }
    }
}
