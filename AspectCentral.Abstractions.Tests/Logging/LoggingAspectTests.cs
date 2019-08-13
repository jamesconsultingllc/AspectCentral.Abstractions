// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggingAspectTests.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The logging aspect tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspectCentral.Abstractions.Tests.Logging
{
    /// <summary>
    ///     The logging aspect tests.
    /// </summary>
    [TestClass]
    public class LoggingAspectTests
    {
        /// <summary>
        ///     The aspect configuration provider
        /// </summary>
        private IAspectConfigurationProvider aspectConfigurationProvider;

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
        ///     The my test method.
        /// </summary>
        [TestMethod]
        public void MyTestMethod()
        {
            instance.Test(1, "2", new MyUnitTestClass(1, "2"));
            logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Exactly(2));
        }

        /// <summary>
        ///     The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            loggerFactory = new Mock<ILoggerFactory>();
            logger = new Mock<ILogger>();
            aspectConfigurationProvider = new InMemoryAspectConfigurationProvider();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            loggerFactory.Setup(x => x.CreateLogger(typeof(MyTestInterface).FullName)).Returns(logger.Object);
            instance = LoggingAspect<ITestInterface>.Create(
                new MyTestInterface(),
                typeof(MyTestInterface),
                loggerFactory.Object,
                aspectConfigurationProvider,
                LoggingAspectFactory.LoggingAspectFactoryType);
        }

        /// <summary>
        /// The test logging async.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [TestMethod]
        public async Task TestLoggingAsync()
        {
            await instance.TestAsync(1, "2", null).ConfigureAwait(false);
            logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Exactly(2));
        }

        /// <summary>
        /// The test logging async with result.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [TestMethod]
        public async Task TestLoggingAsyncWithResult()
        {
            await instance.GetClassByIdAsync(1).ConfigureAwait(false);
            logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Exactly(3));
        }
    }
}