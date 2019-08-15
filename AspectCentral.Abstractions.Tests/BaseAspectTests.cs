// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseAspectTests.cs" company="James Consulting LLC">
//   
// </copyright>
// <summary>
//   The generic base aspect tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using AspectCentral.Abstractions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AspectCentral.Abstractions.Tests
{
    /// <summary>
    ///     The generic base aspect tests.
    /// </summary>
    public class BaseAspectTests
    {
        public static readonly Type ITestInterfaceType = typeof(ITestInterface);
        /// <summary>
        ///     The instance.
        /// </summary>
        private ITestInterface instance;

        /// <summary>
        ///     The logger.
        /// </summary>
        private Mock<ILogger> logger;

        /// <summary>
        ///     The logger factory.
        /// </summary>
        private Mock<ILoggerFactory> loggerFactory;

        /// <summary>
        ///     The test creating task result when method not invoked.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        [Fact]
        public async Task TestCreatingTaskResultWhenMethodNotInvoked()
        {
            var result = await instance.GetClassByIdAsync(12).ConfigureAwait(false);
            logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once());
            Assert.Equal(new MyUnitTestClass(12, "testing 123"), result);
        }

        /// <summary>
        ///     The test initialize.
        /// </summary>
        public BaseAspectTests()
        {
            loggerFactory = new Mock<ILoggerFactory>();
            logger = new Mock<ILogger>();
            loggerFactory.Setup(x => x.CreateLogger(typeof(MyTestInterface).FullName)).Returns(logger.Object);
            var aspectConfigurationProvider = new Mock<IAspectConfigurationProvider>().Object;
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(ITestInterfaceType, MyTestInterface.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(TestAspectFactory.TestAspectFactoryType, ITestInterfaceType.GetMethods());
            aspectConfiguration.AddEntry(TestAspectFactory2.TestAspectFactory2Type, ITestInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            instance = BaseAspectTestClass<ITestInterface>.Create(new MyTestInterface(), typeof(MyTestInterface), loggerFactory.Object, aspectConfigurationProvider);
        }
    }
}