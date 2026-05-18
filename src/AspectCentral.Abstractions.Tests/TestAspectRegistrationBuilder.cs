using System;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspectCentral.Abstractions.Tests
{
    public class TestAspectRegistrationBuilder : AspectRegistrationBuilder
    {
        public TestAspectRegistrationBuilder(IServiceCollection services,
            IAspectConfigurationProvider aspectConfigurationProvider) : base(services, aspectConfigurationProvider)
        {
        }

        public override object InvokeCreateFactory(IServiceProvider serviceProvider,
            AspectConfiguration aspectConfiguration)
        {
            return serviceProvider.GetService(aspectConfiguration.ServiceDescriptor.ServiceType)!;
        }
    }
}
