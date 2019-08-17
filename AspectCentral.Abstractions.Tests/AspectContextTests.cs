//  ----------------------------------------------------------------------------------------------------------------------
//  <copyright file="AspectContextTests.cs" company="James Consulting LLC">
//    Copyright (c) 2019 All Rights Reserved
//  </copyright>
//  <author>Rudy James</author>
//  <summary>
// 
//  </summary>
//  ----------------------------------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    public class AspectContextTests
    {
        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTargetMethodIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectContext(default(MethodInfo), null));
        }

        [Fact]
        public void ConstructorProperlyInitializesObject()
        {
            var methodInfo = MyTestInterface.MyTestInterfaceType.GetMethod(nameof(MyTestInterface.GetClassByIdAsync));
            var context = new AspectContext(methodInfo, new object[] {1}) { InvokeMethod = false, InstanceMethod = methodInfo, InvocationString = "test", ReturnValue = "oh" };
            context.InstanceMethod.Should().BeSameAs(methodInfo);
            context.InvocationString.Should().Be("test");
            context.InvokeMethod.Should().BeFalse();
            context.TargetMethod.Should().BeSameAs(methodInfo);
            context.ParameterValues.Should().BeEquivalentTo(new object[] {1});
            context.ReturnValue.Should().Be("oh");
        }
        
        [Fact]
        public void ConstructorSetsMethodTypeAsyncFunctionWhenMethodIsAsyncWithReturnValue()
        {
            var methodInfo = MyTestInterface.MyTestInterfaceType.GetMethod(nameof(MyTestInterface.GetClassByIdAsync));
            var context = new AspectContext(methodInfo, new object[]{ 1 });
            context.MethodType.Should().BeEquivalentTo(MethodTypeOptions.AsyncFunction);
        }
        [Fact]
        public void ConstructorSetsMethodTypeAsyncActionWhenMethodIsAsyncWithNoReturnValue()
        {
            var methodInfo = MyTestInterface.MyTestInterfaceType.GetMethod(nameof(MyTestInterface.TestAsync));
            var context = new AspectContext(methodInfo, new object[]{ 1, "y", new MyUnitTestClass(1, "y") });
            context.MethodType.Should().BeEquivalentTo(MethodTypeOptions.AsyncAction);
        }
        
        [Fact]
        public void ConstructorSetsMethodTypeSyncFunctionWhenMethodIsAsyncWithReturnValue()
        {
            var methodInfo = MyTestInterface.MyTestInterfaceType.GetMethod(nameof(MyTestInterface.GetClassById));
            var context = new AspectContext(methodInfo, new object[]{ 1 });
            context.MethodType.Should().BeEquivalentTo(MethodTypeOptions.SyncFunction);
        }
        [Fact]
        public void ConstructorSetsMethodTypeSyncActionWhenMethodIsAsyncWithNoReturnValue()
        {
            var methodInfo = MyTestInterface.MyTestInterfaceType.GetMethod(nameof(MyTestInterface.Test));
            var context = new AspectContext(methodInfo, new object[]{ 1, "y", new MyUnitTestClass(1, "y") });
            context.MethodType.Should().BeEquivalentTo(MethodTypeOptions.SyncAction);
        }
    }
}