using System;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    public class AspectContextTests
    {
        [Fact]
        public void ConstructorProperlyInitializesObject()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.GetClassByIdAsync));
            var context = new AspectContext(methodInfo, new object[] { 1 })
            {
                InvokeMethod = false, InstanceMethod = methodInfo, InvocationString = "test", ReturnValue = "oh"
            };
            Assert.Same(methodInfo, context.InstanceMethod);
            Assert.Equal("test", context.InvocationString);
            Assert.False(context.InvokeMethod);
            Assert.Same(methodInfo, context.TargetMethod);
            Assert.Single(context.ParameterValues!);
            Assert.Equal("oh", context.ReturnValue);
        }

        [Fact]
        public void ConstructorSetsMethodTypeAsyncActionWhenMethodIsAsyncWithNoReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.TestAsync));
            var context = new AspectContext(methodInfo, new object[] { 1, "y", new MyUnitTestClass(1, "y") });
            Assert.Equal(MethodTypeOptions.AsyncAction, context.MethodType);
        }

        [Fact]
        public void ConstructorSetsMethodTypeAsyncFunctionWhenMethodIsAsyncWithReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.GetClassByIdAsync));
            var context = new AspectContext(methodInfo, new object[] { 1 });
            Assert.Equal(MethodTypeOptions.AsyncFunction, context.MethodType);
        }

        [Fact]
        public void ConstructorSetsMethodTypeSyncActionWhenMethodIsAsyncWithNoReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.Test));
            var context = new AspectContext(methodInfo, new object[] { 1, "y", new MyUnitTestClass(1, "y") });
            Assert.Equal(MethodTypeOptions.SyncAction, context.MethodType);
        }

        [Fact]
        public void ConstructorSetsMethodTypeSyncFunctionWhenMethodIsAsyncWithReturnValue()
        {
            var methodInfo = MyTestInterface.Type.GetMethod(nameof(MyTestInterface.GetClassById));
            var context = new AspectContext(methodInfo, new object[] { 1 });
            Assert.Equal(MethodTypeOptions.SyncFunction, context.MethodType);
        }

        [Fact]
        public void ConstructorThrowsArgumentNullExceptionWhenTargetMethodIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AspectContext(null!, null!));
        }
    }
}
