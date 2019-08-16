using System;

namespace AspectCentral.Abstractions.Tests
{
    public class TestAspectFactory : IAspectFactory
    {
        public static readonly Type TestAspectFactoryType = typeof(TestAspectFactory);
        
        public T Create<T>(T instance, Type implementationType)
        {
            throw new NotImplementedException();
        }
    }
    
    public class TestAspectFactory2 : IAspectFactory
    {
        public static readonly Type TestAspectFactory2Type = typeof(TestAspectFactory2);
        
        public T Create<T>(T instance, Type implementationType)
        {
            throw new NotImplementedException();
        }
    }
}