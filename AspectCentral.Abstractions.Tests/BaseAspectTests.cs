// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseAspectTests.cs" company="CBRE">
//   
// </copyright>
// <summary>
//   The generic base aspect tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspectCentral.Abstractions.Tests
{
    /// <summary>
    ///     The generic base aspect tests.
    /// </summary>
    [TestClass]
    public class BaseAspectTests
    {
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
        [TestMethod]
        public async Task TestCreatingTaskResultWhenMethodNotInvoked()
        {
            var result = await instance.GetClassByIdAsync(1).ConfigureAwait(false);
            logger.Verify(x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Once());
            Assert.AreEqual(new MyUnitTestClass(12, "testing 123"), result);
        }

        /// <summary>
        ///     The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            loggerFactory = new Mock<ILoggerFactory>();
            logger = new Mock<ILogger>();
            loggerFactory.Setup(x => x.CreateLogger(typeof(MyTestInterface).FullName)).Returns(logger.Object);
            var aspectConfigurationProvider = new InMemoryAspectConfigurationProvider();
            var aspectConfiguration = new AspectConfiguration(new ServiceDescriptor(AspectRegistrationTests.IInterfaceType, AspectRegistrationTests.MyTestInterfaceType, ServiceLifetime.Transient));
            aspectConfiguration.AddEntry(LoggingAspectFactory.LoggingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfiguration.AddEntry(ProfilingAspectFactory.ProfilingAspectFactoryType, AspectRegistrationTests.IInterfaceType.GetMethods());
            aspectConfigurationProvider.AddEntry(aspectConfiguration);
            instance = BaseAspectTestClass<ITestInterface>.Create(new MyTestInterface(), typeof(MyTestInterface), loggerFactory.Object, aspectConfigurationProvider);
        }
    }
}