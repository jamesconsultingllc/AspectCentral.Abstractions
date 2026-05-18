using System;
using System.Reflection;

namespace AspectCentral.Abstractions.Tests
{
    [Aspect]
    public class TestAspect
    {
        public static readonly Type Type = typeof(TestAspect);

        public AspectContext GenerateAspectContext(MethodInfo targetMethod, object[] args)
        {
            return new AspectContext(targetMethod, args);
        }

        public string GenerateMethodNameWithArguments(MethodInfo targetMethod, object[] args,
            out MethodInfo? implementationMethod)
        {
            implementationMethod = null;
            return targetMethod.ToString()!;
        }

        public void PostInvoke(AspectContext aspectContext)
        {
        }

        public void PreInvoke(AspectContext aspectContext)
        {
        }

        public bool ShouldIntercept(AspectContext aspectContext)
        {
            return true;
        }
    }
}
