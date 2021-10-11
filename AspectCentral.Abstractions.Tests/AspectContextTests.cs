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
using FluentAssertions;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    public class AspectContextTests
    {
        [Fact]
        public void ConstructorProperlyInitializesObject()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.GetClassByIdAsync));
            var context = new AspectContext(methodInfo, new object[] {1})
            {
                InvokeMethod = false, InstanceMethod = methodInfo, InvocationString = "test", ReturnValue = "oh"
            };
            context.InstanceMethod.Should().BeSameAs(methodInfo);
            context.InvocationString.Should().Be("test");
            context.InvokeMethod.Should().BeFalse();
            context.TargetMethod.Should().BeSameAs(methodInfo);
            context.ParameterValues.Length.Should().Be(1);
            context.ReturnValue.Should().Be("oh");
        }

        [Fact]
        public void ConstructorSetsMethodTypeAsyncActionWhenMethodIsAsyncWithNoReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.TestAsync));
            var context = new AspectContext(methodInfo, new object[] {1, "y", new MyUnitTestClass(1, "y")});
            context.MethodType.Should().Be(MethodTypeOptions.AsyncAction);
        }

        [Fact]
        public void ConstructorSetsMethodTypeAsyncFunctionWhenMethodIsAsyncWithReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.GetClassByIdAsync));
            var context = new AspectContext(methodInfo, new object[] {1});
            context.MethodType.Should().Be(MethodTypeOptions.AsyncFunction);
        }

        [Fact]
        public void ConstructorSetsMethodTypeSyncActionWhenMethodIsAsyncWithNoReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.Test));
            var context = new AspectContext(methodInfo, new object[] {1, "y", new MyUnitTestClass(1, "y")});
            context.MethodType.Should().Be(MethodTypeOptions.SyncAction);
        }

        [Fact]
        public void ConstructorSetsMethodTypeSyncFunctionWhenMethodIsAsyncWithReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.GetClassById));
            var context = new AspectContext(methodInfo, new object[] {1});
            context.MethodType.Should().Be(MethodTypeOptions.SyncFunction);
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTargetMethodIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectContext(default, null));
        }
    }
}