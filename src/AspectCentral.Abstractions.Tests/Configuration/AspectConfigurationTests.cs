using System;
using System.Linq;
using System.Reflection;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AspectCentral.Abstractions.Tests.Configuration
{
    public class AspectConfigurationTests
    {
        public AspectConfigurationTests()
        {
            instance = new AspectConfiguration(new ServiceDescriptor(TypeOfITestInterface,
                MyTestInterface.Type, ServiceLifetime.Transient));
        }

        private readonly AspectConfiguration instance;

        private static readonly Type TypeOfITestInterface = typeof(ITestInterface);

        [Fact]
        public void AddEntryAddsAllMethodsWhenMethodsToInterceptIsEmptyArray()
        {
            instance.AddEntry(MyTestInterface.Type, 0);
            Assert.True(instance.GetAspects().First().GetMethodsToIntercept()
                .SequenceEqual(TypeOfITestInterface.GetMethods()));
        }

        [Fact]
        public void AddEntryAddsAllMethodsWhenMethodsToInterceptIsNull()
        {
            instance.AddEntry(MyTestInterface.Type, 0);
            Assert.True(instance.GetAspects().First().GetMethodsToIntercept()
                .SequenceEqual(TypeOfITestInterface.GetMethods()));
        }

        [Fact]
        public void AddEntryAddsMethodsToExistingConfigurationEntry()
        {
            instance.AddEntry(MyTestInterface.Type, 0, MyTestInterface.Type.GetMethods().OrderBy(x => x.Name).Skip(1).ToArray());
            instance.AddEntry(MyTestInterface.Type, 0, MyTestInterface.Type.GetMethods().OrderBy(x => x.Name).Take(1).ToArray());
            Assert.True(instance.GetAspects().First().GetMethodsToIntercept().OrderBy(x => x.Name)
                .SequenceEqual(MyTestInterface.Type.GetMethods().OrderBy(x => x.Name)));
        }

        [Fact]
        public void AddEntryAddsRemovesNullMethodInfoEntries()
        {
            instance.AddEntry(MyTestInterface.Type, null,
                MyTestInterface.Type.GetMethods().Concat(new[] { default(MethodInfo)! }).ToArray());
            Assert.True(instance.GetAspects().First().GetMethodsToIntercept()
                .SequenceEqual(MyTestInterface.Type.GetMethods()));
        }

        [Fact]
        public void AddEntryCreatesNewConfigurationEntry()
        {
            instance.AddEntry(MyTestInterface.Type, 0, MyTestInterface.Type.GetMethods());
            var aspect = instance.GetAspects().First();
            Assert.Equal(0, aspect.SortOrder);
            Assert.Equal(MyTestInterface.Type.GetMethods().Length, aspect.GetMethodsToIntercept().Count);
        }

        [Fact]
        public void AddEntryNullSortOrderWithNoEntriesShouldBeOne()
        {
            instance.AddEntry(MyTestInterface.Type, null,
                MyTestInterface.Type.GetMethods().Concat(new[] { default(MethodInfo)! }).ToArray());
            Assert.Equal(1, instance.GetAspects().First().SortOrder);
        }

        [Fact]
        public void AddEntryNullSortOrderWithNoEntriesShouldBeTheMaxSortOrderPlus1()
        {
            instance.AddEntry(MyTestInterface.Type, 3,
                MyTestInterface.Type.GetMethods().Concat(new[] { default(MethodInfo)! }).ToArray());
            instance.AddEntry(MyUnitTestClass.Type, null,
                MyTestInterface.Type.GetMethods().Concat(new[] { default(MethodInfo)! }).ToArray());
            Assert.Equal(4, instance.GetAspects().First(x => x.AspectType == MyUnitTestClass.Type).SortOrder);
        }

        [Fact]
        public void AddEntryThrowsArgumentNullExceptionWhenAspectFactoryTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectFactoryType", () => instance.AddEntry(null!));
        }

        [Fact]
        public void ConstructorContractTypeIsNotInterfaceThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                new AspectConfiguration(new ServiceDescriptor(GetType(), GetType(), ServiceLifetime.Transient)));
        }

        [Fact]
        public void ConstructorContractTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectConfiguration(new ServiceDescriptor(null!, null!)));
        }

        [Fact]
        public void ConstructorImplementationTypeNullThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new AspectConfiguration(new ServiceDescriptor(GetType(), default(Type)!, ServiceLifetime.Transient)));
        }

        [Fact]
        public void ConstructorThrowsArgumentExceptionWhenServiceDescriptorServiceTypeIsNotInterface()
        {
            Assert.Throws<ArgumentException>("serviceDescriptor",
                () => new AspectConfiguration(new ServiceDescriptor(typeof(MyTestInterface), new MyTestInterface())));
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenServiceDescriptorIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceDescriptor", () => new AspectConfiguration(default!));
        }

        [Fact]
        public void EqualsOtherIsNullShouldBeFalse()
        {
            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void EqualsOtherReferencesSameObjectShouldBeTrue()
        {
            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void GetHashCodeShouldEqualServiceDescriptorHashCode()
        {
            var serviceDescriptor = new ServiceDescriptor(TypeOfITestInterface,
                MyTestInterface.Type, ServiceLifetime.Transient);
            Assert.Equal(serviceDescriptor.GetHashCode() * 397,
                new AspectConfiguration(serviceDescriptor).GetHashCode());
        }

        [Fact]
        public void OperatorShouldBeEqual()
        {
            var config2 = new AspectConfiguration(new ServiceDescriptor(TypeOfITestInterface, MyTestInterface.Type,
                ServiceLifetime.Transient));
            var result = instance == config2;
            Assert.True(result);
        }

        [Fact]
        public void OperatorShouldNotBeEqual()
        {
            var config2 = new AspectConfiguration(new ServiceDescriptor(TypeOfITestInterface, MyTestInterface2.Type,
                ServiceLifetime.Transient));
            var result = instance != config2;
            Assert.True(result);
        }

        [Fact]
        public void ShouldBeEqual()
        {
            Assert.True(instance.Equals(new AspectConfiguration(new ServiceDescriptor(TypeOfITestInterface,
                MyTestInterface.Type, ServiceLifetime.Transient))));
        }

        [Fact]
        public void ShouldNotBeEqual()
        {
            Assert.False(instance.Equals(new AspectConfiguration(new ServiceDescriptor(TypeOfITestInterface,
                MyTestInterface2.Type, ServiceLifetime.Transient))));
        }
    }
}
