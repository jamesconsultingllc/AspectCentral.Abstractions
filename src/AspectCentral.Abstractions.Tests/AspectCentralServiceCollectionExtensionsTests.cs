using System;
using System.Linq;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    public class AspectCentralServiceCollectionExtensionsTests
    {
        public AspectCentralServiceCollectionExtensionsTests()
        {
            serviceCollection = new ServiceCollection();
        }

        private readonly IServiceCollection serviceCollection;

        [Fact]
        public void AddAspectSupportSucceeds()
        {
            var builder = serviceCollection.AddAspectSupport(typeof(TestAspectRegistrationBuilder));
            serviceCollection.TryAddSingleton<TestAspect>();
            Assert.Equal(3, builder.Services.Count);
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(IAspectRegistrationBuilder)));
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(TestAspect)));
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(IAspectConfigurationProvider)));
        }

        [Fact]
        public void
            AddAspectSupportThrowsAspectExceptionWhenAspectRegistrationBuilderTypeDoesNotImplementIAspectRegistrationBuilder()
        {
            var ex = Assert.Throws<AspectException>(() => serviceCollection.AddAspectSupport(GetType()));
            Assert.Equal(AspectErrorCodes.InvalidRegistrationBuilderType, ex.ErrorCode);
        }

        [Fact]
        public void AddAspectSupportThrowsArgumentNullExceptionWhenAspectRegistrationBuilderTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>("aspectRegistrationBuilderType",
                () => serviceCollection.AddAspectSupport(default!));
        }

        [Fact]
        public void AddAspectSupportThrowsArgumentNullExceptionWhenServiceCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceCollection",
                () => default(IServiceCollection)!.AddAspectSupport(typeof(TestAspectRegistrationBuilder)));
        }


        [Fact]
        public void AddAspectSupportWithPreLoadedConfiguration()
        {
            var configuration = new InMemoryAspectConfigurationProvider();
            configuration.AddEntry(new AspectConfiguration(ServiceDescriptor.Describe(typeof(ITestInterface),
                typeof(MyTestInterface), ServiceLifetime.Transient)));
            var builder = serviceCollection.AddAspectSupport(typeof(TestAspectRegistrationBuilder), configuration);
            Assert.Equal(5, builder.Services.Count);
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(ITestInterface)));
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(MyTestInterface)));
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(IAspectRegistrationBuilder)));
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(TestAspect)));
            Assert.Equal(1, builder.Services.Count(x => x.ServiceType == typeof(IAspectConfigurationProvider)));
        }

        [Fact]
        public void RegisterAspectsSucceeds()
        {
            do
            {
                serviceCollection.AddAspectSupport(typeof(TestAspectRegistrationBuilder));
            } while (serviceCollection.Count == 0);

            Assert.Equal(3, serviceCollection.Count);
            Assert.Equal(typeof(TestAspect), serviceCollection[0].ServiceType);
        }
    }
}
